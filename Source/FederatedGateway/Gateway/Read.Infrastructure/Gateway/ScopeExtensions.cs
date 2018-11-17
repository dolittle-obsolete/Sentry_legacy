/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using Concepts.Scopes;

namespace Read.Infrastructure.Gateway
{
    /// <summary>
    /// Represents extensions <see cref="Scope">scopes</see>
    /// </summary>
    public static class ScopeExtensions
    {
        /// <summary>
        /// Convert from <see cref="IdentityServer4.Models.IdentityResource"/> to <see cref="Scope"/>
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Scope ToScope(this IdentityServer4.Models.IdentityResource identity)
        {
            return new Scope
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required
            };
        }

        /// <summary>
        /// Convert from <see cref="IdentityServer4.Models.Scope"/> to <see cref="Scope"/>
        /// </summary>
        /// <param name="scope"></param>
        /// <returns></returns>
        public static Scope ToScope(this IdentityServer4.Models.Scope scope)
        {
            return new Scope
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required
            };
        }
    }
}