using CalendarT1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventsSharing
{
	internal class ShareEventICSFile : IShareEvents
	{
		public Task ImportEventAsync(string jsonString)
		{
			throw new NotImplementedException();
		}

		public Task ShareEventAsync(EventModel eventModel)
		{
			throw new NotImplementedException();
		}
	}
}