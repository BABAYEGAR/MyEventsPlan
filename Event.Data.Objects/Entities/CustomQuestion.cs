﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class CustomQuestion
    {
        public long CustomQuestionId { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        public long GuestId { get; set; }
        [ForeignKey("GuestId")]
        public Guest Guest { get; set; }

    }
}
