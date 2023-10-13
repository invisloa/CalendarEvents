using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models.EventTypesModels
{
	public class MainEventType : IMainEventType
	{
		public string Title { get; set; }
		public IconModel SelectedIcon { get; set; }  // new property for the icon
		public override string ToString()
		{
			return Title;
		}
		public MainEventType(string title, IconModel icon)
		{
			Title = title;
			SelectedIcon = icon; 

		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			MainEventType other = (MainEventType)obj;
			return Title == other.Title && SelectedIcon == other.SelectedIcon ;
		}

		public override int GetHashCode()
		{
			return (Title, SelectedIcon).GetHashCode();
		}
	}
}
