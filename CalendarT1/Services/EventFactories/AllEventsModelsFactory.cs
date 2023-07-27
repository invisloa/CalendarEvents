using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventFactories
{
	public class EventModelFactory : IBaseEventFactory
	{
		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, IUserEventTypeModel EventType, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			return new EventModel(title, description, startDateTime, endDateTime, EventType, isCompleted, postponeTime,wasShown);
		}
	}

	public class SpendingModelFactory : ISpendingEventFactory
	{
		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, IUserEventTypeModel EventType, decimal spendingAmount, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			throw new NotImplementedException();
		}

		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, IUserEventTypeModel EventType, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			throw new NotImplementedException();
		}
	}
	public class TaskModelFactory : ITaskEventFactory
	{
		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, IUserEventTypeModel EventType, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			throw new NotImplementedException();
		}
	}
}
