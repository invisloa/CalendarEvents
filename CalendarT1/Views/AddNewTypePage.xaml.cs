using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels;
using CalendarT1.ViewModels.EventOperations;

namespace CalendarT1.Views;

public partial class AddNewTypePage : ContentPage
{
	public AddNewTypePage()
	{
		BindingContext = ServiceHelper.GetService<AddNewTypePageViewModel>();
		InitializeComponent();
	}
}
