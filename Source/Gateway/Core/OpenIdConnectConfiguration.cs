using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Concepts.Authorities;
using Dolittle.Collections;
using IdentityServer4;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Read.Tenants;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenIdConnectConfiguration
    {

        /// <summary>
        /// 
        /// </summary>
        public static IServiceProvider ServiceProvider {get; set;}

        /// <summary>
        /// 
        /// </summary>
        public const string AuthenticationScheme = "9b296977-7657-4bc8-b5b0-3f0a23c43958";
        /// <summary>
        /// 
        /// </summary>
        public const string DisplayName = "Azure Active Directory";
        /// <summary>
        /// 
        /// </summary>
        public const string Authority = "[Not Set]";
        /// <summary>
        /// 
        /// </summary>
        public const string ClientId = "[Not Set]";
        /// <summary>
        /// The <see cref="AuthorityId"/>
        /// </summary>
        public const string AuthorityIdQueryParameter = "authorityId";
        /// <summary>
        /// The <see cref="AuthorityType"/>
        /// </summary>
        public const string AuthorityQueryParameter = "authority";
        /// <summary>
        /// 
        /// </summary>
        public const string TenantIdQueryParameter = "tenant";
        /// <summary>
        /// 
        /// </summary>
        public const string ApplicationNameQueryParameter = "application";

        readonly static string CallbackPath = $"/{AuthenticationScheme}/signin-oidc";

        static readonly TokenValidationParameters _validationParameters = new TokenValidationParameters
        {
            // Todo: We should validate everything!!
            ValidateIssuer = false,

            // Todo: set the correct claim types depending on the target - Azure AD has a different claim for name than others
            NameClaimType = "name",
            RoleClaimType = "role"
        };

        static readonly ICollection<string> _defaultOpenIdScopes = new string[] 
        {
             IdentityServerConstants.StandardScopes.OpenId,
             IdentityServerConstants.StandardScopes.Email,
             IdentityServerConstants.StandardScopes.Profile
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static Action<OpenIdConnectOptions> GetOptionsCallback(IHostingEnvironment hostingEnvironment)
        {
            return options =>
            {
                options.CallbackPath = CallbackPath;
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.SignOutScheme = IdentityServerConstants.SignoutScheme;

                options.Authority = Authority;
                options.ClientId = ClientId;

                // Todo: dynamic scope based on tenant configuration - ask for the same as configured with
                options.Scope.Clear();
                _defaultOpenIdScopes.ForEach(options.Scope.Add);

                options.ConfigurationManager = new CustomOpenIdConfigurationManager();

                options.Events.OnRedirectToIdentityProvider = GetRedirectToIdentityProviderCallback(ServiceProvider, hostingEnvironment);
                options.TokenValidationParameters = _validationParameters;
            };
        }

        static Func<RedirectContext, Task> GetRedirectToIdentityProviderCallback(IServiceProvider serviceProvider, IHostingEnvironment hostingEnvironment)
        {
            return async(context) =>
            {
                var query = context.HttpContext.Request.Query;
                var authorityId = Guid.Parse(query[AuthorityIdQueryParameter].FirstOrDefault());
                var tenantId = Guid.Parse(query[TenantIdQueryParameter].FirstOrDefault());
                var applicationName = query[ApplicationNameQueryParameter].FirstOrDefault();

                var tenantConfiguration = serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                var tenant = tenantConfiguration.GetFor(tenantId);
                var application = tenant.Applications[applicationName];

                var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                context.Options.TokenValidationParameters.ValidAudience = authority.ClientId;                  

                // Todo: Dynamically create a new instance of OpenIdConnectMessage for the actual configuration - this could potentially fall over with multiple users accessing the system at once
                context.ProtocolMessage.ClientId = authority.ClientId;
                // Todo: rename autority.Secret to client secret
                //context.ProtocolMessage.ClientSecret = authority.Secret;

                // Todo: Guide on setting up an app for Azure - include this https://apps.dev.microsoft.com/

                if (!hostingEnvironment.IsDevelopment()) context.ProtocolMessage.RedirectUri = $"https://dolittle.online{CallbackPath}";
                await Task.CompletedTask;
            };
        }
    }
}