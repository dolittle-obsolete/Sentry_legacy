using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Web
{
    /// <summary>
    /// 
    /// </summary>
    public class OpenIdWellKnownConfigurationMiddleware
    {
        
        /// <summary>
        /// 
        /// </summary>
        public const string AuthorityIdQueryParameter = "authorityid";
        /// <summary>
        /// 
        /// </summary>
        public const string TenantIdQueryParameter = "tenant";
        /// <summary>
        /// 
        /// </summary>
        public const string ApplicationNameQueryParameter = "application";

        readonly RequestDelegate _next;
        readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="next"></param>
        /// <param name="serviceProvider"></param>
        public OpenIdWellKnownConfigurationMiddleware(
            RequestDelegate next,
            IServiceProvider serviceProvider)
        {            
            _next = next;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The method that gets invoked by the ASP.NET pipeline
        /// </summary>
        /// <param name="context"><see cref="HttpContext"/> for the request</param>
        public async Task Invoke(HttpContext context)
        {
            var query = context.Request.Query;
            if (query.ContainsKey("tenant") && query.ContainsKey("application") && query.ContainsKey("authority"))
            {
                var authorityId = Guid.Parse(query[AuthorityIdQueryParameter]);
                var tenantId = Guid.Parse(query[TenantIdQueryParameter]);
                var applicationName = query[ApplicationNameQueryParameter];

                var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                var tenant = tenantConfiguration.GetFor(tenantId);
                var application = tenant.Applications[applicationName];
                var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                var url = GetWellKnownOpenIdConfigurationUrl(authority.Authority);
                
                CustomOpenIdConfigurationManager.url = url;
            }

            await _next(context);
        }

        string GetWellKnownOpenIdConfigurationUrl(string authority)
        {
            return $"{authority}/.well-known/openid-configuration";
        }
    }
}