using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	public abstract class EventOperationsBase : BaseViewModel
	{
		protected string _title;
		protected string _description;
		protected ObservableCollection<EventPriority> _eventPriorities;
		protected EventPriority _eventPriority;
		protected DateTime _startDateTime = DateTime.Now;
		protected DateTime _endDateTime = DateTime.Now.AddHours(1);
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		protected TimeSpan _endExactTime = DateTime.Now.AddHours(1).TimeOfDay;
		protected IEventRepository _eventRepository;
		protected EventModel _currentEvent;
		protected string _submitButtonText;
		protected RelayCommand _submitEventCommand;
		public RelayCommand SubmitEventCommand => _submitEventCommand;

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
				_submitEventCommand.RaiseCanExecuteChanged();

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

		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				_startDateTime = value;
				OnPropertyChanged();
				AdjustEndDateTime();
			}
		}

		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				_endDateTime = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<EventPriority> EventPriorities
		{
			get => _eventPriorities;
			set
			{
				_eventPriorities = value;
				OnPropertyChanged();
			}
		}

		protected void AdjustEndDateTime()
		{
			if (StartDateTime > EndDateTime)
			{
				EndDateTime = StartDateTime.AddHours(1);
			}
		}

		protected void AdjustEndExactTime()
		{
			if (StartExactTime > EndExactTime && StartDateTime.Date >= EndDateTime.Date)
			{
				EndExactTime = StartExactTime + TimeSpan.FromHours(1);
			}
		}

		protected void ClearFields()
		{
			Title = "";
			Description = "";
			EventPriority = EventPriorities[0];
			StartDateTime = DateTime.Now;
			EndDateTime = DateTime.Now.AddHours(1);
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.AddHours(1).TimeOfDay;
		}
	}
}