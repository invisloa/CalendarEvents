using CalendarT1.Models.EventModels;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarT1.ViewModels.TypesViewModels
{

	class AllSubTypesPageViewModel : BaseViewModel
	{
		#region Fields

		private IEventRepository _eventRepository;
		private ObservableCollection<IGeneralEventModel> _allEventsListOC;
		private ObservableCollection<IUserEventTypeModel> _allEventTypesOC;

		#endregion

		#region Properties

		public IEventRepository EventRepository
		{
			get => _eventRepository;
		}

		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<IUserEventTypeModel> AllEventTypesOC
		{
			get => _allEventTypesOC;
			set
			{
				if (_allEventTypesOC == value)
				{
					return;
				}
				_allEventTypesOC = value;
				OnPropertyChanged();
			}
		}

		public RelayCommand<IUserEventTypeModel> EditSelectedTypeCommand { get; set; }

		#endregion

		#region Constructor

		public AllSubTypesPageViewModel(IEventRepository eventRepository)
		{
			_eventRepository = eventRepository;
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserEventTypeListChanged += UpdateAllEventTypesList;
			EditSelectedTypeCommand = new RelayCommand<IUserEventTypeModel>(EditSelectedType);
		}

		#endregion

		#region Public Methods

		public void UpdateAllEventList()
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}

		public void UpdateAllEventTypesList()
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}

		#endregion

		#region Private Methods

		private void EditSelectedType(IUserEventTypeModel userTypeToEdit)
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewSubTypePage(EventRepository, userTypeToEdit));
		}

		#endregion
	}
}
