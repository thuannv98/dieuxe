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
    public partial class getuser : ContentPage
    {
        public getuser()
        {
            InitializeComponent();
            this.BindingContext = new getuserViewModels();
        }
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Login());
        }
    }
}