using CalendarT1.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels
{
	public class ScheduleListViewModel
	{
		public ObservableCollection<ScheduleModel> ScheduleList { get; set; } = new ObservableCollection<ScheduleModel>();
		private List<ScheduleModel> _allScheduleList = new List<ScheduleModel>();


		public ScheduleListViewModel() 
		{
			AddAllScheduleList();
			
		}

		private void AddAllScheduleList()
		{
			var scheduleList = new ObservableCollection<ScheduleModel>();
			scheduleList.Add(new ScheduleModel() { })

		}
	}
}
