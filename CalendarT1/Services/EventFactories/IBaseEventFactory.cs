
using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventFactories
{
	public interface IBaseEventFactory
	{
		IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, IUserEventTypeModel EventType, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false);
	}

	public interface IEventFactory : IBaseEventFactory
	{
	}

	public interface ITaskEventFactory : IBaseEventFactory
	{
	}

	public interface ISpendingEventFactory : IBaseEventFactory
	{
		IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, IUserEventTypeModel EventType, decimal spendingAmount, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false);
	}

}
