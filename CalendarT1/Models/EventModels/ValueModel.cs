using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.Models.EventModels
{
	public class ValueModel : AbstractEventModel
	{
		public Quantity Amount { get; set; }

		public ValueModel(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, Quantity amount, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false)
			: base(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown)
		{
			Amount = amount;
		}
	}

	public class Quantity
		{
			public decimal Value { get; private set; }
			public MeasurementUnit Unit { get; private set; }

			public Quantity(decimal value, MeasurementUnit unit)
			{
				Value = value;
				Unit = unit;
			}

			public Quantity ConvertTo(MeasurementUnit targetUnit)
			{
				if (Unit == targetUnit)
					return this;

				switch (Unit)
				{
					case MeasurementUnit.Kilogram when targetUnit == MeasurementUnit.Gram:
						return new Quantity(Value * 1000, targetUnit);
					case MeasurementUnit.Kilogram when targetUnit == MeasurementUnit.Milligram:
						return new Quantity(Value * 1000000, targetUnit);
					case MeasurementUnit.Gram when targetUnit == MeasurementUnit.Kilogram:
						return new Quantity(Value / 1000, targetUnit);
					case MeasurementUnit.Gram when targetUnit == MeasurementUnit.Milligram:
						return new Quantity(Value * 1000, targetUnit);
					case MeasurementUnit.Milligram when targetUnit == MeasurementUnit.Gram:
						return new Quantity(Value / 1000, targetUnit);
					case MeasurementUnit.Milligram when targetUnit == MeasurementUnit.Kilogram:
						return new Quantity(Value / 1000000, targetUnit);
					case MeasurementUnit.Liter when targetUnit == MeasurementUnit.Milliliter:
						return new Quantity(Value * 1000, targetUnit);
					case MeasurementUnit.Milliliter when targetUnit == MeasurementUnit.Liter:
						return new Quantity(Value / 1000, targetUnit);
					case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Centimeter:
						return new Quantity(Value * 100, targetUnit);
					case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Millimeter:
						return new Quantity(Value * 1000, targetUnit);
					case MeasurementUnit.Meter when targetUnit == MeasurementUnit.Kilometer:
						return new Quantity(Value / 1000, targetUnit);
					case MeasurementUnit.Centimeter when targetUnit == MeasurementUnit.Meter:
						return new Quantity(Value / 100, targetUnit);
					case MeasurementUnit.Centimeter when targetUnit == MeasurementUnit.Millimeter:
						return new Quantity(Value * 10, targetUnit);
					case MeasurementUnit.Millimeter when targetUnit == MeasurementUnit.Meter:
						return new Quantity(Value / 1000, targetUnit);
					case MeasurementUnit.Millimeter when targetUnit == MeasurementUnit.Centimeter:
						return new Quantity(Value / 10, targetUnit);
					case MeasurementUnit.Kilometer when targetUnit == MeasurementUnit.Meter:
						return new Quantity(Value * 1000, targetUnit);
					case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.SquareKilometer:
						return new Quantity(Value / 1_000_000, targetUnit);
					case MeasurementUnit.SquareKilometer when targetUnit == MeasurementUnit.SquareMeter:
						return new Quantity(Value * 1_000_000, targetUnit);
					case MeasurementUnit.Are when targetUnit == MeasurementUnit.SquareMeter:
						return new Quantity(Value * 100, targetUnit);
					case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.Are:
						return new Quantity(Value / 100, targetUnit);
					case MeasurementUnit.Hectare when targetUnit == MeasurementUnit.SquareMeter:
						return new Quantity(Value * 10_000, targetUnit);
					case MeasurementUnit.SquareMeter when targetUnit == MeasurementUnit.Hectare:
						return new Quantity(Value / 10_000, targetUnit);
				}
				// if the exception is thrown, show message to the user he tries to convert from incompatible units
				throw new Exception($"Conversion from {Unit} to {targetUnit} not defined.");
			}

			public Quantity Add(Quantity other)
			{
				if (this.Unit == other.Unit)
				{
					return new Quantity(this.Value + other.Value, this.Unit);
				}
				else
				{
					Quantity otherConverted = other.ConvertTo(this.Unit);
					return new Quantity(this.Value + otherConverted.Value, this.Unit);
				}
			}

			public Quantity Subtract(Quantity other)
			{
				if (this.Unit == other.Unit)
				{
					return new Quantity(this.Value - other.Value, this.Unit);
				}
				else
				{
					Quantity otherConverted = other.ConvertTo(this.Unit);
					return new Quantity(this.Value - otherConverted.Value, this.Unit);
				}
			}

			public Quantity Multiply(decimal factor)
			{
				return new Quantity(this.Value * factor, this.Unit);
			}

			public Quantity Divide(decimal divisor)
			{
				if (divisor == 0) throw new DivideByZeroException();
				return new Quantity(this.Value / divisor, this.Unit);
			}
		}
	}

public enum MeasurementUnit
{
	[Description("currency")]
	Money,
	[Display(Name = "mg")]
	Milligram,
	[Display(Name = "g")]
	Gram,
	[Display(Name = "kg")]
	Kilogram,
	[Display(Name = "ml")]
	Milliliter,
	[Display(Name = "L")]
	Liter,
	[Display(Name = "cm")]
	Centimeter,
	[Display(Name = "mm")]
	Millimeter,
	[Display(Name = "m")]
	Meter,
	[Display(Name = "km")]
	Kilometer,


	[Display(Name = "Square Meter (m²)")]
	SquareMeter,
	[Display(Name = "Square Kilometer (km²)")]
	SquareKilometer,
	[Display(Name = "Are (a)")]
	Are,
	[Display(Name = "Hectare (ha)")]
	Hectare

}

// helper class
public class MeasurementUnitItem
{
	public MeasurementUnit TypeOfMeasurementUnit { get; set; }
	public string DisplayName { get; set; }
	public static string GetDescription(MeasurementUnit typeOfMeasurementUnit)
	{
		if (typeOfMeasurementUnit == MeasurementUnit.Money)
		{
			return CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol;
		}

		var type = typeOfMeasurementUnit.GetType();
		var memberInfo = type.GetMember(typeOfMeasurementUnit.ToString());
		var attributes = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
		return attributes.Length > 0 ? ((DescriptionAttribute)attributes[0]).Description : typeOfMeasurementUnit.ToString();
	}
}