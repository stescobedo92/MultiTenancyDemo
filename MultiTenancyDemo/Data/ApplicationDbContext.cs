using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MultiTenancyDemo.Services;

namespace MultiTenancyDemo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        private readonly string _tenantId;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ITenantService tenantService): base(options)
        {
            _tenantId = tenantService.GetTenantId();
        }
    }
}
