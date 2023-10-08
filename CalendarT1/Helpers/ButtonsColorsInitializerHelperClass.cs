using CalendarT1.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Helpers
{
	public class ButtonsColorsInitializerHelperClass
	{
		public ObservableCollection<ButtonProperties> ButtonsColors { get; set; }
		private int BorderSize;
		private int NumberOfColumns = 10;

		public ButtonsColorsInitializerHelperClass(int borderSize)
		{
			BorderSize = borderSize;
			InitializeColorButtons();
		}

		private void InitializeColorButtons()
		{
			ButtonsColors = new ObservableCollection<ButtonProperties>();

			AddColors(GenerateShades(Color.FromRgb(205, 92, 92))); // Base Red
			AddColors(GenerateShades(Color.FromRgb(30, 144, 255))); // Base Blue
			AddColors(GenerateShades(Color.FromRgb(60, 179, 113))); // Base Green
			AddColors(GenerateShades(Color.FromRgb(255, 165, 0))); // Base Orange
			// ... Add more base colors as needed
		}

		private void AddColors(params Color[] colors)
		{
			foreach (var color in colors)
			{
				ButtonsColors.Add(new ButtonProperties { ButtonColor = color, ButtonBorder = BorderSize });
			}
		}

		private Color[] GenerateShades(Color baseColor)
		{
			Color[] shades = new Color[NumberOfColumns];
			double step = 0.1;

			for (int i = 0; i < NumberOfColumns; i++)
			{
				double factor = 0.7 + step * i; // Start from 70% brightness and increment
				shades[i] = Color.FromRgba(
					baseColor.Red * factor,
					baseColor.Green * factor,
					baseColor.Blue * factor,
					1 // Alpha for opacity
				);
			}

			return shades;
		}
	}
	#region Helper Classes

	public class ButtonProperties : BaseViewModel
	{
		private int _borderSize;
		[JsonIgnore]
		public Color ButtonColor { get; set; }
		public int ButtonBorder
		{
			get => _borderSize;
			set
			{
				_borderSize = value;
				OnPropertyChanged();
			}
		}
		public string ButtonColorRgb        // when deserialized it will not fire OnPropertyChanged on ButtonColor to check later if its ok TOCHECK
		{
			get { return $"{ButtonColor.Red}, {ButtonColor.Green}, {ButtonColor.Blue}"; }
			set
			{
				var rgbValues = value.Split(',').Select(int.Parse).ToArray();
				ButtonColor = Color.FromRgb(rgbValues[0], rgbValues[1], rgbValues[2]);
			}
		}
	}
	#endregion

}
