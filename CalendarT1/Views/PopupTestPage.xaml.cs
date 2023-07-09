using CommunityToolkit.Maui.Views;

namespace CalendarT1.Views;

public partial class PopupTestPage : ContentPage
{
	public PopupTestPage()
	{
		InitializeComponent();
	}

	private void Button_Clicked(object sender, EventArgs e)
	{
		this.ShowPopupAsync(new PopupCalendar());
    }
}