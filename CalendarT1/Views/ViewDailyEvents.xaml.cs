using CalendarT1.Helpers;
using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.EventsViewModels;
using System.Collections.ObjectModel;

namespace CalendarT1.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
	{
		BindingContext = ServiceHelper.GetService<DailyEventsViewModel>();
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await (BindingContext as DailyEventsViewModel).LoadAndBindDataToScheduleListAsync();
	}
}