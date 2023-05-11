using FluentAssertions;

using SmartLock.Domain.Entities;
using SmartLock.Domain.Events.User;

namespace SmartLock.Tests.Domain.UnitTest.Entities {
    public sealed class UserTests {

        [Fact]
        public void Should_Create_New_User_And_Registered_Event() {
            Role userRole = new Role(1, "Role", "Role");
            User user = User.Create("TestUsername", "TestPassword", "Salt", userRole);

            user.Should().NotBeNull();
            user.Events.Should().HaveCount(1);
            user.Events.First().Should().BeOfType<UserRegistered>();
        }

        [Fact]
        public void Should_Login_User() {
            Role userRole = new Role(1, "Role", "Role");
            User user = User.Create("TestUsername", "TestPassword", "Salt", userRole);

            string refreshTokenValue = "rfreshToken";
            DateTime expires = DateTime.Now.AddDays(1);

            user.Login(refreshTokenValue, expires);

            user.Should().NotBeNull();
            user.RefreshToken.Should().Be(refreshTokenValue);
            user.RefreshTokenExpireDate.Should().Be(expires);
            user.LastLoginDate.Should().NotBeNull();
        }
    }
}
