using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static IServiceCollection AddSentryAuthentication(this IServiceCollection service, IHostingEnvironment hostingEnvironment)
        {
            service.AddAuthentication()
                .AddSentryOpenIdConnect(hostingEnvironment);

            return service;
        }
    }
}