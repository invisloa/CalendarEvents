using CalendarT1.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class EventPriority : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		// write properties for priority levels and colors
		public EnumPriorityLevels PriorityLevel { get; set; }
		public Color PriorityColor { get; set; }

		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected != value)
				{
					_isSelected = value;
					OnPropertyChanged();
				}
			}
		}
		public override string ToString()
		{
			return PriorityLevel.ToString(); // Return the string representation of the PriorityLevel enum
		}
		public EventPriority(EnumPriorityLevels eventPriorityLevel)
		{
			PriorityLevel = eventPriorityLevel;
			PriorityColor = AssignColorToPriorityLevel(eventPriorityLevel);
			IsSelected = true;  // All priority levels selected by default

		}

		// write method that will assign color to priority level
		public Color AssignColorToPriorityLevel(EnumPriorityLevels eventPriorityLevel)
		{
			switch (eventPriorityLevel)
			{
				case EnumPriorityLevels.Lowest:
					return Colors.LightSeaGreen;
				case EnumPriorityLevels.Low:
					return Colors.Green;
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
