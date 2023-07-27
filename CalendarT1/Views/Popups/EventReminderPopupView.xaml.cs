using CalendarT1.Models.EventModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;
using CommunityToolkit.Maui.Views;

namespace CalendarT1.Views;

public partial class EventReminderPopupView : Popup
{

	public EventReminderPopupView(IEventRepository eventRepository, IGeneralEventModel eventToEdit)
	{

		BindingContext = new EventReminderPopupViewModel(eventRepository, eventToEdit);
		InitializeComponent();
	}
}