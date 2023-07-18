using CalendarT1.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.EventsSharing
{
	interface IShareEvents
	{
		public Task ShareEventAsync(EventModel eventModel);

		public Task ImportEventAsync(string jsonString);
	}
}
