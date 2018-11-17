using Concepts.Scopes;
using Dolittle.Domain;
using Dolittle.Runtime.Events;
using Events.Gateway.Registration;
using System;
using System.Collections.Generic;

namespace Domain.Gateway.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class Registration : AggregateRoot
    {
        /// <summary>
        /// Initializes an instance of <see cref="Registration"/>
        /// </summary>
        public Registration(EventSourceId id) : base(id)
        {
            
        }

        /// <summary>
        /// Grants consent
        /// </summary>
        /// <param name="scopes"></param>
        /// <param name="returnUrl"></param>
        /// <param name="rememberConsent"></param>
        public void GrantConsent(IEnumerable<string> scopes, string returnUrl, bool rememberConsent)
        {
            Apply(new UserConsented(scopes, returnUrl, rememberConsent));
        }
    }
}