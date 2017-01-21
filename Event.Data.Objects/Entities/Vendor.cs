using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class Vendor : Transport
    {
        public long VendorId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        public  long? VendorServiceId { get; set; }
        [ForeignKey("VendorServiceId")]
        public virtual VendorService VendorService { get; set; }
    }
}
