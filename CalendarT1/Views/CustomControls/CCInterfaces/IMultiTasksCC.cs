using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCInterfaces
{
    public interface IMultiTasksCC


    {
        ObservableCollection<MultiTask> MultiTasksOC { get; set; }
        RelayCommand<MultiTask> MultiTaskCompleteSelectedCommand { get; set; }
		public RelayCommand SubTaskCompleteSelectedCommand { get; set; }

	}
}
