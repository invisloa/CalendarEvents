using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace CalendarT1.ViewModels.EventsViewModels
{
    public abstract class AbstractEventViewModel : PlainBaseAbstractEventViewModel
	{
		#region Properties
		private DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate
		{
			get => _currentDate;
		}
		private DateTime _currentSelectedDate = DateTime.Now;
		public DateTime CurrentSelectedDate
		{
			get => _currentSelectedDate;
			set
			{
				if (_currentSelectedDate != value)
				{
					_currentSelectedDate = value;
					OnPropertyChanged();
					BindDataToScheduleList();
					//DatePickerDateSelectedCommand.Execute(_currentSelectedDate);		// TODO: check if this is the right way to do it
				}
			}
		}
		#endregion
		public AbstractEventViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
		}

		#region Commands

		private RelayCommand<DateTime> _datePickerDateSelectedCommand;
		public RelayCommand<DateTime> DatePickerDateSelectedCommand =>
			_datePickerDateSelectedCommand ?? (_datePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected));

		private RelayCommand _goToAddEventPageCommand;
		public RelayCommand GoToAddEventPageCommand =>
			_goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));

		#endregion

		#region Methods

		private void DatePickerDateSelected(DateTime newDate)
		{
			CurrentSelectedDate = newDate;
			BindDataToScheduleList();
		}

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(EventRepository));           // TO DO CONSIDER CHANGE to await Shell.Current.GoToAsync($"AllEventsPageList?{string.Join("&", parameters.Select(p => $"{p.Key}={p.Value}"))}");
		}
		#endregion

		#region Abstract Methods

		#endregion

		protected virtual void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
		{
			var selectedEventTypes = AllEventTypesOC.Where(x => x.IsSelectedToFilter).Select(x => x.EventTypeName).ToList();
			List<IGeneralEventModel> filteredEvents = new List<IGeneralEventModel>();
			foreach (var eventModel in AllEventsListOC)
			{
				if (eventModel.StartDateTime.Date >= startDate && eventModel.EndDateTime.Date <= endDate)
				{
					if (selectedEventTypes.Contains(eventModel.EventType.EventTypeName))
					{
						filteredEvents.Add(eventModel);
					}
				}
			}
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
		}

	}
}
