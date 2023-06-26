using CalendarT1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class EventPriority
	{
		// write properties for priority levels and colors
		public EnumPriorityLevels PriorityLevel { get; set; }
		public Color PriorityColor { get; set; }

		public EventPriority(EnumPriorityLevels eventPriorityLevel)
		{
			PriorityLevel = eventPriorityLevel;
			PriorityColor = AssignColorToPriorityLevel(eventPriorityLevel);
		}

		// write method that will assign color to priority level
		public Color AssignColorToPriorityLevel(EnumPriorityLevels eventPriorityLevel)
		{
			switch (eventPriorityLevel)
			{
				case EnumPriorityLevels.Lowest:
					return Colors.Green;
				case EnumPriorityLevels.Low:
					return Colors.Yellow;
				case EnumPriorityLevels.Medium:
					return Colors.Blue;
				case EnumPriorityLevels.High:
					return Colors.Violet;
				case EnumPriorityLevels.Highest:
					return Colors.Magenta;
				default:
					return Colors.White;
			}
		}
	}
}
