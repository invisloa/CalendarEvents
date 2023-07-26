namespace CalendarT1.Services.SettingsService
{
	public class SettingsService : ISettingsService
	{
		public bool ShowCompletedEvents
		{
			get => Preferences.Get(nameof(ShowCompletedEvents), false);
			set => Preferences.Set(nameof(ShowCompletedEvents), value);
		}

		public SettingsService()
		{
		}
	}
}
