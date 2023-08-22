using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public abstract class AbstractEventViewModel : PlainBaseAbstractEventViewModel
	{
		#region Fields

		private RelayCommand<DateTime> _datePickerDateSelectedCommand;
		private RelayCommand _goToAddEventPageCommand;
		private RelayCommand _goToAddNewTypePageCommand;
		private DateTime _currentSelectedDate = DateTime.Now;
		#endregion
		
		#region Properties
		
		private DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate => _currentDate;
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
					//DatePickerDateSelectedCommand.Execute(_currentSelectedDate); // TODO: check if this is the right way to do it
				}
			}
		}
		#endregion

		#region Constructor

		public AbstractEventViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
		}

		#endregion

		#region Commands
		public RelayCommand GoToAddEventPageCommand => _goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));
		public RelayCommand GoToAddNewTypePageCommand => _goToAddNewTypePageCommand ?? (_goToAddNewTypePageCommand = new RelayCommand(GoToAddNewTypePage));

		public RelayCommand<DateTime> DatePickerDateSelectedCommand =>
			_datePickerDateSelectedCommand ?? (_datePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected));

		#endregion

		#region Private Methods

		private void GoToAddEventPage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new EventPage(EventRepository, _currentSelectedDate));
		}
		private void GoToAddNewTypePage()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewTypePage());
		}

		private void DatePickerDateSelected(DateTime newDate)
		{
			CurrentSelectedDate = newDate;
			BindDataToScheduleList();
		}

		#endregion

		#region Protected Methods

		protected virtual void ApplyEventsDatesFilter(DateTime startDate, DateTime endDate)
			{

				var selectedToFilterEventTypes = AllEventTypesOC
				.Where(x => x.IsSelectedToFilter)
				.Select(x => x.EventTypeName)
				.ToHashSet();

				List<IGeneralEventModel> filteredEvents = AllEventsListOC
					.Where(x => selectedToFilterEventTypes.Contains(x.EventType.ToString()) &&
								x.StartDateTime.Date >= startDate &&
								x.EndDateTime.Date <= endDate)
					.ToList();

				EventsToShowList = new ObservableCollection<IGeneralEventModel>(filteredEvents);
			}

		#endregion
	}
}
