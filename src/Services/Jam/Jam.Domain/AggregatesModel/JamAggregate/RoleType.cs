using Jam.Domain.Exceptions;
using Jam.Domain.SeedWork;

namespace Jam.Domain.AggregatesModel.JamAggregate
{
    public class RoleType : Enumeration
    {
        public static RoleType Vocalist = new(1, nameof(Vocalist));
        public static RoleType LeadGuitarist = new(2, nameof(LeadGuitarist));
        public static RoleType RhythmGuitarist = new(3, nameof(RhythmGuitarist));
        public static RoleType BassGuitarist = new(4, nameof(BassGuitarist));
        public static RoleType Drummer = new(5, nameof(Drummer));
        public static RoleType KeyboardPlayer = new(6, nameof(KeyboardPlayer));

        public RoleType(int id, string name)
            : base(id, name)
        {
        }
        public static IEnumerable<RoleType> List() =>
            new[] { Vocalist, LeadGuitarist, RhythmGuitarist, BassGuitarist, Drummer, KeyboardPlayer };
        public static RoleType FromName(string name)
        {
            var bandRole = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (bandRole == null)
            {
                throw new JamDomainException($"Possible values for JamRoleType: {string.Join(",", List().Select(s => s.Name))}");
            }

            return bandRole;
        }
        public static RoleType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new JamDomainException($"Possible values for JamRoleType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
