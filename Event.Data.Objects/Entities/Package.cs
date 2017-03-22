using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Package : Transport
    {
        public long PackageId { get; set; }
        [Required]
        [DisplayName("Package Name")]
        public string PackageName { get; set; }
        public string Amount { get; set; }
        [Required]
        [DisplayName("Maximum Number Of Events")]
        public long MaximumEvents { get; set; }
    }
}
