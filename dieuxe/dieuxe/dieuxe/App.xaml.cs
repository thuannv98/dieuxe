using dieuxe.Helpers;
using dieuxe.Services;
using dieuxe.Views;
using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using dieuxe.Models;
using Xamarin.Essentials;

namespace dieuxe
{
    public partial class App : Application
    {
        public static ILocationUpdateService LocationUpdateService;

        public App()
        {
            CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            MapFunction.Mapkey = "";
            //LocationUpdateService = DependencyService.Get<ILocationUpdateService>();
            //LocationUpdateService.LocationChanged += LocationUpdateService_LocationChanged;
            //Settings.AccessToken = null;
            InitializeComponent();
            SetMainPage();
            //MainPage = new dieuxe.Views.Map();
            //MainPage = new NavigationPage(new TaiXeView());

            //MainPage = new NavigationPage(new TaiXeView());

            ServiceContainer.Resolve<IPushDemoNotificationActionService>().ActionTriggered += NotificationActionTriggered;
        }

        private void SetMainPage()
        {
            if (string.IsNullOrEmpty(Settings.AccessToken) || string.IsNullOrEmpty(Settings.LoailienHe))
            // if (string.IsNullOrEmpty(Settings.AccessToken))
            {
                MainPage = new NavigationPage(new Login());
            }
            else
            {
                int loainv = Convert.ToInt32(Settings.LoailienHe);

                MainPage = new NavigationPage(new MainTabbedPage(loainv));

                var a = (MainPage as NavigationPage).RootPage as Login;
                var aa = (MainPage as NavigationPage).RootPage as MainTabbedPage;
            }

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

        void NotificationActionTriggered(object sender, PushDemoAction e) => TakeAction(e);

        void TakeAction(PushDemoAction action) => MainThread.BeginInvokeOnMainThread(()
            =>
            {
                try
                {
                    var Page = (MainPage as NavigationPage).RootPage as MainTabbedPage;
                    if(Page != null)    //mainpage là trang tabbedpage
                        Page.CurrentPage = Page.Children[1];
                    else
                    {
                        // vẫn ở trang login, login xong vào thẳng trang thông báo
                        MainPage = new NavigationPage(new Login(1));
                    }
                }
                catch(Exception e)
                {
                    
                }
            });


        //void TakeAction(PushDemoAction action) => MainThread.BeginInvokeOnMainThread(()
        //    => MainPage?.DisplayAlert("PushDemo", $"{action} action received", "OK").ContinueWith((task)
        //       => { if (task.IsFaulted) throw task.Exception; }));
    }
}

