using CalendarT1.Models.EventModels;

namespace CalendarT1.ViewModels.HelperClass
{
	public interface IMeasurementOperationsHelperClass
	{
		decimal AverageOfMeasurements { get; set; }
		decimal MaxOfMeasurements { get; set; }
		decimal MedianOfMeasurements { get; set; }
		decimal MinOfMeasurements { get; set; }
		decimal SumOfMeasurements { get; set; }

		bool DoValueTypesCalculations(List<Quantity> allUserEvents);
	}
}