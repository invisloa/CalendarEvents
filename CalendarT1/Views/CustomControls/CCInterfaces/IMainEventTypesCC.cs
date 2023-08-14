using CalendarT1.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCInterfaces
{
	public interface IMainEventTypesCC
	{
		ObservableCollection<EventVisualDetails> MainEventTypesOC { get; set; }
		RelayCommand<EventVisualDetails> MainEventTypeSelectedCommand { get; }

	}
}
