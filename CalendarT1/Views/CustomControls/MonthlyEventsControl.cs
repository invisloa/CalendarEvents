namespace CalendarT1.Views.CustomControls
{
	using Microsoft.Maui.Graphics;
	using CalendarT1.Models.EventModels;
	using Microsoft.Maui.Layouts;
	using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using MauiGrid = Microsoft.Maui.Controls.Grid;

    public class MonthlyEventsControl : MauiGrid
	{
		private int _displayEventsLimit = 2;
		private int _watermarkDateTextFontSize = 20;
		private int _eventNamesFontSize = 15;
		private int _dayNamesFontSize = 15;

		
		private Color _watermarkDateColor = Color.FromArgb("#FFFFFF");	//gray color


		public static readonly BindableProperty CurrentSelectedDateProperty =
			BindableProperty.Create(
				nameof(CurrentSelectedDate),
				typeof(DateTime),
				typeof(MonthlyEventsControl),
				defaultBindingMode: BindingMode.TwoWay);
		public DateTime CurrentSelectedDate
		{
			get => (DateTime)GetValue(CurrentSelectedDateProperty);
			set => SetValue(CurrentSelectedDateProperty, value);
		}

		public static readonly BindableProperty EventsToShowListProperty =
			BindableProperty.Create(
			nameof(EventsToShowList),
			typeof(ObservableCollection<IGeneralEventModel>),
			typeof(MonthlyEventsControl),
			defaultBindingMode: BindingMode.TwoWay);
		public ObservableCollection<IGeneralEventModel> EventsToShowList
		{
			get => (ObservableCollection<IGeneralEventModel>)GetValue(EventsToShowListProperty);
			set => SetValue(EventsToShowListProperty, value);
		}

		public static readonly BindableProperty AllEventsListProperty =
			BindableProperty.Create(
			nameof(AllEventsList),
			typeof(ObservableCollection<IGeneralEventModel>),
			typeof(MonthlyEventsControl));

		public ObservableCollection<IGeneralEventModel> AllEventsList
		{
			get => (ObservableCollection<IGeneralEventModel>)GetValue(AllEventsListProperty);
			set => SetValue(AllEventsListProperty, value);
		}

		public static readonly BindableProperty EventSelectedCommandProperty =
			BindableProperty.Create(
			nameof(EventSelectedCommand),
			typeof(RelayCommand<IGeneralEventModel>),
			typeof(MonthlyEventsControl));

		public RelayCommand<IGeneralEventModel> EventSelectedCommand
		{
			get => (RelayCommand<IGeneralEventModel>)GetValue(EventSelectedCommandProperty);
			set => SetValue(EventSelectedCommandProperty, value);
		}

		public static readonly BindableProperty GenerateGridCommandProperty = BindableProperty.Create(
			nameof(GenerateGridCommand),
			typeof(RelayCommand),
			typeof(MonthlyEventsControl),
			defaultValue: null,
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: null);

		public RelayCommand GenerateGridCommand
		{
			get => (RelayCommand)GetValue(GenerateGridCommandProperty);
			set => SetValue(GenerateGridCommandProperty, value);
		}

		public static readonly BindableProperty GoToSelectedDateCommandProperty =
			BindableProperty.Create(
			nameof(GoToSelectedDateCommand),
			typeof(RelayCommand<DateTime>),
			typeof(MonthlyEventsControl));  // TO CHECK CHANGED FROM WEEKLY TO MONTHLY

		public RelayCommand<DateTime> GoToSelectedDateCommand
		{
			get => (RelayCommand<DateTime>)GetValue(GoToSelectedDateCommandProperty);
			set => SetValue(GoToSelectedDateCommandProperty, value);
		}

		// add to constructor or initialization method
		public MonthlyEventsControl()
		{
			GenerateGridCommand = new RelayCommand(GenerateGrid);
		}
		public void GenerateGrid()
		{
			// Clear the existing rows and columns
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

			// Add day labels
			string[] dayLabels = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
			for (int day = 0; day < 7; day++)
			{
				var dayLabel = new Label { FontSize = _dayNamesFontSize, FontAttributes = FontAttributes.Bold, Text = dayLabels[day] };
				Grid.SetRow(dayLabel, 0);
				Grid.SetColumn(dayLabel, day);
				Children.Add(dayLabel);
			}

			// Add cells for each day
			for (int day = 1; day <= daysInMonth; day++)
			{
				var stackLayoutForFrame = new StackLayout();

				int gridRow = (firstDayOfWeek + day - 1) / 7 + 1;
				int gridColumn = (firstDayOfWeek + day - 1) % 7;

				var frame = new Frame { BorderColor = Color.FromRgba(255, 255, 255, 255), Padding = 5, MinimumWidthRequest = 100, MinimumHeightRequest = 50 };
				frame.AutomationId = new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, day).ToString("yyyy-MM-dd");


				var dayLabel = new Label    // add Day number watermark
				{
					Text = day.ToString(),
					FontSize = _watermarkDateTextFontSize,
					FontAttributes = FontAttributes.Bold,
					TextColor = _watermarkDateColor,
					VerticalOptions = LayoutOptions.Start,
					HorizontalOptions = LayoutOptions.End
				};
				stackLayoutForFrame.Children.Add(dayLabel);

				var dayEvents = EventsToShowList
					.Where(e => e.StartDateTime.Date == new DateTime(CurrentSelectedDate.Year, CurrentSelectedDate.Month, day))
					.ToList();

				// if there are events on this day, display them
				if (dayEvents != null && dayEvents.Count > 0)
				{
					var eventColor = dayEvents[0].EventVisibleColor;
					frame.BackgroundColor = eventColor;

					// Set a limit to how many items will be displayed
					for (int i = 0; i < Math.Min(dayEvents.Count, _displayEventsLimit); i++)
					{
						var label = new Label
						{
							FontSize = _eventNamesFontSize,
							FontAttributes = FontAttributes.Bold,
							Text = dayEvents[i].Title,
							BackgroundColor = dayEvents[i].EventVisibleColor
						};

						stackLayoutForFrame.Children.Add(label);
					}
					// If there are more items than the limit, add a 'See more' label
					if (dayEvents.Count > _displayEventsLimit)
					{
						var moreLabel = new Label
						{
							FontSize = _eventNamesFontSize,
							FontAttributes = FontAttributes.Italic,
							Text = $"... {dayEvents.Count - _displayEventsLimit} MORE ...",
							HorizontalTextAlignment = TextAlignment.Center,
							TextColor = Color.FromRgba(255, 255, 255, 255),
							BackgroundColor = Color.FromRgba(0, 0, 0, 100)
						};

						stackLayoutForFrame.Children.Add(moreLabel);
					}

					// one tap if there are any events
					var oneTapGestureRecognizerForMoreEvents = new TapGestureRecognizer();
					oneTapGestureRecognizerForMoreEvents.Command = GoToSelectedDateCommand;
					oneTapGestureRecognizerForMoreEvents.CommandParameter = DateTime.Parse(frame.AutomationId);
					frame.GestureRecognizers.Add(oneTapGestureRecognizerForMoreEvents);
				}
				else
				{
					// double tap if there are no event to go to the selected date
					var tapGestureRecognizerForMoreEvents = new TapGestureRecognizer();
					tapGestureRecognizerForMoreEvents.NumberOfTapsRequired = 2;
					tapGestureRecognizerForMoreEvents.Command = GoToSelectedDateCommand;
					tapGestureRecognizerForMoreEvents.CommandParameter = DateTime.Parse(frame.AutomationId);
					frame.GestureRecognizers.Add(tapGestureRecognizerForMoreEvents);
				}

				frame.Content = stackLayoutForFrame;
				Grid.SetRow(frame, gridRow);
				Grid.SetColumn(frame, gridColumn);
				Children.Add(frame);
			}
		}
	}
}
