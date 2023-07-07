using CalendarT1.ViewModels.EventsViewModels;
using System.Globalization;
using System.Diagnostics;

namespace CalendarT1.Views
{
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
}
