namespace CalendarT1.Models
{
	public interface IMainTypeVisualElement
	{
		Color BackgroundColor { get; set; }
		string BackgroundColorString { get; set; }
		string IconName { get; set; }
		Color TextColor { get; set; }
		string TextColorString { get; set; }

		bool Equals(object obj);
		int GetHashCode();
	}
}