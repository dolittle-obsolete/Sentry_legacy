/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dolittle.DependencyInversion;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Web
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
            return Task.FromResult(ConvertToResource<ApiResource>(apiResource));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var filtered = _authContext.Application.ApiResources?.Where(_ => scopeNames.Contains(_.Name.Value)) ?? new Resource[0];
            var apiResources = filtered.Select(_ => ConvertToResource<ApiResource>(_));
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
        public Task<Resources> GetAllResourcesAsync()
        {
            var apiResources = _authContext.Application.ApiResources?.Select(_ => ConvertToResource<ApiResource>(_)) ?? new ApiResource[0];
            var identityResources = GetIdentityResourcesFrom(_authContext);
            var resources = new Resources(identityResources, apiResources);

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

            var resources = authContext.Application.IdentityResources?.Select(_ => ConvertToResource<IdentityResource>(_)) ?? new IdentityResource[0];
            identityResources.AddRange(resources);

            return identityResources;
        }

        T ConvertToResource<T>(Web.Resource resource)where T : IdentityServer4.Models.Resource, new()
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