﻿

// ANDROID IsVisible PROBLEM BUG when something becomes visible layout does not update and has no space to show ui elements


// IconModel => IVisualElementModel - some interface instead of exactr class



// color buttons visuals in maintype page to make the effect on click (border width)
// check equal after changing main event type - icons implementation
// consider removing 					OnPropertyChanged(); in subeventtype model

// https://www.youtube.com/watch?v=rYdJP2t7foU
// change main type color for icons
// main events types do not segregate 
// monthly view days of the week names are not centered
/*Severity Count	Data Context	Binding Path	Target	Target Type	Description	File	Line	Project
Error	1	EventOperationsViewModel	IsCompleteFrameCommand	TapGestureRecognizer.Command	ICommand	'IsCompleteFrameCommand' property not found on 'CalendarT1.ViewModels.EventOperations.EventOperationsViewModel', target property: 'Microsoft.Maui.Controls.TapGestureRecognizer.Command'    C: \Users\invis\source\repos\CalendarT1\CalendarT1\Views\EventPage.xaml	125	CalendarT1

//< Frame.GestureRecognizers >
//	< TapGestureRecognizer Command = "{Binding IsCompleteFrameCommand}" />
//</ Frame.GestureRecognizers >
*/
// EventOperationsBaseViewModel event types selections
// colors for main event types (border color of mainevent type and inside user event type color)
// Can add new usertypeevent without maintypeselected!!!
// ...Add new event error and main events are not working as should (some null reference exception)
// daily view shakes as with something before (collectionview or something)



// make class color picker 	->	private void InitializeColorButtons() // also to extract as a separate custom control

// edit subtasks page?? how to remove and edit??
//     change in cc deselected color to dynamic resource

/*		MAIN EVENT TYPES CHANGE
 * 
 * 
 * public event Action OnMainEventTypesListChanged;    // TODO - implement
 * 
 * in add new type change this for edit mode !!!!
 * DefaultEventTimespanCCHelper.DurationValue = 30;
DefaultEventTimespanCCHelper.SelectedUnitIndex = 2;

 
check what happens if removed		// REGISTER ROUTING
		Routing.RegisterRoute(nameof(AddNewSubTypePage), typeof(AddNewSubTypePage));
		Routing.RegisterRoute(nameof(AllSubTypesPage), typeof(AllSubTypesPage));



		// Change the first day of the week to Monday
		CultureInfo.CurrentCulture = new CultureInfo("pl-PL", false);
		CultureInfo.CurrentUICulture = new CultureInfo("pl-PL", false);
		CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek = DayOfWeek.Monday;

 */


#region Current
// check if event title exists before adding new main event

// change this so it deserialize from file path instead of serializing and deserializing...
/*public List<ISubEventTypeModel> DeepCopyMainEventTypesList()
{
	var settings = JsonSerializerSettings_Auto;
	var serialized = JsonConvert.SerializeObject(_allMainEventTypesList, settings);
	return JsonConvert.DeserializeObject<List<ISubEventTypeModel>>(serialized, settings);
}*/

// calculations page max by week 0 value bug (two same dates shown if the value is 0)
// change color picker
// consider??? weekly add days horizontally so there could be columnspan for time the event takes
//DeleteEventCommand are you sure... popup

//click on iscompleted frame checks the checkbox
// consider changing weekpage gotoevent change to gotoday instead- check on phone if labels are not too small
// add tapgesturerecognizer to weekly page same as monthly to go to expty frame date
// searchbox, iscomplete filtering
// consider text color picker for user settings
//All Events Page EventsToShowList and userEventTypesList

// remove the below code and decouple IHelperclass from ICC and make it a separate interfaces

// search button in allEventsPage
// save events to some random file.
// load events from some random file.
// EventsToShowList in valuetypecalculations page
// add other calculations to calculations page 
// change the looks 

// problem with weekly events page, if i delete new OC page is not showing any events, if i dont, calculations dont update as intended maybe make it create new instance every time it runs??
// WEEKLY PAGE if only 1 event show its name now it shows ---1---
//_eventsOrderedByDateList does not update when adding new event
// consider adding canexecute to calcutations commands with check if same values types are selected
// valuetype calculations advanced calculations adding
//                         <Button Text="{Binding}" Command="{Binding BindingContext.GoToWeeksPageCommand, Source={RelativeSource AncestorType={x:Type ContentPage}}}"

// TODO mapping is not considered in calculations for now!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!



// this cant use (List<Quantity>		public bool DoValueTypesCalculations(List<Quantity> allUserEvents, DateTime from, DateTime to)
// it has to perform complex calculattions and needs to know about each events dates and values




// valuepage to set dates at start 		public void SetFilterDatesValues()




// EDIT VALUE TYPE PAGE TO ADD NEW CONTROLS

// AllEventPpage filtering with main events and user events
//advanced search in all events page



// fix this 	error on weekly page						frame.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);

// WeeklyEventsPage auto GenerateGrid not working 
// Value type operations,
// GoTo change page routing 
// entrytext to change and check
// make same refactoring to EventTypesCustomControl as with MainEventTypesAllCC (helper class and interface)




// 				XXX Visual for selected user type and user type custom controll at all

// TODO THERE IS NO EVENT TYPE SELECTION WORKING!!!! EventOperationsBaseViewModel.cs


// make custom controll for main event types and make interface for it


/*private async Task SubmitType()
{
	if (IsEdit)
	{
		// cannot change main event type => may lead to some future errors???
		_currentType.EventTypeName = TypeName;
		_currentType.EventTypeColor = SelectedColor;
		await _eventRepository.UpdateEventTypeAsync(_currentType);
		await Shell.Current.GoToAsync("..");                                // TODO CHANGE NOT WORKING!!!

	}
	else
	{
		var newUserType = Factory.CreateNewEventType(_selectedEventType, TypeName, _selectedColor);
		await _eventRepository.AddUserEventTypeAsync(newUserType);
		await Shell.Current.GoToAsync("..");                                // TODO CHANGE NOT WORKING!!!
	}
}
*/

// TODO !!!!!!!!!!!!!!!!!!!!!!!!  IF THERE ARE NO ITEMS IN THE REPOSITORY, ADD SOME DEFAULT ITEMS

// Measurement Units added for events operations

// alleventslist with custom filters and sorting for each main operations and with custom date filtering

// IsCompleted should call onpropertychanged for eventvisiblecolor

// order allevents by date and time
// switching between sites stucks sometimes
// can change event types only in its assigned type??
// see what type is event
// in event edit mode selected picker item is not the same as event type

// split DailyEventsPage into two pages one for events and one for all eventslist

// all events page for types deleting (error)


// edit page with no go to all types page button
// After Editing event type in AddNewSubTypePage go back to AllSubTypesPage and update the list of event types list is not updating???
// when deleting an event type ask user what to do with events of this type switch to other similar type or delete all events of this type
// all event types page with select an event type and edit

// notify view when event is added/edited/deleted by using _eventRepository.AllEventsList


// LocalMachineEventRepository Async await fix

// TO Consider AbstractEventModel ctor - postpone time and maybe some other extra options for advanced event adding mode??

// Test json file for event types
// delete bad event types from json file
// page for removing event types
// page for editing event types

// quicknotes page
// all events list page with filrers and sorting

// FACTORIES FOR EACH TYPE OF EVENT - make it work XD


// add eventtypes page

/*add event reminder page and reminding service for it + some refactoring of DI
 * 
// event share if the event has some other type then the users inform user and ask if he wants to add it or set the event to own type
// delete type of event ask the user if he wants to delete all events of this type or switch them to another type

 
 
 * Add some color indicator to show what color the exact event type is in add/editEvent Pages
 * 
 * 	// change shareeventsModel to probably DeepLingking model
 * 	event reminder
	// change inproperties .wait() to async methods with await()
 */ // quicklist shoppinglist/todo list
#endregion









#region High Priority
//if (!typesToSaveFromSpecifiedEvents.Contains(eventItem.EventType))		// consider passing userEventTypesList as parameter

// ADD SEARCH OPTION TO ALL EVENTS PAGE
// Check for data Operations thread safety!!!
// divide dailyeventsPage into two pages one for events and one for all eventslist for types
//Change to also visually select proper event type in AllEventPage (ISubEventTypeModel)

// fix the scrolling isse horizontal and vertical
// save events button available only if there were changes made - share events button only if there are no changes made
// highest priority color in weekly view
// add page if multiple events in one hour on weekly view or some popup to show all events in an hour
// PAGE FOR searching events
// GOOGLE CALENDAR API
// make reminders for events
// show events in the hourly order

// consider habit
#endregion


#region Low Priority
// make a buttons to go one day forward and one day back
// make a button to go to current day (today)
// add checked event button and button to show only unchecked events
#endregion


