/*using Android.Content;
using CalendarT1.Droid.Renderers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.PlatformConfiguration.AndroidSpecific;
using Android.Views;
using Microsoft.Maui.Controls.Compatibility.Platform.Android;
using Microsoft.Maui.Controls.Compatibility;

[assembly: ExportRenderer(typeof(ScrollView), typeof(HorizontalVerticalScrollViewRenderer))]
namespace CalendarT1.Droid.Renderers
{
	public class HorizontalVerticalScrollViewRenderer : ScrollViewRenderer
	{
		public HorizontalVerticalScrollViewRenderer(Context context) : base(context)
		{
		}

		public override bool OnInterceptTouchEvent(MotionEvent ev)
		{
			if (this.Element is ScrollView scrollView && scrollView.On<Microsoft.Maui.Controls.PlatformConfiguration.Android>().IsNestedScrollingEnabled())
			{
				return false;
			}

			return base.OnInterceptTouchEvent(ev);
		}
	}
}
*/