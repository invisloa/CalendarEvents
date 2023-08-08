using CalendarT1.Helpers;
using CalendarT1.ViewModels.EventsViewModels;


namespace CalendarT1.Views
{
	public partial class ViewWeeklyEvents : ContentPage
	{
		public ViewWeeklyEvents()
		{
			InitializeComponent();
			var viewModel = ServiceHelper.GetService<WeeklyEventsViewModel>();
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
			(BindingContext as WeeklyEventsViewModel).BindDataToScheduleList();
		}
	}

}
