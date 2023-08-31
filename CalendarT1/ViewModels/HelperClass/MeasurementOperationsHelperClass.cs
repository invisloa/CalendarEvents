using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.HelperClass
{
	public class MeasurementOperationsHelperClass : IMeasurementOperationsHelperClass
	{
		public decimal SumOfMeasurements { get; set; }
		public decimal AverageOfMeasurements { get; set; }
		public decimal MaxOfMeasurements { get; set; }
		public decimal MinOfMeasurements { get; set; }
		public decimal MedianOfMeasurements { get; set; }

		// Average by Day
		public decimal AverageByDay { get; set; }
		public decimal AverageByWeek { get; set; }
		public decimal AverageByMonth { get; set; }
		public decimal AverageByYear { get; set; }

		public decimal SumByDay { get; set; }
		public decimal SumByWeek { get; set; }
		public decimal SumByMonth { get; set; }
		public decimal SumByYear { get; set; }

		public decimal MaxByDay { get; set; }
		public decimal MaxByWeek { get; set; }
		public decimal MaxByMonth { get; set; }
		public decimal MaxByYear { get; set; }
		public decimal MinByDay { get; set; }
		public decimal MinByWeek { get; set; }
		public decimal MinByMonth { get; set; }
		public decimal MinByYear { get; set; }

		private List<MeasurementUnit> MoneyTypeMeasurements { get; set; }
		private List<MeasurementUnit> WeightTypeMeasurements { get; set; }
		private List<MeasurementUnit> VolumeTypeMeasurements { get; set; }
		private List<MeasurementUnit> DistanceTypeMeasurements { get; set; }
		private List<MeasurementUnit> TimeTypeMeasurements { get; set; }
		private List<MeasurementUnit> TemperatureTypeMeasurements { get; set; }

		Dictionary<MeasurementUnit, List<MeasurementUnit>> measurementTypeMap;


		public MeasurementOperationsHelperClass()
		{
			MoneyTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Money };
			WeightTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Gram, MeasurementUnit.Kilogram, MeasurementUnit.Milligram };
			VolumeTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Liter, MeasurementUnit.Milliliter };
			DistanceTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Centimeter, MeasurementUnit.Kilometer, MeasurementUnit.Meter, MeasurementUnit.Millimeter };
			TimeTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Week, MeasurementUnit.Day, MeasurementUnit.Hour, MeasurementUnit.Minute, MeasurementUnit.Second };
			TemperatureTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Celsius, MeasurementUnit.Fahrenheit, MeasurementUnit.Kelvin };

			measurementTypeMap = InitializeMappingDictionary();
		}
		public bool DoValueTypesCalculations(List<IUserEventTypeModel> allUserEvents, DateTime from, DateTime to)
		{
			if (allUserEvents.Count == 0)
			{
				throw new Exception("The list of events is empty.");
			}
			else
			{
				var firstEvent = allUserEvents[0];
				var firstEventUnit = firstEvent.QuantityAmount.Unit;

				if (!measurementTypeMap.ContainsKey(firstEventUnit))
				{
					throw new Exception("Unsupported measurement unit.");
				}

				var measurementTypeList = measurementTypeMap[firstEventUnit];
				foreach (var item in allUserEvents)
				{
					if (!measurementTypeList.Contains(item.QuantityAmount.Unit))
					{
						// if any event has a different measurement type, return false
						// and do not perform any calculations
						return false;
					}
				}

				// Perform operations if all events are the same type
				SumOfMeasurements = allUserEvents.Sum(x => x.QuantityAmount.Value);
				AverageOfMeasurements = allUserEvents.Average(x => x.QuantityAmount.Value);
				MaxOfMeasurements = allUserEvents.Max(x => x.QuantityAmount.Value);
				MinOfMeasurements = allUserEvents.Min(x => x.QuantityAmount.Value);

				// Median
				var sortedEvents = allUserEvents.Select(x => x.QuantityAmount.Value).OrderBy(x => x).ToList();
				var count = sortedEvents.Count;
				if (count % 2 == 0)
				{
					MedianOfMeasurements = (sortedEvents[count / 2 - 1] + sortedEvents[count / 2]) / 2;
				}
				else
				{
					MedianOfMeasurements = sortedEvents[count / 2];
				}

				// Calculate by Period (using the DateTime parameters)
				// For the purpose of this example, I'll assume you have a way to get the number of days, weeks, months, and years for the given period
				int days = (to - from).Days;
				int weeks = days / 7; // Simplified, consider using a more accurate method
				int months = days / 30; // Simplified
				int years = days / 365; // Simplified

				//if not full time return amount by itself
				AverageByDay = (days != 0) ? SumOfMeasurements / days : SumOfMeasurements;
				AverageByWeek = (weeks != 0) ? SumOfMeasurements / weeks : SumOfMeasurements;
				AverageByMonth = (months != 0) ? SumOfMeasurements / months : SumOfMeasurements;
				AverageByYear = (years != 0) ? SumOfMeasurements / years : SumOfMeasurements;


				// Sums by period
				SumByDay = SumOfMeasurements; 
				SumByWeek = SumOfMeasurements; 
				SumByMonth = SumOfMeasurements; 
				SumByYear = SumOfMeasurements; 

				// Max by period
				MaxByDay = MaxOfMeasurements; 
				MaxByWeek = MaxOfMeasurements; 
				MaxByMonth = MaxOfMeasurements; 
				MaxByYear = MaxOfMeasurements; 

				// Min by period
				MinByDay = MinOfMeasurements; 
				MinByWeek = MinOfMeasurements; 
				MinByMonth = MinOfMeasurements; 
				MinByYear = MinOfMeasurements; 

				return true;
			}
		}

		private Dictionary<MeasurementUnit, List<MeasurementUnit>> InitializeMappingDictionary()
		{
			return new Dictionary<MeasurementUnit, List<MeasurementUnit>>
			{
					{ MeasurementUnit.Money, MoneyTypeMeasurements },
					{ MeasurementUnit.Gram, WeightTypeMeasurements },
					{ MeasurementUnit.Kilogram, WeightTypeMeasurements },
					{ MeasurementUnit.Milligram, WeightTypeMeasurements },
					{ MeasurementUnit.Liter, VolumeTypeMeasurements },
					{ MeasurementUnit.Milliliter, VolumeTypeMeasurements },
					{ MeasurementUnit.Centimeter, DistanceTypeMeasurements },
					{ MeasurementUnit.Kilometer, DistanceTypeMeasurements },
					{ MeasurementUnit.Meter, DistanceTypeMeasurements },
					{ MeasurementUnit.Millimeter, DistanceTypeMeasurements },
					{ MeasurementUnit.Week, TimeTypeMeasurements },
					{ MeasurementUnit.Day, TimeTypeMeasurements },
					{ MeasurementUnit.Hour, TimeTypeMeasurements },
					{ MeasurementUnit.Minute, TimeTypeMeasurements },
					{ MeasurementUnit.Second, TimeTypeMeasurements },
					{ MeasurementUnit.Celsius, TemperatureTypeMeasurements },
					{ MeasurementUnit.Fahrenheit, TemperatureTypeMeasurements },
					{ MeasurementUnit.Kelvin, TemperatureTypeMeasurements }
				};
		}
	}
}
