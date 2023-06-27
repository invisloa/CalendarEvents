using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class ScheduleListView : ContentPage
{
	public ScheduleListView()
	{
		BindingContext = new ScheduleListViewModel();

		InitializeComponent();
	}

	private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		ScheduleListViewModel viewModel = (ScheduleListViewModel)BindingContext;
		viewModel.BindDataToScheduleList();

	}
}