using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ScheduleListView : ContentPage
{
	public ScheduleListView()
	{
		BindingContext = new DailyEventsViewModel();

		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as DailyEventsViewModel).OnAppearing();
	}
}