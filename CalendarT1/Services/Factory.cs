using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.HelperClass;
using CalendarT1.Views.CustomControls;
using CalendarT1.Views.CustomControls.CCViewModels;
using CalendarT1.Views.CustomControls.CCViewModels.CalendarT1.Views.CustomControls.CCHelperClass;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CalendarT1.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using System.Collections.ObjectModel;
using System.Globalization;

namespace CalendarT1.Services
{
    public static class Factory
	{

		// Event Repository
		public static ObservableCollection<MeasurementUnitItem> PopulateMeasurementCollection()
		{
			var measurementUnitItems = new ObservableCollection<MeasurementUnitItem>(
			Enum.GetValues(typeof(MeasurementUnit))
			.Cast<MeasurementUnit>()
			.Select(unit => new MeasurementUnitItem(unit)
				{
					DisplayName = unit == MeasurementUnit.Money
					? CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol
					: unit.GetDescription()

				}));
			return measurementUnitItems;
		}
		public static MeasurementSelectorCCViewModel CreateNewMeasurementSelectorCCHelperClass()
		{
			return new MeasurementSelectorCCViewModel();
		}
		public static IMeasurementOperationsHelperClass CreateMeasurementOperationsHelperClass(ObservableCollection<IGeneralEventModel> eventsToCalculateList)
		{
			return new MeasurementOperationsHelperClass(eventsToCalculateList);
		}
		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, ISubEventTypeModel eventTypeModel, QuantityModel quantityAmount = null, List<MicroTaskModel> microTasks = null, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false)
		{
			EventModelBuilder builder = new EventModelBuilder(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			if (quantityAmount != null)
				builder.SetQuantityAmount(quantityAmount);
			if (microTasks != null)
				builder.SetMicroTasksList(microTasks);
			return builder.Build();
		}
		public static ISubEventTypeModel CreateNewEventType(IMainEventType mainEventType, string eventTypeName, Color eventTypeColor, TimeSpan defaultEventTime, QuantityModel quantity, List<MicroTaskModel> microTasksList)
		{
			return new SubEventTypeModel(mainEventType, eventTypeName, eventTypeColor, defaultEventTime, quantity, microTasksList);
		}


		public static IMainEventTypesCC CreateNewIMainEventTypeHelperClass(List<IMainEventType> mainEventTypes)
		{
			return new MainEventTypesCCViewModel(mainEventTypes);
		}

		public static IFilterDatesCCHelperClass CreateFilterDatesCCHelperClass()
		{
			return new FilterDatesCCViewModel();
		}

		internal static DefaultEventTimespanCCViewModel CreateNewDefaultEventTimespanCCHelperClass()
		{
			return new DefaultEventTimespanCCViewModel();
		}

		internal static IUserTypeExtraOptionsViewModel CreateNewUserTypeExtraOptionsHelperClass(bool isEditMode)
		{
			return new UserTypeExtraOptionsViewModel(isEditMode);
		}

		internal static MicroTasksListCCViewModel CreateNewMicroTasksListCCHelperClass(IGeneralEventModel eventWithMicroTasks)
		{
			
			return new MicroTasksListCCViewModel(eventWithMicroTasks);
		}

		internal static MicroTasksListCCViewModel CreateNewMicroTasksListCCHelperClass(List<MicroTaskModel> listToAddMiroTasks)
		{
			return new MicroTasksListCCViewModel(listToAddMiroTasks);
		}
	}
}
