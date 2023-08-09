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
		#region Properties

		private DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate => _currentDate;

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
					//DatePickerDateSelectedCommand.Execute(_currentSelectedDate); // TODO: check if this is the right way to do it
				}
			}
		}

		#endregion

		#region Fields

		private RelayCommand<DateTime> _datePickerDateSelectedCommand;

		#endregion

		#region Constructor

		public AbstractEventViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
		}

		#endregion

		#region Commands

		public RelayCommand<DateTime> DatePickerDateSelectedCommand =>
			_datePickerDateSelectedCommand ?? (_datePickerDateSelectedCommand = new RelayCommand<DateTime>(DatePickerDateSelected));

		#endregion

		#region Private Methods

		private void DatePickerDateSelected(DateTime newDate)
		{
			CurrentSelectedDate = newDate;
			BindDataToScheduleList();
		}

		#endregion

		#region Protected Methods

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

		#endregion
	}
}
