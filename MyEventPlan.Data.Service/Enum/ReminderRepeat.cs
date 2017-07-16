using System.ComponentModel.DataAnnotations;

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
