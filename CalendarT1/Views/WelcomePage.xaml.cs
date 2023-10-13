using CalendarT1.Helpers;
using CalendarT1.Models;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.Views;

public partial class WelcomePage : ContentPage
{
	IEventRepository _eventRepository;

	public WelcomePage()
	{
		InitializeComponent();
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		//_eventRepository.ClearAllMainEventTypesAsync();
		//_eventRepository.ClearAllSubEventTypesAsync();
		//_eventRepository.ClearAllEventsListAsync();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		var icon = new IconModel("logout.png", Color.FromArgb("#FF0000"), Color.FromArgb("#FF0000"));
		var icon2 = new IconModel("login.png", Color.FromArgb("#FFaaaa"), Color.FromArgb("#FFaaaa"));
		var icon3 = new IconModel("logout.png", Color.FromArgb("#FFdd00"), Color.FromArgb("#FFdd00"));
		IMainEventType userEventTypeModel = new MainEventType("test1", icon);
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel);
		IMainEventType userEventTypeModel2 = new MainEventType("test2", icon2);
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel2);
		IMainEventType userEventTypeModel3 = new MainEventType("test3", icon3);
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel3);


	}
}