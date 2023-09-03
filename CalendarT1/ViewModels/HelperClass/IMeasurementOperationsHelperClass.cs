using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;

namespace CalendarT1.ViewModels.HelperClass
{
	public interface IMeasurementOperationsHelperClass
	{
		decimal AverageOfMeasurements { get; set; }
		decimal MaxOfMeasurements { get; set; }
		decimal MedianOfMeasurements { get; set; }
		decimal MinOfMeasurements { get; set; }
		decimal SumOfMeasurements { get; set; }

		bool DoValueTypesBasicCalculations(List<IGeneralEventModel> allUserEvents, DateTime from, DateTime to);
	}
}