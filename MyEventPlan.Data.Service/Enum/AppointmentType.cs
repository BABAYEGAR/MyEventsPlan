using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
