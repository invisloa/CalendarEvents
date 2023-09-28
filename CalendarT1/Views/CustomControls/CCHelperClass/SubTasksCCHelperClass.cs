using CalendarT1.Models.EventModels;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCHelperClass
{
	public class SubTasksCCHelperClass : ISubTasksCC
	{
        private bool _allSubTasksCompleted;
        public bool AllSubTasksCompleted
        {
			get => _allSubTasksCompleted;
			set
            {
				if (_allSubTasksCompleted == value)
                {
					return;
				}
				_allSubTasksCompleted = value;
			}
		}
		IGeneralEventModel _taskTypeEvent;
		public ObservableCollection<SubTask> SubTasksOC { get; set; }
		public RelayCommand<SubTask> SubTaskCompleteSelectedCommand { get; set; }
		public RelayCommand TaskCompleteSelectedCommand { get; set; }

		public SubTasksCCHelperClass(IGeneralEventModel taskTypeEvent)
        {
			_taskTypeEvent = taskTypeEvent;
			SubTasksOC = new ObservableCollection<SubTask>(taskTypeEvent.SubTasksList);
			SubTaskCompleteSelectedCommand = new RelayCommand<SubTask>(OnSubTaskSelected);
			TaskCompleteSelectedCommand = new RelayCommand(TaskCompleteSelected);
		}
		private void TaskCompleteSelected()
		{
			_taskTypeEvent.IsCompleted = !_taskTypeEvent.IsCompleted;
			foreach (var subTask in SubTasksOC)
			{
				subTask.IsCompleted = _taskTypeEvent.IsCompleted;
			}
		}


		public void OnSubTaskSelected(SubTask subTask)
        {
			subTask.IsCompleted = !subTask.IsCompleted;
			//if subtask was changed to not completed then change maintask to not completed
			if(!subTask.IsCompleted)
			{
				_taskTypeEvent.IsCompleted = false;
				return;
			}
			else
			{
				//check if all subtasks are completed
				foreach (var subTaskItem in SubTasksOC)
				{
					if (!subTaskItem.IsCompleted)
					{
						AllSubTasksCompleted = false;
						return;
					}
				}

			}

			AllSubTasksCompleted = true;

		}
    }
}
