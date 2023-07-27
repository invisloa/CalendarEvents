using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventFactories
{
	public class EventModelFactory : IEventFactory
	{


		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			return new EventModel(title, description, startDateTime, endDateTime);
		}
	}

	public class SpendingModelFactory : ISpendingEventFactory
	{
		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, decimal spendingAmount, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			throw new NotImplementedException();
		}

		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			throw new NotImplementedException();
		}
	}
	public class TaskModelFactory : ITaskEventFactory
	{
		public IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false)
		{
			throw new NotImplementedException();
		}
	}
}
