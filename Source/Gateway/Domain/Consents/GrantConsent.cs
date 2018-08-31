using System.Collections.Generic;
using Concepts;
using Concepts.Scopes;
using Dolittle.Commands;

namespace Domain.Consents
{
    /// <summary>
    /// Represents the intent of the user granting the tenant and application his consent  
    /// </summary>
    public class GrantConsent : ICommand
    {
        /// <summary>
        /// The <see cref="TenantId">Tenant's id</see>
        /// </summary>
        /// <value></value>
        public TenantId Tenant {get; set;}
        /// <summary>
        /// The scopes the user consented to
        /// </summary>
        public IEnumerable<string> Scopes {get; set;} = new List<string>();

        /// <summary>
        /// The return url
        /// </summary>
        /// <value></value>
        public string ReturnUrl {get; set;}

        /// <summary>
        /// Wether or not the consent should be remembered
        /// </summary>
        /// <value></value>
        public bool RememberConsent {get; set;}

    }
}