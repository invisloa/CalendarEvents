using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class ScheduleModel
	{
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public Color BackgroundColor { get; set; }
		

		// isChecked??

	}
}
