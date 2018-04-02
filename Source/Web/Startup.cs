/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Dolittle.DependencyInversion.Autofac;
using Dolittle.Runtime.Events.Coordination;
using IdentityServer4;
using IdentityServer4.Hosting;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders.Physical;
using Microsoft.IdentityModel.Tokens;
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IServiceProvider ServiceProvider { get; private set; }

        BootResult _bootResult;

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

            services.AddMvc();

            services.AddIdentityServer(options =>
                {
                    options.UserInteraction.LoginUrl = "/accounts/login";
                    options.UserInteraction.LogoutUrl = "/accounts/logout";
                    options.UserInteraction.ConsentUrl = "/accounts/consent";
                    //options.Authentication.CheckSessionCookieName = "sentry.session";
                })
                .AddDeveloperSigningCredential()
                
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryClients(Config.GetClients())
                .AddInMemoryPersistedGrants()
                .AddProfileService<MyProfileService>();
                

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
                    //options.Authority = "https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0";
                    options.Authority = "https://login.microsoftonline.com/common";
                    options.ClientId = "Blah";
                    options.Scope.Add(IdentityServerConstants.StandardScopes.Email);

                    options.Events.OnRedirectToIdentityProvider = async(context)=>
                    {
                        context.Options.Authority = "https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0";
                        context.ProtocolMessage.ClientId = "2e2cad73-c11a-4d9f-8af9-beeebcdc5a27";
                        await Task.CompletedTask;
                    };

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
        /// Gets the current <see cref="HttpContext"/>
        /// </summary>
        public static HttpContext HttpContext => ServiceProvider.GetService<IHttpContextAccessor>().HttpContext;
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            ServiceProvider = app.ApplicationServices;

            var committedEventStreamCoordinator = app.ApplicationServices.GetService(typeof(ICommittedEventStreamCoordinator))as ICommittedEventStreamCoordinator;
            committedEventStreamCoordinator.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMiddleware<TenantMiddleware>();
            app.UseIdentityServer();

            app.UseMvc();

            // Keep this last as this is the fallback when nothing else works - spit out the index file           
            app.Run(async context =>
            {
                if( Path.HasExtension(context.Request.Path)) await Task.CompletedTask;
                context.Request.Path = new PathString("/");
                var path = $"{env.ContentRootPath}/wwwroot/index.html";
                await context.Response.SendFileAsync(new PhysicalFileInfo(new FileInfo(path)));
            });            
        }
    }
}