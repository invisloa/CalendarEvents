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
    class AllTypesPageViewModel : BaseViewModel
    {
        private IEventRepository _eventRepository;
		public IEventRepository EventRepository
		{
			get => _eventRepository;
		}
		ObservableCollection<IGeneralEventModel> _allEventsListOC;
		public ObservableCollection<IGeneralEventModel> AllEventsListOC
		{
			get => _allEventsListOC;
			set
			{
				_allEventsListOC = value;
				OnPropertyChanged();
			}
		}
		private ObservableCollection<IUserEventTypeModel> _allEventTypesOC;
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
		private ObservableCollection<IUserEventTypeModel> _typesToShowOC;
		public ObservableCollection<IUserEventTypeModel> TypesToShowOC
		{
			get => _typesToShowOC;
			set
			{
				if (_typesToShowOC == value)
				{
					return;
				}
				_typesToShowOC = value;
				OnPropertyChanged();
			}
		}
		public RelayCommand<IUserEventTypeModel> EditSelectedTypeCommand { get; set; }

		public AllTypesPageViewModel(IEventRepository eventRepository)
        {
			_eventRepository = eventRepository;
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
			TypesToShowOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList); // just temporary To Do: change to only show selected types
			_eventRepository.OnEventListChanged += UpdateAllEventList;
			_eventRepository.OnUserTypeListChanged += UpdateAllEventTypesList;
			EditSelectedTypeCommand = new RelayCommand<IUserEventTypeModel>(EditSelectedType);
		}

		public void UpdateAllEventList()
		{
			AllEventsListOC = new ObservableCollection<IGeneralEventModel>(_eventRepository.AllEventsList);
		}
		public void UpdateAllEventTypesList()
		{
			AllEventTypesOC = new ObservableCollection<IUserEventTypeModel>(_eventRepository.AllUserEventTypesList);
		}
		private void EditSelectedType(IUserEventTypeModel userTypeToEdit)
		{
			Application.Current.MainPage.Navigation.PushAsync(new AddNewTypePage(EventRepository, userTypeToEdit));

		}

	}
}
