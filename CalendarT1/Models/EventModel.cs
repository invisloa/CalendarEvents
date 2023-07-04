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
		public Guid Id { get; set; }
		public DateTime StartDateTime { get; set; }
		public DateTime EndDateTime { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public EventPriority PriorityLevel { get; set; }
		public bool IsCompleted { get; set; }
		public EventModel(string title, string description, EventPriority eventPriority, DateTime startTime, DateTime endTime, bool isCompleted=false )
		{
			Id = Guid.NewGuid();
			Title = title;
			Description = description;
			PriorityLevel = eventPriority;
			StartDateTime = startTime;
			EndDateTime = endTime;
			IsCompleted = isCompleted;
		}
	}
}
