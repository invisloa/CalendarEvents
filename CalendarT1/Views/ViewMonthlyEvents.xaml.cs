using CalendarT1.Helpers;
using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ViewMonthlyEvents : ContentPage
{
	public ViewMonthlyEvents()
	{
		InitializeComponent();
		var viewModel = ServiceHelper.GetService<MonthlyEventsViewModel>();
		BindingContext = viewModel;

		// geterate new grid everytime the list of events to show is updated (e.g. when user adds new event)
		viewModel.OnEventsToShowListUpdated += () =>
		{
			monthlyEventsControl.GenerateGrid();
		};
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		(BindingContext as MonthlyEventsViewModel).OnEventsToShowListUpdated -= monthlyEventsControl.GenerateGrid;
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as MonthlyEventsViewModel).BindDataToScheduleList();
	}
}


