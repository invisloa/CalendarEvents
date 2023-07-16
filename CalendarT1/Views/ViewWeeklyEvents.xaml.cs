using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.ViewModels.EventsViewModels;
using System.Collections.ObjectModel;


namespace CalendarT1.Views
{
	public partial class ViewWeeklyEvents : ContentPage
	{
		public ViewWeeklyEvents()
		{
			InitializeComponent();
			var viewModel = new WeeklyEventsViewModel(Factory.EventRepository);
			BindingContext = viewModel;
			viewModel.OnEventsToShowListUpdated += () =>
			{
				weeklyEventsControl.GenerateGrid();
			};
		}
		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			(BindingContext as WeeklyEventsViewModel).OnEventsToShowListUpdated -= weeklyEventsControl.GenerateGrid;
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await (BindingContext as WeeklyEventsViewModel).LoadAndBindDataToScheduleListAsync();
		}
	}

}
