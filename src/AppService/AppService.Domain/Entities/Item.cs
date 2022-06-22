using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AppService.Domain.Common;

namespace AppService.Domain.Entities
{
    public class Item : AuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
    }
}
