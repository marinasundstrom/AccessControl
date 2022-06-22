using System;
using System.Collections.Generic;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class AccessList : AuditableEntity
    {
        private HashSet<AccessListMembership> _memberships = new HashSet<AccessListMembership>();
        private HashSet<Identity> _members = new HashSet<Identity>();

        internal AccessList() {}

        public AccessList(string name)
        {
            Name = name;
        }

        public Guid Id { get; private set; }

        public string Name { get; private set; } = null!;

        public IReadOnlyCollection<Identity> Members => _members;

        public IReadOnlyCollection<AccessListMembership> Memberships => _memberships;

        public AccessListMembership AddMember(Identity identity)
        {
            var membership = new AccessListMembership(identity);

            _memberships.Add(membership);

            return membership;
        }

        public void RemoveMember(Identity identity)
        {
            _members.Remove(identity);
        }
    }
}
