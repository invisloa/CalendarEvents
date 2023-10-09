using CalendarT1.Helpers;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class AllMainTypesPage : ContentPage
{
	IEventRepository _eventRepository;
	public AllMainTypesPage()
	{
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		var bindingContext = new AddNewMainTypePageViewModel(_eventRepository);

		InitializeComponent();
	}
}