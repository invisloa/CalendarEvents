using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.HelperClass;
using CalendarT1.Views.CustomControls;
using CalendarT1.Views.CustomControls.CCHelperClass;
using CalendarT1.Views.CustomControls.CCHelperClass.CalendarT1.Views.CustomControls.CCHelperClass;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CalendarT1.Services
{
    public static class Factory
	{

		// Event Repository
		public static ObservableCollection<MeasurementUnitItem> PopulateMeasurementCollection()
		{
			return new ObservableCollection<MeasurementUnitItem>(
			Enum.GetValues(typeof(MeasurementUnit))
			.Cast<MeasurementUnit>()
			.Select(unit => new MeasurementUnitItem(unit)
				{
					DisplayName = unit == MeasurementUnit.Money
					? CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol
					: unit.GetDescription()

				}));
		}
		public static IMeasurementSelectorCC CreateMeasurementSelectorCCHelperClass()
		{
			return new MeasurementSelectorCCHelperClass();
		}
		public static IMeasurementOperationsHelperClass CreateMeasurementOperationsHelperClass(ObservableCollection<IGeneralEventModel> eventsToCalculateList)
		{
			return new MeasurementOperationsHelperClass(eventsToCalculateList);
		}
		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, Quantity quantityAmount = null, List<MultiTask> multiTasks = null, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false)
		{
			EventModelBuilder builder = new EventModelBuilder(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			if (quantityAmount != null)
				builder.SetQuantityAmount(quantityAmount);
			if (multiTasks != null)
				builder.SetMultiTasksList(multiTasks);
			return builder.Build();
		}
		public static IUserEventTypeModel CreateNewEventType(IMainEventType mainEventType, string eventTypeName, Color eventTypeColor, TimeSpan defaultEventTime, Quantity quantity)
		{
			return new UserEventTypeModel(mainEventType, eventTypeName, eventTypeColor, defaultEventTime, quantity);
		}


		public static IMainEventTypesCC CreateNewIMainEventTypeHelperClass(List<IMainEventType> mainEventTypes)
		{
			return new MainEventTypesCCHelper(mainEventTypes);
		}

		public static IFilterDatesCCHelperClass CreateFilterDatesCCHelperClass()
		{
			return new FilterDatesCCHelperClass();
		}

		internal static DefaultEventTimespanCCHelperClass CreateNewDefaultEventTimespanCCHelperClass()
		{
			return new DefaultEventTimespanCCHelperClass();
		}
	}
}
