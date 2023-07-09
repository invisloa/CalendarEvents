namespace CalendarT1;

public partial class App : Application
{
	public App()
	{
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


	}
}
