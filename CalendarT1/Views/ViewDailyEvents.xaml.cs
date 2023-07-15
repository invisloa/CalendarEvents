using CalendarT1.Models;
using CalendarT1.Services;
using CalendarT1.ViewModels.EventsViewModels;
using System.Collections.ObjectModel;

namespace CalendarT1.Views;

public partial class ViewDailyEvents : ContentPage
{
	public ViewDailyEvents()
	{
		var priorities = new ObservableCollection<EventPriority>(Factory.CreateAllPrioritiesLevelsEnumerable());

		BindingContext = new DailyEventsViewModel(priorities, Factory.EventRepository);

		InitializeComponent();
	}
	protected override void OnAppearing()
	{
		base.OnAppearing();
		(BindingContext as DailyEventsViewModel).OnAppearing();
	}
}