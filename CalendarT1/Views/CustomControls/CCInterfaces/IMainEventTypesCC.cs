using CalendarT1.Models.EventTypesModels;
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
		public MainEventTypes SelectedMainEventType { get; set; }
		ObservableCollection<MainEventVisualDetails> MainEventTypesOC { get; set; }
		RelayCommand<MainEventVisualDetails> MainEventTypeSelectedCommand { get; }
		public Color MainEventTypeButtonsColor { get; set; }
		public void DisableVisualsForAllMainEventTypes();
	}
}
