namespace CalendarT1.Views.CustomControls
{
	using Microsoft.Maui.Graphics;
	using CalendarT1.Models.EventModels;
	using Microsoft.Maui.Layouts;
	using System;
	using System.Collections.ObjectModel;
	using System.Linq;
	using MauiGrid = Microsoft.Maui.Controls.Grid;

public class BaseEventPageCC : MauiGrid
{

	public static readonly BindableProperty CurrentSelectedDateProperty =
		 BindableProperty.Create(
			 nameof(CurrentSelectedDate),
			 typeof(DateTime),
			 typeof(BaseEventPageCC),
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
		typeof(BaseEventPageCC),
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
		typeof(BaseEventPageCC));

	public ObservableCollection<IGeneralEventModel> AllEventsList
	{
		get => (ObservableCollection<IGeneralEventModel>)GetValue(AllEventsListProperty);
		set => SetValue(AllEventsListProperty, value);
	}

	public static readonly BindableProperty EventSelectedCommandProperty =
		BindableProperty.Create(
		nameof(EventSelectedCommand),
		typeof(RelayCommand<IGeneralEventModel>),
		typeof(BaseEventPageCC));

	public RelayCommand<IGeneralEventModel> EventSelectedCommand
	{
		get => (RelayCommand<IGeneralEventModel>)GetValue(EventSelectedCommandProperty);
		set => SetValue(EventSelectedCommandProperty, value);
	}

	public static readonly BindableProperty GoToSelectedDateCommandProperty =
		BindableProperty.Create(
		nameof(GoToSelectedDateCommand),
		typeof(RelayCommand<DateTime>),
		typeof(BaseEventPageCC));

	public RelayCommand<DateTime> GoToSelectedDateCommand
	{
		get => (RelayCommand<DateTime>)GetValue(GoToSelectedDateCommandProperty);
		set => SetValue(GoToSelectedDateCommandProperty, value);
	}
}