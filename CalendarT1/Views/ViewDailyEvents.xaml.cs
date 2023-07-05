using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
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