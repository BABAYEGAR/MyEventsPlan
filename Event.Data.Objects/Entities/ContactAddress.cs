using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class ContactAddress
    {
        public long ContactAddressId { get; set; }
        [Required]
        public string Type { get; set; }
        [DisplayName("Street 1")]
        [Required]
        public string Street1 { get; set; }
        [DisplayName("Street 2")]
        [Required]
        public string Street2 { get; set; }
        [DisplayName("Postal Code")]
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string State { get; set; }
        [DisplayName("Country/Region")]
        [Required]
        public string Country { get; set; }
        public long? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
