namespace CalendarT1.Views.CustomControls
{
	using Microsoft.Maui.Graphics;
	using CalendarT1.Models.EventModels;
	using System;
    using System.Linq;
	using static CalendarT1.App;

	public class MonthlyEventsControl : BaseEventPageCC
	{
		private readonly int _minimumDayWidthRequest = 45;
		private readonly int _minimumDayHeightRequest = 30;
		private readonly int _dateFontSize = 12;
		private readonly Color _watermarkDateColor = (Color)Application.Current.Resources["MainTextColor"];

		public MonthlyEventsControl()
		{
			// Constructor logic (if any)
		}

		public void GenerateGrid()
		{
			ClearGrid();
			GenerateDayLabels();
			GenerateDateFrames();
		}

		private void ClearGrid()
		{
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();

			int daysInMonth = DateTime.DaysInMonth(CurrentSelectedDate.Year, CurrentSelectedDate.Month);
			int firstDayOfWeek = (int)new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1).DayOfWeek;
			int totalDays = firstDayOfWeek + daysInMonth;

			// Create rows for each week + 1 extra row for the day labels
			int weeksInMonth = (int)Math.Ceiling(totalDays / 7.0);
			for (int i = 0; i < weeksInMonth + 1; i++)
			{
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}

			// Create columns for each day of the week
			for (int i = 0; i < 7; i++)
			{
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
		}

		private void GenerateDayLabels()
		{
			string[] dayLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
			for (int day = 0; day < 7; day++)
			{
				var dayLabel = new Label { FontSize = _dateFontSize, FontAttributes = FontAttributes.Bold, Text = dayLabels[day] };
				Grid.SetRow(dayLabel, 0);
				Grid.SetColumn(dayLabel, day);
				Children.Add(dayLabel);
			}
		}

		private void GenerateDateFrames()
		{
			int daysInMonth = DateTime.DaysInMonth(CurrentSelectedDate.Year, CurrentSelectedDate.Month);
			int firstDayOfWeek = (int)new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, 1).DayOfWeek;

			for (int day = 1; day <= daysInMonth; day++)
			{
				var dateFrame = CreateDateFrame(day, firstDayOfWeek);
				int gridRow = (firstDayOfWeek + day - 1) / 7 + 1;
				int gridColumn = (firstDayOfWeek + day - 1) % 7;
				Grid.SetRow(dateFrame, gridRow);
				Grid.SetColumn(dateFrame, gridColumn);
				Children.Add(dateFrame);
			}
		}

		private Frame CreateDateFrame(int dayNumber, int firstDayOfWeek)
		{
			var dateFrame = new Frame
			{
				BorderColor = _frameBorderColor,
				Padding = 5,
				BackgroundColor = _emptyLabelColor,
				MinimumWidthRequest = _minimumDayWidthRequest,
				MinimumHeightRequest = _minimumDayHeightRequest
			};

			var stackLayout = new StackLayout();
			var dateLabel = new Label
			{
				FontSize = _dateFontSize,
				FontAttributes = FontAttributes.Bold,
				Text = dayNumber.ToString(),
				TextColor = _watermarkDateColor,
				VerticalOptions = LayoutOptions.Start,
				HorizontalOptions = LayoutOptions.End
			};
			stackLayout.Children.Add(dateLabel);

			var dayEvents = EventsToShowList.Where(e => e.StartDateTime.Date == new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, dayNumber)).ToList();
			if (dayEvents.Count > 0)
			{
				var eventStackLayout = GenerateEventStackLayout(dayEvents, dayNumber);
				stackLayout.Children.Add(eventStackLayout);
			}

			dateFrame.Content = stackLayout;
			return dateFrame;
		}
		private StackLayout GenerateEventStackLayout(List<IGeneralEventModel> dayEvents, int dayNumber)
		{
			var stackLayout = new StackLayout();
			if (dayEvents.Count > _displayEventsLimit)
			{
				var moreLabel = GenerateMoreEventsLabel(dayEvents.Count, dayNumber);
				stackLayout.Children.Add(moreLabel);
			}













			else if (dayEvents.Count == 1)
			{
				//var eventFrame = GenerateSingleEventFrame(dayEvents[0]);
				var eventFrame = GenerateSingleEventLabel(dayEvents[0]);
				stackLayout.Children.Add(eventFrame);
			}













			else
			{
				var eventItemsStackLayout = GenerateMultipleEventFrames(dayEvents);
				stackLayout.Children.Add(eventItemsStackLayout);
			}
			return stackLayout;
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
















		private Label GenerateSingleEventLabel(IGeneralEventModel eventItem)
		{
			var titleLabel = new Label
			{
				//HeightRequest = 35,
				FontAttributes = FontAttributes.Bold,
				Text = eventItem.Title,
				FontSize = 10,
				LineBreakMode = LineBreakMode.TailTruncation,
				BackgroundColor = eventItem.EventType.BackgroundColor,

			};
			var tapGestureRecognizer = new TapGestureRecognizer
			{
				Command = EventSelectedCommand,
				CommandParameter = eventItem
			};

			titleLabel.GestureRecognizers.Add(tapGestureRecognizer);


			return titleLabel;

		}
			private Frame GenerateSingleEventFrame(IGeneralEventModel eventItem)
		{
			var title = new Label
			{
				//HeightRequest = 35,
				FontAttributes = FontAttributes.Bold,
				Text = eventItem.Title,
				LineBreakMode = LineBreakMode.TailTruncation,
			};

			//var eventTypeLabel = new Label
			//{
			//	Text = eventItem.EventType.MainEventType.SelectedVisualElement.ElementName,
			//	TextColor = eventItem.EventType.MainEventType.SelectedVisualElement.TextColor,
			//	Style = Styles.GoogleFontStyle,
			//	HorizontalOptions = LayoutOptions.Center,
			//	VerticalOptions = LayoutOptions.Center
			//};

			//var eventTypeFrame = new Frame
			//{
			//	BackgroundColor = eventItem.EventType.MainEventType.SelectedVisualElement.BackgroundColor,
			//	Padding = 0,
			//	Content = eventTypeLabel,
			//	HorizontalOptions = LayoutOptions.End,
			//	VerticalOptions = LayoutOptions.Center
			//};

			var eventFrame = new Frame
			{
				BackgroundColor = eventItem.EventVisibleColor,
				Content = title
			};

			var tapGestureRecognizer = new TapGestureRecognizer
			{
				Command = EventSelectedCommand,
				CommandParameter = eventItem
			};

			eventFrame.GestureRecognizers.Add(tapGestureRecognizer);
			return eventFrame;
		}















		/*		private Frame GenerateSingleEventFrame(IGeneralEventModel eventItem)
				{
					var title = new Label
					{
						//HeightRequest = 35,
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

					var eventStackLayout = new StackLayout { Children = { title } };
					var grid = new Grid
					{
						ColumnDefinitions = {
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					//new ColumnDefinition { Width = new GridLength(50) }
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
					return eventFrame;
				}*/




















		private Label GenerateMoreEventsLabel(int dayEventsCount, int dayOfMonth)
		{
			var moreLabel = new Label
			{
				FontSize = 15,
				FontAttributes = FontAttributes.Italic,
				Text = $"... {dayEventsCount} ...",
				TextColor = _eventTextColor,
				BackgroundColor = _moreEventsLabelColor,
				HorizontalOptions = LayoutOptions.Center,
				VerticalOptions = LayoutOptions.Center
			};

			var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer
			{
				Command = GoToSelectedDateCommand,
				CommandParameter = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, dayOfMonth)
			};

			moreLabel.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
			return moreLabel;
		}


	}
}
