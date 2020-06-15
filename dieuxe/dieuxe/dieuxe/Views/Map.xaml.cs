using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using dieuxe.ViewModels;
using System.Globalization;
using Xamarin.Essentials;
using Xamarin.Forms.Maps;
using dieuxe.Helpers;
using Map = dieuxe.Views.Map;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Map : ContentPage
    {
        public Map()
        {

            InitializeComponent();
            DisplayCurLocation();

            map.IsShowingUser = true;
            QuanLyVM q = new QuanLyVM(map);
            //QuanLyVM q = new QuanLyVM();
            BindingContext = q;
            //System.Console.WriteLine("DEBUG - Button Clicked!: " + q.response);
        }

        public Map(string tuyenduong)
        {
            InitializeComponent();
            DisplayCurLocation();

            map.IsShowingUser = true;
            QuanLyVM q = new QuanLyVM(map);
            //QuanLyVM q = new QuanLyVM();
            BindingContext = q;


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