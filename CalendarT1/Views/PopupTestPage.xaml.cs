using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Maui.Views;

namespace CalendarT1.Views;

public partial class PopupTestPage : ContentPage
{
	IEventRepository eventRepository;
	EventModel eventModel;
	public PopupTestPage(IEventRepository eventRepository, EventModel eventModel)
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		this.ShowPopupAsync(new EventReminderPopupView(eventRepository, eventModel));
	}
}