// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServerApp_with_Implicit_flow;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }

        public Startup(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            var builder = services.AddIdentityServer() //Returns a builder object
                .AddInMemoryIdentityResources(Config.Ids) //Identity of user that needs to be protected in case of authentication.Registers IResourceStore.
                .AddInMemoryApiResources(Config.Apis) //API's that needs to be protected in case of authorization.Registers IResourceStore.
                .AddInMemoryClients(Config.Clients); //Client applications registered with idenity server. Registers IClientStore and ICorsPolicyService. 

            // Creates temporary key material at startup time. The generated key will be persisted in the local directory by default.
            builder.AddDeveloperSigningCredential();

            //Adds a signing key that provides the specified key material to 
            //the various token creation/validation services.
            //builder.AddSigningCredential("CN=sts"); //Windows certificate with common name sts.

            //models a user, their credentials, and claims in IdentityServer. 
            //Registers TestUserStore based on a collection of TestUser objects. 
            //TestUserStore is used by the default quickstart UI. Also registers 
            //implementations of IProfileService and IResourceOwnerPasswordValidator.
            builder.AddTestUsers(TestUsers.Users);
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
