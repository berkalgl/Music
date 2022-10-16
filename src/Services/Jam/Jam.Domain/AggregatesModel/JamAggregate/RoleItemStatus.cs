using Jam.Domain.Exceptions;
using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public class RoleItemStatus : Enumeration
    {
        public static RoleItemStatus Pending = new RoleItemStatus(1, nameof(Pending));
        public static RoleItemStatus Created = new RoleItemStatus(2, nameof(Created));
        public static RoleItemStatus Completed = new RoleItemStatus(3, nameof(Completed));

        public RoleItemStatus(int id, string name)
            : base(id, name)
        {
        }

        public static IEnumerable<RoleItemStatus> List() =>
            new[] { Pending, Created, Completed };

        public static RoleItemStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamRoleItemStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static RoleItemStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamRoleItemStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
