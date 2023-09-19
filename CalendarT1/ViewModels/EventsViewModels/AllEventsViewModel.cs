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

	public class AllEventsViewModel : AbstractEventViewModel, IFilterDatesCC
	{
		//IFilterDatesCC implementation
		#region IFilterDatesCC implementation
		private IFilterDatesCCHelperClass _filterDatesCCHelper = Factory.CreateFilterDatesCCHelperClass();
		public string TextFilterDateFrom { get; set; } = "FILTER FROM:";
		public string TextFilterDateTo { get; set; } = "FILTER UP TO:";
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
		public AsyncRelayCommand SaveBelowEventsToFileCommand { get; set; }
		public AsyncRelayCommand SaveAllEventsToFileCommand { get; set; }
		public AsyncRelayCommand LoadEventsFromFileCommand { get; set; }
		public event Action<MainEventTypes> MainEventTypeChanged;

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
		public AllEventsViewModel(IEventRepository eventRepository) :base(eventRepository)
		{
			InitializeCommon(eventRepository);
			SelectUserEventTypeCommand = new RelayCommand<IUserEventTypeModel>(OnUserEventTypeSelected);
			_eventRepository = eventRepository;
			AllEventTypesOC.Add(new UserEventTypeModel(MainEventTypes.Event, "Some Type Event", Color.FromArgb("#FFC0C0C0")));
			AllEventsListOC.Add(new EventModel("someEvent", "", DateTime.MinValue, DateTime.MinValue, AllEventTypesOC[0]));
			TestOneButtonClick = new RelayCommand(OnTestOneButtonClick);
			TestTwoButtonClick = new RelayCommand(OnTestTwoButtonClick);
			GoToSelectedDateCommand = new RelayCommand<DateTime>(GoToSelectedDatePage);
			SelectTodayDateCommand = new RelayCommand(() => CurrentSelectedDate = CurrentDate);

		}


		public RelayCommand TestOneButtonClick { get; set; }
		public RelayCommand TestTwoButtonClick { get; set; }

		private void OnTestOneButtonClick()
		{
			AllEventTypesOC.Clear();
			OnPropertyChanged(nameof(AllEventTypesOC));
		}
		private void OnTestTwoButtonClick()
		{
			AllEventsListOC.Clear();
			OnPropertyChanged(nameof(AllEventsListOC));

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
			DeleteBelowEventsCommand = new AsyncRelayCommand(DeleteBelowEvents);
			SaveBelowEventsToFileCommand = new AsyncRelayCommand(OnSaveSelectedEventsAndTypesCommand);
			SaveAllEventsToFileCommand = new AsyncRelayCommand(OnSaveEventsAndTypesCommand);
			LoadEventsFromFileCommand = new AsyncRelayCommand(OnLoadEventsAndTypesCommand);
			this.SetFilterDatesValues(); // using extension method
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

		public async Task DeleteBelowEvents()
		{
			try
			{
				var action2 = await App.Current.MainPage.DisplayAlert("DELETE BELOW EVENTS", "ARE YOU SURE YOU WANT TO DELETE ALL BELOW EVENTS??",  "DELETE", "CANCEL" );
				var action = await App.Current.MainPage.DisplayActionSheet("ARE YOU SURE YOU WANT TO DELETE ALL BELOW EVENTS??", "Cancel", null, "DELETE BELOW EVENTS");
				switch (action)
				{
					case "DELETE BELOW EVENTS":
						_eventRepository.AllEventsList.RemoveAll(item => EventsToShowList.Contains(item));
						AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
						await EventRepository.SaveEventsListAsync();
						break;

					default:
						// Cancel was selected or back button was pressed.
						break;
				}
				return;

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

		}
		#endregion

	}
}
