using CalendarT1.Models;
using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class AddEventPage : ContentPage
{
	private AddEventViewModel _viewModel;
	public AddEventPage()
	{
		_viewModel = new AddEventViewModel();
		BindingContext = _viewModel;
		InitializeComponent();
	}
}