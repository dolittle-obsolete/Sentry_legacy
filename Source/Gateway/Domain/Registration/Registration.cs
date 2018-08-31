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
        /// Initializes an instance of <see cref="Registration"/>
        /// </summary>
        public Registration(Guid id) : base(id)
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
            Apply
            (
                new UserConsented()
                {
                    ScopesConsented = scopes,
                    ReturnUrl = returnUrl,
                    RememberConsent = rememberConsent
                }
            );
        }
    }
}