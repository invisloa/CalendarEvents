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
		private bool _basicOperationsVisibility = true;
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
		private bool _maxByWeekOperationsVisibility = true;
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
		private bool _minByWeekOperationsVisibility = true;
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
		private string _maxByWeekCalculationText = "Max by week:";
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
		public string MaxByWeekCalculationText
		{
			get { return _maxByWeekCalculationText; }
			set
			{
				if (_maxByWeekCalculationText != value)
				{
					_maxByWeekCalculationText = value;
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
		public RelayCommand GoToWeeksPageCommand { get; set; }
		public RelayCommand MaxByWeekCalculationsCommand { get; set; }
		public RelayCommand MinByWeekCalculationsCommand { get; set; }

		// CONSTRUCTOR
		public ValueTypeCalcutarionsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList().Where(x => x.MainEventType == MainEventTypes.Value).ToList());
			_measurementOperationsHelperClass = Factory.CreateMeasurementOperationsHelperClass(eventRepository);
			DoBasicCalculationsCommand = new RelayCommand(OnDoBasicCalculationsCommand);
			MaxByWeekCalculationsCommand = new RelayCommand(OnMaxByWeekCalculationsCommand);
			MinByWeekCalculationsCommand = new RelayCommand(OnMinByWeekCalculationsCommand);
			GoToWeeksPageCommand = new RelayCommand(GoToWeeksPage);
			InitializeCommon();
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
		private void GoToWeeksPage()
		{
			// TODO TO CHANGE THE WAY OF NAVIGATING TO WEEKS PAGE
			Application.Current.MainPage.Navigation.PushAsync(new ViewWeeklyEvents(MeasurementCalculationOutcome.MeasurementDatesListOutcome[0].Date));

		}
		private void OnDoBasicCalculationsCommand()
		{
			SetAllCalculationsControlsVisibilityOFF();
			BasicOperationsVisibility = true;
			_measurementOperationsHelperClass.DoBasicCalculations(FilterDateFrom, FilterDateTo);
			TotalOfMeasurements = _measurementOperationsHelperClass.TotalOfMeasurements.ToString();
			AverageOfMeasurements = _measurementOperationsHelperClass.AverageOfMeasurements.ToString();
			MaxOfMeasurements = _measurementOperationsHelperClass.MaxOfMeasurements.ToString();
			MinOfMeasurements = _measurementOperationsHelperClass.MinOfMeasurements.ToString();

		}
		private void SetAllCalculationsControlsVisibilityOFF()
		{
			BasicOperationsVisibility = false;
			MaxByWeekOperationsVisibility = false;
		}
		private void InitializeCommon()
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged; // for future controls use like (last 90 days, last 30 days, etc.)
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged; // for future controls use like (last 90 days, last 30 days, etc.)

			this.SetFilterDatesValues(); // using extension method (last event date and today)
		}

	}
}
