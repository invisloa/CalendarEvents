using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
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
		public AllEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
		}

		public override void BindDataToScheduleList()
		{
			throw new NotImplementedException();
		}

	}
}
