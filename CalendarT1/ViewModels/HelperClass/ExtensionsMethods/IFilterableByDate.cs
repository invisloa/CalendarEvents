using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.HelperClass.ExtensionsMethods
{
	public interface IFilterableByDate
	{
		ObservableCollection<IGeneralEventModel> AllEventsListOC { get; }
		DateTime FilterDateFrom { get; set; }
		DateTime FilterDateTo { get; set; }
	}

	public static class FilterableByDateExtensions
	{
		public static void SetFilterDatesValues(this IFilterableByDate filterable)
		{
			if (filterable.AllEventsListOC.Count != 0)
			{
				filterable.FilterDateFrom = filterable.AllEventsListOC
					.OrderBy(e => e.StartDateTime)
					.FirstOrDefault()
					.StartDateTime;
			}
			else
			{
				filterable.FilterDateFrom = DateTime.Today;
			}
			filterable.FilterDateTo = DateTime.Today;
		}
	}

}
