/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Read.Infrastructure.Gateway
{
    /// <summary>
    /// 
    /// </summary>
    public class PersistedGrantStore : IPersistedGrantStore
    {
        /// <inheritdoc/>
        public Task<IEnumerable<PersistedGrant>> GetAllAsync(string subjectId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<PersistedGrant> GetAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RemoveAllAsync(string subjectId, string clientId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RemoveAllAsync(string subjectId, string clientId, string type)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task RemoveAsync(string key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Task StoreAsync(PersistedGrant grant)
        {
            throw new NotImplementedException();
        }
    }
}