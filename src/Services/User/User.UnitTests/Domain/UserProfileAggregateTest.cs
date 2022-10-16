using User.Domain.AggregatesModel.UserProfileAggregate;

namespace User.UnitTests.Domain
{
    public class UserProfileAggregateTest
    {
        public UserProfileAggregateTest() { }
        [Fact]
        public void Create_user_profile_success()
        {
            //Arrange

            //Act
            var fakeUserProfile = new UserProfile("FakeEmail", "FakePassword", "Player");
            //Assert
            Assert.NotNull(fakeUserProfile);
        }
        [Fact]
        public void Add_role_to_user_profile_success()
        {
            //Arrange

            //Act
            var fakeUserProfile = new UserProfile("FakeEmail", "FakePassword", "Player");
            fakeUserProfile.AddBandRoleType(1);
            //Assert
            Assert.NotEmpty(fakeUserProfile.BandRoles);
        }
        [Fact]
        public void Has_role_user_profile_success()
        {
            //Arrange

            //Act
            var fakeUserProfile = new UserProfile("FakeEmail", "FakePassword", "Player");
            fakeUserProfile.AddBandRoleType(1);

            //Assert
            Assert.True(fakeUserProfile.HasTheRole(1));
        }
    }
}
