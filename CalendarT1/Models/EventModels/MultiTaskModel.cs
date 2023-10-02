using CalendarT1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models.EventModels
{
	public class MultiTask : BaseViewModel
	{
		private string _title;
		private bool _isCompleted;

		public string SubTaskTitle
		{
			get => _title;
			set
			{
				if (_title == value)
				{
					return;
				}
				_title = value;
				OnPropertyChanged();
			}
		}
		public bool IsSubTaskCompleted
		{
			get => _isCompleted;
			set
			{
				if (_isCompleted == value)
				{
					return;
				}
				_isCompleted = value;
				OnPropertyChanged();
			}
		}
		public MultiTask(string title, bool isCompleted = false)
		{
			SubTaskTitle = title;
			IsSubTaskCompleted = isCompleted;
		}

	}
}
