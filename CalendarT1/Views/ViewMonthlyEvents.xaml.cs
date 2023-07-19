using CalendarT1.ViewModels.EventsViewModels;
using CalendarT1.Services;
using CalendarT1.Helpers;

namespace CalendarT1.Views;

public partial class ViewMonthlyEvents : ContentPage
{
	public ViewMonthlyEvents()
	{
		InitializeComponent();
		var viewModel = ServiceHelper.GetService<MonthlyEventsViewModel>();
		BindingContext = viewModel;
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
		await (BindingContext as MonthlyEventsViewModel).LoadAndBindDataToScheduleListAsync();
	}
}


