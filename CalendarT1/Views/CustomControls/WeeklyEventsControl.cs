namespace CalendarT1.Views.CustomControls
{
	using CalendarT1.Models;
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using System.Windows.Input;
	using MauiGrid = Microsoft.Maui.Controls.Grid;

	public class WeeklyEventsControl : MauiGrid
	{
		public static readonly BindableProperty WeeklyEventsProperty = BindableProperty.Create(
			nameof(WeeklyEvents),
			typeof(ObservableCollection<HourlyEvents>),
			typeof(WeeklyEventsControl),
			propertyChanged: OnWeeklyEventsChanged);
		public ObservableCollection<HourlyEvents> WeeklyEvents
		{
			get { return (ObservableCollection<HourlyEvents>)GetValue(WeeklyEventsProperty); }
			set { SetValue(WeeklyEventsProperty, value); }
		}
		private static void OnWeeklyEventsChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var control = (WeeklyEventsControl)bindable;
			control.GenerateGrid();
		}

		public static readonly BindableProperty EventSelectedCommandProperty =
			BindableProperty.Create(
			nameof(EventSelectedCommand),
			typeof(ICommand),
			typeof(WeeklyEventsControl));

		public ICommand EventSelectedCommand
		{
			get => (ICommand)GetValue(EventSelectedCommandProperty);
			set => SetValue(EventSelectedCommandProperty, value);
		}

		private void GenerateGrid()
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

			// Create columns for each day + 1 extra column for the hour indicator
			for (int i = 0; i < 7 + 1; i++)
			{
				ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
			}
			// Add cells for each event
			for (int hour = 0; hour < 24; hour++)
			{
				// Add hour indicator
				var hourLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{hour}:00" };
				Grid.SetRow(hourLabel, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
				Grid.SetColumn(hourLabel, 0);  // Place the hour indicator in the first column
				Children.Add(hourLabel);

				for (int day = 0; day < 7; day++)
				{
					// Create a frame for each cell
					var frame = new Frame { BorderColor = Color.FromRgba(255, 255, 255, 255), Padding = 5 };

					// Check if there is an event for this hour and day
					var hourlyEventsList = WeeklyEvents.Where(e => e.Hour == hour && e.Day == (DayOfWeek)day).ToList();

					foreach (var hourlyEvent in hourlyEventsList)
					{
						if (hourlyEvent != null && hourlyEvent.Events.Count > 0)
						{
							var eventColor = hourlyEvent.Events[0].PriorityLevel.PriorityColor;
							frame.BackgroundColor = eventColor;

							// Create a StackLayout for the events
							var stackLayout = new StackLayout();

							int displayLimit = 2;  // Set a limit to how many items will be displayed
							for (int i = 0; i < Math.Min(hourlyEvent.Events.Count, displayLimit); i++)
							{
								var someEvent = hourlyEvent.Events[i];
								var label = new Label
								{
									FontSize = 10,
									FontAttributes = FontAttributes.Bold,
									Text = someEvent.Title,
									BackgroundColor = someEvent.PriorityLevel.PriorityColor
								};

								var tapGestureRecognizer = new TapGestureRecognizer();
								tapGestureRecognizer.Command = EventSelectedCommand;
								tapGestureRecognizer.CommandParameter = someEvent;
								label.GestureRecognizers.Add(tapGestureRecognizer);

								stackLayout.Children.Add(label);
							}

							// If there are more items than the limit, add a 'See more' label
							if (hourlyEvent.Events.Count > displayLimit)
							{
								var moreLabel = new Label
								{
									FontSize = 10,
									FontAttributes = FontAttributes.Italic,
									Text = $"See {hourlyEvent.Events.Count - displayLimit} more...",
									TextColor = Color.FromRgba(255, 255, 255, 255)
								};
								stackLayout.Children.Add(moreLabel);
							}

							frame.Content = stackLayout;
						}
						Grid.SetRow(frame, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
						Grid.SetColumn(frame, day + 1);  // Adjust column index by 1 to make space for the hour indicator
						Children.Add(frame);
					}
				}

				// Add day of the week labels in the second row
				int dayOfWeek = (int)DateTime.Now.DayOfWeek;
				for (int day = 0; day < 7; day++)
				{
					//	var startOfWeek = _currentSelectedDate.AddDays(-(int)_currentSelectedDate.DayOfWeek);
					var dayOfWeekLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{((DayOfWeek)day).ToString().Substring(0, 3)} {DateTime.Now.AddDays(day-dayOfWeek).ToString("dd-MM")}" };
					Grid.SetRow(dayOfWeekLabel, 1);  // Place the day of the week label in the second row
					Grid.SetColumn(dayOfWeekLabel, day + 1);  // Adjust column index by 1 to make space for the hour indicator
					Children.Add(dayOfWeekLabel);
				}
			}
		}
	}
}