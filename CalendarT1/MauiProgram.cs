using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
using CalendarT1.ViewModels;
using CalendarT1.ViewModels.EventsViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;

namespace CalendarT1;

public static class MauiProgram
{
	private const string DefaultProgramName = "CalendarT1";
	private const string DefaultJsonEventsFileName = "CalendarEventsD";
	private const string DefaultJsonUserTypesFileName = "CalendarTypesOfEventsD";

	//statc mauiapp instance to use it for creating DI
	public static MauiApp Current { get; private set; }


	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseLocalNotification()
			.UseMauiCommunityToolkit()
			// After initializing the .NET MAUI Community Toolkit, add additional fonts
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
				fonts.AddFont("MaterialIcons-Regular.ttf", "GoogleMaterialFont");
			}); 

		// Interfaces DI Dependency Injection for events repository
		builder.Services.AddSingleton<IEventRepository, LocalMachineEventRepository>();			// events repository DI
		builder.Services.AddScoped<IShareEvents, ShareEventsJson>();
		Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient<AddNewSubTypePageViewModel>(builder.Services);


		// ViewModels register
		// AddSingleton - one instance for all timne
		// AddTransient - new instance every time
		builder.Services.AddTransient<AddNewSubTypePageViewModel>();
		builder.Services.AddTransient<AddNewMainTypePageViewModel>();
		builder.Services.AddTransient<MonthlyEventsViewModel>();
		builder.Services.AddTransient<WeeklyEventsViewModel>();
		builder.Services.AddTransient<DailyEventsViewModel>();
		builder.Services.AddTransient<AllEventsViewModel>();
		builder.Services.AddTransient<ValueTypeCalculationsViewModel>();


		// add event dictionary factories DI
		//builder.Services.AddSingleton(eventFactories);

		// Preferences Setting General Properties
		Preferences.Default.Set("ProgramName", "CalendarT1");
		Preferences.Default.Set("JsonEventsFileName", "CalendarEvents");
		Preferences.Default.Set("JsonSubTypesFileName", "CalendarSubTypesOfEvents");
		Preferences.Default.Set("JsonMainTypesFileName", "CalendarMainTypesOfEvents");


#if DEBUG
		builder.Logging.AddDebug();
#endif

		//statc mauiapp instance to use it for creating DI
		Current = builder.Build();

		return Current;
	}
}
