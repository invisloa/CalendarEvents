namespace CalendarT1.Views.CustomControls.CCHelperClass.CalendarT1.Views.CustomControls.CCHelperClass
{
	public interface IFilterDatesCCHelperClass
	{
		DateTime FilterDateFrom { get; set; }
		DateTime FilterDateTo { get; set; }
		string TextFilterDateFrom { get; set; }
		string TextFilterDateTo { get; set; }

		event Action FilterDateFromChanged;
		event Action FilterDateToChanged;
	}
}