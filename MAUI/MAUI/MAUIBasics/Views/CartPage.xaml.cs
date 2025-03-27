using MAUIBasics.ViewModels;

namespace MAUIBasics.Views;

public partial class CartPage : ContentPage
{
	public CartPage(CartPageViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}