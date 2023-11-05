using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.ViewModels;
using CalendarT1.ViewModels.HelperClass;
using CalendarT1.Views.CustomControls.CCInterfaces.UserTypeExtraOptions;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCViewModels
{
    public class MeasurementSelectorCCViewModel : BaseViewModel, IMeasurementSelectorCC
	{
		private ObservableCollection<MeasurementUnitItem> _measurementUnitsOC;
		private MeasurementUnitItem _selectedMeasurementUnit;
		private QuantityModel _eventQuantityAmount;
		private bool _isValueTypeSelected;

		private RelayCommand<MeasurementUnitItem> _measurementUnitSelectedCommand;
		private string _quantityValueText;
		private IMeasurementOperationsHelperClass _measurementOperationsHelperClass;
		public IMeasurementOperationsHelperClass MeasurementOperationsHelperClass { get => _measurementOperationsHelperClass; set => _measurementOperationsHelperClass = value; }
		
		// text in the UI
		public string QuantityValueText { get => _quantityValueText; set => _quantityValueText = value; }
		public MeasurementSelectorCCViewModel()
		{
			_measurementUnitSelectedCommand = new RelayCommand<MeasurementUnitItem>(OnMeasurementUnitSelected);
			_selectedMeasurementUnit = MeasurementUnitsOC[0];   //default value- Currency
			_eventQuantityAmount = new QuantityModel(_selectedMeasurementUnit.TypeOfMeasurementUnit, 0);
		}
		public ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC		// TO CHECK IF THIS IS NEEDED MAYBE REMOVE !!!
		{
			get
			{
				return _measurementUnitsOC ??= Factory.PopulateMeasurementCollection();
			}
			set
			{
				_measurementUnitsOC = value;
				OnPropertyChanged();

			}
		}
		public RelayCommand<MeasurementUnitItem> MeasurementUnitSelectedCommand
		{
			get => _measurementUnitSelectedCommand;
			set
			{
				_measurementUnitSelectedCommand = value;
			}
		}
		public MeasurementUnitItem SelectedMeasurementUnit { get => _selectedMeasurementUnit;
			set 
			{
				if(_selectedMeasurementUnit == value)
				{
					return;
				}
				_selectedMeasurementUnit = value;
				if(_selectedMeasurementUnit != null)
				{
					QuantityAmount.Unit = _selectedMeasurementUnit.TypeOfMeasurementUnit;
				}
				OnPropertyChanged();
				
			}
		}
		public QuantityModel QuantityAmount 
		{ 
			get => _eventQuantityAmount;
			set
			{
				_eventQuantityAmount = value;
				OnPropertyChanged();
				OnPropertyChanged(nameof(QuantityValue));
			}
		}
		public bool IsValueTypeSelected { get => _isValueTypeSelected; set => _isValueTypeSelected = value; }
		public decimal QuantityValue
		{
			get => QuantityAmount.Value;
			set
			{ 
				if (QuantityAmount != null)
				{
					QuantityAmount.Value = value;
				}
				OnPropertyChanged();
			}
		}
		private void OnMeasurementUnitSelected(MeasurementUnitItem measurementUnitItem)
		{
			SelectedMeasurementUnit = measurementUnitItem;
		}
		public void SelectPropperMeasurementData(ISubEventTypeModel userEventTypeModel)
		{
			try
			{
				if (userEventTypeModel.IsValueType)
				{
					SelectedMeasurementUnit = MeasurementUnitsOC.FirstOrDefault(mu => mu.TypeOfMeasurementUnit == userEventTypeModel.DefaultQuantityAmount.Unit);
					IsValueTypeSelected = true;
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error selecting proper measurement data for event type: " + userEventTypeModel.MainEventType, ex);
			}
		}
	}
}
