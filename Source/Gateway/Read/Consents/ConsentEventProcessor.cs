using Dolittle.Events.Processing;
using Events.Consents;
using IdentityServer4.Services;

namespace Read.Consents
{
    /// <summary>
    /// Represents the event processor for events related to user consent
    /// </summary>
    public class ConsentEventProcessor : ICanProcessEvents
    {
        readonly IIdentityServerInteractionService _interaction;
        
        /// <summary>
        /// Initializes an instance of <see cref="ConsentEventProcessor"/> 
        /// </summary>
        /// <param name="interaction"></param>
        public ConsentEventProcessor(IIdentityServerInteractionService interaction)
        {
            _interaction = interaction;
        }
         /// <summary>
        /// Processes the <see cref="UserConsented"/> event
        /// </summary>
        [EventProcessor("3279e73a-d4fe-4980-9b8a-ac8deefcd65e")]
        public void Process(UserConsented @event)
        {
            var grantedConsent = new IdentityServer4.Models.ConsentResponse()
            {
                RememberConsent = @event.RememberConsent,
                ScopesConsented = @event.ScopesConsented
            };

            var request = _interaction.GetAuthorizationContextAsync(@event.ReturnUrl).Result;
            if (request == null) throw new System.Exception("User not authorized");
            _interaction.GrantConsentAsync(request, grantedConsent);

            // Redirect, do in browser?.
        }
    }
}