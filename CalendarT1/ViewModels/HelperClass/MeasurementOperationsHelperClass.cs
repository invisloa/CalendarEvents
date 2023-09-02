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
		public bool DoValueTypesCalculations(List<IGeneralEventModel> allUserEvents, DateTime from, DateTime to)
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
				int days = (to - from).Days;
				int weeks = days / 7; // Simplified, consider using a more accurate method
				int months = days / 30; // Simplified
				int years = days / 365; // Simplified

				//if not full time return amount by itself
				// TO DO TODO Change the below to better calculate the amount by period
				AverageByDay = (days != 0) ? SumOfMeasurements / days : SumOfMeasurements;
				AverageByWeek = (weeks != 0) ? SumOfMeasurements / weeks : SumOfMeasurements;
				AverageByMonth = (months != 0) ? SumOfMeasurements / months : SumOfMeasurements;
				AverageByYear = (years != 0) ? SumOfMeasurements / years : SumOfMeasurements;


				// Max by period
				MaxByDayCalculation(allUserEvents);
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
		#region MeasurementMethods
		// TODO 
		// Avarage by day of occurrence
		private void MaxByDayCalculation(List<IGeneralEventModel> allEventsList)
		{
			if (allEventsList == null || !allEventsList.Any())
			{
				return;
			}

			decimal currentDaySum = 0;
			decimal currentMaxByDay = 0;
			var eventsOrderedByDateList = allEventsList.OrderBy(x => x.StartDateTime).ToList();
			var tempForDayWithMaxValueCheck = eventsOrderedByDateList[0].StartDateTime.Date;
			var daysWithMaxValuesList = new List<DateTime>() { tempForDayWithMaxValueCheck };

			foreach (var item in eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // Skip this iteration if QuantityAmount is null
				}

				if (item.StartDateTime.Date != tempForDayWithMaxValueCheck)
				{
					UpdateMaxByDay(ref currentDaySum, ref currentMaxByDay, tempForDayWithMaxValueCheck, daysWithMaxValuesList);
					currentDaySum = 0;
					tempForDayWithMaxValueCheck = item.StartDateTime.Date;
				}

				currentDaySum += item.QuantityAmount.Value;
			}

			UpdateMaxByDay(ref currentDaySum, ref currentMaxByDay, tempForDayWithMaxValueCheck, daysWithMaxValuesList);

			MaxByDay = currentMaxByDay;
			MaxByDayDatesList = daysWithMaxValuesList;
		}

		private void UpdateMaxByDay(ref decimal currentDaySum, ref decimal currentMaxByDay, DateTime date, List<DateTime> daysWithMaxValuesList)
		{
			if (currentDaySum > currentMaxByDay)
			{
				currentMaxByDay = currentDaySum;
				daysWithMaxValuesList.Clear();
				daysWithMaxValuesList.Add(date);
			}
			else if (currentDaySum == currentMaxByDay)
			{
				daysWithMaxValuesList.Add(date);
			}
		}

		private void MinByDayCalculation(List<IGeneralEventModel> allEventsList)
		{
			if (allEventsList == null || !allEventsList.Any())
			{
				return;
			}

			decimal currentDaySum = 0;
			decimal currentMinByDay = decimal.MaxValue;  // Initialize to maximum decimal value
			var eventsOrderedByDateList = allEventsList.OrderBy(x => x.StartDateTime).ToList();
			var tempForDayWithMinValueCheck = eventsOrderedByDateList[0].StartDateTime.Date;
			var daysWithMinValuesList = new List<DateTime>();

			foreach (var item in eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // Skip this iteration if QuantityAmount is null
				}

				if (item.StartDateTime.Date != tempForDayWithMinValueCheck)
				{
					UpdateMinByDay(ref currentDaySum, ref currentMinByDay, tempForDayWithMinValueCheck, daysWithMinValuesList);
					currentDaySum = 0;
					tempForDayWithMinValueCheck = item.StartDateTime.Date;
				}

				currentDaySum += item.QuantityAmount.Value;
			}

			UpdateMinByDay(ref currentDaySum, ref currentMinByDay, tempForDayWithMinValueCheck, daysWithMinValuesList);

			MinByDay = currentMinByDay; 
			MinByDayDatesList = daysWithMinValuesList; 
		}

		private void UpdateMinByDay(ref decimal currentDaySum, ref decimal currentMinByDay, DateTime date, List<DateTime> daysWithMinValuesList)
		{
			if (currentDaySum < currentMinByDay)
			{
				currentMinByDay = currentDaySum;
				daysWithMinValuesList.Clear();
				daysWithMinValuesList.Add(date);
			}
			else if (currentDaySum == currentMinByDay)
			{
				daysWithMinValuesList.Add(date);
			}
		}
		private void MaxByWeekCalculation(List<IGeneralEventModel> allEventsList)
		{
			if (allEventsList == null || !allEventsList.Any())
			{
				return;
			}
			decimal currentWeekSum = 0;
			decimal currentMaxByWeek = 0;
			var eventsOrderedByDateList = allEventsList.OrderBy(x => x.StartDateTime).ToList();
			var currentWeekInfo = GetWeekInfo(eventsOrderedByDateList[0].StartDateTime);
			var weeksWithMaxValuesList = new List<(int weekOfYear, int year)>();
			foreach (var item in eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // Skip this iteration if QuantityAmount is null
				}
				var thisWeekInfo = GetWeekInfo(item.StartDateTime);
				if (thisWeekInfo != currentWeekInfo)
				{
					UpdateMaxByWeek(ref currentWeekSum, ref currentMaxByWeek, currentWeekInfo, weeksWithMaxValuesList);
					currentWeekSum = 0;
					currentWeekInfo = thisWeekInfo;
				}
				currentWeekSum += item.QuantityAmount.Value;
			}

			UpdateMaxByWeek(ref currentWeekSum, ref currentMaxByWeek, currentWeekInfo, weeksWithMaxValuesList);
			MaxByWeek = currentMaxByWeek; 
			MaxByWeekInfoList = weeksWithMaxValuesList; 
		}

		private (int weekOfYear, int year) GetWeekInfo(DateTime date)
		{
			var calendar = CultureInfo.CurrentCulture.Calendar;
			var weekOfYear = calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Sunday);
			var year = date.Year;
			return (weekOfYear, year);
		}

		private void UpdateMaxByWeek(ref decimal currentWeekSum, ref decimal currentMaxByWeek, (int weekOfYear, int year) weekInfo, List<(int weekOfYear, int year)> weeksWithMaxValuesList)
		{
			if (currentWeekSum > currentMaxByWeek)
			{
				currentMaxByWeek = currentWeekSum;
				weeksWithMaxValuesList.Clear();
				weeksWithMaxValuesList.Add(weekInfo);
			}
			else if (currentWeekSum == currentMaxByWeek)
			{
				weeksWithMaxValuesList.Add(weekInfo);
			}
		}
		private void MinByWeekCalculation(List<IGeneralEventModel> allEventsList)
		{
			if (allEventsList == null || !allEventsList.Any())
			{
				return;
			}

			decimal currentWeekSum = 0;
			decimal currentMinByWeek = decimal.MaxValue;  // Initialize to maximum decimal value
			var eventsOrderedByDateList = allEventsList.OrderBy(x => x.StartDateTime).ToList();
			var currentWeekInfo = GetWeekInfo(eventsOrderedByDateList[0].StartDateTime);
			var weeksWithMinValuesList = new List<(int weekOfYear, int year)>();

			foreach (var item in eventsOrderedByDateList)
			{
				if (item.QuantityAmount?.Value == null)
				{
					throw new Exception(); // Skip this iteration if QuantityAmount is null
				}

				var thisWeekInfo = GetWeekInfo(item.StartDateTime);
				if (thisWeekInfo != currentWeekInfo)
				{
					UpdateMinByWeek(ref currentWeekSum, ref currentMinByWeek, currentWeekInfo, weeksWithMinValuesList);
					currentWeekSum = 0;
					currentWeekInfo = thisWeekInfo;
				}

				currentWeekSum += item.QuantityAmount.Value;
			}

			UpdateMinByWeek(ref currentWeekSum, ref currentMinByWeek, currentWeekInfo, weeksWithMinValuesList);

			MinByWeek = currentMinByWeek; 
			MinByWeekInfoList = weeksWithMinValuesList;  
		}

		private void UpdateMinByWeek(ref decimal currentWeekSum, ref decimal currentMinByWeek, (int weekOfYear, int year) weekInfo, List<(int weekOfYear, int year)> weeksWithMinValuesList)
		{
			if (currentWeekSum < currentMinByWeek)
			{
				currentMinByWeek = currentWeekSum;
				weeksWithMinValuesList.Clear();
				weeksWithMinValuesList.Add(weekInfo);
			}
			else if (currentWeekSum == currentMinByWeek)
			{
				weeksWithMinValuesList.Add(weekInfo);
			}
		}
		#endregion
	}
}
