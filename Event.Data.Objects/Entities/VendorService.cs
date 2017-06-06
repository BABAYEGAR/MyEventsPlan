using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Event.Data.Objects.Entities
{
    public class VendorService : Transport
    {
        
        public long VendorServiceId { get; set; }
        [Required]
        [DisplayName("Service")]
        public string ServiceName { get; set; }

        public IEnumerable<Vendor> Vendors { get; set; }
    }
}
