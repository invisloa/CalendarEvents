using CalendarT1.Models;
using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class AddEventPage : ContentPage
{
	private AddEventCVViewModel _viewModel;
	public AddEventPage(EventModel eventToEdit = null)
	{
		_viewModel = new AddEventCVViewModel(eventToEdit);
		BindingContext = _viewModel;
		InitializeComponent();
	}
}