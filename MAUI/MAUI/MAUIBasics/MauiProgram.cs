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


            return builder.Build();
        }
    }
}
