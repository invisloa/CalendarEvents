using CalendarT1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls
{
	public class WeeklyEventsControl : Grid
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
					var hourlyEvents = WeeklyEvents.FirstOrDefault(e => e.Hour == hour && e.Day == (DayOfWeek)day);
					if (hourlyEvents != null && hourlyEvents.Events.Count > 0)
					{
						var eventColor = hourlyEvents.Events[0].PriorityLevel.PriorityColor;
						frame.BackgroundColor = eventColor;

						// Create a collection view to display events
						var collectionView = new CollectionView
						{
							ItemsSource = hourlyEvents.Events,
							ItemTemplate = new DataTemplate(() =>
							{
								var stackLayout = new StackLayout { };
								var label = new Label { FontSize = 10, FontAttributes = FontAttributes.Bold };
								label.SetBinding(Label.TextProperty, "Title");
								label.SetBinding(Label.BackgroundColorProperty, "PriorityLevel.PriorityColor");

								stackLayout.Children.Add(label);
								return stackLayout;
							}),
							HeightRequest = 30,  // Set a fixed height
							WidthRequest = 30    // Set a fixed width
						};
						frame.Content = collectionView;

						// Add event count to the frame if there are multiple events
						if (hourlyEvents.Events.Count > 1)
						{
							var highestPriorityColor = hourlyEvents.Events.OrderByDescending(e => e.PriorityLevel.PriorityLevel).First().PriorityLevel.PriorityColor;
							frame.Content = new StackLayout
							{
								Children = { collectionView, new Label { Text = $"{hourlyEvents.Events.Count} events", FontSize = 10, FontAttributes = FontAttributes.Italic, TextColor = Color.FromRgba(255,255,255,255) } }
							};
							frame.BackgroundColor = highestPriorityColor;

						}
					}
					Grid.SetRow(frame, hour + 2);  // Adjust row index by 2 to make space for the day of the week and date label
					Grid.SetColumn(frame, day + 1);  // Adjust column index by 1 to make space for the hour indicator
					Children.Add(frame);
				}
			}
			// Add day of the week labels in the second row
			for (int day = 0; day < 7; day++)
			{
				//		var dayOfWeekLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{((DayOfWeek)day).ToString("ddd")} {DateTime.Now.AddDays(day).ToString("dd-MM")}" };
				var dayOfWeekLabel = new Label { FontSize = 12, FontAttributes = FontAttributes.Bold, Text = $"{((DayOfWeek)day).ToString().Substring(0, 3)} {DateTime.Now.AddDays(day).ToString("dd-MM")}" };
				Grid.SetRow(dayOfWeekLabel, 1);  // Place the day of the week label in the second row
				Grid.SetColumn(dayOfWeekLabel, day + 1);  // Adjust column index by 1 to make space for the hour indicator
				Children.Add(dayOfWeekLabel);
			}
		}

	}
}