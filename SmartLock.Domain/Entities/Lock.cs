using SmartLock.Domain.Events.Lock;
using SmartLock.Shared.Abstraction;
using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Entities {
    public sealed class Lock : AggregationRoot<ulong> {

        private Lock() {
        }

        private Lock(string name, string address, bool isPublic) {
            Name = name;
            Address = address;
            IsPublic = isPublic;
        }

        public string Name { get; private set; }
        public string Address { get; private set; }
        public bool IsPublic { get; private set; }
        public bool IsLocked { get; private set; } = true;

        public ICollection<User> AccessedUsers { get; private set; } = new HashSet<User>();

        #region Methods

        public static Lock Create(string name, string address, bool isPublic) {
            Lock result = new(name, address, isPublic);
            result.AddEvent(new LockAdded(result));
            return result;
        }

        public void Edit(string name, string address, bool isPublic) {
            Name = name;
            Address = address;
            IsPublic = isPublic;
            AddEvent(new LockEdited(this));
        }

        public bool GrandAccess(User user) {
            if (!AccessedUsers.Any(a => a == user))
                AccessedUsers.Add(user);

            AddEvent(new UserAccessToLockGranted(this, user));
            return true;
        }

        public bool DenyAccess(User user) {
            AccessedUsers.Remove(user);
            AddEvent(new UserAccessToLockDenied(this, user));
            return true;
        }

        public bool UnLock(User user) {
            bool hasAccess = IsPublic ||
                             string.Equals(user.Role.Key, RoleKeys.Admin) ||
                             AccessedUsers.Contains(user);
            AddEvent(new Unlocked(this, user, hasAccess));

            IsLocked = !hasAccess;
            return hasAccess;
        }

        public void CloseTheDoor(User user) {
            IsLocked = true;
            AddEvent(new Locked(this, user, true));
        }

        #endregion
    }
}