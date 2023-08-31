using CalendarT1.Models.EventModels;
using System.Collections.ObjectModel;

namespace CalendarT1.Views.CustomControls.CCInterfaces
{
    public interface IFilterDatesCC
    {
        DateTime FilterDateFrom { get; set; }
        DateTime FilterDateTo { get; set; }
        string TextFilterDateFrom { get; set; }
        string TextFilterDateTo { get; set; }
		ObservableCollection<IGeneralEventModel> AllEventsListOC { get; }

	}

	// EXTENSION METHODS FOR CUSTOM CONTROL INTERFACE IFilterDatesCC
	public static class FilterableByDateExtensions
	{
		public static void SetFilterDatesValues(this IFilterDatesCC filterable)
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