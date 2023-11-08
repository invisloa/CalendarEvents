using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CalendarT1.ViewModels
{

	public class PreferencesViewModel : BaseViewModel
	{
		// PropertyChanged event declaration

		// Properties to bind to the UI
		private bool selectedLanguage;
		public bool SelectedLanguage
		{
			get => selectedLanguage;
			set
			{
				selectedLanguage = value;
				OnPropertyChanged(nameof(SelectedLanguage));
			}
		}

		private bool subEventTypeTimesDifferent;
		public bool SubEventTypeTimesDifferent
		{
			get => subEventTypeTimesDifferent;
			set
			{
				subEventTypeTimesDifferent = value;
				OnPropertyChanged(nameof(SubEventTypeTimesDifferent));
			}
		}

		private bool mainEventTypeTimesDifferent;
		public bool MainEventTypeTimesDifferent
		{
			get => mainEventTypeTimesDifferent;
			set
			{
				mainEventTypeTimesDifferent = value;
				OnPropertyChanged(nameof(MainEventTypeTimesDifferent));
			}
		}

		private bool weeklyHoursSpant;
		public bool WeeklyHoursSpant
		{
			get => weeklyHoursSpant;
			set
			{
				weeklyHoursSpant = value;
				OnPropertyChanged(nameof(WeeklyHoursSpant));
			}
		}

		// Text properties for labels (assuming you'll localize or set these texts elsewhere)
		public string SelectedLanguageText { get; set; } = "Selected Language";
		public string SubEventTypeTimesDifferentText { get; set; } = "Sub Event Type Times Different";
		public string MainEventTypeTimesDifferentText { get; set; } = "Main Event Type Times Different";
		public string WeeklyHoursSpantText { get; set; } = "Weekly Preferred Hours Span";

		// Save command
		public ICommand SaveCommand { get; }

		public PreferencesViewModel()
		{
			// Initialize preferences
			SelectedLanguage = Preferences.Get(nameof(SelectedLanguage), false);
			SubEventTypeTimesDifferent = Preferences.Get(nameof(SubEventTypeTimesDifferent), false);
			MainEventTypeTimesDifferent = Preferences.Get(nameof(MainEventTypeTimesDifferent), false);
			WeeklyHoursSpant = Preferences.Get(nameof(WeeklyHoursSpant), false);

			// Initialize commands
			SaveCommand = new Command(SavePreferences);
		}

		// Method to save preferences
		private void SavePreferences()
		{
			Preferences.Set(nameof(SelectedLanguage), SelectedLanguage);
			Preferences.Set(nameof(SubEventTypeTimesDifferent), SubEventTypeTimesDifferent);
			Preferences.Set(nameof(MainEventTypeTimesDifferent), MainEventTypeTimesDifferent);
			Preferences.Set(nameof(WeeklyHoursSpant), WeeklyHoursSpant);
		}

	}
}