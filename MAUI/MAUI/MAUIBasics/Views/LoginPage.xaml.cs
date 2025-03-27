using MAUIBasics.ViewModels;


namespace MAUIBasics.Views{
public partial class LoginPage : ContentPage
{


	public LoginPage(LoginPageViewModels vm)
	{
		InitializeComponent();
		this.BindingContext = vm;
    }


}
}