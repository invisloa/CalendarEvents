using CalendarT1.Models.Enums;
using CalendarT1.Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CalendarT1.Models
{
	public class EventPriority : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}


		private IPriorityColorMapper _priorityColorMapper;
		public IPriorityColorMapper PriorityColorMapper
		{
			get
			{
				return _priorityColorMapper;
			}
		}
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
			_priorityColorMapper = Factory.CreatePriorityColorMapper();
			PriorityLevel = eventPriorityLevel;
			PriorityColor = _priorityColorMapper.GetColor(eventPriorityLevel);
			IsSelected = true;  // All priority levels selected by default
		}
	}
}
