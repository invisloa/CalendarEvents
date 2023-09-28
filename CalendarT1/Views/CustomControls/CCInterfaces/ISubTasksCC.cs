using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCInterfaces
{
    public interface ISubTasksCC
    {
        ObservableCollection<SubTask> SubTasksOC { get; set; }
        RelayCommand<SubTask> SubTaskCompleteSelectedCommand { get; set; }
		public RelayCommand TaskCompleteSelectedCommand { get; set; }

	}
}
