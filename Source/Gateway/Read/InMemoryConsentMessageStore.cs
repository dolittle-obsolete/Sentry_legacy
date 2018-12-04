/*---------------------------------------------------------------------------------------------
 *  Copyright (c) Dolittle. All rights reserved.
 *  Licensed under the MIT License. See LICENSE in the project root for license information.
 *--------------------------------------------------------------------------------------------*/
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Read
{
    /// <summary>
    /// An in-memory representation of <see cref="IConsentMessageStore"/>
    /// </summary>
    /// <remarks>
    /// The default store in IdentityServer uses cookies - with limits on size of cookie per request
    /// you can easily get over with an extra consent cookie - which is really not needed in the browser
    /// You can test and read more here: http://browsercookielimits.squawky.net 
    /// 
    /// The downside of it being in-memory is if the instance goes down 
    /// </remarks>
    public class InMemoryConsentMessageStore : IConsentMessageStore
    {
        static readonly Dictionary<string, Message<ConsentResponse>> _consents = new Dictionary<string, Message<ConsentResponse>>();

        /// <inheritdoc/>
        public Task DeleteAsync(string id)
        {
            _consents.Remove(id);
            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task<Message<ConsentResponse>> ReadAsync(string id)
        {
            Message<ConsentResponse> result;
            if( !_consents.ContainsKey(id)) result = null;
            else result = _consents[id];
            return Task.FromResult(result);
        }

        /// <inheritdoc/>
        public Task WriteAsync(string id, Message<ConsentResponse> message)
        {
            _consents[id] = message;
            return Task.CompletedTask;
        }
    }
}