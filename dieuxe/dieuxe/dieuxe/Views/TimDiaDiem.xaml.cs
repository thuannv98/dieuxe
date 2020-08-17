using dieuxe.Models;
using dieuxe.Services;
using dieuxe.ViewModels;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
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
    public partial class TimDiaDiem : ContentPage
    {
        public TimDiaDiem()
        {
            InitializeComponent();
        }
        dangkylichViewmodel dklvm;
        readonly string type;
        public BanDo Map1 { get; set; }
        public TimDiaDiem(string type, dangkylichViewmodel vm, BanDo banDo)
        {
            InitializeComponent();
            dklvm = vm;
            BindingContext = dklvm;
            this.type = type;
            Map1 = banDo;
            Title = type == "noidi" ? "Chọn nơi đi" : "Chọn nơi đến";
            if(!dklvm.DaLayLichSu)
                dklvm.LayDuLieu();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            entryTimKiem.Focus();

            content.Children.Add(kq, null, Constraint.RelativeToView(stackChonDiaDiem, (parent, sibling) =>
            {
                return sibling.Y + sibling.Height + 20;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width ;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height - kq.Y;
            }));

            content.Children.Add(frameLichSu, null, Constraint.RelativeToView(stackChonDiaDiem, (parent, sibling) =>
            {
                return sibling.Y + sibling.Height + 20;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height - frameLichSu.Y;
            }));
        }

        private async void thanhtimkiem_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(entryTimKiem.Text))
            {
                kq.IsVisible = false;
                frameLichSu.IsVisible = true;
            }
            else
            {
                frameLichSu.IsVisible = false;
                await dklvm.GetPlacesPredictionsAsync();
                kq.IsVisible = true;
            }
        }

        private void Listkq_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            var diachi = (e.Item as placesearchdetail.AddressInfo).Address;
            HienThi(diachi);
            kq.IsVisible = false;
        }

        private void lvLichSu_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            var diachi = ((LichSuTimKiem)e.Item).diadiem ;  ///
            HienThi(diachi);
        }
        async void HienThi(string diachi)
        {
            if (type == "noidi")
            {
                dklvm.NoiDi = diachi;
                dklvm.LuuLichSu(diachi);
                await Navigation.PopAsync();
            }
            else if (type == "noiden")
            {
                dklvm.NoiDen = diachi;
                dklvm.LuuLichSu(diachi);
                await Navigation.PopAsync();
            }
            listkq.IsVisible = false;
        }

        private async void LayViTriHienTai_Clicked(object sender, EventArgs e)
        {
            var hasPermission = await Utils.CheckPermissions(Permission.Location);
            if (!hasPermission)
                return;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();

            var diachi = await dklvm.GetAddressAsync(position.Latitude, position.Longitude);
            HienThi(diachi);
        }

        private void ChonTrenBanDo_Clicked(object sender, EventArgs e)
        {
            Map1.Loai = type;
            Navigation.PushAsync(Map1);

            MessagingCenter.Subscribe<BanDo, string>(this, "Đã chọn địa điểm", (s, arg) =>
            {
                dklvm.LuuLichSu(arg);
                Navigation.PopAsync();
            });
        }

        private void delete_text(object sender, EventArgs e)
        {
            entryTimKiem.Text = "";
            entryTimKiem.Focus();
        }

        private async void XoaLichSu(object sender, EventArgs e)
        {
            var option = await DisplayAlert("Thông báo", "Xóa địa điểm này ?", "Xóa", "Hủy");
            if (option)
            {
                var button = sender as ImageButton;
                var lichsu = button.BindingContext as LichSuTimKiem;

                dklvm.XoaLichSu.Execute(lichsu);
            }
        }
    }
}