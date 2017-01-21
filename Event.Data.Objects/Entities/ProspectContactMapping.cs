using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class ProspectContactMapping
    {
        public long ProspectContactMappingId { get; set; }
        public long? ProspectId { get; set; }
        [ForeignKey("ProspectId")]
        public virtual Prospect Prospect { get; set; }
        public long? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
