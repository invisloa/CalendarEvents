using CalendarT1.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels
{
	public partial class ScheduleListViewModel : ObservableObject
	{
		public ObservableCollection<ScheduleModel> ScheduleList { get; set; } = new ObservableCollection<ScheduleModel>();
		private List<ScheduleModel> _allScheduleList = new List<ScheduleModel>();

		[ObservableProperty]
		private DateTime _currentDate = DateTime.Now;


		public ScheduleListViewModel()
		{
			AddAllScheduleList();

		}

		private void AddAllScheduleList()
		{
			var scheduleList = new List<ScheduleModel>();
			// add to scheduleList some dummy data for testing
			//make background color randomized
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
			var filteredScheduleList = _allScheduleList.Where(x => x.StartDateTime.Date == CurrentDate.Date).ToList();

			ScheduleList.Clear();
			foreach (var item in filteredScheduleList)
			{
				ScheduleList.Add(item);
			}
		}
	}

}
