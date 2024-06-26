using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Http;
using MultiTenancyDemo.Entities;
using MultiTenancyDemo.Services;
using System.Linq.Expressions;
using System.Reflection;

namespace MultiTenancyDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly string _tenantId;

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Countries> Countries => Set<Countries>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService): base(options)
        {
            _tenantId = tenantService.GetTenantId();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            foreach (var entity in builder.Model.GetEntityTypes())
            {
                Type clrType = entity.ClrType;

                if (typeof(ITenatEntity).IsAssignableFrom(clrType))
                {
                    //build filter
                    var method = typeof(ApplicationDbContext).GetMethod(nameof(BuildGlobalTenantFilter), BindingFlags.Static | BindingFlags.Public)?.MakeGenericMethod(clrType);

                    var filter = method?.Invoke(null, new object[] { this });
                    entity.SetQueryFilter(queryFilter: filter as LambdaExpression);
                    entity.AddIndex(property: entity.FindProperty(nameof(ITenatEntity.TenatId))!);
                }
                else if(clrType.SkipTenantValidation()) 
                {
                    continue;
                }
                else
                {
                    throw new Exception($"The entity {entity} has not been marked with ITennatEntiy or ICommonEntity");
                }
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries().Where(entry => entry.State.Equals(EntityState.Added) && entry.Entity is ITenatEntity)) 
            {
                if (string.IsNullOrWhiteSpace(_tenantId))
                    throw new Exception($"The tenat with id {_tenantId} was not found when creating the record");

                var entity = item.Entity as ITenatEntity;
                entity!.TenatId = _tenantId;
            }

            return base.SaveChangesAsync(cancellationToken);
        }

        private static LambdaExpression BuildGlobalTenantFilter<TEntity>(ApplicationDbContext applicationDbContext) where TEntity : class, ITenatEntity 
        {
            Expression<Func<TEntity, bool>> filter = tenatEntity => tenatEntity.TenatId.Equals(applicationDbContext._tenantId);

            return filter;
        }
    }
}
