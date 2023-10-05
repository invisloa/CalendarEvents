using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1.Views;

public partial class WelcomePage : ContentPage
{
	public WelcomePage()
	{
		IEventRepository _eventRepository;
		InitializeComponent();
		//_eventRepository = ServiceHelper.GetService<IEventRepository>();
		//_eventRepository.ClearAllMainEventTypesAsync();
		//_eventRepository.ClearAllSubEventTypesAsync();
		//_eventRepository.ClearAllEventsListAsync();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		Color some = (Color)Application.Current.Resources["MainSubTaskBackgroundColor"];

		IMainEventType mainEventType = new MainEventType("Test", some);
		IUserEventTypeModel userEventType = new UserEventTypeModel( mainEventType, "Test", some, TimeSpan.Zero);
		List<IUserEventTypeModel> _allUserTypesForVisuals = new List<IUserEventTypeModel>() { userEventType };
		List<IMainEventType> events = new List<IMainEventType>() { mainEventType };
		var x =  _allUserTypesForVisuals.FindAll(x => x.MainEventType == mainEventType);

		if (events.Contains(mainEventType))
		{
			DisplayAlert("Test", "Test", "Test");
		}

    }
}