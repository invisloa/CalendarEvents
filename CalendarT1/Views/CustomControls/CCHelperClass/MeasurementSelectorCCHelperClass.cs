using CalendarT1.Models.EventModels;
using CalendarT1.Services;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCHelperClass
{
	public class MeasurementSelectorCCHelperClass : IMeasurementSelectorCC
	{
		private ObservableCollection<MeasurementUnitItem> _measurementUnitsOC;
		private MeasurementUnitItem _selectedMeasurementUnit;
		private Quantity _eventQuantity;
		private bool _isValueTypeSelected;
		private decimal _quantityValue = 0;
		private RelayCommand<MeasurementUnitItem> _measurementUnitSelectedCommand;
		private string _quantityValueText = "Default value:";

		public string QuantityValueText { get => _quantityValueText; set => _quantityValueText = value; }
		public MeasurementSelectorCCHelperClass()
		{
			_measurementUnitSelectedCommand = new RelayCommand<MeasurementUnitItem>(OnMeasurementUnitSelected);
			_selectedMeasurementUnit = MeasurementUnitsOC[0];   //default value- Currency
			_eventQuantity = new Quantity(_selectedMeasurementUnit.TypeOfMeasurementUnit, _quantityValue);

		}
		public ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC
		{
			get
			{
				return _measurementUnitsOC ??= Factory.PopulateMeasurementCollection();
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
			}
		}
		public Quantity EventQuantity { get => _eventQuantity; set => _eventQuantity = value; }
		public bool IsValueTypeSelected { get => _isValueTypeSelected; set => _isValueTypeSelected = value; }
		public decimal QuantityValue { get =>_quantityValue; set => _quantityValue = value; }
		private void OnMeasurementUnitSelected(MeasurementUnitItem measurementUnitItem)
		{
			SelectedMeasurementUnit = measurementUnitItem;
		}
	}
}
