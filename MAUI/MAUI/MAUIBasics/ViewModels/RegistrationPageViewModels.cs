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
    public partial class RegistrationPageViewModels : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private const string ApiBaseUrl = "https://localhost:7243/api/User"; // Ändern Sie dies zu Ihrer API-URL
        private const string ApiKey = "2602beb8-013f-4d6b-9451-22c2e549875d";

        public RegistrationPageViewModels()
        {
            _httpClient = new HttpClient
                {
        BaseAddress = new Uri(ApiBaseUrl)
    };

        }

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _name;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _password;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private DateTime _birthdate;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _street;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _houseNumber;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _postalCode;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _city;

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _email;

        [RelayCommand]
        public void ResetRegistrationForm()
        {
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Birthdate = DateTime.Today;
            Street = string.Empty;
            HouseNumber = string.Empty;
            PostalCode = string.Empty;
            City = string.Empty;
        }

        [RelayCommand]
        public async Task GoBackToMain()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand(CanExecute = nameof(ValidateRegistrationForm))]
        public async Task SaveRegister()
        {
            try
            {
                var user = new User
                {
                    Email = Email,
                    Name = Name,
                    Password = Password,
                    Birthdate = Birthdate,
                    Street = Street,
                    HouseNumber = HouseNumber,
                    PostalCode = PostalCode,
                    City = City
                };

                var response = await _httpClient.PostAsJsonAsync($"{ApiBaseUrl}/Register?apiKey={ApiKey}", user);

                if (response.IsSuccessStatusCode)
                {
                    var apiKey = await response.Content.ReadAsStringAsync();
                    // Speichern Sie den API-Key lokal, falls benötigt
                    await Shell.Current.DisplayAlert("Erfolg", "Registrierung erfolgreich!", "OK");
                    ResetRegistrationForm();
                    await GoBackToMain();
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    await Shell.Current.DisplayAlert("Fehler", $"Registrierung fehlgeschlagen: {errorMessage}", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Fehler", $"Registrierung fehlgeschlagen: {ex.Message}", "OK");
            }
        }

        private bool ValidateRegistrationForm()
        {
            if (!string.IsNullOrWhiteSpace(Name) && Name.Trim().Length < 2)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Password) || Password.Trim().Length < 8)
            {
                return false;
            }

            if (Birthdate > DateTime.Today)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Street) || Street.Trim().Length < 2)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(HouseNumber))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(PostalCode) || PostalCode.Trim().Length < 3)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(City) || City.Trim().Length < 2)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                return false;
            }

            return true;
        }
    }
}