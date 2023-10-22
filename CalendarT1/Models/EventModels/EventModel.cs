using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Models.EventModels
{
    public class EventModel : AbstractEventModel
    {
        public EventModel(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel EventType, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false, QuantityModel quantityAmount = null, List<MicroTaskModel> microTasksList = null) : base(title, description, startTime, endTime, EventType, isCompleted, postponeTime, wasShown, quantityAmount, microTasksList)
        {
        }
	}
}
