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
	public class PriorityColorMapper : IPriorityColorMapper
	{
		private readonly Dictionary<EnumPriorityLevels, Color> _mapping;

		public PriorityColorMapper()
		{
			_mapping = new Dictionary<EnumPriorityLevels, Color>()
		{
			{ EnumPriorityLevels.Lowest, Colors.LightSeaGreen },
			{ EnumPriorityLevels.Low, Colors.Green },
			{ EnumPriorityLevels.Medium, Colors.Blue },
			{ EnumPriorityLevels.High, Colors.Violet },
			{ EnumPriorityLevels.Highest, Colors.Magenta }
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
