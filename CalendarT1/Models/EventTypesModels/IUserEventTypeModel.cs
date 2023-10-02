﻿using CalendarT1.Models.EventModels;

namespace CalendarT1.Models.EventTypesModels
{
	public interface IUserEventTypeModel : IEquatable<IUserEventTypeModel>
	{
		IMainEventType MainEventType { get; set; }
		public bool IsValueType { get; set; }	// if the event type is a value type, it will be shown in the value type list
		public bool IsMultiTaskType { get; set; }   // if the event type is a multitask, it will be shown in the multitask list
		Color EventTypeColor { get; set; }	// original event color
		Color BackgroundColor { get; set; }	// color that is currently shown (isCompleted color adjustment)
		string EventTypeColorString { get; set; }	// needed for json serialization
		string EventTypeName { get; set; }	
		bool IsSelectedToFilter { get; set; }
		Quantity QuantityAmount { get; set; }	
		List<MultiTask> MultiTasksList { get; set; }
		public TimeSpan DefaultEventTimeSpan { get; set; }	// default event time for the event type
		string ToString();
		new bool Equals(IUserEventTypeModel other);	// to check if the event type is already in the list
		int GetHashCode();	// to check if the event type is already in the list
	}
}