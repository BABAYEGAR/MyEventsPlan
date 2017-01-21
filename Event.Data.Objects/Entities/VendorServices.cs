using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class VendorService : Transport
    {
        public long VendorServicesId { get; set; }
        public string ServiceName { get; set; }
        public string Scale { get; set; }
        public IEnumerable<Vendor> Vendors { get; set; }
    }
}
