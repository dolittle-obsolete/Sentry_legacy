using System;
using Dolittle.Concepts;

namespace Concepts.Logins
{
    public class CountryId : ConceptAs<Guid>
    {
        public static readonly CountryId Empty = Guid.Empty;
        public static implicit operator CountryId(Guid value)
        {
            return new CountryId { Value = value };
        }

        public static implicit operator CountryId(string value)
        {
            return new CountryId { Value = Guid.Parse(value) };
        }
    }
}