using CalendarT1.Models;
using CalendarT1.Views;
using System.Threading.Tasks;

namespace CalendarT1.Services
{
	public interface IPopupService
	{
		Task ShowReminderPopup(EventModel eventModel);
	}
}
