using CalendarT1.Helpers;
using CalendarT1.Models;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels
{
	public class AddNewMainTypePageViewModel : BaseViewModel
	{
		private readonly IEventRepository _eventRepository;
		private  Dictionary<string, ObservableCollection<string>> _stringToOCMapper;
		private IMainEventType _currentMainType;
		private string _mainTypeName;
		private string _selectedIconString;
		private Color _backgroundColor;
		private Color _textColor;
		private bool _isEdit;
		private Dictionary<string, RelayCommand<SelectableButtonViewModel>> iconCommandsDictionary;
		private string lastSelectedIconType = "Top";

		public string MyTestFont { get; set; } = IconFont.Home_filled;

		public ObservableCollection<SelectableButtonViewModel> MainButtonVisualsSelectors { get; set; }
		public ObservableCollection<SelectableButtonViewModel> IconsTabsOC { get; set; }

		public string SubmitMainTypeButtonText => _isEdit ? "SUBMIT CHANGES" : "ADD NEW MAIN TYPE";
		public string MainTypePlaceholderText => _isEdit ? $"TYPE NEW NAME FOR: {MainTypeName}" : "...NEW MAIN TYPE NAME...";


		#region Properties
		public string MainTypeName
		{
			get => _mainTypeName;
			set
			{
				_mainTypeName = value;
				OnPropertyChanged();
				SubmitAsyncMainTypeCommand.NotifyCanExecuteChanged();
			}
		}
		public bool IsEdit
		{
			get => _isEdit;
			set
			{
				_isEdit = value;
				OnPropertyChanged();
			}
		}
		public Color TextColor
		{
			get => _textColor;
			set
			{
				_textColor = value;
				OnPropertyChanged();
			}
		}
		public Color BackgroundColor
		{
			get => _backgroundColor;
			set
			{
				_backgroundColor = value;
				OnPropertyChanged();
			}
		}
		public string SelectedIconString
		{
			get => _selectedIconString;
			set
			{
				_selectedIconString = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<string> IconsToShowStringsOC { get; set; }
		public RelayCommand GoToAllMainTypesPageCommand { get; set; }
		public RelayCommand<string> ExactIconSelectedCommand { get; set; }
		public AsyncRelayCommand SubmitAsyncMainTypeCommand { get; set; }
		public AsyncRelayCommand DeleteAsyncSelectedEventTypeCommand { get; set; }
		public RelayCommand<SelectableButtonViewModel> ActivitiesIconsCommand { get; set; }
		public RelayCommand<SelectableButtonViewModel> HomeIconsCommand { get; set; }
		public RelayCommand<SelectableButtonViewModel> Top3IconsCommand { get; set; }
		#endregion


		#region Constructors
		//Constructor for create mode
		public AddNewMainTypePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			InitializeCommon();

		}
		//Constructor for edit mode
		public AddNewMainTypePageViewModel(IEventRepository eventRepository, IMainEventType currentMainType)
		{
			_eventRepository = eventRepository;
			MainTypeName = currentMainType.Title;
			_currentMainType = currentMainType;

			InitializeCommon();
		}

		#endregion

		#region public methods

		#endregion



		#region private methods
		private void InitializeCommon()
		{
			RefreshIconsToShowOC();
			InitializeColors();
			InitializeCommands();
			InitializeSelectors();
		}
		private void InitializeIconsTabs()
		{
			IconsTabsOC = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Top", true, new RelayCommand(() => OnExactIconsTabCommand("Top"))),
				new SelectableButtonViewModel("Activities", false, new RelayCommand(() => OnExactIconsTabCommand("Activities"))),
				new SelectableButtonViewModel("Others", false, new RelayCommand(() => OnExactIconsTabCommand("Others"))),
			};
			RefreshIconsToShowOC();
			OnPropertyChanged(nameof(IconsTabsOC));
		}
		private void RefreshIconsToShowOC()
		{
			_stringToOCMapper = new Dictionary<string, ObservableCollection<string>>
			{
				{ "Top", IconsHelperClass.GetTopIcons3() },
				{ "Activities", IconsHelperClass.GetTopIcons() },
				{ "Others", IconsHelperClass.GetTopIcons2() }
			};
		}
		private void InitializeColors()
		{
			BackgroundColor = Color.FromArgb("#fff");
			TextColor = Color.FromArgb("#000");
		}

		private void InitializeCommands()
		{
			GoToAllMainTypesPageCommand = new RelayCommand(OnGoToAllMainTypesPageCommand);
			SubmitAsyncMainTypeCommand = new AsyncRelayCommand(OnSubmitMainTypeCommand, CanExecuteSubmitMainTypeCommand);
			DeleteAsyncSelectedEventTypeCommand = new AsyncRelayCommand(OnDeleteMainTypeCommand);

			ExactIconSelectedCommand = new RelayCommand<string>(OnExactIconSelectedCommand);
		}

		private void InitializeSelectors()
		{
			SelectedIconString = IconFont.Minor_crash;
			MainButtonVisualsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Icons", true, new RelayCommand<SelectableButtonViewModel>(OnShowIconsTabCommand)),
				new SelectableButtonViewModel("Background Colors", false, new RelayCommand<SelectableButtonViewModel>(OnShowBgColorsCommand)),
				new SelectableButtonViewModel("Text Colors", false, new RelayCommand<SelectableButtonViewModel>(OnShowTextColorsCommand)),
			};
			InitializeIconsTabs();
		}

		private async Task OnSubmitMainTypeCommand()
		{
			var iconForMainEventType = Factory.CreateIMainTypeVisualElement(SelectedIconString, BackgroundColor, TextColor);
			if (_isEdit)
			{
				_currentMainType.Title = MainTypeName;
				_currentMainType.SelectedVisualElement = iconForMainEventType;


				await _eventRepository.UpdateMainEventTypeAsync(_currentMainType);
				await Shell.Current.GoToAsync("..");    // TODO CHANGE NOT WORKING!!!
			}
			else
			{
				var newMainType = Factory.CreateNewMainEventType(MainTypeName, iconForMainEventType);
				await _eventRepository.AddMainEventTypeAsync(newMainType);
				await Shell.Current.GoToAsync("..");    // TODO !!!!! CHANGE NOT WORKING!!!
			}
		}
		private void OnGoToAllMainTypesPageCommand()
		{
			Application.Current.MainPage.Navigation.PushAsync(new AllMainTypesPage());
		}
		private async Task OnDeleteMainTypeCommand()
		{
			var eventTypesInDb = _eventRepository.AllEventsList.Where(x => x == _currentMainType); // TODO equals !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
			if (eventTypesInDb.Any())
			{
				var action = await App.Current.MainPage.DisplayActionSheet("This main type is used in some events/sub types.", "Cancel", null, "Delete all associated events and sub types", "Go to All SubTypes Page");
				switch (action)
				{
					case "Delete all associated events and sub types":
						// Perform the operation to delete all events of the event type.
						_eventRepository.AllEventsList.RemoveAll(x => x.EventType.MainEventType == _currentMainType);
						await _eventRepository.SaveEventsListAsync();
						_eventRepository.AllUserEventTypesList.RemoveAll(x => x.MainEventType == _currentMainType);
						await _eventRepository.SaveSubEventTypesListAsync();
						// TODO make a confirmation message
						break;
					case "Go to All SubTypes Page":
						// Redirect to the All Events Page.
						await Shell.Current.GoToAsync("AllSubTypesPage");
						break;
					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;
			}


		}
		private bool CanExecuteSubmitMainTypeCommand()
		{
			return !string.IsNullOrEmpty(MainTypeName);
		}

		private void OnExactIconsTabCommand(string iconType)
		{
			var lastSelectedButton = IconsTabsOC.Single(x => x.ButtonText == iconType);
			OnExactIconsTabClick(lastSelectedButton, _stringToOCMapper[iconType]);
		}
		private void OnExactIconsTabClick(SelectableButtonViewModel clickedButton, ObservableCollection<string> iconsToShowOC)
		{
			SingleButtonSelection(clickedButton, IconsTabsOC);
			lastSelectedIconType = clickedButton.ButtonText;
			IconsToShowStringsOC = iconsToShowOC;
			OnPropertyChanged(nameof(IconsToShowStringsOC));
		}



		private void OnExactIconSelectedCommand(string visualStringSource)
		{
			SelectedIconString = visualStringSource;
		}
		#endregion

		private void OnShowIconsTabCommand(SelectableButtonViewModel clickedButton)
		{
			SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
			InitializeIconsTabs();
			var buttonToSelect = IconsTabsOC.Single(x => x.ButtonText == lastSelectedIconType);
			OnExactIconsTabClick(buttonToSelect, _stringToOCMapper[lastSelectedIconType]);

			// TODO !!!!!!!!!!!!!!!!!
		}
		private void OnShowBgColorsCommand(SelectableButtonViewModel clickedButton)
		{
			SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);
			if(IconsTabsOC != null && IconsTabsOC.Any())
			{
				IconsTabsOC.Clear();
			}
			if (IconsToShowStringsOC != null && IconsToShowStringsOC.Any())
			{
				IconsToShowStringsOC.Clear();
			}
			// TODO!!!!!!!!!!!!!!!!!
		}
		private void OnShowTextColorsCommand(SelectableButtonViewModel clickedButton)
		{
			SingleButtonSelection(clickedButton, MainButtonVisualsSelectors);


			// TODO!!!!!!!!!!!!!!!!!!
		}

		private void SingleButtonSelection(SelectableButtonViewModel clickedButton, ObservableCollection<SelectableButtonViewModel> buttonsToDeselect)
		{
			DeselectAllButtons(buttonsToDeselect);
			clickedButton.IsSelected = true;
		}
		private void DeselectAllButtons(ObservableCollection<SelectableButtonViewModel> buttonsToDeselect)
		{
			foreach (var button in buttonsToDeselect)
			{
				button.IsSelected = false;
			}
		}
	}
}