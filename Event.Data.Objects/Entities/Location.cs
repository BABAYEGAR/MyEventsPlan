using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class Location : Transport
    {
        public long LocationId { get; set; }
        public string Name { get; set; }
    }
}
