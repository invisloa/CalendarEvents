using CalendarT1.ViewModels.CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class AddNewTypePage : ContentPage
{
	public AddNewTypePage()
	{
		InitializeComponent();
		BindingContext = new AddNewTypePageViewModel();
	}
}