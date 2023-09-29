using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models.EventTypesModels
{
	public interface IMainEventType : IEquatable<IMainEventType>
	{
		string Title { get; set; }
		Color MainEventTypeBorderColor { get; set; }
		string MainEventTypeBorderColorString { get; set; }

		new bool Equals(IMainEventType other); // to check if the event type is already in the list

	}
}
