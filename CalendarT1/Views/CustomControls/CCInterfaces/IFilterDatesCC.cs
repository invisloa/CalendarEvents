namespace CalendarT1.Views.CustomControls.CCInterfaces
{
    public interface IFilterDatesCC
    {
        DateTime FilterDateFrom { get; set; }
        DateTime FilterDateTo { get; set; }
        string TextFilterDateFrom { get; set; }
        string TextFilterDateTo { get; set; }
    }
}