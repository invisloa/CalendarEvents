using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
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
		public bool DoValueTypesCalculations(List<Quantity> allUserEvents)
		{
			if (allUserEvents.Count == 0)
			{
				throw new Exception("The list of events is empty.");
			}
			else
			{
				var firstEvent = allUserEvents[0];
				var firstEventUnit = firstEvent.Unit;
				var firstEventValue = firstEvent.Value;

				if (!measurementTypeMap.ContainsKey(firstEventUnit))
				{
					throw new Exception("Unsupported measurement unit.");
				}

				var measurementTypeList = measurementTypeMap[firstEventUnit];
				foreach (var item in allUserEvents)
				{
					if (!measurementTypeList.Contains(item.Unit))
					{
						return false;
					}
				}

				// Perform operations
				SumOfMeasurements = allUserEvents.Sum(x => x.Value);
				AverageOfMeasurements = allUserEvents.Average(x => x.Value);
				MaxOfMeasurements = allUserEvents.Max(x => x.Value);
				MinOfMeasurements = allUserEvents.Min(x => x.Value);
				MedianOfMeasurements = allUserEvents.OrderBy(x => x.Value).ElementAt(allUserEvents.Count / 2).Value;
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
