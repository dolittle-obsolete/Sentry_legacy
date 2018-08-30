using System;
using System.Collections.Generic;
using Concepts.Scopes;
using Dolittle.Domain;
using Events.Consents;
namespace Domain.Registration
{
    /// <summary>
    /// 
    /// </summary>
    public class Registration : AggregateRoot
    {
        /// <summary>
        /// 
        /// </summary>
        public Registration(Guid id) : base(id)
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="application"></param>
        /// <param name="clientName"></param>
        /// <param name="identityScopesConsentedTo"></param>
        /// <param name="resourceScopesConsentedTo"></param>
        public void GrantConsent(string application, string clientName, IEnumerable<Scope> identityScopesConsentedTo, IEnumerable<Scope> resourceScopesConsentedTo)
        {
            Apply(new UserConsented());
        }
    }
}