using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	public abstract class EventOperationsBase : BaseViewModel
	{
		protected IEventRepository _eventRepository;
		protected EventModel _currentEvent;

		protected string _title;
		protected string _description;
		protected ObservableCollection<EventPriority> _eventPriorities;
		protected EventPriority _eventPriority;

		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;

		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;

		protected string _submitButtonText;
		protected AsyncRelayCommand _submitEventCommand;

		// Basic Event Information
		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				OnPropertyChanged();
				_submitEventCommand.NotifyCanExecuteChanged();
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
		public ObservableCollection<EventPriority> EventPriorities
		{
			get => _eventPriorities;
			set
			{
				_eventPriorities = value;
				OnPropertyChanged();
			}
		}
		// Start Date/Time
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
		// End Date/Time
		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				_endDateTime = value;
				ValidateAndAdjustEndDateTime();
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
		// Submit Event Command
		public string SubmitButtonText
		{
			get => _submitButtonText;
			set
			{
				_submitButtonText = value;
				OnPropertyChanged();
			}
		}
		public AsyncRelayCommand SubmitEventCommand => _submitEventCommand;

		// Helper Methods
		protected void AdjustEndDateTime()
		{
			if (StartDateTime > EndDateTime)
			{
				EndDateTime = StartDateTime;
			}
		}
		protected void AdjustEndExactTime()
		{
			EndExactTime = StartExactTime;
		}
		protected void ValidateAndAdjustEndDateTime()
		{
			if (StartDateTime > EndDateTime)
			{
				EndDateTime = StartDateTime;
			}
			OnPropertyChanged(nameof(EndDateTime));
		}
		protected void ClearFields()
		{
			Title = "";
			Description = "";
			EventPriority = EventPriorities[0];
			StartDateTime = DateTime.Now;
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.TimeOfDay;
		}
	}
}
