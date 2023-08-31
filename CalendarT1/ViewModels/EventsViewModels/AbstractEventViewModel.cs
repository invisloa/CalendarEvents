using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using CalendarT1.Views.CustomControls.CCHelperClass;
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
		private RelayCommand _goToAddEventPageCommand;
		private DateTime _currentSelectedDate = DateTime.Now;
		private RelayCommand _goToAddNewTypePageCommand;
		public RelayCommand<DateTime> GoToSelectedDateCommand { get; set; }

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

			GoToSelectedDateCommand = new RelayCommand<DateTime>(GoToSelectedDatePage);
		}

		#endregion

		#region Commands
		public RelayCommand GoToAddEventPageCommand => _goToAddEventPageCommand ?? (_goToAddEventPageCommand = new RelayCommand(GoToAddEventPage));
		public RelayCommand GoToAddNewTypePageCommand => _goToAddNewTypePageCommand ?? (_goToAddNewTypePageCommand = new RelayCommand(GoToAddNewTypePage));
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
		private void GoToSelectedDatePage(DateTime selectedDate)
		{
			var _dailyEventsPage = new ViewDailyEvents();
			var _dailyEventsPageBindingContext = _dailyEventsPage.BindingContext as DailyEventsViewModel;
			_dailyEventsPageBindingContext.CurrentSelectedDate = selectedDate;
			Application.Current.MainPage.Navigation.PushAsync(_dailyEventsPage);
		}
		#endregion

		#region Protected Methods



		#endregion
	}
}
