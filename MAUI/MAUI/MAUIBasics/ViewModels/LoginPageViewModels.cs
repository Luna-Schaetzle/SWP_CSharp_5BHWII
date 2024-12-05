using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models.DB; // Namespace f�r UserContext
using MAUIBasics.Models; // Namespace f�r User
using Microsoft.Maui.Controls; // F�r Shell.Current.DisplayAlert
using Microsoft.EntityFrameworkCore; // F�r EF Core Erweiterungsmethoden wie FirstOrDefaultAsync
using BCrypt.Net; // F�r BCrypt-Hashing

namespace MAUIBasics.ViewModels
{
    public partial class LoginPageViewModels : ObservableObject
    {
    

        // Konstruktor mit Dependency Injection
        // Konstruktor mit Dependency Injection

        // Alle Felder der View

        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty]
        private string _name;

        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty]
        private string _password;

        // Commands

        /// <summary>
        /// Command zum Zur�cksetzen des Login-Formulars.
        /// </summary>
        [RelayCommand]
        public void ResetLoginForm()
        {
            this.Name = string.Empty;
            this.Password = string.Empty;
        }

        /// <summary>
        /// Command zum Ausf�hren des Logins.
        /// Dieser Command ist nur ausf�hrbar, wenn das Formular g�ltig ist.
        /// </summary>
        [RelayCommand(CanExecute = nameof(ValidateLoginForm))]
        public async Task LoginAsync()
        {
            try
            {
                //Datenbank Abfrage
                using var _userContext = new UserContext();

                // Suche den Benutzer in der Datenbank
                //var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));
                var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Name.Equals(this.Name));
                if (user == null)
                {
                    // Benutzername nicht gefunden
                    await Shell.Current.DisplayAlert("Fehler", "Benutzername existiert nicht.", "OK");
                    return;
                }

                // �berpr�fe das Passwort
                bool isPasswordValid = VerifyPassword(this.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    // Passwort ist falsch
                    await Shell.Current.DisplayAlert("Fehler", "Falsches Passwort.", "OK");
                    return;
                }

                // Erfolgreicher Login
                await Shell.Current.DisplayAlert("Erfolg", "Login erfolgreich! Mit Name: " + user.Name, "OK");

                // Formular zur�cksetzen
                ResetLoginForm();

                // Navigation zur Hauptseite oder einer anderen Seite
                await Shell.Current.GoToAsync("//MainPage");
            }
            catch (Exception ex)
            {
                // Fehlerbehandlung
                await Shell.Current.DisplayAlert("Fehler", $"Login fehlgeschlagen: {ex.Message}", "OK");
            }
        }

        /// <summary>
        /// Validierung des Login-Formulars.
        /// </summary>
        /// <returns>True, wenn das Formular g�ltig ist; andernfalls False.</returns>
        private bool ValidateLoginForm()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password);
        }

        /// <summary>
        /// Methode zur �berpr�fung des Passworts mit dem gehashten Passwort.
        /// </summary>
        /// <param name="password">Das eingegebene Passwort.</param>
        /// <param name="hashedPassword">Das gehashte Passwort aus der Datenbank.</param>
        /// <returns>True, wenn das Passwort korrekt ist; andernfalls False.</returns>
        private bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
