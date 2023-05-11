using SmartLock.Shared.Abstraction.Domain;

namespace SmartLock.Domain.Entities {
    public sealed class Role : AggregationRoot<int> {
        public Role(int id, string name, string key) {
            Id = id;
            Name = name;
            Key = key;
        }

        private Role() {
        }

        public string Name { get; private set; }
        public string Key { get; private set; }
    }
}
