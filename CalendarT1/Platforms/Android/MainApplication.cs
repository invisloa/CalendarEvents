using Android.App;
using Android.Runtime;

namespace CalendarT1;

[Application]
public class MainApplication : MauiApplication
{
	public MainApplication(IntPtr handle, JniHandleOwnership ownership)
		: base(handle, ownership)
	{
	}
	public virtual int FirstDayOfWeek { [Android.Runtime.Register("getFirstDayOfWeek", "()I", "GetGetFirstDayOfWeekHandler")] get; [Android.Runtime.Register("setFirstDayOfWeek", "(I)V", "GetSetFirstDayOfWeek_IHandler")] set; }
	protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
