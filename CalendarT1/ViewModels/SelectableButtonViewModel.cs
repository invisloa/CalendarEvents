﻿using System;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{
	public class SelectableButtonViewModel : BaseViewModel
	{
		private bool _isSelected = false;
		private int _borderSize = 7;
		public string ButtonText { get; set; }
		public Color ButtonColor { get; set; }
		public ICommand ButtonCommand { get; set; }

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

		public int ButtonBorder => IsSelected ? 0 : _borderSize;
        public SelectableButtonViewModel(){}

        public SelectableButtonViewModel(string text= null, bool isSelected = false, ICommand selectButtonCommand = null, int borderSize = 7)
		{
			IsSelected = isSelected;
			ButtonText = text;
			ButtonCommand = selectButtonCommand;
			_borderSize = borderSize;
		}
	}
}
