using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Models.EventModels
{
    public class SpendingModel : AbstractEventModel
    {
        public decimal SpendingAmount { get; set; }
        public SpendingModel(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, decimal spendingAmount, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false) : base(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown)
        {
            SpendingAmount = spendingAmount;
        }
    }
}
