using System.ComponentModel.DataAnnotations;

namespace MyEventPlan.Data.Service.Enum
{
    public enum AppointmentType
    {
        [Display(Name = "For An Event")]
        Event,
        [Display(Name = "For My Personal Business")]
        Personal
    }
}
