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
	class AddNewMainTypePageViewModel : BaseViewModel
	{

		public string MyTestFont { get; set; } = IconFont.Home_filled;

		public ObservableCollection<SelectableButtonViewModel> MainButtonVisualsSelectors { get; set; }


		private IEventRepository _eventRepository;
		private IMainEventType _currentMainType;
		private string _mainTypeName;
		private string _selectedIconString;
		private Color _backgroundColor;
		private Color _textColor;
		private bool _isEdit;
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
		public ObservableCollection<string> IconsListStrings { get; set; }  // to initialize in ctor

		public RelayCommand GoToAllMainTypesPageCommand { get; set; }// to initialize in ctor
		public RelayCommand<string> IconSelectedCommand  { get; set; }// to initialize in ctor
		public AsyncRelayCommand SubmitAsyncMainTypeCommand { get; set; }// to initialize in ctor
		public AsyncRelayCommand DeleteAsyncSelectedEventTypeCommand { get; set; }// to initialize in ctor
		public RelayCommand ActivitiesIconsCommand { get; set; }// to initialize in ctor
		public RelayCommand HomeIconsCommand { get; set; }// to initialize in ctor


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
			BackgroundColor = Color.FromArgb("#fff");
			TextColor = Color.FromArgb("#000");

			bool isEditMode = _currentMainType != null;
			IconsListStrings = Factory.CreateIconsListStrings();
			GoToAllMainTypesPageCommand = new RelayCommand(OnGoToAllMainTypesPageCommand);
			SubmitAsyncMainTypeCommand = new AsyncRelayCommand(OnSubmitMainTypeCommand, CanExecuteSubmitMainTypeCommand);
			DeleteAsyncSelectedEventTypeCommand = new AsyncRelayCommand(OnDeleteMainTypeCommand);
			IconSelectedCommand = new RelayCommand<string>(OnIconSelectedCommand);
			ActivitiesIconsCommand = new RelayCommand(OnActivitiesIconsCommand);
			HomeIconsCommand = new RelayCommand(OnHomeIconsCommand);
			SelectedIconString = IconFont.Minor_crash;
			MainButtonVisualsSelectors = new ObservableCollection<SelectableButtonViewModel>
			{
				new SelectableButtonViewModel("Icons", true, new RelayCommand<SelectableButtonViewModel>(ShowIcons)),
				new SelectableButtonViewModel("Background Colors", false, new RelayCommand<SelectableButtonViewModel>(ShowBackgroundColors)),
				new SelectableButtonViewModel("Text Colors", false, new RelayCommand<SelectableButtonViewModel>(ShowTextColors)),

			};
		}

		private async Task OnSubmitMainTypeCommand()
		{
			var iconForMainEventType = Factory.CreateIMainTypeVisualElement(SelectedIconString, BackgroundColor, TextColor);
			if (_isEdit)
			{
				_currentMainType.Title = MainTypeName;
				_currentMainType.SelectedVisualElement = iconForMainEventType;


				await _eventRepository.UpdateMainEventTypeAsync(_currentMainType);
				await Shell.Current.GoToAsync("..");	// TODO CHANGE NOT WORKING!!!
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
		private void OnIconSelectedCommand(string visualStringSource)
		{
			SelectedIconString = visualStringSource;
		}

		private void OnActivitiesIconsCommand()
		{
			var newIconsListStrings = new ObservableCollection<string>
				{
					IconFont.Language,
					IconFont.Landslide,
					IconFont.Landscape
				};

			IconsListStrings = newIconsListStrings;
			OnPropertyChanged(nameof(IconsListStrings));
		}

		private void OnHomeIconsCommand()
		{
			var newIconsListStrings = new ObservableCollection<string>
				{
					IconFont.Safety_check,
					IconFont.Safety_divider,
					IconFont.Sailing
				};

			IconsListStrings = newIconsListStrings;
			OnPropertyChanged(nameof(IconsListStrings));
		}
		#endregion


		private void ShowIcons(SelectableButtonViewModel clickedButton)
		{
			DeselectAllButtons();
			clickedButton.IsSelected = true;

			// TODO !!!!!!!!!!!!!!!!!
		}
		private void ShowBackgroundColors(SelectableButtonViewModel clickedButton)
		{
			DeselectAllButtons();
			clickedButton.IsSelected = true;

			// TODO!!!!!!!!!!!!!!!!!
		}
		private void ShowTextColors(SelectableButtonViewModel clickedButton)
		{
			DeselectAllButtons();
			clickedButton.IsSelected = true;

			// TODO!!!!!!!!!!!!!!!!!!
		}
		private void DeselectAllButtons()
		{
			foreach (var button in MainButtonVisualsSelectors)
			{
				button.IsSelected = false;
			}
		}
	}
}