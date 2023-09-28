using CalendarT1.Services.DataOperations.Interfaces;

namespace CalendarT1;

public partial class App : Application
{
	private readonly IEventRepository _repository;

	public App(IEventRepository repository)
	{
		_repository = repository;
		//repository.ClearAllUserTypesAsync();
		//repository.ClearEventsListAsync();
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
	}
}
