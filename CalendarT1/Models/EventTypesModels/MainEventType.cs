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
		private Color _mainEventTypeBackgroundColor;

		[JsonIgnore]
		public Color MainEventTypeBackgroundColor
		{
			get => _mainEventTypeBackgroundColor;
			set
			{
				if (_mainEventTypeBackgroundColor != value)
				{
					_mainEventTypeBackgroundColor = value;
				}
			}
		}
		public string MainEventTypeBackgroundColorString       
		{
			get
			{
				return _mainEventTypeBackgroundColor.ToArgbHex();
			}
			set
			{
				_mainEventTypeBackgroundColor = Color.FromArgb(value);
			}
		}
		private Color _mainEventTypeTextColor;
		[JsonIgnore]
		public Color MainEventTypeTextColor 
		{ get => _mainEventTypeTextColor;
			set
			{
				if(_mainEventTypeTextColor != value)
				{
					_mainEventTypeTextColor = value;
				}
			}
		}

		public string MainEventTypeTextColorString
		{
			get
			{
				return _mainEventTypeTextColor.ToArgbHex();
			}

			set
			{
				_mainEventTypeTextColor = Color.FromArgb(value);
			}
		}
		public override string ToString()
		{
			return Title;
		}
		public MainEventType(string title, Color mainEventTypeBackgroundColor, Color mainEventTypeTextColor)
		{
			Title = title;
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
			return Title == other.Title;
		}

		public override int GetHashCode()
		{
			return Title.GetHashCode();
		}

	}
}
