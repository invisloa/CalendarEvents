using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class HourlyEvents
	{
		public DayOfWeek Day { get; set; }
		public int Hour { get; set; }
		public ObservableCollection<EventModel> Events { get; set; }
	}
}
