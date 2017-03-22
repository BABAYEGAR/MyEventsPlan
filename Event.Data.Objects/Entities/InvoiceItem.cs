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
    public class InvoiceItem :  Transport
    {
        public long InvoiceItemId { get; set; }
        [Required]
        [DisplayName("Item Description")]
        public string ItemName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [DisplayName("Item Date")]
        public DateTime ItemDate { get; set; }
        [Required]
        public long Quantity { get; set; }
        [Required]
        [DisplayName("Unit Cost")]
        public long UnitCost { get; set; }
        public long? InvoiceId { get; set; }
        [ForeignKey("InvoiceId")]
        public Invoice Invoice { get; set; }
    }
}
