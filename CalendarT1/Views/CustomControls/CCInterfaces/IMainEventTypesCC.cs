﻿using CalendarT1.Models.EventTypesModels;
using CalendarT1.Views.CustomControls.CCHelperClass;
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
		ObservableCollection<MainEventVisualDetails> MainEventTypesVisualsOC { get; set; }
		RelayCommand<MainEventVisualDetails> MainEventTypeSelectedCommand { get; }
		public Color MainEventTypeButtonsColor { get; set; } 
		public void DisableVisualsForAllMainEventTypes();
		public event Action<IMainEventType> MainEventTypeChanged;
	}
}
