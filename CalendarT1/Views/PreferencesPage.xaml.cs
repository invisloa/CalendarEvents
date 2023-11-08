using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class PreferencesPage : ContentPage
{
	public PreferencesPage()
	{
		var vm = new PreferencesViewModel();
		BindingContext = vm;
		InitializeComponent();
	}
}