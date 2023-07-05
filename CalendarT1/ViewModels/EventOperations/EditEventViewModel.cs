using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels
{

	public class EditEventViewModel : EventOperationsBase
	{
		private RelayCommand _submitEventCommand;
		public RelayCommand SubmitEventCommand
		{
			get
			{
				return _submitEventCommand ?? (_submitEventCommand = new RelayCommand(
						execute: () =>
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

						},
					canExecute: () =>
					{
						return !string.IsNullOrEmpty(Title);
					}));
			}
		}
		public EditEventViewModel(EventModel eventToEdit)
		{
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
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


}