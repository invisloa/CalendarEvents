using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
	public abstract class EventOperationsBase : BaseViewModel
	{
		public EventOperationsBase()
		{
			var json = Preferences.Get("event_types", "");
			EventTypesOC = JsonConvert.DeserializeObject<ObservableCollection<EventTypeModel>>(json);
			if (EventTypesOC == null)
			{
				EventTypesOC.Add(new EventTypeModel("BasicEvent", Color.FromHex("#FF0000"), false));
				EventTypesOC.Add(new EventTypeModel("BasicTask", Color.FromHex("#00FFFF"), true));
			}

		}
		#region Properties
		protected IEventRepository _eventRepository;
		protected EventModel _currentEvent;
		protected bool _isCompleted;
		protected string _title;
		protected string _description;


		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;

		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;

		protected string _submitButtonText;
		protected AsyncRelayCommand _submitEventCommand;
		// Basic Event Information
		public bool IsCompleted
		{
			get => _isCompleted;
			set
			{
				_isCompleted = value;
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
		// Start Date/Time
		public DateTime StartDateTime
		{
			get => _startDateTime;
			set
			{
				_startDateTime = value;
				OnPropertyChanged();
				if (_startDateTime > _endDateTime)
				{
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(EndDateTime));
				}
			}
		}

		public TimeSpan StartExactTime
		{
			get => _startExactTime;
			set
			{
				_startExactTime = value;
				OnPropertyChanged();
				if (_startDateTime.Date == _endDateTime.Date && _startExactTime > _endExactTime)
				{
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(EndExactTime));
				}
			}
		}

		public DateTime EndDateTime
		{
			get => _endDateTime;
			set
			{
				if (_startDateTime > value)
				{
					_startDateTime = value;
					_endDateTime = _startDateTime;
					OnPropertyChanged(nameof(StartDateTime));
				}
				else
				{
					_endDateTime = value;
				}
				OnPropertyChanged();
			}
		}

		public TimeSpan EndExactTime
		{
			get => _endExactTime;
			set
			{
				if (_startDateTime.Date == _endDateTime.Date && value < _startExactTime)
				{
					_startExactTime = value;
					_endExactTime = _startExactTime;
					OnPropertyChanged(nameof(StartExactTime));

				}
				else
				{
					_endExactTime = value;
				}
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
		#endregion

		// Helper Methods
		#region Date adjusting methods
		protected void ClearFields()
		{
			Title = "";
			Description = "";
			if (EventTypesOC != null && EventTypesOC.Count > 0)
			{
				EventType = EventTypesOC[0];
			}
			StartDateTime = DateTime.Today;
			EndDateTime = DateTime.Today;
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.TimeOfDay;
			IsCompleted = false;
		}
		#endregion
		protected ObservableCollection<EventTypeModel> _eventTypesOC;
		public ObservableCollection<EventTypeModel> EventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}

		protected EventTypeModel _eventType;
		public EventTypeModel EventType
		{
			get => _eventType;
			set
			{
				_eventType = value;
				OnPropertyChanged();
			}
		}

	}
}
