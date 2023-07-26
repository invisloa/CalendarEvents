﻿using CalendarT1.Models;
using CalendarT1.Services.DataOperations.Interfaces;
using CalendarT1.Services.EventsSharing;
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
		builder.Services.AddSingleton<WeeklyEventsViewModel>();
		builder.Services.AddSingleton<DailyEventsViewModel>();
		builder.Services.AddSingleton<MonthlyEventsViewModel>();
		builder.Services.AddSingleton<AbstractEventModel>();
		builder.Services.AddScoped<IEventRepository, LocalMachineEventRepository>();
		builder.Services.AddScoped<IShareEvents, ShareEventsJson>();



		Preferences.Default.Set("ProgramName", "CalendarT1");
		Preferences.Default.Set("JsonFileName", "CalendarEvents");


#if DEBUG
		builder.Logging.AddDebug();
#endif

		//statc mauiapp instance to use it for creating DI
		Current = builder.Build();

		return Current;
	}
}
