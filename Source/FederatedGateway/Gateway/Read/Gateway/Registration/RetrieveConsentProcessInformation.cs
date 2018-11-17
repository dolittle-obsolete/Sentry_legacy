using System;
using System.Linq;
using Dolittle.Queries;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Read.Infrastructure.Gateway;

namespace Read.Gateway.Registration
{
    /// <summary>
    /// Represents a query for the Consent Information
    /// </summary>
    public class RetrieveConsentProcessInformation : IQueryFor<ConsentProcessInformation>
    {
        readonly IIdentityServerInteractionService _interaction;
        readonly IClientStore _clientStore;
        readonly IResourceStore _resourceStore;

        /// <summary>
        /// Instantiates an instance of <see cref="RetrieveConsentProcessInformation"/>
        /// </summary>
        /// <param name="interaction"></param>
        /// <param name="clientStore"></param>
        /// <param name="resourceStore"></param>
        public RetrieveConsentProcessInformation(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore
        )
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
        }

        /// <summary>
        /// The return url
        /// </summary>
        public string ReturnUrl {get; set;}

        /// <summary>
        /// The query
        /// </summary>
        public IQueryable<ConsentProcessInformation> Query
        {
            get
            {
                if (string.IsNullOrEmpty(ReturnUrl)) throw new ArgumentException("ReturnUrl must be set", "ReturnUrl");
                var information = new ConsentProcessInformation();

                var request = _interaction.GetAuthorizationContextAsync(ReturnUrl).Result;
                if (request != null)
                {
                    var client = _clientStore.FindEnabledClientByIdAsync(request.ClientId).Result;
                    if (client != null)
                    {
                        var resources = _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested).Result;
                        if (resources != null && (resources.IdentityResources.Any()|| resources.ApiResources.Any()))
                        {
                            information.ClientName = client.ClientName ?? client.ClientId;
                            information.ClientUrl = client.ClientUri;
                            information.ClientLogoUrl = client.LogoUri;
                            information.AllowRememberConsent = client.AllowRememberConsent;
                            information.IdentityScopes = resources.IdentityResources.Select(_ => _.ToScope());
                            information.ResourceScopes = resources.ApiResources.SelectMany(_ => _.Scopes).Select(_ => _.ToScope());
                        }
                        else
                        {
                            information.AddError($"No scopes matching: {request.ScopesRequested.Aggregate((x, y) => x + ", " + y)}");
                        }
                    }
                    else
                    {
                        information.AddError($"Invalid client id: {request.ClientId}");
                    }
                }
                else
                {
                    information.AddError($"No consent request matching request: {ReturnUrl}");
                }

                return new[] {information}.AsQueryable();
            }
        }
    }
}