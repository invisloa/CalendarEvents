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
			typeof(ObservableCollection<EventModel>),
			typeof(WeeklyEventsControl),
			defaultBindingMode: BindingMode.TwoWay);
		public ObservableCollection<EventModel> EventsToShowList
		{
			get => (ObservableCollection<EventModel>)GetValue(EventsToShowListProperty);
			set => SetValue(EventsToShowListProperty, value);
		}


		public static readonly BindableProperty AllEventsListProperty =
			BindableProperty.Create(
			nameof(AllEventsList),
			typeof(List<EventModel>),
			typeof(WeeklyEventsControl));

		public List<EventModel> AllEventsList
		{
			get => (List<EventModel>)GetValue(AllEventsListProperty);
			set => SetValue(AllEventsListProperty, value);
		}

		public static readonly BindableProperty EventSelectedCommandProperty =
			BindableProperty.Create(
			nameof(EventSelectedCommand),
			typeof(ICommand),
			typeof(WeeklyEventsControl));

		public RelayCommand EventSelectedCommand
		{
			get => (RelayCommand)GetValue(EventSelectedCommandProperty);
			set => SetValue(EventSelectedCommandProperty, value);
		}
		public static readonly BindableProperty GenerateGridCommandProperty = BindableProperty.Create(
			nameof(GenerateGridCommand),
			typeof(ICommand),
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
				
				var selectedDayOfWeek = (int)CurrentSelectedDate.DayOfWeek;

				/// TO DO : Add events to the grid
			}
		}
	}
}