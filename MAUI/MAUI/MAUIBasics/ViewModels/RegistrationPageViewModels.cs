using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MAUIBasics.ViewModels
{
    /*
        es muss das NuGet-Paket "CommunityToolkit.Mvvm" installiert werden
            => für die Codegenerierung der Commands und für DataBinding
            => DataBinding ... es wird für uns der Code fpr das Observer-Pattern generiert
        
        diese VM-Klasse muss von ObservableObject abgeleitet sein, alle private-Felder müssen mit 
        [ObservableProperty] annotiert sein, damit die Properties für das DataBinding generiert werden
        alle Command mpssen mit [RelayCommand] annotiert sein, damit die Commands generiert werden

        die VM-Klassen muss partil sein -> da die Klassen auf mehrere Dateien aufgeteilt wird
        - wir programmieren eine Teil, der Rest wird in einer anderen Datei von MVVM generiert
    */

    //partial weil es nur ein Teil der Klasse ist der Rest wird automatisch ergänzt 
    internal partial class RegistrationPageViewModels : ObservableObject
    {
        //alle Felder der View müssen hier hinzugefügt werden

        //[NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))] ... wird benötigt, damit die CanExecute-Methode
        //bei jeder Änderung der Properties aufgerufen wird
        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        //wir geben nur das privat field an
        // => es wird das Property Firstname vom Codegenerator erzeugt 
        // (inkl. Observer-Pattern-Teil)
        [ObservableProperty]
        private string _firstname;

        [NotifyCanExecuteChangedFor(nameof(SaveRegisterCommand))]
        [ObservableProperty]
        private string _lastname;

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



        // LoginPage
        // + LoginPageViewModel + DataBinding + Commands

        //ORM verwenden (für Reg. und Login)

        



        // [RelayCommand] ... damit der Codegenerator die Methode in ein Command umwandelt
        // => wird z.B. von einem Button in der View aufgerufen
        [RelayCommand]
        public void ResetRegistrationForm()
        {
            this.Firstname = "";
            this.Lastname = "";
            this.Birthdate = DateTime.Today;
            this.Street = "";
            this.HouseNumber = "";
            this.PostalCode = "";
            this.City = "";
        }

        //RelayCommands können auch asynchron sein
        [RelayCommand]
        public async Task GoBackToMain()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        //einem RelayCommand kann eine CanExecute-Methode übergeben werden 
        //   liefert die CanExecute-Methode false zurück, wird der Button deaktiviert
        //   liefert die CanExecute-Methode true zurück, wird der Button aktiviert
        //   und man kann die Methode SaveRegister() aufrufen
        [RelayCommand(CanExecute = nameof(ValidateRegistrationForm))]
        public void SaveRegister()
        {
            // 1. Validierung ausführen (siehe canExecute-Methode)
            // 2. In der DB speichern (ORM)
            // 3. Bestätigung anzeigen
            // 4. Zur Hauptseite zurückkehren
        }

        private bool ValidateRegistrationForm()
        {
            //Kriterien:
            //- Firstname ist kein Pflichtfeld, aber wenn etwas eingegeben wurde, muss es mindestens 2 Zeichen lang sein
            //- Lastname ist ein Pflichtfeld
            //- birthdate ist kein Pflichtfeld, aber wenn etwas eingegeben wurde, muss es in der Vergangenheit liegen
            //- Street ist ein Pflichtfeld
            //- HouseNumber ist ein Pflichtfeld
            //- PostalCode ist ein Pflichtfeld
            //- City ist ein Pflichtfeld
        
            if (!string.IsNullOrEmpty(Firstname) && Firstname.Trim().Length < 2)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(Lastname) || Lastname.Trim().Length < 2)
            {
                return false;
            }
            else if (Birthdate > DateTime.Today)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(Street) || Street.Trim().Length < 2)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(HouseNumber))
            {
                return false;
            }
            else if (string.IsNullOrEmpty(PostalCode) || PostalCode.Trim().Length < 3)
            {
                return false;
            }
            else if (string.IsNullOrEmpty(City) || City.Trim().Length < 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        

    }
}
}
