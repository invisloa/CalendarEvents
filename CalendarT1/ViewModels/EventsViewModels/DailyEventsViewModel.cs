using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class DailyEventsViewModel : AbstractEventViewModel
	{
		private IUserEventTypeModel _eventType;
		public string AboveEventsListText
		{
			get
			{
			// switch selectedLanguage()
				{
					return "EVENTS LIST";
				}
			}
		}
		public DailyEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
		}
		public DailyEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
		}
		public override void BindDataToScheduleList()
		{
				 ApplyEventsDatesFilter(CurrentSelectedDate.Date, CurrentSelectedDate.AddDays(1));
		}

	}
}
