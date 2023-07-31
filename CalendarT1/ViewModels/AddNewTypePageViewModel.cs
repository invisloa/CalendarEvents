using System.Collections.ObjectModel;

namespace CalendarT1.ViewModels
{
	namespace CalendarT1.ViewModels
	{
		public class AddNewTypePageViewModel : BaseViewModel
		{
			private enum TypeOfEvent
			{
				Event,
				Task,
				Spending
			}
			private const int fullOpacity = 1;
			private const float _fadedOpacity = 0.3f;
			private const int _noBorderSize = 0;
			private const int _borderSize = 10;
			private Color _selectedColor;
			private string _typeName;
			private float _eventOpacity;
			private float _taskOpacity;
			private float _spendingOpacity;
			private string _eventText = "Event";
			private string _taskText = "Task";
			private string _spendingText = "Spending";
			private TypeOfEvent _typeOfEvent = TypeOfEvent.Event;
			public string EventText
			{
				get => _eventText;
				//set
				//{
				//	if (_eventText == value)
				//	{
				//		return;
				//	}
				//	_eventText = value;
				//	OnPropertyChanged();
				//}
			}
			public string TaskText
			{
				get => _taskText;
				//set
				//{
				//	if (_taskText == value)
				//	{
				//		return;
				//	}
				//	_taskText = value;
				//	OnPropertyChanged();
				//}
			}
			public string SpendingText
			{
				get => _spendingText;
				//set
				//{
				//	if (_spendingText == value)
				//	{
				//		return;
				//	}
				//	_spendingText = value;
				//	OnPropertyChanged();
				//}
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
			public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }
			private void SetEventTypeSelected(string typeOfEvent)
			{
				if (Enum.TryParse(typeOfEvent, out TypeOfEvent parsedTypeOfEvent))
				{
					_typeOfEvent = parsedTypeOfEvent;
				}
				else
				{
					// Handle the case where the string could not be parsed to TypeOfEvent
					throw new ArgumentException($"Invalid TypeOfEvent value: {typeOfEvent}");
				}
				SetVisualsForEventTask();
			}
			private void SetVisualsForEventTask()
			{
				SetVisualsForEventType(_eventText, _typeOfEvent.ToString() == _eventText);
				SetVisualsForEventType(_taskText, _typeOfEvent.ToString() == _taskText);
				SetVisualsForEventType(_spendingText, _typeOfEvent.ToString() == _spendingText);
			}
			private void SetVisualsForEventType(string eventType, bool isSelected)
			{
				float opacity = isSelected ? fullOpacity : _fadedOpacity;
				int border = isSelected ? _noBorderSize : _borderSize;

				if (eventType == _eventText)
				{
					EventOpacity = opacity;
					EventBorder = border;
				}
				else if (eventType == _taskText)
				{
					TaskOpacity = opacity;
					TaskBorder = border;
				}
				else if (eventType == _spendingText)
				{
					SpendingOpacity = opacity;
					SpendingBorder = border;
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
			public RelayCommand<ButtonProperties> SelectColorCommand { get; private set; }
			public AddNewTypePageViewModel()
			{
				ColorSelectionCommand = new RelayCommand<string>(OnColorSelection);
				EventTypeSelectedCommand = new RelayCommand<string>(SetEventTypeSelected);
				SelectedColor = Color.FromInt(1234);
				SelectColorCommand = new RelayCommand<ButtonProperties>(SelectColor);
				ButtonsColors = new ObservableCollection<ButtonProperties>
{
					new ButtonProperties { ButtonColor = Color.FromRgb(255, 0, 0), IsSelected = false }, // Red
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 0), IsSelected = false }, // Green
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 255), IsSelected = false }, // Blue
					new ButtonProperties { ButtonColor = Color.FromRgb(255, 0, 0), IsSelected = false }, // Red
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 0), IsSelected = false }, // Green
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 255), IsSelected = false }, // Blue
					new ButtonProperties { ButtonColor = Color.FromRgb(255, 0, 0), IsSelected = false }, // Red
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 255, 0), IsSelected = false }, // Green
					new ButtonProperties { ButtonColor = Color.FromRgb(0, 0, 255), IsSelected = false }, // Blue
				};
				//ResetBorders();

				SetVisualsForEventTask();
			}
			private void ResetBorders()
			{
				foreach (var button in ButtonsColors)
				{
					button.ButtonBorder = _borderSize;
				}
			}
			private void OnColorSelection(string buttonIdentifier)
			{
				foreach (var button in ButtonsColors)
				{
					if (button.ButtonColor.ToString() == buttonIdentifier)
					{
						button.ButtonBorder = _noBorderSize;
					}
					else
					{
						button.ButtonBorder = _borderSize;
					}
				}
			}
			private void SelectColor(ButtonProperties selectedColor)
			{
				SelectedColor = selectedColor.ButtonColor;

				foreach (var button in ButtonsColors)
				{
					button.ButtonBorder = button == selectedColor ? 0 : 10;
				}

				OnPropertyChanged(nameof(SelectedColor));
			}
		}
		public class ButtonProperties
		{
			public Color ButtonColor { get; set; }
			public int ButtonBorder { get; set; } = 10;
			public bool IsSelected { get; set; }
		}
	}
}