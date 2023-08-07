using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventsViewModels;

namespace CalendarT1.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
	{
		BindingContext = ServiceHelper.GetService<DailyEventsViewModel>();
		InitializeComponent();
	}
	public ViewDailyEvents(IEventRepository eventRepository, IUserEventTypeModel eventType)
	{
		BindingContext = new DailyEventsViewModel(eventRepository, eventType);
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await (BindingContext as DailyEventsViewModel).LoadAndBindDataToScheduleListAsync();
	}

}