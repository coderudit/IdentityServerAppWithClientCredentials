using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MVCApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            /*Authentication handlers are registered with there configuration options which
             *combine to form schemes. eg: Cookies, JwtBearer. These handlers are registered
             * with the help of scheme specific extensions eg: AddCookie. These handlers are
             * then used by the IAuthentication service of Authentication middleware.
             * Authorization policies and attributes can be used to specify which 
             * authorization scheme to be used.
             */
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies"; //Refers to default authentication scheme.
                options.DefaultChallengeScheme = "oidc"; //Refers to default challenge scheme.
            })
                //Implements authentication
                .AddCookie("Cookies")
                //Implements challenge scheme
                .AddOpenIdConnect("oidc",  options =>
                {
                    options.Authority = "https://localhost:5000"; //What OpenId Connect provider do we trust. Here it is Identity Server App.
                    options.RequireHttpsMetadata = false;
                    options.ClientId = "MVCApp";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code id_token"; //Type of token that is needed.
                    options.SaveTokens = true; //Same token can be passed while logging out.
                    options.SignInScheme = "Cookies";
                    options.Scope.Clear();
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("office");
                    options.Scope.Add("email");
                    options.Scope.Add("securecoreapi");
                    options.Scope.Add("offline_access");
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.MapJsonKey("website", "website");
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            /*Use it before Authentication middleware, so that route information is available
              to authentication middleware. */
            app.UseRouting(); 

            //Handled by IAuthenticationService
            app.UseAuthentication();
            app.UseAuthorization();
            
            /*Use it after authentication middleware, so that endpoints can be accessed after
              authentication and authorization.*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
        .RequireAuthorization();
            });
        }
    }
}
