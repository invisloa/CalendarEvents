using CalendarT1.Helpers;
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
		IMainEventType userEventTypeModel = new MainEventType("test1", "ue002", Color.FromArgb("#FF0000"), Color.FromArgb("#FF0000"));
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel);
		IMainEventType userEventTypeModel2 = new MainEventType("test2", "ue043", Color.FromArgb("#FFaaaa"), Color.FromArgb("#FFaaaa"));
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel2);
		IMainEventType userEventTypeModel3 = new MainEventType("test3", "ue043", Color.FromArgb("#FFdd00"), Color.FromArgb("#FFdd00"));
		_eventRepository.AddMainEventTypeAsync(userEventTypeModel3);


	}
}