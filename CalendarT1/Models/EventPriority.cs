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
					return Color.FromRgb(0, 255, 0); // Green
				case EnumPriorityLevels.Low:
					return Color.FromRgb(255, 255, 0); // Yellow
				case EnumPriorityLevels.Medium:
					return Color.FromRgb(0, 0, 255); // Blue
				case EnumPriorityLevels.High:
					return Color.FromRgb(238, 130, 238); // Violet
				case EnumPriorityLevels.Highest:
					return Color.FromRgb(255, 0, 255); // Magenta
				default:
					return Colors.White; // Default color (white)
			}
		}
	}
}
