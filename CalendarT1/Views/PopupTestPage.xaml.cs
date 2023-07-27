using CalendarT1.Models.EventModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Maui.Views;

namespace CalendarT1.Views;

public partial class PopupTestPage : ContentPage
{
	IEventRepository eventRepository;
	IGeneralEventModel eventModel;
	public PopupTestPage(IEventRepository eventRepository, IGeneralEventModel eventModel)
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		this.ShowPopupAsync(new EventReminderPopupView(eventRepository, eventModel));
	}
}