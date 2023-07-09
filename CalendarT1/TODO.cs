




#region Current
/*





using Microsoft.Maui.Controls.Compatibility.Platform.UWP;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.UI.Xaml.Controls;
using YourNamespace;

[assembly: ExportRenderer(typeof(Xamarin.Forms.DatePicker), typeof(CustomDatePickerRenderer))]
namespace YourNamespace
{
	public class CustomDatePickerRenderer : DatePickerRenderer
	{
		public CustomDatePickerRenderer(Context context) : base(context)
		{
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.DatePicker> e)
		{
			base.OnElementChanged(e);

			if (Control != null)
			{
				Control.FirstDayOfWeek = 1;
			}
		}
	}
}

 
 */
#endregion









#region High Priority
// GOOGLE CALENDAR API
// make reminders for events
// show events in the hourly order
#endregion


#region Low Priority
// make current week view, current month view, current day view
// change priority level picker to button like choosing  <Picker Title="Priority Level" x: Name = "PriorityPicker" ItemsSource = "{Binding EventPriorities}" SelectedItem = "{Binding EventPriority, Mode=TwoWay}" >
// make a buttons to go one day forward and one day back
// make a button to go to current day (today)
// add delete event button
// add checked event button and button to show only checked events
#endregion



#region done
// Window For event details?? click on event command?
// CHANGE 		private void ExecuteSelectEventCommand(EventModel selectedEvent) TO show a new window with event details

// account wide properties to show events
// save and load interfaces
// button to add new event -  create a new page for adding events

// make a checkboxes for different events to show
// if event datetime end is lower than start change it to start + 1 hour

#endregion

