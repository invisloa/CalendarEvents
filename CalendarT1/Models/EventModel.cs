using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class EventModel : AbstractEventModel
	{
		public override bool IsCompleted
		{
			get
			{
				if(EndDateTime < DateTime.Now)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		public EventModel(string title, string description, DateTime startTime, DateTime endTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false) : base(title, description, startTime, endTime, isCompleted, postponeTime, wasShown)
		{
		}
		
	}
}
