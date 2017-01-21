using System;
using System.ComponentModel;

namespace Event.Data.Objects.Entities
{
    public class Transport
    {
        [DisplayName("Created By")]
        public long? CreatedBy { get; set; }

        [DisplayName("Date Created")]
        public DateTime DateCreated { get; set; }

        [DisplayName("Date Last Modified")]
        public DateTime DateLastModified { get; set; }

        [DisplayName("Last Modified By")]
        public long? LastModifiedBy { get; set; }
    }
}