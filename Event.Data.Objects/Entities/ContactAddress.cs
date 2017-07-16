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
        public string Type { get; set; }
        [DisplayName("Address Line 1")]
        public string Street1 { get; set; }
        [DisplayName("Postal Code")]
        public string PostalCode { get; set; }
        public string State { get; set; }
        [DisplayName("Country/Region")]
        public string Country { get; set; }
        public long? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
