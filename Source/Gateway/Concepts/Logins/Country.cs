using Dolittle.Concepts;

namespace Concepts.Logins
{
    public class Country : ConceptAs<string>
    {
        public static readonly Country NotSet = string.Empty;

        public static implicit operator Country(string country)
        {
            return new Country { Value = country };
        }
    }
}
