using CalendarT1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services
{
	/// <summary>
	/// Class that maps priority levels to colors
	/// </summary>
	public class PriorityColorMapper 
	{
		private readonly Dictionary<EnumPriorityLevels, Color> _mapping;

		public PriorityColorMapper()
		{
			_mapping = new Dictionary<EnumPriorityLevels, Color>()
		{
			{ EnumPriorityLevels.Low, Colors.LightGreen },
			{ EnumPriorityLevels.Medium, Colors.BlueViolet },
			{ EnumPriorityLevels.High, Colors.Red },
		};
		}

		public Color GetColor(EnumPriorityLevels priority)
		{
			if (_mapping.TryGetValue(priority, out Color color))
			{
				return color;
			}
			else
			{
				throw new KeyNotFoundException($"No color mapping found for priority level {priority}");
			}
		}
	}
}
