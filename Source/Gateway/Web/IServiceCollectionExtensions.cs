using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Web
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
        /// <param name="serviceProvider"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static IServiceCollection AddSentryAuthentication(this IServiceCollection service, IServiceProvider serviceProvider, IHostingEnvironment hostingEnvironment)
        {
            service.AddAuthentication()
                .AddSentryOpenIdConnect(serviceProvider, hostingEnvironment);

            return service;
        }
    }
}