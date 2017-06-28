﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Event.Data.Objects.Entities
{
    public class ContactWebsite
    {
        public long ContactWebsiteId { get; set; }
        [Required]
        public string Type { get; set; }
        [Required]
        public string Website { get; set; }
        public long? ContactId { get; set; }
        [ForeignKey("ContactId")]
        public virtual Contact Contact { get; set; }
    }
}
