using CalendarT1.Models;
using CalendarT1.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{
	public class ScheduleListViewModel : BaseViewModel
	{
		public ObservableCollection<EventModel> EventsList { get; set; } = new ObservableCollection<EventModel>();
		private List<EventModel> _allEventsList = new List<EventModel>();
		public ObservableCollection<EventPriority> EventPriorities { get; set; } = new ObservableCollection<EventPriority>()
		{
			new EventPriority(EnumPriorityLevels.Lowest),
			new EventPriority(EnumPriorityLevels.Low),
			new EventPriority(EnumPriorityLevels.Medium),
			new EventPriority(EnumPriorityLevels.High),
			new EventPriority(EnumPriorityLevels.Highest),
		};

		// region for Properties
		#region Properties
		private DateTime _currentDate = DateTime.Now;
		public DateTime CurrentDate
		{
			get => _currentDate;
		}

		private DateTime _currentSelectedDate = DateTime.Now;
		public DateTime CurrentSelectedDate
		{
			get => _currentSelectedDate;
			set
			{
				if (_currentSelectedDate != value)
				{
					_currentSelectedDate = value;
					OnPropertyChanged();
					DatePickerDateSelectedCommand.Execute(_currentSelectedDate);
				}
			}
		}
		#endregion

		public ICommand DatePickerDateSelectedCommand { get; set; }

		public ScheduleListViewModel()
		{
			DatePickerDateSelectedCommand = new Command<DateTime>(DatePickerDateSelected);
			AddAllScheduleList();
		}
		private void AddAllScheduleList()
		{
			var scheduleList = new List<EventModel>();
			for (int i = 0; i < 5; i++)
			{
				scheduleList.Add(new EventModel
				{
					StartDateTime = DateTime.Now.AddDays(i),
					EndDateTime = DateTime.Now.AddDays(i).AddHours(1),
					Title = $"Test {i + 1}",
					Description = $"Test {i + 1} Description",
					PriorityLevel = new EventPriority(EnumPriorityLevels.Lowest+i)
				}) ;
			}
			_allEventsList.AddRange(scheduleList);

			BindDataToScheduleList();
		}

		private void BindDataToScheduleList()
		{
			var selectedPriorities = EventPriorities.Where(x => x.IsSelected).Select(x => x.PriorityLevel).ToList();

			var filteredScheduleList = _allEventsList
				.Where(x => (x.StartDateTime.Date == _currentSelectedDate.Date ||
							 x.EndDateTime.Date == _currentSelectedDate.Date) &&
							 selectedPriorities.Contains(x.PriorityLevel.PriorityLevel))
				.ToList();

			EventsList.Clear();
			foreach (var item in filteredScheduleList)
			{
				EventsList.Add(item);
			}
		}

		private void DatePickerDateSelected(DateTime newDate)
		{
			_currentSelectedDate = newDate;
			BindDataToScheduleList();
		}
	}

}
