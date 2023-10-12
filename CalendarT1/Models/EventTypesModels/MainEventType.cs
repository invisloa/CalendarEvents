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
		public string Icon { get; set; }  // new property for the icon

		private Color _mainEventTypeBackgroundColor;
		[JsonIgnore]
		public Color MainEventTypeBackgroundColor
		{
			get => _mainEventTypeBackgroundColor;
			set => _mainEventTypeBackgroundColor = value; 
		}

		public string MainEventTypeBackgroundColorString
		{
			get => _mainEventTypeBackgroundColor.ToArgbHex();
			set => _mainEventTypeBackgroundColor = Color.FromArgb(value); 
		}

		private Color _mainEventTypeTextColor;
		[JsonIgnore]
		public Color MainEventTypeTextColor
		{
			get => _mainEventTypeTextColor;
			set => _mainEventTypeTextColor = value; 
		}

		public string MainEventTypeTextColorString
		{
			get => _mainEventTypeTextColor.ToArgbHex(); 
			set => _mainEventTypeTextColor = Color.FromArgb(value); 
		}
		public override string ToString()
		{
			return Title;
		}
		public MainEventType(string title, string icon, Color mainEventTypeBackgroundColor, Color mainEventTypeTextColor)
		{
			Title = title;
			Icon = icon; 
			MainEventTypeBackgroundColor = mainEventTypeBackgroundColor;
			MainEventTypeTextColor = mainEventTypeTextColor;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			MainEventType other = (MainEventType)obj;
			return Title == other.Title && Icon == other.Icon && MainEventTypeBackgroundColorString == other.MainEventTypeBackgroundColorString && MainEventTypeTextColor == other.MainEventTypeTextColor ;
		}

		public override int GetHashCode()
		{
			return (Title, Icon, MainEventTypeBackgroundColorString, MainEventTypeTextColor).GetHashCode();
		}
	}
}
