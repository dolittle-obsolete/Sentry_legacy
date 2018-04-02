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

namespace Infrastructure
{
    /// <summary>
    /// 
    /// </summary>
    public class ResourceStore : IResourceStore
    {
        readonly IContainer _container;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        public ResourceStore(IContainer container)
        {
            _container = container;
        }

        /// <inheritdoc/>
        public Task<ApiResource> FindApiResourceAsync(string name)
        {
            var authContext = _container.Get<AuthContext>();
            var apiResource = authContext.Application.ApiResources.SingleOrDefault(_ => _.Name == name);
            if( apiResource == null ) return Task.FromResult((ApiResource)null);
            return Task.FromResult(ConvertToResource<ApiResource>(apiResource));
        }

        /// <inheritdoc/>
        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var authContext = _container.Get<AuthContext>();
            var filtered = authContext.Application.ApiResources?.Where(_ => scopeNames.Contains(_.Name.Value)) ?? new Read.Management.Resource[0];
            var apiResources = filtered.Select(_ => ConvertToResource<ApiResource>(_));
            return Task.FromResult(apiResources);
        }

        /// <inheritdoc/>
        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var authContext = _container.Get<AuthContext>();

            var identityResources = GetIdentityResourcesFrom(authContext);
            var filtered = identityResources.Where(_ => scopeNames.Contains(_.Name));
            return Task.FromResult(identityResources);
        }

        /// <inheritdoc/>
        public Task<Resources> GetAllResourcesAsync()
        {
            var authContext = _container.Get<AuthContext>();

            var apiResources = authContext.Application.ApiResources?.Select(_ => ConvertToResource<ApiResource>(_)) ?? new ApiResource[0];
            var identityResources = GetIdentityResourcesFrom(authContext);
            var resources = new Resources(identityResources, apiResources);

            return Task.FromResult(resources);
        }

        private IEnumerable<IdentityResource> GetIdentityResourcesFrom(AuthContext authContext)
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

        T ConvertToResource<T>(Read.Management.Resource resource)where T : Resource, new()
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