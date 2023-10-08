using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.ViewModels;
using CalendarT1.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCViewModels
{
    internal class UserTypeExtraOptionsViewModel : BaseViewModel, IUserTypeExtraOptionsCC
	{
		public MeasurementSelectorCCViewModel MeasurementSelectorCCHelper { get; set; } = Factory.CreateNewMeasurementSelectorCCHelperClass();
		public UserTypeExtraOptionsViewModel()
		{


			IsValueTypeSelectedCommand = new RelayCommand(() => IsValueTypeSelected = !IsValueTypeSelected);
			IsMicroTaskListTypeSelectedCommand = new RelayCommand(() => IsMictoTasksTypeSelected = !IsMictoTasksTypeSelected);
			IsDefaultTimespanSelectedCommand = new RelayCommand(() => IsDefaultEventTimespanSelected = !IsDefaultEventTimespanSelected);
		}
		/*		
		 *					SelectSubTaskCommand = new RelayCommand<MultiTask>(OnSubTaskSelected);

		 *		DefaultEventTimespanCCHelper.DurationValue = 30;
					DefaultEventTimespanCCHelper.SelectedUnitIndex = 2;
								AddSubTaskEventCommand = new RelayCommand(OnAddSubTaskEventCommand, CanExecuteAddSubTaskEventCommand);
				public RelayCommand<MeasurementUnitItem> MeasurementUnitSelectedCommand { get; set; }
				private bool CanExecuteAddSubTaskEventCommand() => !string.IsNullOrEmpty(SubTaskToAddTitle);

				public DefaultEventTimespanCCViewModel DefaultEventTimespanCCHelper { get; set; } = Factory.CreateNewDefaultEventTimespanCCHelperClass();

				private void OnAddSubTaskEventCommand()
				{
					SubTasksListOC.Add(new MultiTask(SubTaskToAddTitle));
					SubTaskToAddTitle = String.Empty;
				}
				private void OnSubTaskSelected(MultiTask multiTask)
				{
					multiTask.IsSubTaskCompleted = !multiTask.IsSubTaskCompleted;
					OnPropertyChanged(nameof(SubTasksListOC));
				}
				public string SubTaskToAddTitle
				{
					get => _subtaskToAddTitle;
					set
					{
						if (_subtaskToAddTitle != value)
						{
							_subtaskToAddTitle = value;
							OnPropertyChanged();
							AddSubTaskEventCommand.RaiseCanExecuteChanged();
						}
					}
				}

				public TimeSpan DefaultTimespan
		{
			get
			{
				return IsDefaultEventTimespanSelected ? DefaultEventTimespanCCHelper.GetDefaultDuration() : TimeSpan.Zero;
			}
		}
				private string _subtaskToAddTitle;

		public ObservableCollection<MultiTask> SubTasksListOC { get; set; } = new ObservableCollection<MultiTask>();

		public RelayCommand AddSubTaskEventCommand { get; set; }
		public RelayCommand<MultiTask> SelectSubTaskCommand { get; set; }


		*/


		public RelayCommand IsValueTypeSelectedCommand { get; set; }
		public RelayCommand IsMicroTaskListTypeSelectedCommand { get; set; }
		public RelayCommand IsDefaultTimespanSelectedCommand { get; set; }

		private Color _deselectedColor = (Color)Application.Current.Resources["DeselectedBackgroundColor"];
		private bool _isValueTypeSelected;
		private Color _selectedColor = (Color)Application.Current.Resources["MainButtonBackgroundColor"];
		public bool IsValueTypeSelected
		{
			get => _isValueTypeSelected;
			set
			{
				if (_isValueTypeSelected != value)
				{
					_isValueTypeSelected = value;
					OnPropertyChanged(nameof(IsValueTypeSelected));
					OnPropertyChanged(nameof(IsValueTypeColor));
				}
			}
		}

		private bool _isSubTaskListSelected;
		public bool IsMictoTasksTypeSelected
		{
			get => _isSubTaskListSelected;
			set
			{
				if (_isSubTaskListSelected != value)
				{
					_isSubTaskListSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsMicroTaskListTypeColor));
				}
			}
		}
		private bool _isDefaultEventTimespanSelected;
		public bool IsDefaultEventTimespanSelected
		{
			get => _isDefaultEventTimespanSelected;
			set
			{
				if (_isDefaultEventTimespanSelected != value)
				{
					_isDefaultEventTimespanSelected = value;
					OnPropertyChanged();
					OnPropertyChanged(nameof(IsDefaultTimespanColor));
				}
			}
		}


		public Color IsValueTypeColor
		{
			get
			{
				if (IsValueTypeSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
		public Color IsMicroTaskListTypeColor
		{
			get
			{
				if (IsMictoTasksTypeSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
		public Color IsDefaultTimespanColor
		{
			get
			{
				if (IsDefaultEventTimespanSelected)
				{
					return _selectedColor;
				}
				else
				{
					return _deselectedColor;
				}
			}
		}
	}
}
