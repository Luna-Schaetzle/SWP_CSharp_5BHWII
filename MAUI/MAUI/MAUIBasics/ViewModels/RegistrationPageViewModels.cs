using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models.DB; // Namespace für UserContext
using MAUIBasics.Models; // Namespace für User
using Microsoft.Maui.Controls; // Für Shell.Current.DisplayAlert
using BCrypt.Net; // Für BCrypt-Hashing

namespace MAUIBasics.ViewModels
{
    internal partial class RegistrationPageViewModels : ObservableObject
    {
        // Alle Felder der View

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _name;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _password;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private DateTime _birthdate;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _street;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _houseNumber;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _postalCode;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _city;

        // Commands

        [RelayCommand]
        public void ResetRegistrationForm()
        {
            this.Name = string.Empty;
            this.Password = string.Empty;
            this.Birthdate = DateTime.Today;
            this.Street = string.Empty;
            this.HouseNumber = string.Empty;
            this.PostalCode = string.Empty;
            this.City = string.Empty;
        }

        [RelayCommand]
        public async Task GoBackToMain()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        [RelayCommand(CanExecute = nameof(ValidateRegistrationForm))]
        public async Task SaveRegisterAsync()
        {
            try
            {
                // Passwort hashen
                string hashedPassword = HashPassword(this.Password);

                // Benutzerobjekt erstellen
                var user = new User
                {
                    Name = this.Name,
                    PasswordHash = hashedPassword,
                    Birthdate = this.Birthdate,
                    Street = this.Street,
                    HouseNumber = this.HouseNumber,
                    PostalCode = this.PostalCode,
                    City = this.City
                };

                // Datenbank speichern
                using (var context = new UserContext())
                {
                    context.Users.Add(user);
                    await context.SaveChangesAsync();
                }

                // Bestätigung anzeigen
                await Shell.Current.DisplayAlert("Erfolg", "Registrierung erfolgreich!", "OK");

                // Formular zurücksetzen
                ResetRegistrationForm();

                // Zur Hauptseite zurückkehren
                await GoBackToMain();
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                await Shell.Current.DisplayAlert("Fehler", $"Registrierung fehlgeschlagen: {ex.Message}", "OK");
            }
        }

        private bool ValidateRegistrationForm()
        {
            // Kriterien:
            // - Name ist kein Pflichtfeld, aber wenn etwas eingegeben wurde, muss es mindestens 2 Zeichen lang sein.
            // - Password ist ein Pflichtfeld und muss mindestens 8 Zeichen lang sein.
            // - Birthdate ist kein Pflichtfeld, aber wenn etwas eingegeben wurde, muss es in der Vergangenheit liegen.
            // - Street ist ein Pflichtfeld und muss mindestens 2 Zeichen lang sein.
            // - HouseNumber ist ein Pflichtfeld.
            // - PostalCode ist ein Pflichtfeld und muss mindestens 3 Zeichen lang sein.
            // - City ist ein Pflichtfeld und muss mindestens 2 Zeichen lang sein.

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

            return true;
        }

        // Passwort-Hashing-Methode
        private string HashPassword(string password)
        {
            // Verwende eine sichere Hashing-Bibliothek wie BCrypt
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
