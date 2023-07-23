using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.Maui.Graphics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{
	namespace CalendarT1.ViewModels
	{
		public class AddNewTypePageViewModel : BaseViewModel
		{
			private string _selectedColor;
			private string _typeName;
			private bool _isTask;

			public RelayCommand<string> ColorSelectionCommand { get; private set; }
			public RelayCommand<string> EventTypeSelectedCommand { get; private set; }
			private void SetEventTypeSelected(string isTask)
			{
				if (isTask == "Task") 
				{
					IsTask = true;
				}
				else
				{
					IsTask = false;
				}
			}
			public string SelectedColor
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
			}
			private void OnColorSelection(string buttonIdentifier)
			{
				ResetBorders();

				switch (buttonIdentifier)
				{
					case "OrangeLight":
						OrangeLBorder = 0;
						break;
					case "OrangeNormal":
						OrangeNBorder = 0;
						break;
					case "OrangeDark":
						OrangeDBorder = 0;
						break;
					// Add more cases for other colors and shades
					case "RedLight":
						RedLBorder = 0;
						break;
					case "RedNormal":
						RedNBorder = 0;
						break;
					case "RedDark":
						RedDBorder = 0;
						break;
					case "BlueLight":
						BlueLBorder = 0;
						break;
					case "BlueNormal":
						BlueNBorder = 0;
						break;
					case "BlueDark":
						BlueDBorder = 0;
						break;
					case "GreenLight":
						GreenLBorder = 0;
						break;
					case "GreenNormal":
						GreenNBorder = 0;
						break;
					case "GreenDark":
						GreenDBorder = 0;
						break;
					case "YellowLight":
						YellowLBorder = 0;
						break;
					case "YellowNormal":
						YellowNBorder = 0;
						break;
					case "YellowDark":
						YellowDBorder = 0;
						break;
					case "PinkLight":
						PinkLBorder = 0;
						break;
					case "PinkNormal":
						PinkNBorder = 0;
						break;
					case "PinkDark":
						PinkDBorder = 0;
						break;
				}
			}


			#region Button Borders 
			private void ResetBorders()
			{
				// Reset all colors and shades
				OrangeLBorder = 10;
				OrangeNBorder = 10;
				OrangeDBorder = 10;
				RedLBorder = 10;
				RedNBorder = 10;
				RedDBorder = 10;
				BlueLBorder = 10;
				BlueNBorder = 10;
				BlueDBorder = 10;
				GreenLBorder = 10;
				GreenNBorder = 10;
				GreenDBorder = 10;
				YellowLBorder = 10;
				YellowNBorder = 10;
				YellowDBorder = 10;
				PinkLBorder = 10;
				PinkNBorder = 10;
				PinkDBorder = 10;
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