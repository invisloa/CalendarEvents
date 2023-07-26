using CalendarT1.Helpers;
using CalendarT1.
/* Unmerged change from project 'CalendarT1 (net7.0-ios)'
Before:
using CalendarT1.Helpers;
After:
using CalendarT1.ViewModels.EventsViewModels;
*/

/* Unmerged change from project 'CalendarT1 (net7.0-android)'
Before:
using CalendarT1.Helpers;
After:
using CalendarT1.ViewModels.EventsViewModels;
*/

/* Unmerged change from project 'CalendarT1 (net7.0-maccatalyst)'
Before:
using CalendarT1.Helpers;
After:
using CalendarT1.ViewModels.EventsViewModels;
*/
ViewModels.EventsViewModels;

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


