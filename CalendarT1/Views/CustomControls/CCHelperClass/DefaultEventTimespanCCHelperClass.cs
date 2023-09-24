using CalendarT1.ViewModels;
using CalendarT1.ViewModels.EventsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCHelperClass
{
	public class DefaultEventTimespanCCHelperClass : BaseViewModel
	{
		private int _selectedUnitIndex;
		public int SelectedUnitIndex
		{
			get => _selectedUnitIndex;
			set
			{
				_selectedUnitIndex = value;
				OnPropertyChanged(nameof(SelectedUnitIndex));
			}
		}

		private double _durationValue;
		public double DurationValue
		{
			get => _durationValue;
			set
			{
				_durationValue = value;
				OnPropertyChanged(nameof(DurationValue));
			}
		}

		public TimeSpan GetDefaultDuration()
		{
			switch (SelectedUnitIndex)
			{
				case 0: // Days
					return TimeSpan.FromDays(DurationValue);
				case 1: // Hours
					return TimeSpan.FromHours(DurationValue);
				case 2: // Minutes
					return TimeSpan.FromMinutes(DurationValue);
				case 3: // Seconds
					return TimeSpan.FromSeconds(DurationValue);
				default:
					return TimeSpan.Zero;
			}
		}
	}
}
