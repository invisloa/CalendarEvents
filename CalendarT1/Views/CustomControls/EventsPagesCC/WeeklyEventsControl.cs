namespace CalendarT1.Views.CustomControls
{
	using CalendarT1.Models.EventModels;
	using Microsoft.Maui.Controls;
	using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using MauiGrid = Microsoft.Maui.Controls.Grid;

    public class WeeklyEventsControl : BaseEventPageCC
	{
		private readonly int _minimumDayOfWeekWidthRequest = 45;
		private readonly int _minimumDayOfWeekHeightRequest = 55;
		private readonly double _firstColumnForHoursWidth = 35;

		Color _buttonsBackgroundColor = (Color)Application.Current.Resources["MainBackgroundColor"];
		public void GenerateGrid()
		{
			// Clear the existing rows and columns
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();
			int dayOfWeekNumber = (int)CurrentSelectedDate.DayOfWeek;

			for (int day = 0; day < 7; day++)
			{
				//	var startOfWeek = _currentSelectedDate.AddDays(-(int)_currentSelectedDate.DayOfWeek);
				var dayOfWeekLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold,
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
					var frame = new Frame { BorderColor = Color.FromRgba(255, 255, 255, 255), Padding = 5, BackgroundColor = _buttonsBackgroundColor, MinimumWidthRequest = _minimumDayOfWeekWidthRequest, MinimumHeightRequest = _minimumDayOfWeekHeightRequest };

					var dayEvents = EventsToShowList
						.Where(e => e.StartDateTime.Date == CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek).Date
									&& e.StartDateTime.Hour <= hour && e.EndDateTime.Hour >= hour)
						.ToList(); // to Check
					if (dayEvents != null && dayEvents.Count > 0)
					{
						frame.BackgroundColor = dayEvents[0].EventVisibleColor;
						// Create a StackLayout for the events
						var stackLayout = new StackLayout();

						for (int i = 0; i < Math.Min(dayEvents.Count, _displayEventsLimit); i++)
						{
							var label = new Label
							{
								FontSize = 10,
								FontAttributes = FontAttributes.Bold,
								Text = dayEvents[i].Title,
								BackgroundColor = dayEvents[i].EventVisibleColor
							};
							if (i < _displayEventsLimit - 1 || dayEvents.Count == 1)
							{
								var tapGestureRecognizer = new TapGestureRecognizer();
								tapGestureRecognizer.Command = EventSelectedCommand;
								tapGestureRecognizer.CommandParameter = dayEvents[i];
								label.GestureRecognizers.Add(tapGestureRecognizer);
								stackLayout.Children.Add(label);
							}
						}
						// If there are more items than the limit, add a 'See more' label
						if (dayEvents.Count > _displayEventsLimit)
						{
							var moreLabel = new Label
							{
								FontSize = 15,
								FontAttributes = FontAttributes.Italic,
								Text = $"... {dayEvents.Count} ...",
								TextColor = Color.FromRgba(255, 255, 255, 255),
								BackgroundColor = Color.FromRgba(0, 0, 0, 100)
							};
							var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer();
							tapGestureRecognizerForMoreEvents.Command = GoToSelectedDateCommand;
							tapGestureRecognizerForMoreEvents.CommandParameter = CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek);


							frame.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
							moreLabel.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
							stackLayout.Children.Add(moreLabel);
						}

						frame.Content = stackLayout;
					}

					Grid.SetRow(frame, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
					Grid.SetColumn(frame, dayOfWeek + 1);  // Adjust column index by 1 to make space for the hour indicator
					Children.Add(frame);
				}

			}
		}
	}
}
