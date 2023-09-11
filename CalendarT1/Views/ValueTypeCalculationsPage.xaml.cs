using CalendarT1.Helpers;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels;
using CalendarT1.ViewModels.EventOperations;

namespace CalendarT1.Views;

public partial class ValueTypeCalculationsPage : ContentPage
{
	public ValueTypeCalculationsPage()
	{
		var viewModel = ServiceHelper.GetService<ValueTypeCalculationsViewModel>();

		BindingContext = viewModel;
		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		var viewModel = BindingContext as ValueTypeCalculationsViewModel;
		viewModel.OnAppearing();
	}
}