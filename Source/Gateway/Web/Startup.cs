/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.DependencyInversion.Autofac;
using IdentityServer4;
using IdentityServer4.Stores;
using Infrastructure;
using Infrastructure.Resources;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Web
{
    /// <summary>
    /// 
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

            // Todo: RSA Signing Key for Dolittle as Authority

            services.AddCors();

            services.AddMvc();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/accounts/login";
                    options.UserInteraction.LogoutUrl = "/accounts/logout";
                    options.UserInteraction.ConsentUrl = "/accounts/consent";
                    //options.Authentication.CheckSessionCookieName = "sentry.session";
                })

                // Todo: We need our own signing key
                //.AddSigningCredential(credentials)
                .AddDeveloperSigningCredential()

                // Todo: Persist grants per application for the unique user
                .AddInMemoryPersistedGrants()
                .AddResourceStore<ResourceStore>()
                .AddClientStore<ClientStore>()
                .AddProfileService<ProfileService>();

            services.Add(new ServiceDescriptor(typeof(IConsentMessageStore), typeof(InMemoryConsentMessageStore), ServiceLifetime.Transient));

            services.AddAuthentication()
                // Todo: b2c should have its own provider type, since it has a bunch of different policy things attached to it for signup and signin

                .AddOpenIdConnect("9b296977-7657-4bc8-b5b0-3f0a23c43958", "Azure Active Directory", options =>
                {
                    options.CallbackPath = "/9b296977-7657-4bc8-b5b0-3f0a23c43958/signin-oidc";
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                    options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                    options.Authority = "[Not Set]";
                    options.ClientId = "[Not Set]";

                    // Todo: dynamic scope based on tenant configuration - ask for the same as configured with
                    options.Scope.Clear();
                    options.Scope.Add(IdentityServerConstants.StandardScopes.OpenId);
                    options.Scope.Add(IdentityServerConstants.StandardScopes.Email);
                    options.Scope.Add(IdentityServerConstants.StandardScopes.Profile);

                    options.ConfigurationManager = new CustomOpenIdConfigurationManager();

                    options.Events.OnRedirectToIdentityProvider = async(context) =>
                    {
                        var query = context.HttpContext.Request.Query;
                        var authorityId = Guid.Parse(query["authorityid"]);
                        var tenantId = Guid.Parse(query["tenant"]);
                        var applicationName = query["application"];

                        var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                        var tenant = tenantConfiguration.GetFor(tenantId);
                        var application = tenant.Applications[applicationName];

                        var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                        context.Options.TokenValidationParameters.ValidAudience = authority.ClientId;                  

                        // Todo: Dynamically create a new instance of OpenIdConnectMessage for the actual configuration - this could potentially fall over with multiple users accessing the system at once
                        context.ProtocolMessage.ClientId = authority.ClientId;
                        // Todo: rename autority.Secret to client secret
                        //context.ProtocolMessage.ClientSecret = authority.Secret;

                        // Todo: Guide on setting up an app for Azure - include this https://apps.dev.microsoft.com/

                        if (!_hostingEnvironment.IsDevelopment()) context.ProtocolMessage.RedirectUri = $"https://dolittle.online{options.CallbackPath}";
                        await Task.CompletedTask;
                    };

                    
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // Todo: We should validate everything!!
                        ValidateIssuer = false,

                        // Todo: set the correct claim types depending on the target - Azure AD has a different claim for name than others
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
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Todo: this probably is a bit too lose.. 
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());

            app.UseMiddleware<AuthContextMiddleware>();

            app.Use(async(context, next) =>
            {
                var query = context.Request.Query;
                if (query.ContainsKey("tenant") && query.ContainsKey("application") && query.ContainsKey("authority"))
                {
                    var authorityId = Guid.Parse(query["authorityid"]);
                    var tenantId = Guid.Parse(query["tenant"]);
                    var applicationName = query["application"];

                    var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                    var tenant = tenantConfiguration.GetFor(tenantId);
                    var application = tenant.Applications[applicationName];
                    var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                    var url = $"{authority.Authority}/.well-known/openid-configuration";
                    CustomOpenIdConfigurationManager.url = url;
                }

                await next();
            });
            app.UseIdentityServer();

            app.UseMvc();

            app.UseDolittle();

            app.RunAsSinglePageApplication();
        }
    }
}
