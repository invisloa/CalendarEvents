using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
		public List<DateTime> MaxByDayDatesList { get; set; }
		public decimal MaxByWeek { get; set; }
		public List<(int, int)> MaxByWeekInfoList { get; set; }
		public decimal MaxByMonth { get; set; }
		public decimal MaxByYear { get; set; }
		public decimal MinByDay { get; set; }
		public List<DateTime> MinByDayDatesList { get; set; }
		public decimal MinByWeek { get; set; }
		public List<(int, int)> MinByWeekInfoList { get; set; }
		public decimal MinByMonth { get; set; }
		public decimal MinByYear { get; set; }


		private List<MeasurementUnit> MoneyTypeMeasurements { get; set; }
		private List<MeasurementUnit> WeightTypeMeasurements { get; set; }
		private List<MeasurementUnit> VolumeTypeMeasurements { get; set; }
		private List<MeasurementUnit> DistanceTypeMeasurements { get; set; }
		private List<MeasurementUnit> TimeTypeMeasurements { get; set; }
		private List<MeasurementUnit> TemperatureTypeMeasurements { get; set; }
		private List<IGeneralEventModel> _eventsOrderedByDateList { get; set; }

		Dictionary<MeasurementUnit, List<MeasurementUnit>> measurementTypeMap;

		// CTOR
		public MeasurementOperationsHelperClass(List<IGeneralEventModel> allUserEventsList)
		{
			MoneyTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Money };
			WeightTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Gram, MeasurementUnit.Kilogram, MeasurementUnit.Milligram };
			VolumeTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Liter, MeasurementUnit.Milliliter };
			DistanceTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Centimeter, MeasurementUnit.Kilometer, MeasurementUnit.Meter, MeasurementUnit.Millimeter };
			TimeTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Week, MeasurementUnit.Day, MeasurementUnit.Hour, MeasurementUnit.Minute, MeasurementUnit.Second };
			TemperatureTypeMeasurements = new List<MeasurementUnit> { MeasurementUnit.Celsius, MeasurementUnit.Fahrenheit, MeasurementUnit.Kelvin };
			measurementTypeMap = InitializeMappingDictionary();
			_eventsOrderedByDateList = allUserEventsList.OrderBy(x => x.StartDateTime).ToList();

		}


		// Basic Calculations
		public bool DoValueTypesBasicCalculations(DateTime from, DateTime to)
		{
			if (CheckIfEventsAreSameType())
			{
				// Perform operations if all events are the same type
				SumOfMeasurements = _eventsOrderedByDateList.Sum(x => x.QuantityAmount.Value);
				AverageOfMeasurements = _eventsOrderedByDateList.Average(x => x.QuantityAmount.Value);
				MaxOfMeasurements = _eventsOrderedByDateList.Max(x => x.QuantityAmount.Value);
				MinOfMeasurements = _eventsOrderedByDateList.Min(x => x.QuantityAmount.Value);


				// Calculate by Period (using the DateTime parameters)
				int days = (to - from).Days;
				int weeks = days / 7; // Simplified, consider using a more accurate method
				int months = days / 30; // Simplified
				int years = days / 365; // Simplified

				//if not full period time return amount by itself
				// TO DO TODO Change the below to better calculate the amount by period
				AverageByDay = (days != 0) ? SumOfMeasurements / days : SumOfMeasurements;
				AverageByWeek = (weeks != 0) ? SumOfMeasurements / weeks : SumOfMeasurements;
				AverageByMonth = (months != 0) ? SumOfMeasurements / months : SumOfMeasurements;
				AverageByYear = (years != 0) ? SumOfMeasurements / years : SumOfMeasurements;
				return true;
			}
			else
			{
				return false;
			}

		}
		private bool CheckIfEventsAreSameType()
		{
			if (_eventsOrderedByDateList.Count == 0)
			{
				throw new Exception("The list of events is empty.");
			}
			else
			{
				var firstEvent = _eventsOrderedByDateList[0];
				var firstEventUnit = firstEvent.QuantityAmount.Unit;
				if (!measurementTypeMap.ContainsKey(firstEventUnit))
			{
				throw new Exception("Unsupported measurement unit.");
			}
				var measurementTypeList = measurementTypeMap[firstEventUnit];
			foreach (var item in _eventsOrderedByDateList)
				{
					if (!measurementTypeList.Contains(item.QuantityAmount.Unit))
					{
						// if any event has a different measurement type, return false
						// and do not perform any calculations
						return false;
					}
				}
				return true;
			}
		}

		// TODO mapping is not considered in calculations for now!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
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



		// ADVANCED MEASUREMENT METHODS
		#region AdvancedMeasurementMethods
		// TODO 
		// Avarage by day of occurrence (if it appears what is the average of its occurence)?- how many tickets on a day of service (average)
		// Define a delegate for the update action



		// Using custom delegate- action does not support ref parameters
		private delegate void UpdateByDayDelegate(ref decimal currentDaySum, ref decimal currentByDay, DateTime date, List<DateTime> daysWithValuesList);

		private MeasurementCalculationsOutcome CalculateByDay(UpdateByDayDelegate updateDelegate, decimal initialByDayValue)
		{
			if (_eventsOrderedByDateList == null || !_eventsOrderedByDateList.Any())
			{
				throw new Exception("The list of events is empty.");
			}

			decimal currentDaySum = 0;
			decimal currentByDay = initialByDayValue;
			var currentDateForDayWithExtremeValue = _eventsOrderedByDateList[0].StartDateTime.Date;
			var daysWithExtremeValuesList = new List<DateTime>() { currentDateForDayWithExtremeValue };

			foreach (var item in _eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // Exception if QuantityAmount is null
				}

				if (item.StartDateTime.Date != currentDateForDayWithExtremeValue)
				{
					updateDelegate(ref currentDaySum, ref currentByDay, currentDateForDayWithExtremeValue, daysWithExtremeValuesList);
					currentDaySum = 0;
					currentDateForDayWithExtremeValue = item.StartDateTime.Date;
				}

				currentDaySum += item.QuantityAmount.Value;
			}

			updateDelegate(ref currentDaySum, ref currentByDay, currentDateForDayWithExtremeValue, daysWithExtremeValuesList);

			return new MeasurementCalculationsOutcome(currentByDay, daysWithExtremeValuesList);
		}



		// Common update method
		private void UpdateByDay(ref decimal currentDaySum, ref decimal currentByDay, DateTime date, List<DateTime> daysWithValuesList, Func<decimal, decimal, bool> comparison)
		{
			if (comparison(currentDaySum, currentByDay))
			{
				currentByDay = currentDaySum;
				daysWithValuesList.Clear();
				daysWithValuesList.Add(date);
			}
			else if (currentDaySum == currentByDay)
			{
				daysWithValuesList.Add(date);
			}
		}
		// Max comparison
		private bool MaxComparison(decimal currentDaySum, decimal currentMaxByDay)
		{
			return currentDaySum > currentMaxByDay;
		}

		// Min comparison
		private bool MinComparison(decimal currentDaySum, decimal currentMinByDay)
		{
			return currentDaySum < currentMinByDay;
		}

		// Methods to call
		public MeasurementCalculationsOutcome MaxByDayCalculation()
		{
			return CalculateByDay((ref decimal sum, ref decimal byDay, DateTime date, List<DateTime> list) =>
			UpdateByDay(ref sum, ref byDay, date, list, MaxComparison), 0);
		}
		public MeasurementCalculationsOutcome MinByDayCalculation()
		{
			return CalculateByDay((ref decimal sum, ref decimal byDay, DateTime date, List<DateTime> list) =>
			UpdateByDay(ref sum, ref byDay, date, list, MinComparison), decimal.MaxValue);
		}

		private MeasurementCalculationsOutcome MaxByWeekCalculation()
		{
			if (_eventsOrderedByDateList == null || !_eventsOrderedByDateList.Any())
			{
				throw new Exception(); // Exception if _eventsOrderedByDateList is null
			}
			decimal currentWeekSum = 0;
			decimal currentMaxByWeek = 0;
			var calendar = CultureInfo.CurrentCulture.Calendar;

			var currentWeekDate = _eventsOrderedByDateList[0].StartDateTime.Date;
			var maxValueWeekDate = _eventsOrderedByDateList[0].StartDateTime.Date;
			var lastWeekNumber = calendar.GetWeekOfYear(maxValueWeekDate, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
			var weeksWithMaxValuesList = new List<DateTime>(); // any date from a week will work with my ViewWeeklyEvents page
			foreach (var item in _eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // Exception if QuantityAmount is null
				}
				currentWeekDate = item.StartDateTime.Date;
				var itemWeekNumber = calendar.GetWeekOfYear(item.StartDateTime.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
				if (itemWeekNumber != lastWeekNumber)
				{
					UpdateMaxByWeek(ref currentWeekSum, ref currentMaxByWeek, currentWeekDate, ref maxValueWeekDate, weeksWithMaxValuesList);
					currentWeekSum = 0;
					lastWeekNumber = itemWeekNumber;
				}
				currentWeekSum += item.QuantityAmount.Value;
			}

			UpdateMaxByWeek(ref currentWeekSum, ref currentMaxByWeek, currentWeekDate, ref maxValueWeekDate, weeksWithMaxValuesList);
			return new MeasurementCalculationsOutcome(currentMaxByWeek, weeksWithMaxValuesList);
		}


		private void UpdateMaxByWeek(ref decimal currentWeekSum, ref decimal currentMaxByWeek, DateTime currentWeekDate, ref DateTime maxValueWeekDate, List<DateTime> weeksWithMaxValuesList)
		{
			if (currentWeekSum > currentMaxByWeek)
			{
				currentMaxByWeek = currentWeekSum;
				weeksWithMaxValuesList.Clear();
				weeksWithMaxValuesList.Add(maxValueWeekDate);
				maxValueWeekDate = currentWeekDate;
			}
			else if (currentWeekSum == currentMaxByWeek)
			{
				weeksWithMaxValuesList.Add(maxValueWeekDate);
				maxValueWeekDate = currentWeekDate;
			}
		}

		#endregion
	}




	#region HelperClass TO MOVE TO SEPARATE FILE

	public class MeasurementCalculationsOutcome
	{
		public MeasurementCalculationsOutcome(decimal measurementValueOutcome, List<DateTime> measurementsEventsOutcome)
		{
			MeasurementValueOutcome = measurementValueOutcome;
			MeasurementDatesListOutcome = measurementsEventsOutcome;
		}

		public decimal MeasurementValueOutcome { get; set; }
		public List<DateTime> MeasurementDatesListOutcome { get; set; }
	}
	#endregion
}
