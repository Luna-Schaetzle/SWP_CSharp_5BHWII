using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MAUIBasics.ViewModels
{
    internal partial class RegistrationPageViewModels : ObservableObject
    {
        [ObservableProperty]
        private string _firstname;







        [RelayCommand]
        public void ResetRegistrationForm()
        {
            this.Firstname = "Max";
        }

    }
}
