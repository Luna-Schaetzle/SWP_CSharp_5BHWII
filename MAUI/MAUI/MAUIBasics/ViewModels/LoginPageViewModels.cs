using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models;
using Microsoft.Maui.Controls;
using MAUIBasics.Services;

namespace MAUIBasics.ViewModels
{
    public partial class LoginPageViewModels : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;

        private const string ApiBaseUrl = "https://localhost:7243/api/User"; // Ändern Sie dies zu Ihrer API-URL
        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";
        public LoginPageViewModels(IUserService userService)
        {
            _httpClient = new HttpClient();
            _userService = userService;

        }

        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty]
        private string _name;

        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty]
        private string _password;

        [RelayCommand]
        public void ResetLoginForm()
        {
            Name = string.Empty;
            Password = string.Empty;
        }

        [RelayCommand(CanExecute = nameof(ValidateLoginForm))]
        public async Task LoginAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"{ApiBaseUrl}/Login?name={Uri.EscapeDataString(Name)}&password={Uri.EscapeDataString(Password)}&apiKey={ApiKey}");

                if (response.IsSuccessStatusCode)
                {
                    var user = await response.Content.ReadFromJsonAsync<User>();

                    if (user != null)
                    {
                        // Debugging-Ausgabe hinzufügen
                        Console.WriteLine($"Login successful for user: {user.Name}");

                        _userService.CurrentUser = user; // Hier wird der User gesetzt

                        await Shell.Current.DisplayAlert("Erfolg", $"Login erfolgreich! Mit Name: {user.Name}", "OK");
                        ResetLoginForm();
                        //_userService.CurrentUser = user;
                        //await Task.Delay(100); // Kurze Verzögerung
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Fehler", "Ungültige Antwort vom Server", "OK");
                    }
                }
                // ... Rest des Codes bleibt gleich ...
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Login fehlgeschlagen: {ex.Message}", "OK");
            }
        }

        private bool ValidateLoginForm()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password);
        }
    }
}