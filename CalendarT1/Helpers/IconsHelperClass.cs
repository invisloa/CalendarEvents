using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Helpers
{
	internal class IconsHelperClass
	{

		public static ObservableCollection<string> GetTopIcons()
		{
			return new ObservableCollection<string>(TopIcons);
		}
		public static ObservableCollection<string> GetTopIcons2()
		{
			return new ObservableCollection<string>(TopIcons2);
		}
		public static ObservableCollection<string> GetTopIcons3()
		{
			return new ObservableCollection<string>(TopIcons3);
		}


		public static readonly List<string> TopIcons = new List<string> // todo change those lists to real ones
		{
			IconFont.Icon1,
			IconFont.Icon2,
			IconFont.Icon3,
			IconFont.Icon4,
			IconFont.Icon5,
			IconFont.Icon6
		};
		public static readonly List<string> TopIcons2 = new List<string>
		{
			IconFont.Icon11,
			IconFont.Icon12,
			IconFont.Icon13,
			IconFont.Icon14,
			IconFont.Icon15,
			IconFont.Icon16
		};
		public static readonly List<string> TopIcons3 = new List<string>
		{
			IconFont.Icon21,
			IconFont.Icon22,
			IconFont.Icon23,
			IconFont.Icon24,
			IconFont.Icon25,
			IconFont.Icon26
		};
	}
}
