using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Stripe;
using CapstoneCustomerRelationsSystem.TechnicalServices;
using Microsoft.AspNetCore.Routing;

namespace CapstoneCustomerRelationsSystem
{
    public class Startup
    {
        public static string CRSConnectionString { get; set; }
        private Configurations ConfigurationsManager = new Configurations();
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration _configuration) {
            Configuration = _configuration;
        }//end Startup

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            CRSConnectionString = ConfigurationsManager.GetConfiguration("ConnectionStrings", "CRS Local");
            //CRSConnectionString = ConfigurationsManager.GetConfiguration("ConnectionStrings", "CRS Remote");

            services.Configure<CookiePolicyOptions>(options => {
                 options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;
            });
        
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(_cookieOptions => {
                    _cookieOptions.LoginPath = "/Login";
                    _cookieOptions.LogoutPath = "/Logout";
                    _cookieOptions.SlidingExpiration = true;
                    _cookieOptions.AccessDeniedPath = "/Forbidden";
                    _cookieOptions.ExpireTimeSpan = TimeSpan.FromDays(2);
                    _cookieOptions.Cookie.HttpOnly = true;
            });

            services.AddAuthorization(_options => {
                _options.AddPolicy("RequireAdmin", _y => _y
                    .RequireRole("Admin"));
                _options.AddPolicy("RequiredSignedInUser", _y => _y
                    .RequireAuthenticatedUser());
            });

            services.AddRazorPages(_x =>
            _x.Conventions.AuthorizeFolder("/Common/Admin", "RequireAdmin"));

            services.Configure<RouteOptions>(options => {
                options.LowercaseUrls = true;
            });

        }//end ConfigureServices

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            StripeConfiguration.ApiKey = Configuration.GetSection("Stripe")["SecretKey"];


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
            });
        }
    }
}