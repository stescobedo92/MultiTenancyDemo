using Microsoft.AspNetCore.Identity;
using MultiTenancyDemo.Entities;

namespace MultiTenancyDemo.Services
{
    public static class TenatValidationExtention
    {
        public static bool SkipTenantValidation(this Type type)
        {
            List<bool> bools = new()
            {
                type.IsAssignableFrom(typeof(IdentityRole)),
                type.IsAssignableFrom(typeof(IdentityRoleClaim<string>)),
                type.IsAssignableFrom(typeof(IdentityUser)),
                type.IsAssignableFrom(typeof(IdentityUserLogin<string>)),
                type.IsAssignableFrom(typeof(IdentityUserRole<string>)),
                type.IsAssignableFrom(typeof(IdentityUserToken<string>)),
                type.IsAssignableFrom(typeof(IdentityUserClaim<string>)),
                typeof(ICommonEntity).IsAssignableFrom(type)
            };

            return bools.Aggregate((boolean1, boolean2) => boolean1 || boolean2);
        }
    }
}
