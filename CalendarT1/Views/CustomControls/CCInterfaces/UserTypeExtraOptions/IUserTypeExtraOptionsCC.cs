using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Views.CustomControls.CCViewModels;
using System.Collections.ObjectModel;

namespace CalendarT1.Views.CustomControls.CCInterfaces.UserTypeExtraOptions
{
    public interface IUserTypeExtraOptionsCC 
	{
        bool IsDefaultEventTimespanSelected { get; set; }
        Color IsDefaultTimespanColor { get; }
        RelayCommand IsDefaultTimespanSelectedCommand { get; set; }
        bool IsSubTaskListSelected { get; set; }
        Color IsSubTaskListTypeColor { get; }
        RelayCommand IsSubTaskListTypeSelectedCommand { get; set; }
        Color IsValueTypeColor { get; }
        bool IsValueTypeSelected { get; set; }
        RelayCommand IsValueTypeSelectedCommand { get; set; }
    }
}