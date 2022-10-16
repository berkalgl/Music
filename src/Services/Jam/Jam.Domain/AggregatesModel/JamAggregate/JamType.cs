using Jam.Domain.Exceptions;
using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public class JamType : Enumeration
    {
        public static JamType Private = new JamType(1, nameof(Private));
        public static JamType Public = new JamType(2, nameof(Public));

        public JamType(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<JamType> List() =>
            new[] { Private, Public };

        public static JamType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static JamType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
