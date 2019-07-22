using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using Autofac;
using Dolittle.AspNetCore.Bootstrap;
using Dolittle.Booting;
using Dolittle.DependencyInversion.Autofac;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
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
            
            JwtSecurityTokenHandler.DefaultInboundClaimFilter.Clear();
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // Todo: understand anti forgery
            //services.AddAntiforgery();

            // Todo: RSA Signing Key for Dolittle as Authority

            services.AddCors();            
            services.AddMvc();

            // /api/508c1745-5f2a-4b4c-b7a5-2fbb1484346d/Studio/accounts/login
            /*
            services.AddIdentityServer(options => {
                    options.UserInteraction.LoginUrl = "/accounts/login";
                    options.UserInteraction.LogoutUrl = "/accounts/logout";
                    options.UserInteraction.DeviceVerificationUrl = "/device/verify";
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
            */

            services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseSuccessEvents = true;
            })
                .AddCustomTokenRequestValidator<AuthenticationMiddleware.TokenRequestValidator>()
                .AddInMemoryApiResources(Config.GetApis())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryClients(Config.GetClients())
                //.AddTestUsers(TestUsers.Users)
                .AddDeveloperSigningCredential(persistKey: false);


            services
                .AddAuthentication()
                .AddCookie("Dolittle")
                .AddOpenIdConnect("AzureAD",options => {
                    options.Authority = "https://login.microsoftonline.com/381088c1-de08-4d18-9e60-bbe2c94eccb5/v2.0";
                    options.ClientId = "0d10f319-ec64-4df3-aa14-ea15f7c9bbd9";
                    options.Scope.Add("profile");
                    options.SignInScheme = "Dolittle";
                    options.TokenValidationParameters.ValidIssuer = "https://edge.dolittle.local";
                    options.Events.OnRedirectToIdentityProvider = async (context) => {
                        context.ProtocolMessage.RedirectUri = "https://edge.dolittle.local/signin-oidc";
                        await Task.CompletedTask;
                    };
                    
                })
                .AddJwtBearer("Bearer", options =>
                {
                    //options.Authority = "https://demo.identityserver.io";
                    options.Authority = "https://edge.dolittle.local";
                    options.Audience = "api";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters.ValidIssuer = "https://edge.dolittle.local";
                });
            
            //services.AddSentryAuthentication(_hostingEnvironment);
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

            // Todo: Why do we need to do this?
            IdentityModelEventSource.ShowPII = true;    
            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Todo: this probably is a bit too lose.. 
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .AllowCredentials());
            
            //app.UseMiddleware<AuthContextMiddleware>();
            //app.UseMiddleware<OpenIdWellKnownConfigurationMiddleware>();
            app.UseIdentityServer();

            app.UseAuthentication();

            app.UseMvc();

            app.UseDolittle();
        }
    }
}