using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Read.Infrastructure.Gateway;

namespace Core
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
                var authorityId = Guid.Parse(query[OpenIdConnectConfiguration.AuthorityIdQueryParameter].FirstOrDefault());
                var tenantId = Guid.Parse(query[OpenIdConnectConfiguration.TenantIdQueryParameter].FirstOrDefault());
                var applicationName = query[OpenIdConnectConfiguration.ApplicationNameQueryParameter].FirstOrDefault();

                var tenantConfiguration = _serviceProvider.GetService(typeof(ITenantConfiguration)) as ITenantConfiguration;
                var tenant = tenantConfiguration.GetFor(tenantId);
                var application = tenant.Applications[applicationName];
                var authority = application.ExternalAuthorities.Single(_ => _.Id.Value == authorityId);
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