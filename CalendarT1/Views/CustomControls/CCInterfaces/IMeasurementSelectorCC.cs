using CalendarT1.Models.EventModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.Views.CustomControls.CCInterfaces
{

	/// <summary>
	/// Its good to use MeasurementOperationsHelperClass
	/// </summary>
	public interface IMeasurementSelectorCC
    {
        // Properties
		ObservableCollection<MeasurementUnitItem> MeasurementUnitsOC { get;}
		RelayCommand<MeasurementUnitItem> MeasurementUnitSelectedCommand { get; set; }
        MeasurementUnitItem SelectedMeasurementUnit { get; set; }
        Quantity EventQuantity { get; set; }
        bool IsValueTypeSelected { get; set; }
        public string QuantityValueText { get; set; }
		public decimal QuantityValue { get; set; }

	}
}
