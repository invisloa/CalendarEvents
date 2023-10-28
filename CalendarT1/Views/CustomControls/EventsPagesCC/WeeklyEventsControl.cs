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
		private readonly int _minimumDayOfWeekHeightRequest = 30;
		private readonly double _firstColumnForHoursWidth = 35;

		public void GenerateGrid()
		{
			ClearGrid();
			GenerateDayLabels();
			GenerateHourLabels();
			GenerateEventFrames();
		}

		private void ClearGrid()
		{
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();

			for (int i = 0; i < 24 + 2; i++)
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

			ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(_firstColumnForHoursWidth) });
			for (int i = 1; i < 8; i++)
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
		}

		private void GenerateDayLabels()
		{
			int dayOfWeekNumber = (int)CurrentSelectedDate.DayOfWeek;
			for (int day = 0; day < 7; day++)
			{
				var dayOfWeekLabel = new Label { FontSize = _dayNamesFontSize, FontAttributes = FontAttributes.Bold, Text = $"{((DayOfWeek)day).ToString().Substring(0, 3)} {CurrentSelectedDate.AddDays(day - dayOfWeekNumber).ToString("dd")}" };
				Grid.SetRow(dayOfWeekLabel, 1);
				Grid.SetColumn(dayOfWeekLabel, day + 1);
				Children.Add(dayOfWeekLabel);
			}
		}

		private void GenerateHourLabels()
		{
			for (int hour = 0; hour < 24; hour++)
			{
				var hourLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{hour:D2}", WidthRequest = _firstColumnForHoursWidth };
				Grid.SetRow(hourLabel, hour + 2);
				Grid.SetColumn(hourLabel, 0);
				Children.Add(hourLabel);
			}
		}

		private void GenerateEventFrames()
		{
			for (int hour = 0; hour < 24; hour++)
				for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
				{
					var frame = CreateEventFrame(hour, dayOfWeek);
					Grid.SetRow(frame, hour + 2);
					Grid.SetColumn(frame, dayOfWeek + 1);
					Children.Add(frame);
				}
		}

		private Frame CreateEventFrame(int hour, int dayOfWeek)
		{
			var frame = new Frame { BorderColor = _frameBorderColor, Padding = 5, BackgroundColor = _emptyLabelColor, MinimumWidthRequest = _minimumDayOfWeekWidthRequest, MinimumHeightRequest = _minimumDayOfWeekHeightRequest };
			var dayEvents = EventsToShowList.Where(e => e.StartDateTime.Date == CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek).Date && e.StartDateTime.Hour == hour).ToList();
			if (dayEvents.Count > 0)
			{
				var stackLayout = GenerateEventStackLayout(dayEvents, dayOfWeek);
				frame.Content = stackLayout;
			}
			return frame;
		}

		private StackLayout GenerateEventStackLayout(List<IGeneralEventModel> dayEvents, int dayOfWeek)
		{
			var stackLayout = new StackLayout();
			if (dayEvents.Count > _displayEventsLimit)
			{
				var moreLabel = GenerateMoreEventsLabel(dayEvents, dayOfWeek);
				stackLayout.Children.Add(moreLabel);
			}
			else if (dayEvents.Count == 1)
			{
				var eventFrame = GenerateSingleEventFrame(dayEvents[0]);
				stackLayout.Children.Add(eventFrame);
			}
			else
			{
				var eventItemsStackLayout = GenerateMultipleEventFrames(dayEvents);
				stackLayout.Children.Add(eventItemsStackLayout);
			}
			return stackLayout;
		}

		private Label GenerateMoreEventsLabel(List<IGeneralEventModel> dayEvents, int dayOfWeek)
		{
			var moreLabel = new Label { FontSize = 15, FontAttributes = FontAttributes.Italic, Text = $"... {dayEvents.Count} ...", TextColor = _eventTextColor, BackgroundColor = _moreEventsLabelColor };
			var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer { Command = GoToSelectedDateCommand, CommandParameter = CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek) };
			moreLabel.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
			return moreLabel;
		}

		private Frame GenerateSingleEventFrame(IGeneralEventModel eventItem)
		{
			var title = new Label
			{
				FontAttributes = FontAttributes.Bold,
				Text = eventItem.Title,
				LineBreakMode = LineBreakMode.TailTruncation,
			};

			var description = new Label { Text = eventItem.Description };
			var eventTypeLabel = new Label { Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName, TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor, Style = Styles.GoogleFontStyle, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
			var eventTypeFrame = new Frame { BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor, Padding = 0, Content = eventTypeLabel, HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
			var eventStackLayout = new StackLayout { Children = { title, description } };
			var grid = new Grid { ColumnDefinitions = { new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }, new ColumnDefinition { Width = new GridLength(50) } }, Children = { eventStackLayout, eventTypeFrame } };
			Grid.SetColumn(eventTypeFrame, 1);
			var eventFrame = new Frame { BackgroundColor = eventItem.EventVisibleColor, Content = grid };
			var tapGestureRecognizer = new TapGestureRecognizer { Command = EventSelectedCommand, CommandParameter = eventItem };
			eventFrame.GestureRecognizers.Add(tapGestureRecognizer);
			return eventFrame;
		}


		private StackLayout GenerateMultipleEventFrames(List<IGeneralEventModel> dayEvents)
		{
			var eventItemsStackLayout = new StackLayout
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 2,
			};

			for (int i = 0; i < Math.Min(dayEvents.Count, _displayEventsLimit); i++)
			{
				var eventItem = dayEvents[i];

				var title = new Label
				{
					FontAttributes = FontAttributes.Bold,
					Text = eventItem.Title,
					LineBreakMode = LineBreakMode.TailTruncation,
				};

				var eventTypeLabel = new Label
				{
					Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
					TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
					Style = Styles.GoogleFontStyle,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
				};

				var eventTypeFrame = new Frame
				{
					BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor,
					Padding = 0,
					Content = eventTypeLabel,
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center,
					WidthRequest = 30
				};

				var grid = new Grid
				{
					ColumnDefinitions =
			{
				new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				new ColumnDefinition { Width = new GridLength(30) }
			},
					Children = { title, eventTypeFrame }
				};
				Grid.SetColumn(title, 0);
				Grid.SetColumn(eventTypeFrame, 1);

				var eventFrame = new Frame
				{
					BackgroundColor = eventItem.EventVisibleColor,
					Content = grid,
					Padding = 2,
					HasShadow = false,
				};

				var tapGestureRecognizer = new TapGestureRecognizer
				{
					Command = EventSelectedCommand,
					CommandParameter = eventItem
				};

				eventFrame.GestureRecognizers.Add(tapGestureRecognizer);
				eventItemsStackLayout.Children.Add(eventFrame);
			}

			return eventItemsStackLayout;
		}

	}
}