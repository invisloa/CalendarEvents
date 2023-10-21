using CalendarT1.Models;
using CalendarT1.Models.EventTypesModels;
using CalendarT1.Services;
using CalendarT1.ViewModels;
using CalendarT1.Views.CustomControls.CCInterfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace CalendarT1.Views.CustomControls.CCViewModels
{
	public class MainEventTypesSelectorCCViewModel : BaseViewModel, IMainEventTypesCCViewModel
	{
		// Constants
		private const int FullOpacity = 1;
		private const float FadedOpacity = 0.3f;
		private const int NoBorderSize = 0;
		private const int BorderSize = 10;

		// Fields
		private readonly List<IMainEventType> _mainEventTypesList;
		private readonly Dictionary<IMainEventType, MainEventTypeViewModel> _eventVisualDetails;
		private IMainEventType _selectedMainEventType;
		private IMainTypeVisualModel _selectedIcon;
		private Color _selectedColor = Color.FromRgb(255, 0, 0); // Default to red

		// Properties
		public ObservableCollection<MainEventTypeViewModel> MainEventTypesVisualsOC { get; set; }
		public RelayCommand<MainEventTypeViewModel> MainEventTypeSelectedCommand { get; private set; }
		public IMainEventType SelectedMainEventType
		{
			get => _selectedMainEventType;
			set
			{
				if (_selectedMainEventType != value)
				{
					_selectedMainEventType = value;
					OnPropertyChanged(nameof(SelectedMainEventType));
					MainEventTypeChanged?.Invoke(_selectedMainEventType);
				}
			}
		}
		public IMainTypeVisualModel SelectedIcon
		{
			get => _selectedIcon;
			set => _selectedIcon = value;
		}

		// Events
		public event Action<IMainEventType> MainEventTypeChanged;

		// Constructor
		public MainEventTypesSelectorCCViewModel(List<IMainEventType> mainEventTypesList)
		{
			_mainEventTypesList = mainEventTypesList ?? throw new ArgumentNullException(nameof(mainEventTypesList));
			_eventVisualDetails = new Dictionary<IMainEventType, MainEventTypeViewModel>();
			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(SetMainEventTypeFromViewModel);
			InitializeMainEventTypesVisuals();
		}

		// Private Methods
		private void SetMainEventTypeFromViewModel(MainEventTypeViewModel viewModel)
		{
			var selectedMainEventType = _mainEventTypesList.FirstOrDefault(o => o.Equals(viewModel.MainEventType));
			if (selectedMainEventType == null)
			{
				throw new ArgumentException($"Invalid TypeOfEvent value: {viewModel.MainEventType}");
			}
			SelectedMainEventType = selectedMainEventType;
			deselectAllMainEventTypes();
			viewModel.IsSelected = true;
			MainEventTypeChanged?.Invoke(SelectedMainEventType);

		}

		private void deselectAllMainEventTypes()
		{
			foreach (var eventType in _eventVisualDetails.Values)
			{
				eventType.IsSelected = false;
			}
		}

		private void InitializeMainEventTypesVisuals()
		{
			MainEventTypesVisualsOC = new ObservableCollection<MainEventTypeViewModel>();

			foreach (IMainEventType eventType in _mainEventTypesList)
			{
				var viewModel = new MainEventTypeViewModel(eventType);
				_eventVisualDetails[eventType] = viewModel;
				MainEventTypesVisualsOC.Add(viewModel);
			}
		}
	}

	public class MainEventTypeViewModel : BaseViewModel
	{
		// Fields
		private readonly IMainEventType _mainEventType;
		private string _mainEventTitle;
		private bool _isSelected;
		public bool IsSelected
		{
			get => _isSelected;
			set
			{
				_isSelected = value;
				OnPropertyChanged();
			}
		}
		// Properties
		public IMainEventType MainEventType => _mainEventType;
		public string MainEventTitle
		{
			get => _mainEventTitle;
			set { _mainEventTitle = value; OnPropertyChanged(); }
		}
		// Constructor
		public MainEventTypeViewModel(IMainEventType mainEventType)
		{
			_mainEventType = mainEventType;
			MainEventTitle = _mainEventType.Title;
		}
	}
}
