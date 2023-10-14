using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels
{
	public class SelectableButtonViewModel : BaseViewModel
	{
		private bool _isSelected;
		private int _borderSize = 7;

		public string ButtonText { get; set; }
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				if (_isSelected == value) { return; }
				_isSelected = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(ButtonBorder));

			}
		}
        public SelectableButtonViewModel(){}
        public SelectableButtonViewModel(int bordersize)
        {
			_borderSize = bordersize;
		}
        public Color ButtonColor { get; set; }
		public int ButtonBorder
		{
			get
			{
				if (IsSelected)
				{
					return 0;
				}
				else
				{
					return _borderSize;
				}
			}
		}

		
	}
}
