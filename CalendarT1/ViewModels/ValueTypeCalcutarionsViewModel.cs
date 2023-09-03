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
using CalendarT1.Models.EventTypesModels;
using System.Collections.ObjectModel;
using CalendarT1.ViewModels.HelperClass;
using CalendarT1.Models.EventModels;

namespace CalendarT1.ViewModels
{
	public class ValueTypeCalcutarionsViewModel : PlainBaseAbstractEventViewModel, IFilterDatesCC
	{
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();



		// TODO change the below to factory and interface LATER
		private MeasurementOperationsHelperClass _measurementOperationsHelperClass;

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

		public string CalculationsText { get; set; } = "Calculations:";
		public RelayCommand DoCalculationsCommand { get; set; }

		public ValueTypeCalcutarionsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList().Where(x => x.MainEventType == MainEventTypes.Value).ToList());
			_measurementOperationsHelperClass = new MeasurementOperationsHelperClass(new List<IGeneralEventModel>(AllEventsListOC));
			DoCalculationsCommand = new RelayCommand(OnDoCalculations);
			InitializeCommon();
		}
		private void OnDoCalculations()
		{
			_measurementOperationsHelperClass.DoValueTypesBasicCalculations(FilterDateFrom, FilterDateTo);
			CalculationsText = _measurementOperationsHelperClass.MaxByDay.ToString();
		}
		private void InitializeCommon()
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged;
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged;

			this.SetFilterDatesValues(); // using extension method
		}

	}
}
