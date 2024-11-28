using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MAUIBasics.Models.DB; // Namespace für UserContext
using MAUIBasics.Models; // Namespace für User
using Microsoft.Maui.Controls; // Für Shell.Current.DisplayAlert
using Microsoft.EntityFrameworkCore; // Für EF Core Erweiterungsmethoden wie FirstOrDefaultAsync
using BCrypt.Net; // Für BCrypt-Hashing

namespace MAUIBasics.ViewModels
{
    internal partial class LoginPageViewModels : ObservableObject
    {
        private readonly UserContext _userContext;

        // Konstruktor mit Dependency Injection
        /*
        public LoginPageViewModels(UserContext userContext)
        {
            _userContext = userContext;
        }
        */

        // Alle Felder der View

        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty]
        private string _name;

        [NotifyCanExecuteChangedFor(nameof(LoginCommand))]
        [ObservableProperty]
        private string _password;

        // Commands

        /// <summary>
        /// Command zum Zurücksetzen des Login-Formulars.
        /// </summary>
        [RelayCommand]
        public void ResetLoginForm()
        {
            this.Name = string.Empty;
            this.Password = string.Empty;
        }

        /// <summary>
        /// Command zum Ausführen des Logins.
        /// Dieser Command ist nur ausführbar, wenn das Formular gültig ist.
        /// </summary>
        [RelayCommand(CanExecute = nameof(ValidateLoginForm))]
        public async Task LoginAsync()
        {
            try
            {
                // Suche den Benutzer in der Datenbank
                var user = await _userContext.Users.FirstOrDefaultAsync(u => u.Name.Equals(this.Name, StringComparison.OrdinalIgnoreCase));

                if (user == null)
                {
                    // Benutzername nicht gefunden
                    await Shell.Current.DisplayAlert("Fehler", "Benutzername existiert nicht.", "OK");
                    return;
                }

                // Überprüfe das Passwort
                bool isPasswordValid = VerifyPassword(this.Password, user.PasswordHash);

                if (!isPasswordValid)
                {
                    // Passwort ist falsch
                    await Shell.Current.DisplayAlert("Fehler", "Falsches Passwort.", "OK");
                    return;
                }

                // Erfolgreicher Login
                await Shell.Current.DisplayAlert("Erfolg", "Login erfolgreich!", "OK");

                // Formular zurücksetzen
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
        /// <returns>True, wenn das Formular gültig ist; andernfalls False.</returns>
        private bool ValidateLoginForm()
        {
            return !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Password);
        }

        /// <summary>
        /// Methode zur Überprüfung des Passworts mit dem gehashten Passwort.
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
