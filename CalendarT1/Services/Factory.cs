﻿using CalendarT1.Models.EventModels;
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
		public static IMeasurementOperationsHelperClass CreateMeasurementOperationsHelperClass(IEventRepository eventRepository)
		{
			return new MeasurementOperationsHelperClass(eventRepository);
		}
		public static IGeneralEventModel CreatePropperEvent(string title, string description, DateTime startTime, DateTime endTime, IUserEventTypeModel eventTypeModel, Quantity quantityAmount = null, bool isCompleted = false, TimeSpan? postponeTime = null, bool wasShown = false)
		{
			if (eventTypeModel.MainEventType == MainEventTypes.Event)
			{
				return new EventModel(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			}
			else if (eventTypeModel.MainEventType == MainEventTypes.Task)
			{
				return new TaskModel(title, description, startTime, endTime, eventTypeModel, isCompleted, postponeTime, wasShown);
			}
			else	// Value type event
			{
				return new ValueModel(title, description, startTime, endTime, eventTypeModel, quantityAmount, isCompleted, postponeTime, wasShown);
			}
		}

		public static IUserEventTypeModel CreateNewEventType(MainEventTypes mainEventType, string eventTypeName, Color eventTypeColor, Quantity quantity)
		{
			return new UserEventTypeModel(mainEventType, eventTypeName, eventTypeColor, quantity);
		}

		public static IMainEventTypesCC CreateNewIMainEventTypeHelperClass()
		{
			return new MainEventTypesCCHelper();
		}

		public static IFilterDatesCCHelperClass CreateFilterDatesCCHelperClass()
		{
			return new FilterDatesCCHelperClass();
		}
	}
}
