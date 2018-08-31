using System.Collections.Generic;
using Dolittle.Events;

namespace Events.Consents
{
    /// <summary>
    /// The event that represnets that a user has consented
    /// </summary>
    public class UserConsented : IEvent
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> ScopesConsented {get; set;}
        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool RememberConsent {get; set;}
    }
}