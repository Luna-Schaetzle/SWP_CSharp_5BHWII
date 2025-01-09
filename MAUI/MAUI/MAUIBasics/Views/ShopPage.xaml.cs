using MAUIBasics.ViewModels;
using Microsoft.Maui.Controls;

namespace MAUIBasics.Views
{
    public partial class ShopPage : ContentPage
    {
        public ShopPage(ShopPageViewModel vm)
        {
            InitializeComponent();
            //BindingContext = new ShopViewModel();
            BindingContext = vm;
        }
    }
}