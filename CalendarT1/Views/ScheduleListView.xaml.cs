using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class ScheduleListView : ContentPage
{
	public ScheduleListView()
	{
		InitializeComponent();
		BindingContext = new ScheduleListViewModel();
	}
}