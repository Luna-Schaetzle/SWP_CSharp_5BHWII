namespace MAUIBasics.Views;

public partial class Registrierungsformular : ContentPage
{
	public Registrierungsformular()
	{
		InitializeComponent();
	}

	   private async void OnRegisterClicked(object sender, EventArgs e)
        {
            string firstName = FirstNameEntry.Text;
            string lastName = LastNameEntry.Text;
            DateTime birthDate = BirthDatePicker.Date;
            string street = StreetEntry.Text;
            string houseNumber = HouseNumberEntry.Text;
            string postalCode = PostalCodeEntry.Text;
            string city = CityEntry.Text;

            // Validierung
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) ||
                string.IsNullOrWhiteSpace(street) || string.IsNullOrWhiteSpace(houseNumber) ||
                string.IsNullOrWhiteSpace(postalCode) || string.IsNullOrWhiteSpace(city))
            {
                await DisplayAlert("Fehler", "Bitte alle Felder ausfüllen", "OK");
                return;
            }

            // Registrierung erfolgreich
            await DisplayAlert("Erfolg", "Registrierung erfolgreich", "OK");
        }

        // Formular löschen (Felder leeren)
        private void OnClearClicked(object sender, EventArgs e)
        {
            FirstNameEntry.Text = string.Empty;
            LastNameEntry.Text = string.Empty;
            BirthDatePicker.Date = DateTime.Today; // Geburtsdatum auf das heutige Datum zurücksetzen
            StreetEntry.Text = string.Empty;
            HouseNumberEntry.Text = string.Empty;
            PostalCodeEntry.Text = string.Empty;
            CityEntry.Text = string.Empty;
        }
}