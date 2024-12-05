namespace MAUIBasics.Views;
using MAUIBasics.ViewModels;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModels vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
    }


}