using Jam.Domain.Exceptions;
using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public class Status : Enumeration
    {
        public static Status Pending = new Status(1, nameof(Pending));
        public static Status Started = new Status(2, nameof(Started));
        public static Status Completed = new Status(3, nameof(Completed));

        public Status(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<Status> List() =>
            new[] { Pending, Started, Completed };

        public static Status FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Status From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
