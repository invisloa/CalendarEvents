using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.TypesViewModels;

namespace CalendarT1.Views;

public partial class AllTypesPage : ContentPage
{
	public AllTypesPage(IEventRepository eventRepository)
	{
		BindingContext = new AllTypesPageViewModel(eventRepository);
		InitializeComponent();
	}
}