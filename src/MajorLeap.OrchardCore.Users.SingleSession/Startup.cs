using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using OrchardCore.Modules;
using OrchardCore.Users.Services;
using System;
using Microsoft.AspNetCore.Authentication;
using OrchardCore.Users;
using MajorLeap.OrchardCore.Users.SingleSession.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace MajorLeap.OrchardCore.Users.SingleSession
{
    public class Startup : StartupBase
    {
        internal const string _MODULE_ID = "MajorLeap.OrchardCore.Users.SingleSession";
        internal const string _MODULE_PREFIX = "ML_SingleSession_";
        internal const string _SESSION_ID_KEY = $"{_MODULE_PREFIX}_SessionId";

        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IUserClaimsProvider, SessionInformationClaimsProvider>();

            services.ConfigureApplicationCookie(options =>
            {
                var oldEventHandler = options.Events?.OnValidatePrincipal;
                if(options.Events == null)
                    options.Events = new CookieAuthenticationEvents();
                options.Events.OnValidatePrincipal = async context =>
                {
                    var principal = context.Principal;
                    if (principal?.Identity?.IsAuthenticated ?? false)
                    {
                        var distributedCache = context.HttpContext.RequestServices.GetService<IDistributedCache>() ?? throw new InvalidOperationException($"Couldn't resolve service {nameof(IDistributedCache)}");
                        var storedSessionId = await distributedCache.GetStringAsync(_SESSION_ID_KEY);
                        var claimsSessionId = principal.FindFirst(_SESSION_ID_KEY)?.Value;
                        if (storedSessionId == null || claimsSessionId == null || storedSessionId != claimsSessionId)
                        {
                            //code taken from ValidateAsync(CookieValidatePrincipalContext context) in https://source.dot.net/#Microsoft.AspNetCore.Identity/SecurityStampValidator.cs,135
                            context.RejectPrincipal();
                            var signInManager = context.HttpContext.RequestServices.GetService<SignInManager<IUser>>() ?? throw new InvalidOperationException($"Couldn't resolve service {nameof(SignInManager<IUser>)}");
                            await signInManager.SignOutAsync();
                            await signInManager.Context.SignOutAsync(IdentityConstants.TwoFactorRememberMeScheme);
                        }
                    }

                    if (oldEventHandler != null)
                        await oldEventHandler(context);
                };
            });
        }

        public override void Configure(IApplicationBuilder builder, IEndpointRouteBuilder routes, IServiceProvider serviceProvider)
        {
            /*routes.MapAreaControllerRoute(
                name: "Home",
                areaName: _MODULE_ID,
                pattern: "Home/Index",
                defaults: new { controller = "Home", action = "Index" }
            );*/
        }
    }
}
