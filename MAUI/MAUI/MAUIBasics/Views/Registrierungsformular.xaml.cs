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

}