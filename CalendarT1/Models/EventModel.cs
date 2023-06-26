using CalendarT1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class EventModel

	{
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public EnumColors PriorityLevel { get; set; }
		public bool IsCompleted { get; set; }


	}
}
