
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;

namespace MultiTenancyDemo.Services
{
    public class TenantService : ITenantService
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public TenantService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string GetTenantId()
        {
            var httpContext = _contextAccessor.HttpContext;

            if (httpContext is null)
                return string.Empty;

            var authTicket = DecryptAuthCookie(httpContext);

            if (authTicket is null)
                return string.Empty;

            var claimTenant = authTicket.Principal.Claims.FirstOrDefault(authTick => authTick.Type == TenantConstants.CLAIM_TENANT_ID);

            return claimTenant is null ? string.Empty : claimTenant.Value;
        }

        private static AuthenticationTicket? DecryptAuthCookie(HttpContext httpContext)
        {
            var opt = httpContext.RequestServices
                .GetRequiredService<IOptionsMonitor<CookieAuthenticationOptions>>()
                .Get("Identity.Application");

            var cookie = opt.CookieManager.GetRequestCookie(httpContext, opt.Cookie.Name!);

            return opt.TicketDataFormat.Unprotect(cookie);
        }
    }
}
