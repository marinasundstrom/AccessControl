using System;

namespace AppService.Domain.Entities
{
    public class AccessListMembership
    {
        public Guid AccessListId { get; set; }

        public AccessList AccessList { get; set; }

        public Identity Identity { get; set; }

        public Guid IdentityId { get; set; }

        public DateTime AddedDate { get; set; } = DateTime.Now;

        public DateTime? RemovedDate { get; set; }

    }
}
