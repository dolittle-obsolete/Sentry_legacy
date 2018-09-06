using System;
using Concepts;
using Domain.Consents;
using Machine.Specifications;
using System.Linq;
namespace Domain.Specs.for_consents
{
    public class when_creating_GrantConsent_command
    {
        readonly static TenantId tenant_id = Guid.NewGuid();
        const bool remember_consent = true;
        const string return_url = "/";
        readonly static string[] scopes = new [] {"one"};
        static GrantConsent command;


        Establish context = () => command = new GrantConsent()
            {
                Tenant = tenant_id,
                RememberConsent = remember_consent,
                ReturnUrl = return_url,
                Scopes = scopes
            };

        It should_not_be_null = () => command.ShouldNotBeNull();
        It should_have_tenant_id = () => command.Tenant.ShouldEqual(tenant_id);
        It should_have_remember_consent = () => command.RememberConsent.ShouldEqual(remember_consent);
        It should_have_return_url = () => command.ReturnUrl.ShouldEqual(return_url);
        It should_have_scopes = () => command.Scopes.ShouldNotBeEmpty();
        It should_have_scopes_with_one_element = () => command.Scopes.Count().ShouldEqual(1);
        

    }
}