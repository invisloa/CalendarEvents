using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventFactories;
using CalendarT1.Services.EventsSharing;
using CalendarT1.ViewModels;
using CalendarT1.ViewModels.EventsViewModels;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace CalendarT1;

public static class MauiProgram
{

	//statc mauiapp instance to use it for creating DI
	public static MauiApp Current { get; private set; }


	public static MauiApp CreateMauiApp()
	{
		// event factories dictionary DI Factory 
		var eventFactories = new Dictionary<string, IBaseEventFactory>
		{
			{ "Event", new EventModelFactory() },
			{ "Spending", new SpendingModelFactory() },
			{ "Task", new TaskModelFactory() },
			//... add more factories
		};


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

		// Interfaces DI
		builder.Services.AddScoped<IEventRepository, LocalMachineEventRepository>();
		builder.Services.AddScoped<IShareEvents, ShareEventsJson>();
		Microsoft.Extensions.DependencyInjection.ServiceCollectionServiceExtensions.AddTransient<AddNewTypePageViewModel>(builder.Services);


		// Pages 
		builder.Services.AddTransient<AddNewTypePageViewModel>();
		builder.Services.AddSingleton<MonthlyEventsViewModel>();
		builder.Services.AddSingleton<WeeklyEventsViewModel>();
		builder.Services.AddSingleton<DailyEventsViewModel>();


		// add event dictionary factories DI
		builder.Services.AddSingleton(eventFactories);






		// Preferences Setting General Properties
		Preferences.Default.Set("ProgramName", "CalendarT1");
		Preferences.Default.Set("JsonEventsFileName", "CalendarEvents");
		Preferences.Default.Set("JsonUserTypesFileName", "CalendarTypesOfEvents");


#if DEBUG
		builder.Logging.AddDebug();
#endif

		//statc mauiapp instance to use it for creating DI
		Current = builder.Build();

		return Current;
	}
}
