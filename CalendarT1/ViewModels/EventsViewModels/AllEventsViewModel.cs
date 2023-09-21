using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.ViewModels.HelperClass;
using CalendarT1.Views.CustomControls.CCHelperClass;
using CalendarT1.Views.CustomControls.CCInterfaces;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.EventsViewModels
{
	public class AllEventsViewModel : AbstractEventViewModel, IMainEventTypesCC, IFilterDatesCC
	{
		public event Action<MainEventTypes> MainEventTypeChanged;

		//MainEventTypesCC implementation
		#region MainEventTypesCC implementation
		protected IMainEventTypesCC _mainEventTypesCCHelper = Factory.CreateNewIMainEventTypeHelperClass();
		protected List<IUserEventTypeModel> _allUserTypesForVisuals;

		public MainEventTypes SelectedMainEventType
		{
			get => _mainEventTypesCCHelper.SelectedMainEventType;
			set
			{
				_mainEventTypesCCHelper.SelectedMainEventType = value;
				FilterAllEventTypesOCByMainEventType(value);
				OnPropertyChanged();
			}
		}
		private void FilterAllEventTypesOCByMainEventType(MainEventTypes value)
		{
			var tempFilteredEventTypes = FilterUserTypesForVisuals(value);
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(tempFilteredEventTypes);
			OnPropertyChanged(nameof(AllEventTypesOC));
		}
		private List<IUserEventTypeModel> FilterUserTypesForVisuals(MainEventTypes value)
		{
			return _allUserTypesForVisuals.FindAll(x => x.MainEventType == value);
		}
		public ObservableCollection<MainEventVisualDetails> MainEventTypesOC
		{
			get => _mainEventTypesCCHelper.MainEventTypesOC;
			set => _mainEventTypesCCHelper.MainEventTypesOC = value;
		}
		public RelayCommand<MainEventVisualDetails> MainEventTypeSelectedCommand { get; set; }
		public Color MainEventTypeButtonsColor { get; set; } = Color.FromRgb(0, 0, 153); // Defeault color is blue
		public void DisableVisualsForAllMainEventTypes()
		{
			_mainEventTypesCCHelper.DisableVisualsForAllMainEventTypes();
		}
		protected void OnMainEventTypeSelected(MainEventVisualDetails eventType)
		{
			_mainEventTypesCCHelper.MainEventTypeSelectedCommand.Execute(eventType);
			SelectedMainEventType = _mainEventTypesCCHelper.SelectedMainEventType;
			//OnUserEventTypeSelected(AllEventTypesOC[0]);
		}

		#endregion

		//IFilterDatesCC implementation
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();
		public string TextFilterDateFrom { get; set; } = "FROM:";
		public string TextFilterDateTo { get; set; } = "TO:";
		public DateTime FilterDateFrom
		{
			get => _filterDatesCCHelper.FilterDateFrom;
			set
			{
				if (_filterDatesCCHelper.FilterDateFrom == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateFrom = value;
				OnPropertyChanged();
				BindDataToScheduleList();
			}
		}
		public DateTime FilterDateTo
		{
			get => _filterDatesCCHelper.FilterDateTo;
			set
			{
				if (_filterDatesCCHelper.FilterDateTo == value)
				{
					return;
				}
				_filterDatesCCHelper.FilterDateTo = value;
				OnPropertyChanged();
				BindDataToScheduleList();
			}
		}
		private void OnFilterDateFromChanged()
		{
			FilterDateFrom = _filterDatesCCHelper.FilterDateFrom;
		}

		private void OnFilterDateToChanged()
		{
			FilterDateTo = _filterDatesCCHelper.FilterDateTo;
		}
		#endregion

		#region Fields

		private IUserEventTypeModel _eventType;

		#endregion

		#region Properties
		public AsyncRelayCommand DeleteBelowEventsCommand { get; set; }
		public AsyncRelayCommand DeleteAllEventsCommand { get; set; }
		public AsyncRelayCommand DeleteAllUserTypesCommand { get; set; }
		public AsyncRelayCommand SaveBelowEventsToFileCommand { get; set; }
		public AsyncRelayCommand SaveAllEventsToFileCommand { get; set; }
		public AsyncRelayCommand LoadEventsFromFileCommand { get; set; }
		public string AboveEventsListText
		{
			get
			{
				// switch selectedLanguage()
				{
					return "EVENTS LIST";
				}
			}
		}

		#endregion

		#region Constructors

		// All Events MODE
		public AllEventsViewModel(IEventRepository eventRepository) : base(eventRepository)
		{
			InitializeCommon(eventRepository);
			_allUserTypesForVisuals = new List<IUserEventTypeModel>(eventRepository.DeepCopyUserEventTypesList());
			SelectUserEventTypeCommand = new RelayCommand<IUserEventTypeModel>(OnUserEventTypeSelected);
			MainEventTypeSelectedCommand = new RelayCommand<MainEventVisualDetails>(OnMainEventTypeSelected);
			_mainEventTypesCCHelper.DisableVisualsForAllMainEventTypes();
		}

		// Single Event Type MODE
		public AllEventsViewModel(IEventRepository eventRepository, IUserEventTypeModel eventType) : base(eventRepository)
		{
			InitializeCommon(eventRepository);

			var allTempTypes = new List<IUserEventTypeModel>();
			foreach (var item in AllEventTypesOC)
			{
				allTempTypes.Add(item);
			}
		}

		private void InitializeCommon(IEventRepository eventRepository)
		{
			_filterDatesCCHelper.FilterDateFromChanged += OnFilterDateFromChanged;
			_filterDatesCCHelper.FilterDateToChanged += OnFilterDateToChanged;
			DeleteBelowEventsCommand = new AsyncRelayCommand(DeleteShownEvents);
			DeleteAllEventsCommand = new AsyncRelayCommand(DeleteShownEvents);
			DeleteAllUserTypesCommand = new AsyncRelayCommand(DeleteAllUserTypes);
			SaveBelowEventsToFileCommand = new AsyncRelayCommand(OnSaveSelectedEventsAndTypesCommand);
			SaveAllEventsToFileCommand = new AsyncRelayCommand(OnSaveEventsAndTypesCommand);
			LoadEventsFromFileCommand = new AsyncRelayCommand(OnLoadEventsAndTypesCommand);
			this.SetFilterDatesValues(false); // using extension method
		}

		#endregion

		#region Commands

		public async Task DeleteOneEvent()
		{
			if (AllEventsListOC.Count == 0)
			{
				return;
			}
			var firstEvent = AllEventsListOC[0];
			await EventRepository.DeleteFromEventsListAsync(firstEvent);
		}

		public async Task DeleteShownEvents()
		{
			try
			{
				_eventRepository.AllEventsList.RemoveAll(item => EventsToShowList.Contains(item));
				AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
				await EventRepository.SaveEventsListAsync();
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}
		public async Task DeleteAllEvents()
		{
			try
			{
				_eventRepository.AllEventsList.Clear();
				AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
				await EventRepository.SaveEventsListAsync();

			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		public async Task DeleteAllUserTypes()
		{
			try
			{
				_eventRepository.AllUserEventTypesList.Clear();
				AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
				await EventRepository.SaveUserEventTypesListAsync();
			}
			catch (Exception ex)
			{
				await App.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
			}
		}

		#endregion

		#region Private Methods
		private async Task OnSaveEventsAndTypesCommand()
		{
			await _eventRepository.SaveEventsAndTypesToFile();
		}
		private async Task OnSaveSelectedEventsAndTypesCommand()
		{
			await _eventRepository.SaveEventsAndTypesToFile(new List<IGeneralEventModel>(EventsToShowList));
		}
		public AsyncRelayCommand TestButtonCommand2 { get; set; }

		private async Task OnLoadEventsAndTypesCommand()
		{
			await _eventRepository.LoadEventsAndTypesFromFile();
		}
		#endregion

		#region Override Methods

		public override void BindDataToScheduleList()
		{

			if (_eventType == null) // all events mode
			{
				ApplyEventsDatesFilter(FilterDateFrom, FilterDateTo);
			}
			else // single event type mode
			{
				// TODO Change to also visually select proper event type
				List<IGeneralEventModel> filteredEvents = AllEventsListOC
					.Where(x => x.EventType.EventTypeName == _eventType.EventTypeName)
					.ToList();

				// Clear existing items in the EventsToShowList
				EventsToShowList.Clear();

				// Add filtered items to the EventsToShowList
				foreach (var eventItem in filteredEvents)
				{
					EventsToShowList.Add(eventItem);
				}
			}
			EventsToShowList = new ObservableCollection<IGeneralEventModel>(EventsToShowList);


		}
		#endregion
		public void OnAppearing()
		{ 
			BindDataToScheduleList();
		}
	}
}
