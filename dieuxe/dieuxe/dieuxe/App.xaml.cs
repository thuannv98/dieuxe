using dieuxe.Helpers;
using dieuxe.Services;
using dieuxe.Views;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dieuxe
{
    public partial class App : Application
    {
        public static ILocationUpdateService LocationUpdateService;

        public App()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            //LocationUpdateService = DependencyService.Get<ILocationUpdateService>();
            //LocationUpdateService.LocationChanged += LocationUpdateService_LocationChanged;

            InitializeComponent();
            //SetMainPage();
            //MainPage = new dieuxe.Views.Map();
            //MainPage = new NavigationPage(new TaiXeView());

            MainPage = new NavigationPage(new QuanLy());
        }

        private void SetMainPage()
        {
            if (!string.IsNullOrEmpty(Settings.AccessToken))
            {
                MainPage = new NavigationPage(new MainPage());
            }
            else
                MainPage = new NavigationPage(new Login());

        }

        private void LocationUpdateService_LocationChanged(object sender, ILocationEventArgs e)
        {
            //Here you can get the user's location from "e" -> new Location(e.Latitude, e.Longitude);
            //new Location is from Xamarin.Essentials Location object.
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

