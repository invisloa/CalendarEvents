using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ViewWeeklyEvents : ContentPage
{
	public ViewWeeklyEvents()
	{
		InitializeComponent();
		BindingContext = new WeeklyEventsViewModel();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as WeeklyEventsViewModel).OnAppearing();
	}
}