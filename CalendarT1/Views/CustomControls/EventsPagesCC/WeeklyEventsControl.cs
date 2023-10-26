namespace CalendarT1.Views.CustomControls
{
	using CalendarT1.Models.EventModels;
	using Microsoft.Maui.Controls;
	using System;
    using System.Collections.ObjectModel;
    using System.Linq;
	using static CalendarT1.App;
	using MauiGrid = Microsoft.Maui.Controls.Grid;

    public class WeeklyEventsControl : BaseEventPageCC
	{
		private readonly int _minimumDayOfWeekWidthRequest = 45;
		private readonly int _minimumDayOfWeekHeightRequest = 55;
		private readonly double _firstColumnForHoursWidth = 35;

		public void GenerateGrid()
		{
			// Clear the existing rows and columns
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();
			int dayOfWeekNumber = (int)CurrentSelectedDate.DayOfWeek;

			for (int day = 0; day < 7; day++)
			{
				var dayOfWeekLabel = new Label { FontSize = _dayNamesFontSize, FontAttributes = FontAttributes.Bold,
									Text = $"{((DayOfWeek)day).ToString().Substring(0, 3)} {CurrentSelectedDate.AddDays(day - dayOfWeekNumber).ToString("dd")}" };
				Grid.SetRow(dayOfWeekLabel, 1);  // Place the day of the week label in the second row
				Grid.SetColumn(dayOfWeekLabel, day + 1);  // Adjust column index by 1 to make space for the hour indicator
				Children.Add(dayOfWeekLabel);
			}

			// Create rows for each hour + 2 extra rows for the day of the week and date label
			for (int i = 0; i < 24 + 2; i++)
			{
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}

			// Create columns for each day + first extra column for the hour indicator
			for (int i = 0; i < 8; i++)
			{
				if (i == 0)
					ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_firstColumnForHoursWidth) });
				else
					ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}

			// Add cells for each event
			for (int hour = 0; hour < 24; hour++)
			{
				// Add hour indicator
				var hourLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{hour:D2}" };
				hourLabel.WidthRequest = _firstColumnForHoursWidth;
				Grid.SetRow(hourLabel, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
				Grid.SetColumn(hourLabel, 0);  // Place the hour indicator in the first column
				Children.Add(hourLabel);

				for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
				{
					// Create a frame for each cell
					var frame = new Frame { BorderColor = _frameBorderColor, Padding = 5, BackgroundColor = _emptyLabelColor, MinimumWidthRequest = _minimumDayOfWeekWidthRequest, MinimumHeightRequest = _minimumDayOfWeekHeightRequest };
					
					// Get the events for the current day and hour
					var dayEvents = EventsToShowList
						.Where(e => e.StartDateTime.Date == CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek).Date
									&& e.StartDateTime.Hour == hour).ToList(); // to Check ??

					// If there are events
					if (dayEvents != null && dayEvents.Count > 0)
					{
						// Create a StackLayout for the events
						var stackLayout = new StackLayout();

						// If there are more items than the limit, add a 'See more' label
						if (dayEvents.Count > _displayEventsLimit)
						{
							var moreLabel = new Label
							{
								FontSize = 15,
								FontAttributes = FontAttributes.Italic,
								Text = $"... {dayEvents.Count} ...",
								TextColor = _eventTextColor,
								BackgroundColor = _moreEventsLabelColor
							};
							var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer();
							tapGestureRecognizerForMoreEvents.Command = GoToSelectedDateCommand;
							tapGestureRecognizerForMoreEvents.CommandParameter = CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek);
							frame.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
							moreLabel.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
							stackLayout.Children.Add(moreLabel);
						}

						// If there is only one event, add a label with the event title and its icon
						else if (dayEvents.Count == 1)
						{
							var eventItem = dayEvents[0];

							var title = new Label
							{
								FontAttributes = FontAttributes.Bold,
								Text = eventItem.Title,
							};

							var description = new Label
							{
								Text = eventItem.Description,
							};

							var eventTypeLabel = new Label
							{
								Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
								TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
								Style = Styles.GoogleFontStyle,
								HorizontalOptions = LayoutOptions.Center,
								VerticalOptions = LayoutOptions.Center
							};

							var eventTypeFrame = new Frame
							{
								BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor,
								Padding = 0,
								Content = eventTypeLabel,
								HorizontalOptions = LayoutOptions.End,
								VerticalOptions = LayoutOptions.Center
							};

							var eventStackLayout = new StackLayout
							{
								Children = { title, description }
							};
							var grid = new Grid
							{
								ColumnDefinitions =
								{
									new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
									new ColumnDefinition { Width = new GridLength(50) }
								},
								Children = { eventStackLayout, eventTypeFrame }
							};

							Grid.SetColumn(eventTypeFrame, 1);

							var eventFrame = new Frame
							{
								BackgroundColor = eventItem.EventVisibleColor,
								Content = grid
							};

							var tapGestureRecognizer = new TapGestureRecognizer
							{
								Command = EventSelectedCommand,
								CommandParameter = eventItem
							};

							eventFrame.GestureRecognizers.Add(tapGestureRecognizer);
							stackLayout.Children.Add(eventFrame);  
							frame.Content = stackLayout;
						}
						else
						{
							var eventItemsStackLayout = new StackLayout
							{
								Orientation = StackOrientation.Vertical,  // Changed to Vertical to stack events vertically
								Spacing = 2, // Minimal spacing between event items
							};

							for (int i = 0; i < Math.Min(dayEvents.Count, _displayEventsLimit); i++)
							{
								var eventItem = dayEvents[i];

								var title = new Label
								{
									FontAttributes = FontAttributes.Bold,
									Text = eventItem.Title,
									LineBreakMode = LineBreakMode.TailTruncation, // Ensure text doesn't overflow
								};

								var eventTypeLabel = new Label
								{
									Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
									TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
									Style = Styles.GoogleFontStyle,
									HorizontalOptions = LayoutOptions.End,
									VerticalOptions = LayoutOptions.Center,
								};

								var eventTypeFrame = new Frame
								{
									BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor,
									Padding = 0,
									Content = eventTypeLabel,
									HorizontalOptions = LayoutOptions.End,
									VerticalOptions = LayoutOptions.Center,
									WidthRequest = 50  // Set a fixed width for the event type frame
								};

								var grid = new Grid
								{
									ColumnDefinitions =
									{
										new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
										new ColumnDefinition { Width = new GridLength(50) }  // Same width as the eventTypeFrame
									},
									Children = { title, eventTypeFrame }
								};
								Grid.SetColumn(title, 0);
								Grid.SetColumn(eventTypeFrame, 1);

								var eventFrame = new Frame
								{
									BackgroundColor = eventItem.EventVisibleColor,
									Content = grid,
									Padding = 2, // Reduce padding inside the frame
									HasShadow = false, // Optional: remove shadow for a flatter look
								};

								var tapGestureRecognizer = new TapGestureRecognizer
								{
									Command = EventSelectedCommand,
									CommandParameter = eventItem
								};

								eventFrame.GestureRecognizers.Add(tapGestureRecognizer);
								eventItemsStackLayout.Children.Add(eventFrame);
							}

							frame.Content = eventItemsStackLayout;
						}

					}

					Grid.SetRow(frame, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
					Grid.SetColumn(frame, dayOfWeek + 1);  // Adjust column index by 1 to make space for the hour indicator
					Children.Add(frame);
				}

			}
		}
	}
}
//else
//{
//	for (int i = 0; i < Math.Min(dayEvents.Count, _displayEventsLimit); i++)
//	{
//		var label = new Label
//		{
//			FontSize = _eventNamesFontSize,
//			FontAttributes = FontAttributes.Bold,
//			Text = dayEvents[i].Title,
//			BackgroundColor = dayEvents[i].EventVisibleColor
//		};
//		if (i < _displayEventsLimit - 1 || dayEvents.Count == 1)
//		{
//			var tapGestureRecognizer = new TapGestureRecognizer();
//			tapGestureRecognizer.Command = EventSelectedCommand;
//			tapGestureRecognizer.CommandParameter = dayEvents[i];
//			label.GestureRecognizers.Add(tapGestureRecognizer);
//			stackLayout.Children.Add(label);
//		}
//	}
//}
