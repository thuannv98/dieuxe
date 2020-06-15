using dieuxe.ViewModels;
using dieuxe.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace dieuxe
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            DisplayCurLocation();
            this.mahoadiachi();
            map.IsShowingUser = true;
            this.BindingContext = new dangkylichViewmodel();
        }
        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
           System.Diagnostics.Debug.WriteLine($"MapClick:{e.Position.Latitude}, {e.Position.Longitude}");
            var position = new Position(e.Position.Latitude, e.Position.Longitude);
            var pin =new Pin
         {
                Type = PinType.Place,
                Position = position,
                Label = "ngu người",
                Address = "ngu người 1 "
            };
        }
        public async void DisplayCurLocation()
        {
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                var location = await Geolocation.GetLocationAsync(request);



                if (location != null)
                {
                    Position p = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(.444));
                    map.MoveToRegion(mapSpan);
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
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
        public async void mahoadiachi()
        {
            try
            {
                var address = "Microsoft Building 25 Redmond WA USA";
                var locations = await Geocoding.GetLocationsAsync(address);

                var location = locations?.FirstOrDefault();
                //if (location != null)
                //{
                //   await DisplayAlert("latitude:"+location.Latitude, "longtitude:"+location.Longitude, "OK");
                //}
                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Feature not supported on device
            }
            catch (Exception ex)
            {
                // Handle exception that may have occurred in geocoding
            }
        }

        private async void xemlichdangky_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new xemlichdadangky());
        }

        private async void dangkilich_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new dangkythongtin());
        }

        private async void profile_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new getuser());
        }

        private async void Map_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.Map());
        }
    }
}
