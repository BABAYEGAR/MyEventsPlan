using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class VendorService : Transport
    {
        public long VendorServiceId { get; set; }
        [Required]
        public string ServiceName { get; set; }
        [Required]
        public string Scale { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
    }
}
