using Jam.Domain.AggregatesModel.JamAggregate;
using Jam.Domain.Exceptions;
using Xunit;

namespace Jam.UnitTests.Domain
{
    public class JamAggregateTest
    {
        public JamAggregateTest(){ }
        [Fact]
        public void Create_jam_item_success()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 1;

            //Act
            var fakeJamItem = new Jam.Domain.AggregatesModel.JamAggregate.Jam(createdHostId, jamTypeId);

            //Assert
            Assert.NotNull(fakeJamItem);
        }
        [Fact]
        public void Add_role_item_success()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem = new Jam.Domain.AggregatesModel.JamAggregate.Jam(createdHostId, jamTypeId);
            fakeJamItem.AddRoleItem(1);

            //Assert
            Assert.NotEmpty(fakeJamItem.RoleItems);
        }
        [Fact]
        public void Start_jam_item_status_success()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .RegisterPreferredRole(1, 1)
                .UpdateRoleItemSuccessStatus(1, 1)
                .Build();
            fakeJamItem.StartJamStatus(1);
            //Assert
            Assert.Equal(fakeJamItem.JamStatusId, Status.Started.Id);
        }
        [Fact]
        public void Start_jam_item_status_throws_exception_with_unregistired_role()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .RegisterPreferredRole(1, 1)
                .Build();

            //Assert
            Assert.Throws<JamDomainException>(() => fakeJamItem.StartJamStatus(1));
        }
        [Fact]
        public void Register_preferred_role_throws_exception_with_nonpublic()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 1;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .Build();

            //Assert
            Assert.Throws<JamDomainException>(() => fakeJamItem.RegisterPreferredRole(1,1));
        }
        [Fact]
        public void Register_preferred_role_throws_exception_with_another_user_register()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .RegisterPreferredRole(4,1)
                .Build();

            //Assert
            Assert.Throws<JamDomainException>(() => fakeJamItem.RegisterPreferredRole(1, 1));
        }
        [Fact]
        public void Register_preferred_role_throws_exception_with_another_user_register_completed()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .RegisterPreferredRole(4, 1)
                .UpdateRoleItemSuccessStatus(4, 1)
                .Build();

            //Assert
            Assert.Throws<JamDomainException>(() => fakeJamItem.RegisterPreferredRole(1, 1));
        }
        [Fact]
        public void Register_preferred_role_throws_exception_with_already_has_role()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 1;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .Build();

            //Assert
            Assert.Throws<JamDomainException>(() => fakeJamItem.RegisterPreferredRole(1, 1));
        }
        [Fact]
        public void Start_jam_item_status_throws_exception_with_wrong_host_id()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 1;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .Build();

            //Assert
            Assert.Throws<JamDomainException>(() => fakeJamItem.StartJamStatus(2));
        }
        [Fact]
        public void Update_role_item_status_success()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem = 
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .RegisterPreferredRole(1, 1)
                .Build();

            fakeJamItem.UpdateRoleItemSuccessStatus(1, 1);
            var role = fakeJamItem.RoleItems.FirstOrDefault();
            //Assert
            Assert.NotNull(role.RegisteredUserId);
            Assert.Equal(role.RoleItemStatusId, RoleItemStatus.Completed.Id);
        }
        [Fact]
        public void Update_role_item_failed_success()
        {
            //Arrange
            int createdHostId = 1;
            int jamTypeId = 2;

            //Act
            var fakeJamItem =
                new JamBuilder(createdHostId, jamTypeId)
                .AddRoleItem(1)
                .RegisterPreferredRole(1, 1)
                .Build();

            fakeJamItem.UpdateRoleItemFailedStatus(1, 1);
            var role = fakeJamItem.RoleItems.FirstOrDefault();
            //Assert
            Assert.Null(role.RegisteredUserId);
            Assert.Equal(role.RoleItemStatusId, RoleItemStatus.Created.Id);
        }
    }
}