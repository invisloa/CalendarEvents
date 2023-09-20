using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views.CustomControls.CCInterfaces;
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
		protected override void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
		{

			var selectedToFilterEventTypes = AllEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

			List<IGeneralEventModel> filteredEvents = AllEventsListOC
				.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()) &&
							x.StartDateTime.Date == startDate.Date &&
							x.EndDateTime.Date <= endDate.Date)
				.ToList();

			// Clear existing items in the EventsToShowList
			EventsToShowList.Clear();

			// Add filtered items to the EventsToShowList
			foreach (var eventItem in filteredEvents)
			{
				EventsToShowList.Add(eventItem);
			}
		}
		public override void BindDataToScheduleList()
		{
				 ApplyEventsDatesFilter(CurrentSelectedDate.Date, DateTime.MaxValue);
		}

		public void OnAppearing()
		{
			BindDataToScheduleList();
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(EventsToShowList);
		}




	}
}
