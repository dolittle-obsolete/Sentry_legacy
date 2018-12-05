using Dolittle.Concepts;

namespace Concepts
{
    public class TenantName : ConceptAs<string>
    {
        public static readonly TenantName NotSet = string.Empty;

        public static implicit operator TenantName(string tenantName)
        {
            return new TenantName { Value = tenantName };
        }
    }
}
