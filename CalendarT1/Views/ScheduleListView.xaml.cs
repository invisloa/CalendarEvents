using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class ScheduleListView : ContentPage
{
	public ScheduleListView()
	{
		BindingContext = new ScheduleListViewModel();

		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as ScheduleListViewModel).OnAppearing();
	}
}