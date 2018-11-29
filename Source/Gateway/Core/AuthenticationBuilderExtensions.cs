using System;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class AuthenticationBuilderExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authBuilder"></param>
        /// <param name="hostingEnvironment"></param>
        /// <returns></returns>
        public static AuthenticationBuilder AddSentryOpenIdConnect(this AuthenticationBuilder authBuilder, IHostingEnvironment hostingEnvironment)
        {
            authBuilder.AddOpenIdConnect(OpenIdConnectConfiguration.AuthenticationScheme, OpenIdConnectConfiguration.DisplayName, 
                OpenIdConnectConfiguration.GetOptionsCallback(hostingEnvironment));
            return authBuilder;
        }
    }
}