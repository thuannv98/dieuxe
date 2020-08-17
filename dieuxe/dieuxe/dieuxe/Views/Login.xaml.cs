using dieuxe.Helpers;
using dieuxe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Login : ContentPage
    {
        public Login(int tabindex = 0)
        {
            InitializeComponent();
            BindingContext = new Login_ViewModels();

            loginButton.SetBinding(Button.CommandParameterProperty, new Binding(tabindex.ToString()));
        }

    }
}