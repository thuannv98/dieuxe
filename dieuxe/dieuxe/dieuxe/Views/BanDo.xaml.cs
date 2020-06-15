using dieuxe.Helpers;
using dieuxe.Models;
using dieuxe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Distance = Xamarin.Forms.Maps.Distance;
using Polyline = Xamarin.Forms.Maps.Polyline;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BanDo : ContentPage
    {
        public BanDo()
        {
            InitializeComponent();

            map.IsShowingUser = true;
            DisplayCurLocation();
        }

        public BanDo(string tuyenduong)
        {
            InitializeComponent();

            map.IsShowingUser = true;
            DisplayCurLocation();


            var listPoint = MapFunction.Decode(tuyenduong);
            Polyline polyline = new Polyline
            {
                StrokeWidth = 8,
                StrokeColor = Color.FromHex("#0000FF"),
                //FillColor = Color.FromHex("#881BA1E2"),
                Geopath = { }
            };
            foreach (Models.Coordinate point in listPoint)
            {
                polyline.Geopath.Add(new Position(point.Latitude, point.Longitude));
            }
            map.MapElements.Add(polyline);

        }

        public BanDo(List<lich> danhsachdangky)
        {
            InitializeComponent();

            Button buttonPhantich = new Button();
            //buttonPhantich.Text = "Bắt đầu phân tích";
            //buttonPhantich.SetBinding(Button.CommandProperty, "phantichcum");

            //(Content as StackLayout).Children.Add(buttonPhantich);

            ToolbarItem toolbarItem = new ToolbarItem();
            toolbarItem.Text = "Bắt đầu phân tích";
            toolbarItem.SetBinding(ToolbarItem.CommandProperty, "phantichcum");
            ToolbarItems.Add(toolbarItem);

            QuanLyVM q = new QuanLyVM(map);
            BindingContext = q;
        }

        public async void DisplayCurLocation()
        {
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);



                if (location != null)
                {
                    Position p = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(.444));

                    //Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                }
                else
                {
                    await DisplayAlert("Permission Denied", "dell kết nối đc gps", "OK");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
    }
}