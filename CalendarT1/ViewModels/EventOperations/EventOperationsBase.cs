﻿using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using Newtonsoft.Json;
using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels.EventOperations
{
    public abstract class EventOperationsBase : BaseViewModel
	{
		public EventOperationsBase(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserTypeListChanged += UpdateAllEventTypesList;
		}
		public void UpdateAllEventList()
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}
		public void UpdateAllEventTypesList()
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}
		#region Properties
		protected IEventRepository _eventRepository;
		protected IGeneralEventModel _currentEvent;
		protected bool _isCompleted;
		protected string _title;
		protected string _description;
		protected decimal _valueAmount;
		protected DateTime _startDateTime = DateTime.Today;
		protected TimeSpan _startExactTime = DateTime.Now.TimeOfDay;
		protected DateTime _endDateTime = DateTime.Today;
		protected TimeSpan _endExactTime = DateTime.Now.TimeOfDay;
		protected string _submitButtonText;
		protected AsyncRelayCommand _submitEventCommand;
		public string EventTypePickerText { get; set; } = "Select event Type";

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
		public decimal ValueAmount
		{
			get => _valueAmount;
			set
			{
				_valueAmount = value;
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
			if (AllEventTypesOC != null && AllEventTypesOC.Count > 0)
			{
				EventType = AllEventTypesOC[0];
			}
			StartDateTime = DateTime.Today;
			EndDateTime = DateTime.Today;
			StartExactTime = DateTime.Now.TimeOfDay;
			EndExactTime = DateTime.Now.TimeOfDay;
			IsCompleted = false;
		}
		#endregion
		protected ObservableCollection<IUserEventTypeModel> _eventTypesOC;
		public ObservableCollection<IUserEventTypeModel> AllEventTypesOC
		{
			get => _eventTypesOC;
			set
			{
				_eventTypesOC = value;
				OnPropertyChanged();
			}
		}

		protected ObservableCollection<IGeneralEventModel> _allEventsListOC;
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}

		protected IUserEventTypeModel _eventType;
		public IUserEventTypeModel EventType
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
