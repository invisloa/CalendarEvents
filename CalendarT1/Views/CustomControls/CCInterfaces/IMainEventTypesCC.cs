using CalendarT1.Models.EventTypesModels;
using CalendarT1.Views.CustomControls.CCViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCInterfaces
{

    /// <summary>
    /// When using this interface consider using MainEventTypesCCHelper class
    /// MainEventTypesCCHelper implements this interface and helps to set the logic for control operations 
    /// </summary>
    public interface IMainEventTypesCC
	{
		public IMainEventType SelectedMainEventType { get; set; }
		ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get; set; }
		RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; }
		public void DisableVisualsForAllMainEventTypes();
		public event Action<IMainEventType> MainEventTypeChanged;
	}
}
