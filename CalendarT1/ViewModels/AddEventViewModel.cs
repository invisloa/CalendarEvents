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
	class AddEventViewModel : EventOperationsBase
	{
		private RelayCommand _addEventCommand;
		public RelayCommand AddEventCommand
		{
			get
			{
				return _addEventCommand ?? (_addEventCommand = new RelayCommand(
						execute: () =>
						{

							_currentEvent = new EventModel(Title, Description, EventPriority, StartDateTime + StartExactTime, EndDateTime + EndExactTime);
							_eventRepository.AddEvent(_currentEvent);
							Title = "";
							Description = "";
							EventPriority = EventPriorities[0];
							StartDateTime = DateTime.Now;
							EndDateTime = DateTime.Now.AddHours(1);
							StartExactTime = DateTime.Now.TimeOfDay;
							EndExactTime = DateTime.Now.AddHours(1).TimeOfDay;

						},
						canExecute: () =>
						{
							return !string.IsNullOrEmpty(Title);
						}));
			}
		}
		public AddEventViewModel()
		{
			EventPriorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());
			_eventRepository = Factory.EventRepository;
			ClearFields();
			SubmitButtonText = "Add Event";
		}
	}
}