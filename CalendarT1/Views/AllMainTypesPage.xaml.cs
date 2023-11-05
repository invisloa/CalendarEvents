using CalendarT1.Helpers;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels;
using CalendarT1.ViewModels.TypesViewModels;

namespace CalendarT1.Views;

public partial class AllMainTypesPage : ContentPage
{
	IEventRepository _eventRepository;
	public AllMainTypesPage()
	{
		_eventRepository = ServiceHelper.GetService<IEventRepository>();
		BindingContext = new AllMainTypesPageViewModel(_eventRepository);
		
		InitializeComponent();
	}
}