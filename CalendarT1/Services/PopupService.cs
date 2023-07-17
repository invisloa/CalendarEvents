using CalendarT1.Models;
using CalendarT1.Views;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace CalendarT1.Services
{
	public class PopupService : IPopupService
	{
		public async Task ShowReminderPopup(EventModel eventModel)
		{
			// Create a new popup
			var popup = new PopupCalendar();

			// Update the popup content based on the event
			// Replace this line with the appropriate code to update your popup
			popup.Content = new Label { Text = $"Reminder: {eventModel.Title}" };
			// Display the popup
			Page page = Application.Current?.MainPage ?? throw new NullReferenceException();
			page.ShowPopup(popup);

		}
	}
}
