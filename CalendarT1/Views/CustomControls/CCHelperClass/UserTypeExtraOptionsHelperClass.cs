using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCHelperClass
{
	internal class UserTypeExtraOptionsHelperClass
	{
		public RelayCommand IsValueTypeSelectedCommand { get; set; }
		public RelayCommand IsSubTaskListTypeSelectedCommand { get; set; }
		public RelayCommand IsDefaultTimespanSelectedCommand { get; set; }
	}
}
