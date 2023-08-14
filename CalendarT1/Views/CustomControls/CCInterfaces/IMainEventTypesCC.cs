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
		public const int FullOpacity = 1;
		public const float FadedOpacity = 0.3f;
		public const int NoBorderSize = 0;
		public const int BorderSize = 10;
		public string MainEventNameText { get; set; }
		public Color MainEventTypesColor { get; set; }
		public RelayCommand MainEventTypeSelectedCommand { get; set; }
		public ObservableCollection<IUserEventTypeModel> MainEventTypesOC { get; set; }
		public void InitializeMainEventTypes();


	}
}
