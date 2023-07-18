using CalendarT1.Services.DataOperations.Interfaces;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace CalendarT1;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			// After initializing the .NET MAUI Community Toolkit, optionally add additional fonts
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddTransient<IEventRepository, LocalMachineEventRepository>();
		Preferences.Default.Set("ProgramName", "CalendarT1");


#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
