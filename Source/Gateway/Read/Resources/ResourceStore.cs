/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Read.Resources
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceStore : IResourceStore
    {
        readonly IAuthContext _authContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authContext"></param>
        public ResourceStore(IAuthContext authContext)
        {
            _authContext = authContext;
        }

        /// <inheritdoc/>
        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var apiResource = _authContext.Application.ApiResources.SingleOrDefault(_ => _.Name == name);
            if( apiResource == null ) return Task.FromResult((ApiResource)null);
            return Task.FromResult(apiResource);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var apiResources = _authContext.Application.ApiResources?.Where(_ => scopeNames.Contains(_.Name)) ?? new ApiResource[0];
            return Task.FromResult(apiResources);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = GetIdentityResourcesFrom(_authContext);
            var filtered = identityResources.Where(_ => scopeNames.Contains(_.Name));
            return Task.FromResult(identityResources);
        }

        /// <inheritdoc/>
        public Task<IdentityServer4.Models.Resources> GetAllResourcesAsync()
        {
            var apiResources = _authContext.Application.ApiResources ?? new ApiResource[0];
            var identityResources = GetIdentityResourcesFrom(_authContext);
            var resources = new IdentityServer4.Models.Resources(identityResources, apiResources);

            return Task.FromResult(resources);
        }

        private IEnumerable<IdentityResource> GetIdentityResourcesFrom(IAuthContext authContext)
        {
            var wellKnownIdentityResourceTypes = typeof(IdentityResources).GetNestedTypes();

            var identityResources = authContext.Application.WellKnownIdentityResources.Select(_ =>
            {
                var resourceName = _.ToLowerInvariant();
                var resourceType = wellKnownIdentityResourceTypes.SingleOrDefault(resource => resourceName == resource.Name.ToLowerInvariant());
                if (resourceType == null) return new IdentityResource(resourceName, new string[] { resourceName });
                return Activator.CreateInstance(resourceType) as IdentityResource;
            }).ToList();

            var resources = authContext.Application.IdentityResources ?? new IdentityResource[0];
            identityResources.AddRange(resources);

            return identityResources;
        }

        T ConvertToResource<T>(Resource resource)where T : IdentityServer4.Models.Resource, new()
        {
            return new T()
            {
                Name = resource.Name,
                    DisplayName = resource.DisplayName,
                    Description = resource.Description,
                    UserClaims = resource.UserClaims.Select(_ => _.Value).ToList()
            };
        }

    }
}