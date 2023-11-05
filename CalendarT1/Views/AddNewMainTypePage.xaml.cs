using CalendarT1.Helpers;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels;

namespace CalendarT1.Views;

public partial class AddNewMainTypePage : ContentPage
{
	public AddNewMainTypePage()
	{
		BindingContext = ServiceHelper.GetService<AddNewMainTypePageViewModel>();
		InitializeComponent();
	}
	public AddNewMainTypePage(IEventRepository eventRepository, IMainEventType mainEventType)
	{
		BindingContext = new AddNewMainTypePageViewModel(eventRepository, mainEventType);
		InitializeComponent();
	}


	//// padding modification for entry control So that the text is not too close to the edge of the control - extra space for icon to show
	//void ModifyEntryPadding()
	//{
	//	Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("AddLeftPadding", (handler, view) =>
	//	{
	//	#if ANDROID
	//			handler.PlatformView.SetPadding(45, 0, 10, 0);  // Add 45px padding to the left and 10px to the right
	//	#elif IOS || MACCATALYST
	//				var leftPaddingView = new UIKit.UIView(new CoreGraphics.CGRect(0, 0, 45, 0)); // 45px width transparent view for left padding
	//				var rightPaddingView = new UIKit.UIView(new CoreGraphics.CGRect(0, 0, 10, 0)); // 10px width transparent view for right padding

	//				handler.PlatformView.LeftView = leftPaddingView;
	//				handler.PlatformView.LeftViewMode = UIKit.UITextFieldViewMode.Always;
	//				handler.PlatformView.RightView = rightPaddingView;
	//				handler.PlatformView.RightViewMode = UIKit.UITextFieldViewMode.Always;
	//	#elif WINDOWS
	//			handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(45, 0, 10, 0); // Add 45px padding to the left and 10px to the right
	//	#endif
	//	});
	//}

}

