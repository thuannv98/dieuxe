using dieuxe.Models;
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
    public partial class QuanLy : ContentPage
    {
        public QuanLy()
        {
            InitializeComponent();
            BanDo map = new BanDo();
            BindingContext = new QuanLyVM();
        }

        private void chitietdangky(object sender, ItemTappedEventArgs e)
        {
            var itemtapped = e.Item as lich;
            chitietngaydangky.Text = itemtapped.ngaydangky;
            chitietnhanvien.Text = itemtapped.nhanviendangky;
            chitietsonguoi.Text = itemtapped.songuoi.ToString();
            chitietnoidon.Text = itemtapped.noidi;
            chitietnoiden.Text = itemtapped.noiden;
            chitietgioden.Text = itemtapped.gioden;
            chitietgiove.Text = itemtapped.giove;

            popupcontent.IsVisible = true;
            this.popuplayout.AnchorX = 0.5;
            this.popuplayout.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => this.popuplayout.Scale = f,
                0.5,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => this.popuplayout.Opacity = f,
                0.2,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(this.popuplayout, "popupScaleAnimation", 250);
            fadeAnimation.Commit(this.popuplayout, "popupFadeAnimation", 250);
        }

        private void showpopup(object sender, EventArgs e)
        {
            popupcontent.IsVisible = true;
            this.popuplayout.AnchorX = 0.5;
            this.popuplayout.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => this.popuplayout.Scale = f,
                0.5,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => this.popuplayout.Opacity = f,
                0.2,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(this.popuplayout, "popupScaleAnimation", 250);
            fadeAnimation.Commit(this.popuplayout, "popupFadeAnimation", 250);
        }

        private void show(object sender, EventArgs e)
        {
            DisplayAlert("vãi", "đêa", "OKK");
        }

        private async void close(object sender, EventArgs e)
        {
            await Task.WhenAny<bool>(this.popuplayout.FadeTo(0, 200, Easing.SinInOut));
            popupcontent.IsVisible = false;
        }

        private void buttonphantich_Clicked(object sender, EventArgs e)
        {
            List<lich> dsdangky = danhsachdangky.ItemsSource as List<lich>;
            Navigation.PushAsync(new BanDo(dsdangky));
        }
    }
}