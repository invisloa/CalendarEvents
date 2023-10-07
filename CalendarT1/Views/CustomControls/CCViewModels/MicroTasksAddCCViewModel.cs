using CalendarT1.Models.EventModels;
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
	public class MicroTasksAddCCViewModel : BaseViewModel, IMicroTasksAddCC
	{
		List<MicroTaskModel> _listToAddMicroTasks;
		public MicroTasksAddCCViewModel(List<MicroTaskModel> listToAddMicroTasks)
		{
			_listToAddMicroTasks = listToAddMicroTasks;
			AddMicroTaskEventCommand = new RelayCommand(AddMicroTaskEvent, CanAddMicroTaskEvent);
		}
		private string _microTaskToAddName;
		public string MicroTaskToAddName { get => _microTaskToAddName;
			set
			{
				if (_microTaskToAddName == value) { return; }
				_microTaskToAddName = value;
				OnPropertyChanged();
				AddMicroTaskEventCommand.RaiseCanExecuteChanged();
			}
		}
		public RelayCommand AddMicroTaskEventCommand { get; set; }
		public void AddMicroTaskEvent()
		{
			_listToAddMicroTasks.Add(new MicroTaskModel(MicroTaskToAddName));
			MicroTaskToAddName = "";
		}
		public bool CanAddMicroTaskEvent() => !string.IsNullOrWhiteSpace(MicroTaskToAddName);

	}
}
