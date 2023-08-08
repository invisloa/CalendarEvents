using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
	{
		var viewModel = ServiceHelper.GetService<DailyEventsViewModel>();
		BindingContext = viewModel;
		InitializeComponent();
		viewModel.OnEventsToShowListUpdated += () =>
		{
			viewModel.BindDataToScheduleList();
		};
	}
	protected override void OnDisappearing()
	{
		base.OnDisappearing();
		(BindingContext as DailyEventsViewModel).OnEventsToShowListUpdated -= (BindingContext as DailyEventsViewModel).BindDataToScheduleList;
	}
	public ViewDailyEvents(IEventRepository eventRepository, IUserEventTypeModel eventType)
	{
		BindingContext = new DailyEventsViewModel(eventRepository, eventType);
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as DailyEventsViewModel).BindDataToScheduleList();
	}

}