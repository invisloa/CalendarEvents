using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models.EventTypesModels
{
	public class MainEventType : IMainEventType
	{
		public string Title { get; set; }
		[JsonIgnore]
		public Color MainEventTypeBorderColor { get; set; }
		public string MainEventTypeBorderColorString       
		{
			get
			{
				return MainEventTypeBorderColor.ToArgbHex();
			}

			set
			{
				MainEventTypeBorderColor = Color.FromArgb(value);
			}
		}
		public override string ToString()
		{
			return Title;
		}
		public MainEventType(string title, Color mainEventTypeBorderColor)
		{
			Title = title;
			MainEventTypeBorderColor = mainEventTypeBorderColor;
		}

		public bool Equals(IMainEventType obj)
		{
			return Title == obj.Title;
		}
		public override int GetHashCode()
		{
			unchecked
			{
				int hash = 17;
				hash = hash * 23 + Title.GetHashCode();
				return hash;
			}
		}

	}
}
