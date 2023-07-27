namespace CalendarT1.Views.CustomControls
{
    using System;
    using System.Collections.ObjectModel;
    using System.Linq;
    using MauiGrid = Microsoft.Maui.Controls.Grid;

    public class WeeklyEventsControl : MauiGrid
	{
		public static readonly BindableProperty CurrentSelectedDateProperty =
			BindableProperty.Create(
				nameof(CurrentSelectedDate),
				typeof(DateTime),
				typeof(WeeklyEventsControl),
				defaultBindingMode: BindingMode.TwoWay);
		public DateTime CurrentSelectedDate
		{
			get => (DateTime)GetValue(CurrentSelectedDateProperty);
			set => SetValue(CurrentSelectedDateProperty, value);
		}

		public static readonly BindableProperty EventsToShowListProperty =
			BindableProperty.Create(
			nameof(EventsToShowList),
			typeof(ObservableCollection<AbstractEventModel>),
			typeof(WeeklyEventsControl),
			defaultBindingMode: BindingMode.TwoWay);
		public ObservableCollection<AbstractEventModel> EventsToShowList
		{
			get => (ObservableCollection<AbstractEventModel>)GetValue(EventsToShowListProperty);
			set => SetValue(EventsToShowListProperty, value);
		}


		public static readonly BindableProperty AllEventsListProperty =
			BindableProperty.Create(
			nameof(AllEventsList),
			typeof(List<AbstractEventModel>),
			typeof(WeeklyEventsControl));

		public List<AbstractEventModel> AllEventsList
		{
			get => (List<AbstractEventModel>)GetValue(AllEventsListProperty);
			set => SetValue(AllEventsListProperty, value);
		}

		public static readonly BindableProperty EventSelectedCommandProperty =
			BindableProperty.Create(
			nameof(EventSelectedCommand),
			typeof(RelayCommand<AbstractEventModel>),
			typeof(WeeklyEventsControl));

		public RelayCommand<AbstractEventModel> EventSelectedCommand
		{
			get => (RelayCommand<AbstractEventModel>)GetValue(EventSelectedCommandProperty);
			set => SetValue(EventSelectedCommandProperty, value);
		}
		public static readonly BindableProperty GenerateGridCommandProperty = BindableProperty.Create(
			nameof(GenerateGridCommand),
			typeof(RelayCommand),
			typeof(WeeklyEventsControl),
			defaultValue: null,
			defaultBindingMode: BindingMode.OneWay,
			propertyChanged: null);
		public RelayCommand GenerateGridCommand
		{
			get => (RelayCommand)GetValue(GenerateGridCommandProperty);
			set => SetValue(GenerateGridCommandProperty, value);
		}

		// add to constructor or initialization method
		public WeeklyEventsControl()
		{
			GenerateGridCommand = new RelayCommand(ExecuteGenerateGridCommand);
		}

		private void ExecuteGenerateGridCommand()
		{
			GenerateGrid();
		}
		public void GenerateGrid()
		{
			// Clear the existing rows and columns
			RowDefinitions.Clear();
			ColumnDefinitions.Clear();
			Children.Clear();
			// Create rows for each hour + 2 extra rows for the day of the week and date label
			for (int i = 0; i < 24 + 2; i++)
			{
				RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
			}


			// Create columns for each day + first extra column for the hour indicator
			for (int i = 0; i < 8; i++)
			{
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
			int dayOfWeekNumber = (int)DateTime.Now.DayOfWeek;

			// Add cells for each event
			for (int hour = 0; hour < 24; hour++)
			{
				// Add hour indicator
				var hourLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{hour}:00" };
				Grid.SetRow(hourLabel, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
				Grid.SetColumn(hourLabel, 0);  // Place the hour indicator in the first column
				Children.Add(hourLabel);
				for (int dayOfWeek = 0; dayOfWeek < 7; dayOfWeek++)
				{
					// Create a frame for each cell
					var frame = new Frame { BorderColor = Color.FromRgba(255, 255, 255, 255), Padding = 5, MinimumWidthRequest = 100, MinimumHeightRequest = 50 };

					var dayEvents = EventsToShowList
						.Where(e => e.StartDateTime.Date == CurrentSelectedDate.AddDays(dayOfWeek - (int)CurrentSelectedDate.DayOfWeek).Date
									&& e.StartDateTime.Hour <= hour && e.EndDateTime.Hour >= hour)
						.ToList(); // to Check
					if (dayEvents != null && dayEvents.Count > 0)
					{
						var eventColor = dayEvents[0].EventVisibleColor;
						frame.BackgroundColor = eventColor;
						// Create a StackLayout for the events
						var stackLayout = new StackLayout();

						int displayLimit = 2;  // Set a limit to how many items will be displayed
						for (int i = 0; i < Math.Min(dayEvents.Count, displayLimit); i++)
						{
							var label = new Label
							{
								FontSize = 10,
								FontAttributes = FontAttributes.Bold,
								Text = dayEvents[i].Title,
								BackgroundColor = dayEvents[i].EventVisibleColor
							};
							var tapGestureRecognizer = new TapGestureRecognizer();
							tapGestureRecognizer.Command = EventSelectedCommand;
							tapGestureRecognizer.CommandParameter = dayEvents[i];
							label.GestureRecognizers.Add(tapGestureRecognizer);
							stackLayout.Children.Add(label);
						}
						// If there are more items than the limit, add a 'See more' label
						if (dayEvents.Count > displayLimit)
						{
							var moreLabel = new Label
							{
								FontSize = 10,
								FontAttributes = FontAttributes.Italic,
								Text = $"See {dayEvents.Count - displayLimit} more...",
								TextColor = Color.FromRgba(255, 255, 255, 255)
							};
							stackLayout.Children.Add(moreLabel);
						}

						frame.Content = stackLayout;
					}

					Grid.SetRow(frame, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
					Grid.SetColumn(frame, dayOfWeek + 1);  // Adjust column index by 1 to make space for the hour indicator
					Children.Add(frame);
				}
				for (int day = 0; day < 7; day++)
				{
					//	var startOfWeek = _currentSelectedDate.AddDays(-(int)_currentSelectedDate.DayOfWeek);
					var dayOfWeekLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{((DayOfWeek)day).ToString().Substring(0, 3)} {DateTime.Now.AddDays(day - dayOfWeekNumber).ToString("dd-MM")}" };
					Grid.SetRow(dayOfWeekLabel, 1);  // Place the day of the week label in the second row
					Grid.SetColumn(dayOfWeekLabel, day + 1);  // Adjust column index by 1 to make space for the hour indicator
					Children.Add(dayOfWeekLabel);
				}

			}
		}
	}
}
