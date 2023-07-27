
using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventFactories
{
    public interface IEventFactory
	{
		IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false);
	}
	public interface ITaskEventFactory : IEventFactory
	{
		IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false);
	}
	public interface ISpendingEventFactory : IEventFactory
	{
		IGeneralEventModel CreateEvent(string title, string description, DateTime startDateTime, DateTime endDateTime, decimal spendingAmount, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false);
	}

}
