using System.ComponentModel.DataAnnotations.Schema;

namespace Event.Data.Objects.Entities
{
    public class VendorImage : Transport
    {
        public long VendorImageId { get; set; }
        public string ImageOne { get; set; }
        public string ImageTwo { get; set; }
        public string ImageThree { get; set; }
        public string ImageFour { get; set; }
        public string ImageFive { get; set; }
        public string ImageSix { get; set; }
        public string ImageSeven { get; set; }
        public string ImageEight { get; set; }
        public string ImageNine { get; set; }
        public string ImageTen { get; set; }
        public long? VendorId { get; set; }
        [ForeignKey("VendorId")]
        public  Vendor Vendor { get; set; }
        
    }
}
