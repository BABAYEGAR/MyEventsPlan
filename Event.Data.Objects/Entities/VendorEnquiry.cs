using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class VendorEnquiry : Transport
    {
        public long VendorEnquiryId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public DateTime EventDate { get; set; }
        public string Note { get; set; }
        public long? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public Vendor Vendor { get; set; }
    }
}
