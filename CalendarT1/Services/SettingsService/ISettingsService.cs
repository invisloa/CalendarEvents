using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Services.SettingsService
{
	public interface ISettingsService
	{
		bool ShowCompletedEvents { get; set; }
	}
}
