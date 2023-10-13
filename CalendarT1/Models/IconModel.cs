using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models
{
	public class IconModel : IMainTypeVisualElement
	{
		public string IconName { get; set; }

		private Color _backgroundColor;
		[JsonIgnore]
		public Color BackgroundColor
		{
			get => _backgroundColor;
			set => _backgroundColor = value;
		}

		public string BackgroundColorString
		{
			get => _backgroundColor.ToArgbHex();
			set => _backgroundColor = Color.FromArgb(value);
		}

		private Color _textColor;
		[JsonIgnore]
		public Color TextColor
		{
			get => _textColor;
			set => _textColor = value;
		}

		public string TextColorString
		{
			get => _textColor.ToArgbHex();
			set => _textColor = Color.FromArgb(value);
		}

		public IconModel(string icon, Color backgroundColor, Color textColor)
		{
			IconName = icon;
			BackgroundColor = backgroundColor;
			TextColor = textColor;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}

			IconModel other = (IconModel)obj;
			return IconName == other.IconName &&
				   BackgroundColorString == other.BackgroundColorString &&
				   TextColorString == other.TextColorString;
		}

		public override int GetHashCode()
		{
			return (IconName, BackgroundColorString, TextColorString).GetHashCode();
		}
	}
}