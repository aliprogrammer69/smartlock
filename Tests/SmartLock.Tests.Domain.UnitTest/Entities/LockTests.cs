using FluentAssertions;

using SmartLock.Domain.Entities;
using SmartLock.Domain.Events.Lock;

namespace SmartLock.Tests.Domain.UnitTest.Entities {
    public class LockTests {
        private readonly Lock _lock;
        private readonly User _user;
        public LockTests() {
            _lock = Lock.Create("TestLock", "TestAddress", false);
            _user = User.Create("Test", "Test", "Test", new Role(1, "test", "test"));
        }

        [Fact]
        public void Should_Create_Lock_And_LockAdded_Event() {
            _lock.Should().NotBeNull();
            _lock.Events.Should().HaveCount(1);
            _lock.Events.First().Should().BeOfType<LockAdded>();
        }

        [Fact]
        public void Should_Edit_Lock_And_Raise_LockEdited_Event() {
            string newName = "newName";
            string newAddress = "newAddress";

            _lock.Edit(newName, newAddress, true);

            _lock.Address.Should().Be(newAddress);
            _lock.Name.Should().Be(newName);
            _lock.IsPublic.Should().BeTrue();
            _lock.Events.Last().Should().BeOfType<LockEdited>();
        }

        [Fact]
        public void Should_Grant_Access() {
            _lock.GrandAccess(_user);

            _lock.AccessedUsers.Should().HaveCount(1);
            _lock.AccessedUsers.First().Should().Be(_user);
            _lock.Events.Last().Should().BeOfType<UserAccessToLockGranted>();
        }

        [Fact]
        public void Should_Deny_Access() {
            _lock.GrandAccess(_user);

            _lock.DenyAccess(_user);

            _lock.AccessedUsers.Should().HaveCount(0);
            _lock.Events.Last().Should().BeOfType<UserAccessToLockDenied>();
        }

        [Fact]
        public void Should_Lock_TheLock() {
            _lock.CloseTheDoor(_user);

            _lock.IsLocked.Should().BeTrue();
            _lock.Events.Should().HaveCount(2);
            _lock.Events.Last().Should().BeOfType<Locked>();
        }

        [Fact]
        public void Should_Unlock_Public_Lock() {
            _lock.Edit(_lock.Name, _lock.Address, true);
            _lock.UnLock(_user);
            _lock.IsLocked.Should().BeFalse();
            _lock.Events.Last().As<Unlocked>().Success.Should().BeTrue();
        }

        [Fact]
        public void Should_Refuse_To_Unlock() {
            bool lockResult = _lock.UnLock(_user);

            lockResult.Should().BeFalse();
            _lock.Events.Last().As<Unlocked>().Success.Should().BeFalse();
        }

        [Fact]
        public void Should_Unlock_By_Accessed_User() {
            _lock.GrandAccess(_user);
            _lock.UnLock(_user);

            _lock.IsLocked.Should().BeFalse();
            _lock.Events.Last().As<Unlocked>().Success.Should().BeTrue();
        }
    }
}
