﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class VendorReview : Transport
    {
        public long VendorReviewId { get; set; }
        [Required]
        public string ReviewerName { get; set; }
        [Required]
        public string ReviewerEmail { get; set; }
        [Required]
        public string ReviewTitle { get; set; }
        [Required]
        public string ReviewBody { get; set; }
        public long? Rating { get; set; }
        public long? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public  Vendor Vendor { get; set; }
        
    }
}
