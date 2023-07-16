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
			BindingContext = new WeeklyEventsViewModel(Factory.EventRepository);
			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await (BindingContext as WeeklyEventsViewModel).LoadAndBindDataToScheduleListAsync();
		}
	}

}
