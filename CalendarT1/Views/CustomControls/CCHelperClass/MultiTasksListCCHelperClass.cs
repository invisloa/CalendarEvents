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
	public class MultiTasksListCCHelperClass : IMultiTasksCC
	{
        private bool _allMultiTasksCompleted;
        public bool AllMultiTasksCompleted
        {
			get => _allMultiTasksCompleted;
			set
            {
				if (_allMultiTasksCompleted == value)
                {
					return;
				}
				_allMultiTasksCompleted = value;
			}
		}
		IGeneralEventModel _taskTypeEvent;
		public ObservableCollection<MultiTask> MultiTasksOC { get; set; }
		public RelayCommand<MultiTask> MultiTaskCompleteSelectedCommand { get; set; }
		public RelayCommand SubTaskCompleteSelectedCommand { get; set; }

		public MultiTasksListCCHelperClass(IGeneralEventModel taskTypeEvent)
        {
			_taskTypeEvent = taskTypeEvent;
			MultiTasksOC = new ObservableCollection<MultiTask>(taskTypeEvent.MultiTasksList);
			MultiTaskCompleteSelectedCommand = new RelayCommand<MultiTask>(OnMultiTaskSelected);
			SubTaskCompleteSelectedCommand = new RelayCommand(OnSubTaskCompleteSelected);
		}
		private void OnSubTaskCompleteSelected()
		{
			_taskTypeEvent.IsCompleted = !_taskTypeEvent.IsCompleted;
			foreach (var multiTask in MultiTasksOC)
			{
				multiTask.IsSubTaskCompleted = _taskTypeEvent.IsCompleted;
			}
		}


		public void OnMultiTaskSelected(MultiTask multiTask)
        {
			multiTask.IsSubTaskCompleted = !multiTask.IsSubTaskCompleted;
			//if multiTask was changed to not completed then change maintask to not completed
			if (!multiTask.IsSubTaskCompleted)
			{
				_taskTypeEvent.IsCompleted = false;
				return;
			}
			else
			{
				//check if all multiTask are completed
				foreach (var multiTaskItem in MultiTasksOC)
				{
					if (!multiTask.IsSubTaskCompleted)
					{
						AllMultiTasksCompleted = false;
						return;
					}
				}

			}

			AllMultiTasksCompleted = true;

		}
    }
}
