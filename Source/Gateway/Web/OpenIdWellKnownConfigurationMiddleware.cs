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
            if (QueryHasRequiredParameters(query))
            {
                var authorityId = Guid.Parse(query[OpenIdConnectConfiguration.AuthorityIdQueryParameter]);
                var tenantId = Guid.Parse(query[OpenIdConnectConfiguration.TenantIdQueryParameter]);
                var applicationName = query[OpenIdConnectConfiguration.ApplicationNameQueryParameter];

                var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                var tenant = tenantConfiguration.GetFor(tenantId);
                var application = tenant.Applications[applicationName];
                var authority = application.ExternalAuthorities.Single(_ => _.Id == authorityId);
                var url = GetWellKnownOpenIdConfigurationUrl(authority.Authority);

                CustomOpenIdConfigurationManager.url = url;
            }

            await _next(context);
        }
        bool QueryHasRequiredParameters(IQueryCollection query)
        {
            return query.ContainsKey(OpenIdConnectConfiguration.TenantIdQueryParameter) 
                && query.ContainsKey(OpenIdConnectConfiguration.ApplicationNameQueryParameter) 
                && query.ContainsKey(OpenIdConnectConfiguration.AuthorityIdQueryParameter);
        }
        string GetWellKnownOpenIdConfigurationUrl(string authority)
        {
            return $"{authority}/.well-known/openid-configuration";
        }
    }
}