//using Android.Hardware.Camera2;
using MAUIBasics.Models.DB;
using MAUIBasics.ViewModels;
using Microsoft.Extensions.Logging;

namespace MAUIBasics
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .RegisterViews()
                .RegisterViewModels()
                .RegisterServices()
                .LoginServices()
                .LoginViews()
                .LoginViewModels()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registrierung von UserContext als Singleton
            builder.Services.AddSingleton<UserContext>();

            // Registrierung von ViewModels
            builder.Services.AddTransient<RegistrationPageViewModels>();
            builder.Services.AddTransient<LoginPageViewModels>();

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddDbContext<UserContext>(ServiceLifetime.Singleton);

            return builder.Build();
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<RegistrationPageViewModels>();
            mauiAppBuilder.Services.AddTransient<LoginPageViewModels>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<RegistrationPageViewModels>();
            mauiAppBuilder.Services.AddTransient<LoginPageViewModels>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<UserContext>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder LoginViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<LoginPageViewModels>();

            return mauiAppBuilder;
        }
        public static MauiAppBuilder LoginServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<UserContext>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder LoginViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<LoginPageViewModels>();

            return mauiAppBuilder;
        }
    }
}
