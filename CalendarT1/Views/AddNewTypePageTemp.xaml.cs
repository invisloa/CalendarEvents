using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.Views;

public partial class AddNewTypePageTemp : ContentPage
{
	IEventRepository _eventRepository;
	public AddNewTypePageTemp()
	{
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		IMainEventType userEventTypeModel = new MainEventType("test1", Color.FromArgb("#FF0000"));
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel);

	}
}