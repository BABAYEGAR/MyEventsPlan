﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyEventPlan.Data.Service.Enum
{
    public enum TaskStausEnum
    {
        [Description("2048FF")]
        InProgress,
        Missed,
        [Description("16FF51")]
        Completed
      
    }
}