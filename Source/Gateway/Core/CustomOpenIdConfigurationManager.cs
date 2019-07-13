/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Dolittle.Collections;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Core
{
    // Todo: Support all the different endpoints types for Azure AD as options : https://docs.microsoft.com/en-us/azure/active-directory/develop/active-directory-v2-protocols-oidc

    /// <summary>
    /// 
    /// </summary>
    public class CustomOpenIdConfigurationManager : IConfigurationManager<Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration>
    {
        static ConcurrentDictionary<string, Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration> _configurations = new ConcurrentDictionary<string, Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration>();

        /// <summary>
        /// 
        /// </summary>
        public static string url = "https://login.microsoftonline.com/common/.well-known/openid-configuration";

        /// <inheritdoc/>
        public async Task<Microsoft.IdentityModel.Protocols.OpenIdConnect.OpenIdConnectConfiguration> GetConfigurationAsync(CancellationToken cancel)
        {
            if (_configurations.ContainsKey(url)) return _configurations[url];
            var configuration = await OpenIdConnectConfigurationRetriever.GetAsync(url, cancel);
            _configurations[url] = configuration;
            return configuration;
        }
        

        /// <inheritdoc/>
        public void RequestRefresh()
        {
            _configurations.Keys.ForEach(url =>
            {
                OpenIdConnectConfigurationRetriever.GetAsync(url, CancellationToken.None).ContinueWith(result =>
                {
                    _configurations[url] = result.Result;
                });
            });
        }
    }
}
