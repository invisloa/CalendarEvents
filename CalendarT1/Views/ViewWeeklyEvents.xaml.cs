using CalendarT1.ViewModels.EventsViewModels;
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
			InitializeComponent();
			BindingContext = new WeeklyEventsViewModel();
		}

		protected override void OnAppearing()
		{
			try
			{

				base.OnAppearing();
				(BindingContext as WeeklyEventsViewModel).OnAppearing();
			}
			catch
			{
				throw;
			}
		}
	}

}
