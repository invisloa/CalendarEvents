using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCHelperClass
{
	using global::CalendarT1.Views.CustomControls.CCInterfaces;
    using System;
	using System.Collections.ObjectModel;

	namespace CalendarT1.Views.CustomControls.CCHelperClass
    {
        public class FilterDatesCCHelperClass : IFilterDatesCC, IFilterDatesCCHelperClass
		{
			private DateTime _filterDateFrom;
			private DateTime _filterDateTo;

			// Event for FilterDateFrom property changed
			public event Action FilterDateFromChanged;

			// Event for FilterDateTo property changed
			public event Action FilterDateToChanged;

			public string TextFilterDateFrom { get; set; } = "FILTER FROM:";

			public string TextFilterDateTo { get; set; } = "FILTER UP TO:";

			public DateTime FilterDateFrom
			{
				get => _filterDateFrom;
				set
				{
					if (_filterDateFrom != value)
					{
						_filterDateFrom = value;
						// Fire the event after value is set
						FilterDateFromChanged?.Invoke();
					}
				}
			}

			public DateTime FilterDateTo
			{
				get => _filterDateTo;
				set
				{
					if (_filterDateTo != value)
					{
						_filterDateTo = value;
						// Fire the event after value is set
						FilterDateToChanged?.Invoke();
					}
				}
			}

			public ObservableCollection<Models.EventModels.IGeneralEventModel> AllEventsListOC { get; set; }
		}
	}

}
