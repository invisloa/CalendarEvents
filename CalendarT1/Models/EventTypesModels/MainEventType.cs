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

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			MainEventType other = (MainEventType)obj;
			return Title == other.Title;
		}

		public override int GetHashCode()
		{
			return Title.GetHashCode();
		}

	}
}
