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
    public class MicroTasksListCCViewModel : BaseViewModel, IMicroTasksCC
	{
		List<MicroTaskModel> _listToAddMicroTasks;
		public MicroTasksListCCViewModel(List<MicroTaskModel> listToAddMicroTasks)
		{
			MicroTasksOC = new ObservableCollection<MicroTaskModel>(listToAddMicroTasks);
			AddMicroTaskEventCommand = new RelayCommand(AddMicroTaskEvent, CanAddMicroTaskEvent);
		}
		private string _microTaskToAddName;
		public string MicroTaskToAddName
		{
			get => _microTaskToAddName;
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
			MicroTasksOC.Add(new MicroTaskModel(MicroTaskToAddName));
			MicroTaskToAddName = "";
		}
		public bool CanAddMicroTaskEvent() => !string.IsNullOrWhiteSpace(MicroTaskToAddName);
		private bool _allMicroTasksCompleted;
        public bool AllMicroTasksCompleted
        {
			get => _allMicroTasksCompleted;
			set
            {
				if (_allMicroTasksCompleted == value)
                {
					return;
				}
				_allMicroTasksCompleted = value;
			}
		}
		IGeneralEventModel _taskTypeEvent;
		public ObservableCollection<MicroTaskModel> MicroTasksOC { get; set; }
		public RelayCommand<MicroTaskModel> SelectMicroTaskCommand { get; set; }
		public MicroTasksListCCViewModel(IGeneralEventModel taskTypeEvent) //taskTypeEvent contains its own MicroTasksList
        {
			_taskTypeEvent = taskTypeEvent;
			MicroTasksOC = new ObservableCollection<MicroTaskModel>(taskTypeEvent.MicroTasksList);
			SelectMicroTaskCommand = new RelayCommand<MicroTaskModel>(OnMultiTaskSelected);
		}

		public void OnMultiTaskSelected(MicroTaskModel microTask)
        {
			microTask.IsMicroTaskCompleted = !microTask.IsMicroTaskCompleted;
			//if MicroTask was changed to not completed then change parenteventtask to not completed
			if (!microTask.IsMicroTaskCompleted)
			{
				_taskTypeEvent.IsCompleted = false;
				return;
			}
			else
			{
				//check if all MicroTasks are completed
				foreach (var microTaskItem in MicroTasksOC)
				{
					if (!microTask.IsMicroTaskCompleted)
					{
						AllMicroTasksCompleted = false;
						return;
					}
				}

			}
			AllMicroTasksCompleted = true;
		}
    }
}



//public void CompleteAllSubTasks()
//{
//	_taskTypeEvent.IsCompleted = !_taskTypeEvent.IsCompleted;
//	foreach (var multiTask in MicroTasksOC)
//	{
//		multiTask.IsMicroTaskCompleted = _taskTypeEvent.IsCompleted;
//	}
//}
