using Autofac;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.Bootstrapping;
using Dolittle.DependencyInversion.Autofac;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Read;
using Read.Resources;

namespace Core
{
    public partial class Startup
    {
        readonly IHostingEnvironment _hostingEnvironment;
        readonly ILoggerFactory _loggerFactory;
        BootloaderResult _bootResult;

        public Startup(IHostingEnvironment hostingEnvironment, ILoggerFactory loggerFactory)
        {
            _hostingEnvironment = hostingEnvironment;
            _loggerFactory = loggerFactory;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Todo: understand anti forgery
            //services.AddAntiforgery();

            // Todo: RSA Signing Key for Dolittle as Authority

            services.AddCors();
            
            services.AddMvc();
            
            services.AddIdentityServer(options => {
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
            
            services.AddSentryAuthentication(_hostingEnvironment);            
            
            _bootResult = services.AddDolittle(_loggerFactory);
        }


        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.AddDolittle(_bootResult.Assemblies, _bootResult.Bindings);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            
            OpenIdConnectConfiguration.ServiceProvider = app.ApplicationServices;
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
            app.UseMiddleware<OpenIdWellKnownConfigurationMiddleware>();

            app.UseIdentityServer();
            app.UseMvc();

            app.UseDolittle();

            app.RunAsSinglePageApplication();
        }
    }
}