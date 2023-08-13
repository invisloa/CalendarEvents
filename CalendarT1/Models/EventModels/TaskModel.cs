using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Models.EventModels
{
    public class TaskModel : AbstractEventModel
    {
        // STATUS : DONE ...
        // Assigned to ...

        public TaskModel(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false) : base(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown)
        { }
    }
}
