using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.ViewModels.EventsViewModels;
using System.Collections.ObjectModel;
/* Unmerged change from project 'CalendarT1 (net7.0-android)'
Before:
using System.Globalization;
using System.Diagnostics;
After:
using System.Diagnostics;
using System.Globalization;
*/

/* Unmerged change from project 'CalendarT1 (net7.0-windows10.0.22621.0)'
Before:
using System.Globalization;
using System.Diagnostics;
After:
using System.Diagnostics;
using System.Globalization;
*/

/* Unmerged change from project 'CalendarT1 (net7.0-maccatalyst)'
Before:
using System.Globalization;
using System.Diagnostics;
After:
using System.Diagnostics;
using System.Globalization;
*/


namespace CalendarT1.Views
{
	public partial class ViewWeeklyEvents : ContentPage
	{
		public ViewWeeklyEvents()
		{
			var priorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());

			BindingContext = new WeeklyEventsViewModel(priorities, Factory.EventRepository);

			InitializeComponent();
		}
		protected override async void OnAppearing()
		{
			base.OnAppearing();
			await (BindingContext as DailyEventsViewModel).InitializeAsync();
		}
	}

}
