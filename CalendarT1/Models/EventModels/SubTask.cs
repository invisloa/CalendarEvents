using CalendarT1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Models.EventModels
{
	public class SubTask : BaseViewModel
	{
		private string _title;
		private string _description;
		private bool _isCompleted;

		public string Title
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
		public string Description
		{
			get => _description;
			set
			{
				if (_description == value)
				{
					return;
				}
				_description = value;
				OnPropertyChanged();
			}
		}

		public bool IsCompleted
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

	}
}
