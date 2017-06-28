using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEventPlan.Data.Service.Enum
{
   public enum ReminderRepeat
    {
        [Display(Name = "No Repeat")]
            norepeat,
            Daily,
            Weekly,
            Monthly,
            Yearly
    }
}
