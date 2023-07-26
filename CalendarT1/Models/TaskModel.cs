using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class TaskModel : AbstractEventModel
	{
		// STATUS : DONE ...
		// Assigned to ...

		public TaskModel(string title, string description, DateTime startTime, DateTime endTime, bool isCompleted = false, DateTime? postponeTime = null, bool wasShown = false) : base(title, description, startTime, endTime, isCompleted, postponeTime, wasShown)
		{ }
	}
}
