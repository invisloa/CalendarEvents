using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
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
		public List<IMainEventType> MainEventTypesList{ get; set ; }
		private IMainEventType _currentMainType;
		private string _selectedIconSource;

		public List<string> Icons { get; } = new List<string>
		{
			"logout.png",
			"login.png",
			// ... add other icon sources here ...
		};

		public string SelectedIconSource
		{
			get => _selectedIconSource;
			set
			{
				if (_selectedIconSource != value)
				{
					_selectedIconSource = value;
					OnPropertyChanged(nameof(SelectedIconSource));
				}
			}
		}
		private string _typeName;

		private string _mainTypeName;
		private IEventRepository _eventRepository;
		public string MainTypePageTitle => IsEdit ? "EDIT MAIN TYPE" : "ADD NEW MAIN TYPE";
		public string MainTypePlaceholderText => IsEdit ? $"TYPE NEW NAME FOR: {MainTypeName}" : "...NEW MAIN TYPE NAME...";
		public string MainTypeSubmitButtonText => IsEdit ? "SUBMIT CHANGES" : "ADD NEW MAIN TYPE";
		public bool IsEdit => _currentMainType != null;
		public bool IsNotEdit => !IsEdit;
		public string MainTypeName
		{
			get => _typeName;
			set
			{
				if (value == _typeName) return;
				_typeName = value;
				SubmitMainTypeCommand.NotifyCanExecuteChanged();
				OnPropertyChanged();
			}
		}


		public RelayCommand GoToAllMainTypesPageCommand { get; private set; }
		public AsyncRelayCommand SubmitMainTypeCommand { get; private set; }
		public AsyncRelayCommand DeleteMainTypeCommand { get; private set; }
		#region Commands CanExecute
		private bool CanExecuteSubmitMainTypeCommand() => !string.IsNullOrEmpty(MainTypeName);

		#region Constructors
		// constructor for create mode
		public AddNewMainTypePageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			InitializeCommon();

		}

		// constructor for edit mode
		public AddNewMainTypePageViewModel(IEventRepository eventRepository, IMainEventType currentMainType)
		{
			_eventRepository = eventRepository;
			MainTypeName = currentMainType.Title;
			_currentMainType = currentMainType;

			InitializeCommon();
		}

		private void InitializeCommon()
		{
			bool isEditMode = _currentMainType != null;
			GoToAllMainTypesPageCommand = new RelayCommand(OnGoToAllMainTypesPageCommand);
			SubmitAsyncMainTypeCommand = new AsyncRelayCommand(OnSubmitMainTypeCommand, CanExecuteSubmitMainTypeCommand);
			DeleteMainTypeCommand = new AsyncRelayCommand(OnDeleteMainTypeCommand);
		}


		#region Methods
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
			await _eventRepository.DeleteFromMainEventTypesListAsync(_currentMainType);
			await Shell.Current.Navigation.PopAsync();

			//await Shell.Current.GoToAsync($"{nameof(AllSubTypesPage)}");
		}
		private async Task OnSubmitMainTypeCommand()
		{
			//if (IsEdit)
			//{
			//	_currentMainType.Title = MainTypeName;
			//	_currentMainType.EventTypeColor = _selectedColor;
			//	await _eventRepository.UpdateMainEventTypeAsync(_currentMainType);
			//	await Shell.Current.GoToAsync("..");                                // TODO CHANGE NOT WORKING!!!
			//}
			//else
			//{
			//	var newMainType = Factory.CreateNewMainEventType(MainTypeName, _selectedColor);
			//	await _eventRepository.AddMainEventTypeAsync(newMainType);
			//	await Shell.Current.GoToAsync("..");    // TODO !!!!! CHANGE NOT WORKING!!!
			//}
		}
		private void OnGoToAllMainTypesPageCommand()
		{
			//Application.Current.MainPage.Navigation.PushAsync(new AllMainTypesPage());
		}
	}
	#endregion


}
#endregion
#endregion
