using Microsoft.Maui.Layouts;
using System.Collections;

namespace CalendarT1.Views.CustomControls
{
	public partial class SelectableButtonFlexLayoutCC : FlexLayout
	{
		public SelectableButtonFlexLayoutCC()
		{
			InitializeComponent();
		}

		public static readonly BindableProperty ItemsSourceProperty =
			BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(SelectableButtonFlexLayoutCC), null, propertyChanged: OnItemsSourceChanged);

		public IEnumerable ItemsSource
		{
			get => (IEnumerable)GetValue(ItemsSourceProperty);
			set => SetValue(ItemsSourceProperty, value);
		}

		private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
		{
			if (bindable is SelectableButtonFlexLayoutCC control && newValue is IEnumerable items)
			{
				BindableLayout.SetItemsSource(control, items);
			}
		}
	}
}
