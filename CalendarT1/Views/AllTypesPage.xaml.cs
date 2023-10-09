using CalendarT1.Helpers;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.TypesViewModels;
using System.Net.WebSockets;

namespace CalendarT1.Views;

public partial class AllSubTypesPage : ContentPage
{
	public AllSubTypesPage()
	{
		var eventRepository = ServiceHelper.GetService<IEventRepository>();
		BindingContext = new AllSubTypesPageViewModel(eventRepository);
		InitializeComponent();
	}
}