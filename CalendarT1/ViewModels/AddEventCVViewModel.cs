using CalendarT1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{
	class AddEventCVViewModel : BaseViewModel
	{
		private string _title;
		private string _description;
		private EventPriority _eventPriority;
		private ObservableCollection<EventPriority> _eventPriorities;
		private DateTime _startDateTime = DateTime.Now;
		private DateTime _endDateTime = DateTime.Now.AddHours(1);
		private TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		private TimeSpan _endExactTime = DateTime.Now.AddHours(1).TimeOfDay;

		//create a property for the start time
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				_startExactTime = value;
				OnPropertyChanged();
			}
		}
		//create property for the end time
		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				_endExactTime = value;
				OnPropertyChanged();
				AdjustEndExactTime();
			}
		}

		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged();
			}
		}
		public string Description
		{
			get => _description;
			set
			{
				_description = value;
				OnPropertyChanged();
			}
		}
		public EventPriority EventPriority
		{
			get => _eventPriority;
			set
			{
				_eventPriority = value;
				OnPropertyChanged();
			}
		}

		public DateTime StartDateTime { get => _startDateTime; set
			{
				_startDateTime = value;
				OnPropertyChanged();
				AdjustEndDateTime(); // EndDateTime=StartDateTime + 1 hour
			}
		}
		public DateTime EndDateTime { get => _endDateTime; set
			{
				_endDateTime = value;
				OnPropertyChanged();
			}
		}

		private ICommand _addEventCommand;
		public ObservableCollection<EventPriority> EventPriorities
		{
			get => _eventPriorities;
			set
			{
				_eventPriorities = value;
				OnPropertyChanged();
			}
		}

		public ICommand AddEventCommand
		{
			get
			{
				return _addEventCommand ?? (_addEventCommand = new Command(
					execute: () =>
					{
						// You will need to add the logic to add the event.
					},
					canExecute: () =>
					{
						// You can add a condition to check if all required data is filled before adding the event.
						return !string.IsNullOrEmpty(Title) && EventPriority != null;
					}));
			}
		}


		public AddEventCVViewModel()
		{
			// Initialize the EventPriorities collection
			// _eventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevels()); (example)
		}

		private void AdjustEndDateTime()
		{
			if (StartDateTime > EndDateTime)
			{
				EndDateTime = StartDateTime.AddHours(1);
			}
		}
		
		private void AdjustEndExactTime()
		{
			if (StartExactTime > EndExactTime && StartDateTime.Date <= EndDateTime.Date)
			{
				EndExactTime = StartExactTime.Add(new TimeSpan(1,0,0));
			}
		}
	}
}