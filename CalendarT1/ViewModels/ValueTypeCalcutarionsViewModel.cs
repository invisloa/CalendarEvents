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
using CalendarT1.Views;

namespace CalendarT1.ViewModels
{
	public class ValueTypeCalcutarionsViewModel : PlainBaseAbstractEventViewModel, IFilterDatesCC
	{
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();
		private bool _canExecuteCalculationsCommands()
		{
			return _measurementOperationsHelperClass.CheckIfEventsAreSameType();
		}
		// TODO change the below to factory and interface LATER
		private IMeasurementOperationsHelperClass _measurementOperationsHelperClass;

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
				_measurementOperationsHelperClass.DateFrom = value;
				OnPropertyChanged();
				BindDataToScheduleList();	// Datepicker does not support commands, so we have to do it here
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
				_measurementOperationsHelperClass.DateTo = value;
				_filterDatesCCHelper.FilterDateTo = value;
				OnPropertyChanged();
				BindDataToScheduleList();   // Datepicker does not support commands, so we have to do it here
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

		// CONTROLS PROPERTIES
		#region Controls properties
		public string MinByWeekCalculationText { get; set; } = "MIN WEEKS VALUE";
		public string MaxByWeekCalculationText { get; set; } = " WEEKS VALUE";
		private bool _basicOperationsVisibility = false;
		public bool BasicOperationsVisibility
		{
			get { return _basicOperationsVisibility; }
			set
			{
				if (_basicOperationsVisibility != value)
				{
					_basicOperationsVisibility = value;
					OnPropertyChanged();
				}
			}
		}
		//ADVANCED CALCULATIONS VISIBILITY PROPERTIES
		#region AdvancedCalculationsVISIBILITYProperties
		private bool _maxByWeekOperationsVisibility = false;
		public bool MaxByWeekOperationsVisibility
		{
			get { return _maxByWeekOperationsVisibility; }
			set
			{
				if (_maxByWeekOperationsVisibility != value)
				{
					_maxByWeekOperationsVisibility = value;
					OnPropertyChanged();
				}
			}
		}
		private bool _minByWeekOperationsVisibility = false;
		public bool MinByWeekOperationsVisibility
		{
			get { return _minByWeekOperationsVisibility; }
			set
			{
				if (_minByWeekOperationsVisibility != value)
				{
					_minByWeekOperationsVisibility = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion

		private string _totalOfMeasurementsTextAbove = "Total of measurements:";
		private string _totalOfMeasurements = "0";
		private string _averageOfMeasurementsTextAbove = "Average of measurements:";
		private string _averageOfMeasurements = "0";
		private string _maxOfMeasurementsTextAbove = "Max of measurements:";
		private string _maxOfMeasurements = "0";
		private string _minOfMeasurementsTextAbove = "Min of measurements:";
		private string _minOfMeasurements = "0";
		private MeasurementCalculationsOutcome _measurementCalulationOutcome;
		public MeasurementCalculationsOutcome MeasurementCalculationOutcome
		{
			get { return _measurementCalulationOutcome; }
			set
			{
				if (_measurementCalulationOutcome != value)
				{
					_measurementCalulationOutcome = value;
					OnPropertyChanged();
				}
			}
		}
		public string TotalOfMeasurementsTextAbove
		{
			get { return _totalOfMeasurementsTextAbove; }
			set
			{
				if (_totalOfMeasurementsTextAbove != value)
				{
					_totalOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string TotalOfMeasurements
		{
			get { return _totalOfMeasurements; }
			set
			{
				if (_totalOfMeasurements != value)
				{
					_totalOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}

		public string AverageOfMeasurementsTextAbove
		{
			get { return _averageOfMeasurementsTextAbove; }
			set
			{
				if (_averageOfMeasurementsTextAbove != value)
				{
					_averageOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string AverageOfMeasurements
		{
			get { return _averageOfMeasurements; }
			set
			{
				if (_averageOfMeasurements != value)
				{
					_averageOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}

		public string MaxOfMeasurementsTextAbove
		{
			get { return _maxOfMeasurementsTextAbove; }
			set
			{
				if (_maxOfMeasurementsTextAbove != value)
				{
					_maxOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string MaxOfMeasurements
		{
			get { return _maxOfMeasurements; }
			set
			{
				if (_maxOfMeasurements != value)
				{
					_maxOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}

		public string MinOfMeasurementsTextAbove
		{
			get { return _minOfMeasurementsTextAbove; }
			set
			{
				if (_minOfMeasurementsTextAbove != value)
				{
					_minOfMeasurementsTextAbove = value;
					OnPropertyChanged();
				}
			}
		}

		public string MinOfMeasurements
		{
			get { return _minOfMeasurements; }
			set
			{
				if (_minOfMeasurements != value)
				{
					_minOfMeasurements = value;
					OnPropertyChanged();
				}
			}
		}
		#endregion
		public RelayCommand DoBasicCalculationsCommand { get; set; }
		public RelayCommand<DateTime> GoToWeeksPageCommand { get; set; }
		public RelayCommand MaxByWeekCalculationsCommand { get; set; }
		public RelayCommand MinByWeekCalculationsCommand { get; set; }
		protected override void OnUserEventTypeSelected(IUserEventTypeModel eventSubType)
		{
			base.OnUserEventTypeSelected(eventSubType);  // Call to the base class method

			CanExecuteChangedCalculationsCommandsNotifier();
		}


		// CONSTRUCTOR
		public ValueTypeCalcutarionsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList().Where(x => x.MainEventType == MainEventTypes.Value).ToList());
			DoBasicCalculationsCommand = new RelayCommand(OnDoBasicCalculationsCommand, _canExecuteCalculationsCommands);
			MaxByWeekCalculationsCommand = new RelayCommand(OnMaxByWeekCalculationsCommand, _canExecuteCalculationsCommands);
			MinByWeekCalculationsCommand = new RelayCommand(OnMinByWeekCalculationsCommand, _canExecuteCalculationsCommands);
			GoToWeeksPageCommand = new RelayCommand<DateTime>(GoToWeeksPage);
			_measurementOperationsHelperClass = Factory.CreateMeasurementOperationsHelperClass(EventsToShowList);
			InitializeCommon();
		}

		private void CanExecuteChangedCalculationsCommandsNotifier()
		{
			DoBasicCalculationsCommand.NotifyCanExecuteChanged();
			MaxByWeekCalculationsCommand.NotifyCanExecuteChanged();
			MinByWeekCalculationsCommand.NotifyCanExecuteChanged();
		}
		private void OnMinByWeekCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			MinByWeekOperationsVisibility = true;
			MeasurementCalculationOutcome = _measurementOperationsHelperClass.MinByWeekCalculation();
		}
		private void OnMaxByWeekCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			MaxByWeekOperationsVisibility = true;
			MeasurementCalculationOutcome = _measurementOperationsHelperClass.MaxByWeekCalculation();
			
		}
		private void GoToWeeksPage(DateTime weeksDate)
		{
			// TODO TO CHANGE THE WAY OF NAVIGATING TO WEEKS PAGE
			Application.Current.MainPage.Navigation.PushAsync(new ViewWeeklyEvents(weeksDate));

		}
		private void OnDoBasicCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			BasicOperationsVisibility = true;
			_measurementOperationsHelperClass.DoBasicCalculations();
			TotalOfMeasurements = _measurementOperationsHelperClass.TotalOfMeasurements.ToString();
			AverageOfMeasurements = _measurementOperationsHelperClass.AverageOfMeasurements.ToString("F2");
			MaxOfMeasurements = _measurementOperationsHelperClass.MaxOfMeasurements.ToString();
			MinOfMeasurements = _measurementOperationsHelperClass.MinOfMeasurements.ToString();
		}
		private void SetAllCalculationsControlsVisibilityOFF()
		{
			BasicOperationsVisibility = false;
			MaxByWeekOperationsVisibility = false;
			MinByWeekOperationsVisibility = false;
		}
		private void InitializeCommon()
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged; // for future controls use like (last 90 days, last 30 days, etc.)
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged; // for future controls use like (last 90 days, last 30 days, etc.)

			this.SetFilterDatesValues(); // using extension from IFilterDatesCC method (oldest event date and today)
		}

	}
}
