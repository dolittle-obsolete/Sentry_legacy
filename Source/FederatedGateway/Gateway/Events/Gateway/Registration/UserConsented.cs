using System.Collections.Generic;
using Dolittle.Events;

namespace Events.Gateway.Registration
{
    /// <summary>
    /// The event that represnets that a user has consented
    /// </summary>
    public class UserConsented : IEvent
    {
        public UserConsented(IEnumerable<string> scopesConsented, string returnUrl, bool rememberConsent) 
        {
            ScopesConsented = scopesConsented;
            ReturnUrl = returnUrl;
            RememberConsent = rememberConsent;
        }
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ScopesConsented {get; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; }
        /// <summary>
        /// 
        /// </summary>
        public bool RememberConsent {get; }
    }
}