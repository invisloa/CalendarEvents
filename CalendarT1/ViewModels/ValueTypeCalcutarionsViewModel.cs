using CalendarT1.ViewModels.EventsViewModels;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Mvvm.Input;

namespace CalendarT1.ViewModels
{
	public class ValueTypeCalcutarionsViewModel : PlainBaseAbstractEventViewModel, IFilterDatesCC
	{
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();



		public string TextFilterDateFrom { get; set; } = "FILTER FROM:";
		public string TextFilterDateTo { get; set; } = "FILTER UP TO:";
		public DateTime FilterDateFrom
		{
			get => _filterDatesCCHelper.FilterDateFrom;
			set
			{
				if (_filterDatesCCHelper.FilterDateFrom == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateFrom = value;
				OnPropertyChanged();
				BindDataToScheduleList();
			}
		}
		public DateTime FilterDateTo
		{
			get => _filterDatesCCHelper.FilterDateTo;
			set
			{
				if (_filterDatesCCHelper.FilterDateTo == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateTo = value;
				OnPropertyChanged();
				BindDataToScheduleList();
			}
		}

		public override void BindDataToScheduleList()
		{
			ApplyEventsDatesFilter(FilterDateFrom, FilterDateTo);
		}

		private void OnFilterDateFromChanged()
		{
			FilterDateFrom = _filterDatesCCHelper.FilterDateFrom;
		}

		private void OnFilterDateToChanged()
		{
			FilterDateTo = _filterDatesCCHelper.FilterDateTo;
		}
		#endregion

		public ValueTypeCalcutarionsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{

		}
		private void InitializeCommon(IEventRepository eventRepository)
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged;
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged;

			SetFilterDatesValues();
		}
	}
}
