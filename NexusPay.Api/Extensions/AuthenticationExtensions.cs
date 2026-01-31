using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;

namespace NexusPay.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        //TODO: make sure strong types options are used
        public static IServiceCollection AddGoogleAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax; // Default is Lax, but good to be explicit
                    //options.ExpireTimeSpan = TimeSpan.FromMinutes(60);     // Session timeout
                    //options.SlidingExpiration = true;                      // Extend on activity
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"]!;
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"]!;
                });

            // Later:
            // auth.AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));

            services.AddAuthorization();

            return services;
        }
    }
}
