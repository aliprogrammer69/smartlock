using System.Diagnostics.CodeAnalysis;

using SmartLock.Domain.Events.User;
using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Entities {
    public sealed class User : AggregationRoot<ulong>, IEqualityComparer<User> {
        private User() {
        }

        private User(string userName, string password,
                     string passwordSalt, Role role) {
            UserName = userName;
            Password = password;
            PaswordSalt = passwordSalt;
            Role = role;
        }

        public string UserName { get; private set; }
        public string Password { get; private set; }
        public string PaswordSalt { get; private set; }
        public string RefreshToken { get; private set; }
        public DateTime? RefreshTokenExpireDate { get; private set; }

        public DateTime? LastLoginDate { get; private set; }
        public IEnumerable<Lock> AccessedLocks { get; private set; } = new HashSet<Lock>();
        public Role Role { get; private set; }
        public int RoleId { get; private set; }

        #region Methods
        public static User Create(string userName, string password,
                                  string passwordSalt, Role role) {
            User user = new(userName, password, passwordSalt, role) {
                RoleId = role.Id
            };
            user.AddEvent(new UserRegistered(user));
            return user;
        }

        public void Login(string refreshToken, DateTime expires) {
            RefreshToken = refreshToken;
            RefreshTokenExpireDate = expires;
            LastLoginDate = DateTime.Now;
            AddEvent(new UserLoggedIn(this, DateTime.Now));
        }

        public void Edit(string username, Role role) {
            UserName = username;
            Role = role;
            AddEvent(new UserEdited(this, DateTime.Now));
        }

        #endregion

        #region IEqualityComparer Members

        public bool Equals(User x, User y) => x.Id == y.Id;

        public int GetHashCode([DisallowNull] User obj) =>
            obj.GetHashCode();

        #endregion
    }
}
