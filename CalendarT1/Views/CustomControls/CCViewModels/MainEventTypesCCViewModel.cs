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
	public class MainEventTypesCCViewModel : IMainEventTypesCC
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
		private IconModel _selectedIcon;
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
					UpdateSelectedMainEventTypeVisuals(_selectedMainEventType);
					MainEventTypeChanged?.Invoke(_selectedMainEventType);
				}
			}
		}
		public IconModel SelectedIcon
		{
			get => _selectedIcon;
			set => _selectedIcon = value;
		}

		// Events
		public event Action<IMainEventType> MainEventTypeChanged;

		// Constructor
		public MainEventTypesCCViewModel(List<IMainEventType> mainEventTypesList)
		{
			_mainEventTypesList = mainEventTypesList ?? throw new ArgumentNullException(nameof(mainEventTypesList));
			_eventVisualDetails = new Dictionary<IMainEventType, MainEventTypeViewModel>();

			MainEventTypeSelectedCommand = new RelayCommand<MainEventTypeViewModel>(SetMainEventTypeFromViewModel);
			InitializeMainEventTypesVisuals();
		}

		// Private Methods
		private void SetMainEventTypeFromViewModel(MainEventTypeViewModel viewModel)
		{
			var selectedEventType = _mainEventTypesList.FirstOrDefault(sc => sc.Title == viewModel.MainEventTitle);
			if (selectedEventType == null)
			{
				throw new ArgumentException($"Invalid TypeOfEvent value: {viewModel.MainEventTitle}");
			}
			SelectedMainEventType = selectedEventType;
		}

		private void UpdateSelectedMainEventTypeVisuals(IMainEventType mainEventType)
		{
			DisableVisualsForAllMainEventTypes();

			if (mainEventType != null && _eventVisualDetails.TryGetValue(mainEventType, out var details))
			{
				details.Opacity = FullOpacity;
				details.Border = NoBorderSize;
			}
		}

		public void DisableVisualsForAllMainEventTypes()
		{
			foreach (var eventType in _eventVisualDetails.Values)
			{
				eventType.Opacity = FadedOpacity;
				eventType.Border = BorderSize;
			}
		}

		private void InitializeMainEventTypesVisuals()
		{
			MainEventTypesVisualsOC = new ObservableCollection<MainEventTypeViewModel>();

			foreach (IMainEventType eventType in _mainEventTypesList)
			{
				var viewModel = new MainEventTypeViewModel(eventType, SelectedIcon, BorderSize, FadedOpacity);
				_eventVisualDetails[eventType] = viewModel;
				MainEventTypesVisualsOC.Add(viewModel);
			}

			if (_selectedMainEventType != null)
			{
				UpdateSelectedMainEventTypeVisuals(_selectedMainEventType);
			}
		}
	}

	public class MainEventTypeViewModel : BaseViewModel
	{
		// Fields
		private readonly IMainEventType _mainEventType;
		private string _mainEventTitle;
		private float _opacity;
		private int _border;
		private IconModel _selectedIcon;

		// Properties
		public string MainEventTitle
		{
			get => _mainEventTitle;
			set { _mainEventTitle = value; OnPropertyChanged(); }
		}
		public IconModel SelectedIcon
		{
			get => _selectedIcon;
			set { _selectedIcon = value; OnPropertyChanged(); }
		}
		public float Opacity
		{
			get => _opacity;
			set { _opacity = value; OnPropertyChanged(); }
		}
		public int Border
		{
			get => _border;
			set { _border = value; OnPropertyChanged(); }
		}

		// Constructor
		public MainEventTypeViewModel(IMainEventType mainEventType, IconModel selectedIcon, int borderWidth, float opacity)
		{
			_mainEventType = mainEventType ?? throw new ArgumentNullException(nameof(mainEventType));
			MainEventTitle = _mainEventType.Title;
			SelectedIcon = selectedIcon;
			Border = borderWidth;
			Opacity = opacity;
		}
	}
}
