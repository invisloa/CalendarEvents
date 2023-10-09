using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels;

using CalendarT1.ViewModels.EventOperations;
using Microsoft.Maui.Controls.PlatformConfiguration;

namespace CalendarT1.Views;

public partial class AddNewSubTypePage : ContentPage
{
	public AddNewSubTypePage()
	{
		BindingContext = ServiceHelper.GetService<AddNewSubTypePageViewModel>();
		InitializeComponent();

	}
	public AddNewSubTypePage(IEventRepository eventRepository, ISubEventTypeModel userEventTypeModel)	// edit mode
	{
		BindingContext = new AddNewSubTypePageViewModel(eventRepository, userEventTypeModel);
		InitializeComponent();

	}

}
