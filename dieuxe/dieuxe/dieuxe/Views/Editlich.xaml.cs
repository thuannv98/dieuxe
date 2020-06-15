using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using dieuxe.ViewModels;
using dieuxe.Models;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Editlich : ContentPage
    {
        public Editlich(lich lich)
        {
            InitializeComponent();
            var editlichViewmodel = new editlichViewmodel();
            editlichViewmodel.Lich = lich;
            BindingContext = editlichViewmodel;
        }
        private void sualich_Clicked(object sender, EventArgs e)
        {

        }
        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            DisplayAlert("Thông tin", "Show map", "OK");
        }
    }
}
