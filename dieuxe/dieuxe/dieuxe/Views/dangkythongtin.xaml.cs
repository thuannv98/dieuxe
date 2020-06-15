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
    public partial class dangkythongtin : ContentPage
    {
        public dangkythongtin()
        {
            InitializeComponent();
            this.BindingContext = new dangkylichViewmodel();
        }
        private void dangky(object sender, EventArgs e)
        {
            DisplayAlert("Thông tin", "Đăng ký thành công", "OK");
            DisplayAlert("dsd", ngayden.Date.ToString("yyyy-MM-dd"), "ok");
            DisplayAlert("d", gioden.Time.ToString(), "ok");
            DisplayAlert("ngu nguoi", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "ok");
        }
        private async void Logout_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new Login());
        }
        private void OnTapGestureRecognizerTapped(object sender, EventArgs e)
        {
            DisplayAlert("Thông tin", "Show map", "OK");
        }
    }
}