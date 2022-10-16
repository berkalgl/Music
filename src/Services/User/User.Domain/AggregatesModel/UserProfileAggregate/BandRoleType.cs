using User.Domain.Exceptions;
using User.Domain.SeedWork;

namespace User.Domain.AggregatesModel.UserProfileAggregate
{
    public class BandRoleType : Enumeration
    {
        public static BandRoleType Vocalist = new(1, nameof(Vocalist));
        public static BandRoleType LeadGuitarist = new(2, nameof(LeadGuitarist));
        public static BandRoleType RhythmGuitarist = new(3, nameof(RhythmGuitarist));
        public static BandRoleType BassGuitarist = new(4, nameof(BassGuitarist));
        public static BandRoleType Drummer = new(5, nameof(Drummer));
        public static BandRoleType KeyboardPlayer = new(6, nameof(KeyboardPlayer));

        public BandRoleType(int id, string name)
            : base(id, name)
        {
        }
        public static IEnumerable<BandRoleType> List() =>
            new[] { Vocalist, LeadGuitarist, RhythmGuitarist, BassGuitarist, Drummer, KeyboardPlayer };
        public static BandRoleType FromName(string name)
        {
            var bandRole = List()
                .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (bandRole == null)
            {
                throw new UserDomainException($"Possible values for bandRole: {string.Join(",", List().Select(s => s.Name))}");
            }

            return bandRole;
        }
        public static BandRoleType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new UserDomainException($"Possible values for bandRole: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
