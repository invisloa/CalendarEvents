using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventOperations;
using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ViewAllEventsPage : ContentPage
{
	// for viewing all events
	public ViewAllEventsPage()
	{
		// Retrieve the view model from DI container
		var viewModel = MauiProgram.Current.Services.GetService<AllEventsViewModel>();

		BindingContext = viewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
			viewModel.BindDataToScheduleList();
		};
	}
	// for viewing all specific type of events
	public ViewAllEventsPage(IEventRepository eventRepository, ISubEventTypeModel eventTypeModel)
	{
		BindingContext = new AllEventsViewModel(eventRepository, eventTypeModel);
		var viewModel = BindingContext as AllEventsViewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
			viewModel.BindDataToScheduleList();
		};
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();

		(BindingContext as AllEventsViewModel).OnEventsToShowListUpdated -= (BindingContext as AllEventsViewModel).BindDataToScheduleList;
	}

	protected override  void OnAppearing()
	{
		var viewModel = BindingContext as AllEventsViewModel;
		base.OnAppearing();
		viewModel.OnAppearing();
	}
}