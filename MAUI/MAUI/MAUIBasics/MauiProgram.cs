//using Android.Hardware.Camera2;
using MAUIBasics.Models.DB;
using MAUIBasics.Services;
using MAUIBasics.ViewModels;
using MAUIBasics.Views;
using Microsoft.Extensions.DependencyInjection;
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
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Registrierung von UserContext als Singleton
            builder.Services.AddSingleton<UserContext>();


            // Registrierung von ViewModels

#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddDbContext<UserContext>(ServiceLifetime.Singleton);


            return builder.Build();
        }

        public static MauiAppBuilder RegisterViews(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<Registrierungsformular>();
            mauiAppBuilder.Services.AddTransient<LoginPage>();
            mauiAppBuilder.Services.AddTransient<ShopPage>();
            mauiAppBuilder.Services.AddTransient<MainPage>();
            mauiAppBuilder.Services.AddTransient<CartPage>();



            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterViewModels(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddTransient<RegistrationPageViewModels>();
            mauiAppBuilder.Services.AddTransient<LoginPageViewModels>();
            mauiAppBuilder.Services.AddTransient<ShopPageViewModel>();
            mauiAppBuilder.Services.AddTransient<MainPageViewModel>();
            mauiAppBuilder.Services.AddTransient<CartPageViewModel>();

            return mauiAppBuilder;
        }

        public static MauiAppBuilder RegisterServices(this MauiAppBuilder mauiAppBuilder)
        {
            mauiAppBuilder.Services.AddSingleton<UserContext>();
            mauiAppBuilder.Services.AddSingleton<IUserService, UserService>();
            mauiAppBuilder.Services.AddSingleton<ICartService, CartService>();
            return mauiAppBuilder;
        }


    }
}
