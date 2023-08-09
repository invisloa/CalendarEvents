using Android.Service.Autofill;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	internal class AllEventsViewModel : AbstractEventViewModel
	{
		#region Fields

		private IUserEventTypeModel _eventType;

		#endregion

		#region Properties

		public AsyncRelayCommand DeleteAllEventsCommand { get; set; } // for testing purposes
		public AsyncRelayCommand DeleteOneEventCommand { get; set; }  // for testing purposes

		#endregion

		#region Constructor

		public AllEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
			_eventType = eventType;

			var allTempTypes = new List<IUserEventTypeModel>();
			foreach (var item in AllEventTypesOC)
			{
				allTempTypes.Add(item);
			}

			DeleteOneEventCommand = new AsyncRelayCommand(DeleteOneEvent); // for testing purposes
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteAllEvents); // for testing purposes
		}

		#endregion

		#region Commands

		public async Task DeleteOneEvent()
		{
			if (AllEventsListOC.Count == 0)
			{
				return;
			}
			var firstEvent = AllEventsListOC[0];
			await EventRepository.DeleteFromEventsListAsync(firstEvent);
		}

		public async Task DeleteAllEvents()
		{
			await EventRepository.ClearEventsListAsync();
		}

		#endregion

		#region Override Methods

		public override void BindDataToScheduleList()
		{
			throw new NotImplementedException();
		}

		#endregion
	}
}
