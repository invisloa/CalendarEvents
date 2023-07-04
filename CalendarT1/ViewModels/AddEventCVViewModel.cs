using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
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
		private ObservableCollection<EventPriority> _eventPriorities;
		private EventPriority _eventPriority;
		private DateTime _startDateTime = DateTime.Now;
		private DateTime _endDateTime = DateTime.Now.AddHours(1);
		private TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		private TimeSpan _endExactTime = DateTime.Now.AddHours(1).TimeOfDay;
		private IEventRepository _eventRepository;
		private EventModel _currentEvent;
		private string _submitButtonText;
		public string SubmitButtonText
		{
			get => _submitButtonText;
			set
			{
				_submitButtonText = value;
				OnPropertyChanged();
			}
		}
		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				_startExactTime = value;
				OnPropertyChanged();
				AdjustEndExactTime();
			}
		}
		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				_endExactTime = value;
				OnPropertyChanged();
			}
		}
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged();
				AddEventCommand.RaiseCanExecuteChanged();
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
				AddEventCommand.RaiseCanExecuteChanged();
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
		private RelayCommand _addEventCommand;
		public ObservableCollection<EventPriority> EventPriorities
		{
			get => _eventPriorities;
			set
			{
				_eventPriorities = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand AddEventCommand
		{
			get
			{
				return _addEventCommand ?? (_addEventCommand = new RelayCommand(
						execute: () =>
						{
							if (_currentEvent == null) 
							{
								_currentEvent = new EventModel(Title, Description, EventPriority,StartDateTime+StartExactTime,EndDateTime+EndExactTime);
								_eventRepository.AddEvent(_currentEvent);
								Title = "";
								Description = "";
								EventPriority = EventPriorities[0];
								StartDateTime = DateTime.Now;
								EndDateTime = DateTime.Now.AddHours(1);
								StartExactTime = DateTime.Now.TimeOfDay;
								EndExactTime = DateTime.Now.AddHours(1).TimeOfDay;
							}
							else
							{
								// Update fields
								_currentEvent.Title = Title;
								_currentEvent.Description = Description;
								_currentEvent.PriorityLevel = EventPriority;
								_currentEvent.StartDateTime = StartDateTime.Date + StartExactTime;
								_currentEvent.EndDateTime = EndDateTime.Date + EndExactTime;
								_currentEvent.IsCompleted = false;
								_eventRepository.UpdateEvent(_currentEvent);
								Shell.Current.GoToAsync("..");
							}
                    },
					canExecute: () =>
					{
						return !string.IsNullOrEmpty(Title);
					}));
			}
		}
		public AddEventCVViewModel(EventModel eventToEdit = null)
		{
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;

			if (eventToEdit == null) // Create new Event
			{
				ClearFields();
				SubmitButtonText = "Add Event";
			}
			else // Edit existing Event
			{
				_currentEvent = eventToEdit;
				Title = _currentEvent.Title;
				Description = _currentEvent.Description;
				EventPriority = _currentEvent.PriorityLevel;
				StartDateTime = _currentEvent.StartDateTime.Date;
				EndDateTime = _currentEvent.EndDateTime.Date;
				StartExactTime = _currentEvent.StartDateTime.TimeOfDay;
				EndExactTime = _currentEvent.EndDateTime.TimeOfDay;
				SubmitButtonText = "Edit Event";
			}
		}
		#region Commands
		private void ClearFields()
		{
			Title = "";
			Description = "";
			EventPriority = EventPriorities[0];
			StartDateTime = DateTime.Now;
			EndDateTime = DateTime.Now.AddHours(1);
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.AddHours(1).TimeOfDay;
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
			if (StartExactTime > EndExactTime && StartDateTime.Date >= EndDateTime.Date)
			{
				EndExactTime = StartExactTime + TimeSpan.FromHours(1);
			}
		}
		#endregion
	}
}