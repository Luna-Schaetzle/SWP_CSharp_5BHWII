//using Android.Systems;
using MAUIBasics.ViewModels;

namespace MAUIBasics.Views;

public partial class Registrierungsformular : ContentPage
{
	public Registrierungsformular(RegistrationPageViewModels vm)
	{
		InitializeComponent();
                this.BindingContext = vm; 
	}

	   private async void OnRegisterClicked(object sender, EventArgs e)
        {
           
        }

        // Formular löschen (Felder leeren)
        private void OnClearClicked(object sender, EventArgs e)
        {

        }
}