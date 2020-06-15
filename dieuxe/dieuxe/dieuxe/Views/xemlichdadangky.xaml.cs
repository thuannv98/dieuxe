using dieuxe.ViewModels;
using dieuxe.Models;
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
    public partial class xemlichdadangky : ContentPage
    {
        public xemlichdadangky()
        {
            InitializeComponent();
            this.BindingContext = new xemlichdadangkyViemodel();
        }
        //public xemlichdadangky(lich lich)
        //{
        //    var xemlichviewmodel = new xemlichdadangkyViemodel();
        //    xemlichviewmodel.Lich = lich;
        //    BindingContext = xemlichviewmodel;
        //    InitializeComponent();
        //    //this.BindingContext = new xemlichdadangkyViemodel();
        //}

        private async void sualich_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var lich = e.Item as lich;
            await Navigation.PushAsync(new Editlich(lich));
        }
    }
}