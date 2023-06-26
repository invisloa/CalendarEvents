using CalendarT1.Models;
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
		public ObservableCollection<ScheduleModel> ScheduleList { get; set; } = new ObservableCollection<ScheduleModel>();
		private List<ScheduleModel> _allScheduleList = new List<ScheduleModel>();

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
			var scheduleList = new List<ScheduleModel>();
			for (int i = 0; i < 5; i++)
			{
				scheduleList.Add(new ScheduleModel
				{
					StartDateTime = DateTime.Now.AddDays(i),
					EndDateTime = DateTime.Now.AddDays(i).AddHours(1),
					Title = $"Test {i + 1}",
					Description = $"Test {i + 1} Description",
					BackgroundColor = Color.FromRgba(255, 0, 0, 0.5)
				});
			}
			_allScheduleList.AddRange(scheduleList);

			BindDataToScheduleList();
		}

		private void BindDataToScheduleList()
		{
			var filteredScheduleList = _allScheduleList.Where(x => x.StartDateTime.Date == _currentSelectedDate.Date).ToList();

			ScheduleList.Clear();
			foreach (var item in filteredScheduleList)
			{
				ScheduleList.Add(item);
			}
		}

		private void DatePickerDateSelected(DateTime newDate)
		{
			_currentSelectedDate = newDate;
			BindDataToScheduleList();
		}
	}

}
