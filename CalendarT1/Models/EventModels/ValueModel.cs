using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Models.EventModels
{
    public class ValueModel : AbstractEventModel
    {
        public decimal Value { get; set; }
        public ValueModel
            (string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, decimal valueAmount, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false) : base(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown)
        {
            Value = valueAmount;
        }
    }
}
