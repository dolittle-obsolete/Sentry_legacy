/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.DependencyInversion.Autofac;
using Dolittle.Runtime.Applications.Serialization.Json;
using Dolittle.Runtime.Events.Coordination;
using Dolittle.Serialization.Json;
using IdentityServer4;
using IdentityServer4.Stores;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.IdentityModel.Tokens;
using Read.Management;
using Swashbuckle.AspNetCore.Swagger;

namespace Web
{
    /// <summary>
    /// 
    /// be4c4da6-5ede-405f-a947-8aedad564b7f - Tenant       - Red Cross
    /// 25c7ddac-dd1b-482a-8638-aaa909fd1f1c - Application  - CBS
    /// 
    /// Authorities:
    /// 9b296977-7657-4bc8-b5b0-3f0a23c43958 - Azure Active Directory
    /// </summary>
    public partial class Startup
    {
        readonly IHostingEnvironment _hostingEnvironment;
        IServiceProvider _serviceProvider;

        BootResult _bootResult;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Todo: understand anti forgery
            //services.AddAntiforgery();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddCors();

            services.AddMvc();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/accounts/login";
                    options.UserInteraction.LogoutUrl = "/accounts/logout";
                    options.UserInteraction.ConsentUrl = "/accounts/consent";
                    //options.Authentication.CheckSessionCookieName = "sentry.session";
                })
                .AddDeveloperSigningCredential()
                .AddInMemoryPersistedGrants()
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddProfileService<ProfileService>();
                

            services.Add(new ServiceDescriptor(typeof(IConsentMessageStore), typeof(InMemoryConsentMessageStore), ServiceLifetime.Transient));

            services.AddAuthentication()
                .AddOpenIdConnect("9b296977-7657-4bc8-b5b0-3f0a23c43958", "Azure Active Directory", options =>
                {
                    options.CallbackPath = "/signin-oidc-9b296977-7657-4bc8-b5b0-3f0a23c43958";
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    // https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols-oidc
                    // https://login.microsoftonline.com/{tenant}/v2.0/.well-known/openid-configuration
                    // https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0/.well-known/openid-configuration

                    // http://localhost:5000/Accounts/ExternalLogin?tenant=be4c4da6-5ede-405f-a947-8aedad564b7f&authority=undefined&returnUrl=%2Fbe4c4da6-5ede-405f-a947-8aedad564b7f%2FCBS%2Fconnect%2Fauthorize%2Fcallback%3Fclient_id%3D25c7ddac-dd1b-482a-8638-aaa909fd1f1c%26redirect_uri%3Dhttp%253A%252F%252Flocalhost%253A5000%252FRegistration%252FRequestAccessOidcCallback%26response_type%3Did_token%26scope%3Dopenid%2520email%2520profile%26state%3Dfcecbea8cd2b47f98ef62b9b5f2d0827%26nonce%3D1c0a46ff32c94b19939801eb80e8152f

                    //options.Authority = "https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0";
                    options.Authority = "https://login.microsoftonline.com/common";
                    options.ClientId = "Blah";
                    //options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                    options.Scope.Clear();
                    options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);

                    options.Events.OnRedirectToIdentityProvider = async(context)=>
                    {
                        var query = context.HttpContext.Request.Query;
                        var authorityId = Guid.Parse(query["authorityid"]);
                        var tenantId = Guid.Parse(query["tenant"]);
                        var applicationName = query["application"];

                        var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                        var tenant = tenantConfiguration.GetFor(tenantId);
                        var application = tenant.Applications[applicationName];
                        
                        var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                        //AzureAdB2COptions.Authority

                        //context.Options.Authority = "https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0";
                        //context.Options.Authority = $"https://login.microsoftonline.com/{authContext}/v2.0";
                        //context.ProtocolMessage.ClientId = "2e2cad73-c11a-4d9f-8af9-beeebcdc5a27";
                        context.Options.Authority = authority.Authority;
                        context.ProtocolMessage.ClientId = authority.ClientId;

                        if( !_hostingEnvironment.IsDevelopment() ) context.ProtocolMessage.RedirectUri = $"https://dolittle.online{options.CallbackPath}";
                        await Task.CompletedTask;
                    };

                    // https://login.microsoftonline.com/dbed8645-9f1f-4ccc-9e61-4e7f92e2a27d/oauth2/v2.0/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&scope=openid%20email&response_type=id_token
                    // https://login.microsoftonline.com/common/oauth2/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2Fsignin-oidc-9b296977-7657-4bc8-b5b0-3f0a23c43958&response_type=id_token&scope=openid&response_mode=form_post&nonce=636584610907046420.YTBmNjNmZDYtZDA5Yy00ODc0LThmYzYtNzA1MTM5MDdkY2I1NWE5NDI4YzQtZjQ4NC00MTQ0LTk1NWYtZmM0YTNkZTRkNjcz&state=CfDJ8CeXhV5JBLFMkMDAcLC1jYJ0JbP1AHhNgbBRK8uDeBXs8aAAbmg4xcJqOO3Ee66CuCzyS88FDJjAI7C6ik2wa_lBP4F0uOwoB9NkFZF92suDklX2PZw9WNXgGKhHtK0HrE15Z2Y96y7S8F25isk0CP8--EDkIT2viijS8hlMl0nR_xb8IJBZYbn30GWHp_iRYkxmmFABuJtPHX79YA6U7p8HTbJQvHZkXCoUyu_L9d4GjLDoXItDBRSHZ0ZFVopyF8DJLMacuA6ox85DuAzKnZ8odF8GasMhvIlRqQiyQ5kBsczn120VOh2JtCELP-a7w5TIP-pf6LpWk4zdGEbem1taVvnsMTEESRPBheX1Yl8vKnHh1BOYFe0eWNspVemV3D3x23OCI3ON_oVmD9oG7x2xpJ5oP71h9dKCI4NoyL1dPQ3bKw06xQn6Qol-fZxXszbw2GJhCKGwz26Rf_HtSR8Ur4hKjbVoSBzqBlHDmOkuQagGuD8PJbUzd2fgANrxFxIe4tWc9EPtEuPigbNRfGY_E5vIOrG3rv2OyLVhDCNoJi8FVOIM9czYYMYXHdRkjq0vQzwwwjQIjOep_dMjeR3NcxShGHDcXuikoQ_JapHSyMw-VAmhWFMNYCMkka4HpgiD35PYSzpDJ39NvS1G8TUAgPJahhLRotdwG26WVKP3sMY_hmyrfMH4-lXCo7r7F02BHrWNNwr4xuA5JxQlBLE8xJARecqno6zqPWCLOoqafDs450HqhfHekP3AjvSSzlvHAG89XEWBl8s8ViBj3mGh0uMK6Re1fHX-kEX8q0pv5a4UOSR38UdAT6Cju4RRML5Xj6x19vfRw69sfNRtYQkTmtgvBvjbBoazQ9Z2R1rWGKvsVSbbWrew3lFS-cvqEocQlLALWxAy78AJW0V1EmJbwUf3k9M0uPzpbv4T87WkKMBpa73FQU1yyQJ6-GZReQ&x-client-SKU=ID_NET&x-client-ver=2.1.4.0
                    // https://login.microsoftonline.com/common/oauth2/authorize?client_id=0734a066-658a-47e6-aea5-a036c997b61f&redirect_uri=http%3A%2F%2Flocalhost%3A5000%2Fsignin-oidc-9b296977-7657-4bc8-b5b0-3f0a23c43958&response_type=id_token&scope=openid&response_mode=form_post

                    options.Events.OnTokenResponseReceived = async(context)=>
                    {
                        await Task.CompletedTask;
                    };

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                });

            _bootResult = services.AddDolittle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddDolittle(_bootResult.Assemblies, _bootResult.Bindings);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            _serviceProvider = app.ApplicationServices;

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                });
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            
            app.UseMiddleware<AuthContextMiddleware>();
            app.UseIdentityServer();

            app.UseMvc();
            app.UseDolittle();

            app.RunAsSinglePageApplication();
        }
    }
}