using CalendarT1.Services.DataOperations.Interfaces;
using System.Globalization;

namespace CalendarT1;

public partial class App : Application
{
	private readonly IEventRepository _repository;

	public App(IEventRepository repository)
	{
		_repository = repository;
/*		_repository.ClearAllMainEventTypesAsync();
		_repository.ClearAllEventsListAsync();
		_repository.ClearAllSubEventTypesAsync();*/


		InitializeComponent();
		MainPage = new AppShell();

	}
	protected override async void OnStart()
	{
		// Call base method 
		base.OnStart();


		// Check or request StorageRead permission
		var statusStorageRead = await Permissions.CheckStatusAsync<Permissions.StorageRead>();
		if (statusStorageRead != PermissionStatus.Granted)
		{
			statusStorageRead = await Permissions.RequestAsync<Permissions.StorageRead>();
		}
		var statusStorageWrite = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
		if (statusStorageWrite != PermissionStatus.Granted)
		{
			statusStorageWrite = await Permissions.RequestAsync<Permissions.StorageRead>();
		}

		// load repository data OnStart of the app
		await _repository.InitializeAsync();



		// TODO !!!!!!!!!!!!!!!!!!!!!!!!  IF THERE ARE NO ITEMS IN THE REPOSITORY, ADD SOME DEFAULT ITEMS
/*		if (_repository.AllMainEventTypesList.Count == 0)
		{
			_repository.AddEventAsync;
		}
		if (_repository.AllEventsList.Count == 0)
		{
			_repository.LoadEventsAndTypesFromFile();
		}
*/	}
}
