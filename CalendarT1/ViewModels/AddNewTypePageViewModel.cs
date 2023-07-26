namespace CalendarT1.ViewModels
{
	namespace CalendarT1.ViewModels
	{
		public class AddNewTypePageViewModel : BaseViewModel
		{
			private Color _selectedColor;
			private string _typeName;
			private bool _isTask = false;
			private float _eventOpacity;
			private float _taskOpacity;
			private float _spendingOpacity;
			private int _borderSize = 10;

			private string _eventText = "Event";
			private string _taskText = "Task";
			private string _spendingText = "Spending";

			public string EventText
			{
				get => _eventText;
				set
				{
					if (_eventText == value)
					{
						return;
					}
					_eventText = value;
					OnPropertyChanged();
				}
			}
			public string TaskText
			{
				get => _taskText;
				set
				{
					if (_taskText == value)
					{
						return;
					}
					_taskText = value;
					OnPropertyChanged();
				}
			}
			public string SpendingText
			{
				get => _spendingText;
				set
				{
					if (_spendingText == value)
					{
						return;
					}
					_spendingText = value;
					OnPropertyChanged();
				}
			}
			public float EventOpacity
			{
				get => _eventOpacity;
				set
				{
					if (_eventOpacity == value)
					{
						return;
					}
					_eventOpacity = value;
					OnPropertyChanged();
				}
			}
			public float TaskOpacity
			{
				get => _taskOpacity;
				set
				{
					if (_taskOpacity == value)
					{
						return;
					}
					_taskOpacity = value;
					OnPropertyChanged();
				}
			}
			public float SpendingOpacity
			{
				get => _spendingOpacity;
				set
				{
					if (_spendingOpacity == value)
					{
						return;
					}
					_spendingOpacity = value;
					OnPropertyChanged();
				}
			}
			private void SetEventTypeSelected(string isTask)
			{
				if (isTask == "Task")
				{
					IsTask = true;
				}
				else if (isTask == "Event")
				{
					IsTask = false;
				}
				SetVisualsForEventTask();
			}
			private void SetVisualsForEventTask()
			{
				if (IsTask)
				{
					EventOpacity = 0.3f;
					TaskOpacity = 1;
					EventBorder = 10;
					TaskBorder = 0;
				}
				else if (IsTask)
				{
					EventOpacity = 1;
					TaskOpacity = 0.3f;
					EventBorder = 0;
					TaskBorder = 10;
				}
			}
			public RelayCommand<string> ColorSelectionCommand { get; private set; }
			public RelayCommand<string> EventTypeSelectedCommand { get; private set; }

			public Color SelectedColor
			{
				get => _selectedColor;
				set
				{
					if (_selectedColor == value)
					{
						return;
					}
					_selectedColor = value;
					OnPropertyChanged();
				}
			}

			public string TypeName
			{
				get => _typeName;
				set
				{
					if (_typeName == value)
					{
						return;
					}
					_typeName = value;
					OnPropertyChanged();
				}
			}

			public bool IsTask
			{
				get => _isTask;
				set
				{
					if (_isTask == value)
					{
						return;
					}
					_isTask = value;
					OnPropertyChanged();
				}
			}

			public AddNewTypePageViewModel()
			{
				ResetBorders();
				ColorSelectionCommand = new RelayCommand<string>(OnColorSelection);
				EventTypeSelectedCommand = new RelayCommand<string>(SetEventTypeSelected);
				SelectedColor = StringToHexConverter("1F59A7");
				BlueNBorder = 0;
				SetVisualsForEventTask();

			}
			private Color StringToHexConverter(string hex)
			{
				return Color.FromHex($"#{hex}");
			}
			private void OnColorSelection(string buttonIdentifier)
			{
				ResetBorders();

				switch (buttonIdentifier)
				{
					case "FFB74D":
						OrangeLBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);
						break;
					case "FF9800":
						OrangeNBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "E65100":
						OrangeDBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "FF5252":
						RedLBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "F44336":
						RedNBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "D32F2F":
						RedDBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "64B5F6":
						BlueLBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "1F59A7":
						BlueNBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "0D468F":
						BlueDBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "69F0AE":
						GreenLBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "00E676":
						GreenNBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "00C853":
						GreenDBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "FFEE58":
						YellowLBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "FFEB3B":
						YellowNBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "FDD835":
						YellowDBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "FF80AB":
						PinkLBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "FF4081":
						PinkNBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
					case "F50057":
						PinkDBorder = 0;
						SelectedColor = StringToHexConverter(buttonIdentifier);

						break;
				}
			}

			#region Button Borders 
			private void ResetBorders()
			{
				// Reset all colors and shades
				OrangeLBorder = _borderSize;
				OrangeNBorder = _borderSize;
				OrangeDBorder = _borderSize;
				RedLBorder = _borderSize;
				RedNBorder = _borderSize;
				RedDBorder = _borderSize;
				BlueLBorder = _borderSize;
				BlueNBorder = _borderSize;
				BlueDBorder = _borderSize;
				GreenLBorder = _borderSize;
				GreenNBorder = _borderSize;
				GreenDBorder = _borderSize;
				YellowLBorder = _borderSize;
				YellowNBorder = _borderSize;
				YellowDBorder = _borderSize;
				PinkLBorder = _borderSize;
				PinkNBorder = _borderSize;
				PinkDBorder = _borderSize;
			}
			private int _eventBorder;
			public int EventBorder
			{
				get => _eventBorder;
				set
				{
					if (_eventBorder == value)
					{
						return;
					}
					_eventBorder = value;
					OnPropertyChanged();
				}
			}
			private int _taskBorder;
			public int TaskBorder
			{
				get => _taskBorder;
				set
				{
					if (_taskBorder == value)
					{
						return;
					}
					_taskBorder = value;
					OnPropertyChanged();
				}
			}
			private int _spendingBorder;
			public int SpendingBorder
			{
				get => _spendingBorder;
				set
				{
					if (_spendingBorder == value)
					{
						return;
					}
					_spendingBorder = value;
					OnPropertyChanged();
				}
			}


			private int _orangeLBorder;
			public int OrangeLBorder
			{
				get => _orangeLBorder;
				set
				{
					if (_orangeLBorder == value)
					{
						return;
					}
					_orangeLBorder = value;
					OnPropertyChanged();
				}
			}

			private int _orangeNBorder;
			public int OrangeNBorder
			{
				get => _orangeNBorder;
				set
				{
					if (_orangeNBorder == value)
					{
						return;
					}
					_orangeNBorder = value;
					OnPropertyChanged();
				}
			}

			private int _orangeDBorder;
			public int OrangeDBorder
			{
				get => _orangeDBorder;
				set
				{
					if (_orangeDBorder == value)
					{
						return;
					}
					_orangeDBorder = value;
					OnPropertyChanged();
				}
			}

			// Properties for Red color
			private int _redLBorder;
			public int RedLBorder
			{
				get => _redLBorder;
				set
				{
					if (_redLBorder == value)
					{
						return;
					}
					_redLBorder = value;
					OnPropertyChanged();
				}
			}

			private int _redNBorder;
			public int RedNBorder
			{
				get => _redNBorder;
				set
				{
					if (_redNBorder == value)
					{
						return;
					}
					_redNBorder = value;
					OnPropertyChanged();
				}
			}

			private int _redDBorder;
			public int RedDBorder
			{
				get => _redDBorder;
				set
				{
					if (_redDBorder == value)
					{
						return;
					}
					_redDBorder = value;
					OnPropertyChanged();
				}
			}

			// Properties for Blue color
			private int _blueLBorder;
			public int BlueLBorder
			{
				get => _blueLBorder;
				set
				{
					if (_blueLBorder == value)
					{
						return;
					}
					_blueLBorder = value;
					OnPropertyChanged();
				}
			}

			private int _blueNBorder;
			public int BlueNBorder
			{
				get => _blueNBorder;
				set
				{
					if (_blueNBorder == value)
					{
						return;
					}
					_blueNBorder = value;
					OnPropertyChanged();
				}
			}

			private int _blueDBorder;
			public int BlueDBorder
			{
				get => _blueDBorder;
				set
				{
					if (_blueDBorder == value)
					{
						return;
					}
					_blueDBorder = value;
					OnPropertyChanged();
				}
			}

			// Properties for Green color
			private int _greenLBorder;
			public int GreenLBorder
			{
				get => _greenLBorder;
				set
				{
					if (_greenLBorder == value)
					{
						return;
					}
					_greenLBorder = value;
					OnPropertyChanged();
				}
			}

			private int _greenNBorder;
			public int GreenNBorder
			{
				get => _greenNBorder;
				set
				{
					if (_greenNBorder == value)
					{
						return;
					}
					_greenNBorder = value;
					OnPropertyChanged();
				}
			}

			private int _greenDBorder;
			public int GreenDBorder
			{
				get => _greenDBorder;
				set
				{
					if (_greenDBorder == value)
					{
						return;
					}
					_greenDBorder = value;
					OnPropertyChanged();
				}
			}

			// Properties for Yellow color
			private int _yellowLBorder;
			public int YellowLBorder
			{
				get => _yellowLBorder;
				set
				{
					if (_yellowLBorder == value)
					{
						return;
					}
					_yellowLBorder = value;
					OnPropertyChanged();
				}
			}

			private int _yellowNBorder;
			public int YellowNBorder
			{
				get => _yellowNBorder;
				set
				{
					if (_yellowNBorder == value)
					{
						return;
					}
					_yellowNBorder = value;
					OnPropertyChanged();
				}
			}

			private int _yellowDBorder;
			public int YellowDBorder
			{
				get => _yellowDBorder;
				set
				{
					if (_yellowDBorder == value)
					{
						return;
					}
					_yellowDBorder = value;
					OnPropertyChanged();
				}
			}

			// Properties for Pink color
			private int _pinkLBorder;
			public int PinkLBorder
			{
				get => _pinkLBorder;
				set
				{
					if (_pinkLBorder == value)
					{
						return;
					}
					_pinkLBorder = value;
					OnPropertyChanged();
				}
			}

			private int _pinkNBorder;
			public int PinkNBorder
			{
				get => _pinkNBorder;
				set
				{
					if (_pinkNBorder == value)
					{
						return;
					}
					_pinkNBorder = value;
					OnPropertyChanged();
				}
			}

			private int _pinkDBorder;
			public int PinkDBorder
			{
				get => _pinkDBorder;
				set
				{
					if (_pinkDBorder == value)
					{
						return;
					}
					_pinkDBorder = value;
					OnPropertyChanged();
				}
			}

			#endregion
		}
	}
}