namespace CalendarT1.Views.CustomControls.CCViewModels
{
	public interface IUserTypeExtraOptionsViewModel
	{
		bool IsDefaultEventTimespanSelected { get; set; }
		Color IsDefaultTimespanColor { get; }
		RelayCommand IsDefaultTimespanSelectedCommand { get; set; }
		bool IsEditMode { get; set; }
		bool IsNotEditMode { get; }
		RelayCommand IsMicroTaskListTypeSelectedCommand { get; set; }
		Color IsMicroTasksListTypeColor { get; }
		bool IsMicroTasksTypeSelected { get; set; }
		Color IsValueTypeColor { get; }
		bool IsValueTypeSelected { get; set; }
		RelayCommand IsValueTypeSelectedCommand { get; set; }
	}
}