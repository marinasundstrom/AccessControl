using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Foobiq.AccessPoint.Models
{
    public class Parameter
    {
        [Key]
        public string Key { get; set; }

        public string Value { get; set; }
    }
}
