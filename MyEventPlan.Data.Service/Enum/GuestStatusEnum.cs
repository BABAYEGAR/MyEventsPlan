using System.ComponentModel.DataAnnotations;

namespace MyEventPlan.Data.Service.Enum
{
    public enum GuestStatusEnum
    {
        Attending,
        [Display(Name = "Not Attending")]
        NotAttending
    }
}
