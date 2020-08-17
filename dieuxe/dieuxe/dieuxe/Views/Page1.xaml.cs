using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using dieuxe.Models;
using Plugin.Geolocator;
using dieuxe.Services;
using Plugin.Permissions.Abstractions;
using Plugin.Geolocator.Abstractions;
using System.Collections.ObjectModel;
using dieuxe.ViewModels;
using dieuxe.CustomControl;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;
using System.Net.Http;
using dieuxe.Helpers;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
	{
		//int count;
		//bool tracking;
		public ObservableCollection<Xamarin.Forms.Maps.Position> Positions { get; } = new ObservableCollection<Xamarin.Forms.Maps.Position>();

        public List<point_detail> list { get; set; }

        readonly INotificationRegistrationService _notificationRegistrationService;

        public Page1()
        {
            //list = new List<point_detail> { new point_detail { thoigianden = "xe1" }, new point_detail { thoigianden = "xe2" } };
            InitializeComponent();
            //this.BindingContext = new ChiTietChuyenDiViewModel();

            //lv.HeightRequest = l.Count * lv.RowHeight;
            //lv.ItemAppearing += Lv_ItemAppearing;
            //lv.SizeChanged += Lv_SizeChanged;


            //ListViewPositions.ItemsSource = Positions;

            //CustomPin pin = new CustomPin
            //{
            //    Type = PinType.Place,
            //    Position = new Xamarin.Forms.Maps.Position(10.832282, 106.778043),
            //    Label = "Xamarin San Francisco Office",
            //    Address = "394 Pacific Ave, San Francisco CA",
            //    Name = "Xamarin",
            //    Url = "http://xamarin.com/about/"
            //};
            //CustomPin pin2 = new CustomPin
            //{
            //    Type = PinType.Place,
            //    Position = new Xamarin.Forms.Maps.Position(10.845966, 106.765224),
            //    Label = "2",
            //    Address = "add 2",
            //    Name = "Xamarin",
            //    Url = "http://xamarin.com/about/"
            //};
            //CustomPin pin3 = new CustomPin
            //{
            //    Type = PinType.Place,
            //    Position = new Xamarin.Forms.Maps.Position(10.868034, 106.780647),
            //    Label = "3",
            //    Address = "add 3",
            //    Name = "Xamarin",
            //    Url = "http://xamarin.com/about/"
            //};
            //customMap.CustomPins = new List<CustomPin> { pin, pin2, pin3 };
            //customMap.CustomPins.First().TrangThai = 0;
            //customMap.Pins.Add(pin);
            //customMap.Pins.Add(pin2);
            //customMap.Pins.Add(pin3);
            //customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Xamarin.Forms.Maps.Position(10.832282, 106.778043), Xamarin.Forms.Maps.Distance.FromMiles(1.0)));

            //ToolbarItem toolbarItem = new ToolbarItem();
            //toolbarItem.Text = "next";
            //toolbarItem.Clicked += ToolbarItem_Clicked;
            //ToolbarItems.Add(toolbarItem);

            _notificationRegistrationService = ServiceContainer.Resolve<INotificationRegistrationService>();

        }

        void RegisterButtonClicked(object sender, EventArgs e)
    => _notificationRegistrationService.RegisterDeviceAsync().ContinueWith((task)
        => {
            ShowAlert(task.IsFaulted ?
               task.Exception.Message :
               $"Device registered");
        });

        void DeregisterButtonClicked(object sender, EventArgs e)
            => _notificationRegistrationService.DeregisterDeviceAsync().ContinueWith((task)
                => {
                    ShowAlert(task.IsFaulted ?
               task.Exception.Message :
               $"Device deregistered");
                });

        void ShowAlert(string message)
            => MainThread.BeginInvokeOnMainThread(()
                => DisplayAlert("PushDemo", message, "OK").ContinueWith((task)
                    => { if (task.IsFaulted) throw task.Exception; }));

        //int index = 0;
        //private void ToolbarItem_Clicked(object sender, EventArgs e)
        //{
        //    if(index >= customMap.CustomPins.Count)
        //    {
        //        DisplayAlert("S", "end", "ok");
        //        return;
        //    }    
        //    customMap.CustomPins[index].TrangThai = -1;
        //    customMap.CustomPins[index + 1].TrangThai = 0;
        //    index++;
        //}

        private void Button_Clicked(object sender, EventArgs e)
        {
            //foreach (ThongTinDiemDung item in lv.ItemsSource)
            //{

            //    //Console.WriteLine(item.selected == null ? "null" : item.selected.bienkiemsoat);
            //}
        }

        private async void guitb(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://pushnotificationapp.azurewebsites.net/");
            client.DefaultRequestHeaders.Add("apikey", "apikey");

            string jsonData = @"{""text"": ""Message from Postman!"", ""action"": ""action_b""}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/notifications/requests", content);

            // this result string should be something like: "{"token":"rgh2ghgdsfds"}"
            var result = await response.Content.ReadAsStringAsync();

        }

        private async void guitbquang(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://pushnotificationapp.azurewebsites.net/");
            client.DefaultRequestHeaders.Add("apikey", "apikey");

            string jsonData = @"{""text"": ""Message from Postman!"", ""action"": ""action_b"", ""tags"": [""quang@gmail.com""]}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/notifications/requests", content);
        }
        private async void guitbthuan(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://pushnotificationapp.azurewebsites.net/");
            client.DefaultRequestHeaders.Add("apikey", "apikey");

            string jsonData = @"{""text"": ""Message from Postman!"", ""action"": ""action_b"", ""tags"": [""thuan@gmail.com""]}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/notifications/requests", content);
        }
        private async void guitbt(object sender, EventArgs e)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://pushnotificationapp.azurewebsites.net/");
            client.DefaultRequestHeaders.Add("apikey", "apikey");

            string jsonData = @"{""text"": ""Message from Postman!\nXin cám ơn"", ""action"": ""action_b"", ""tags"": [""t@gmail.com""]}";
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/notifications/requests", content);
        }
        //double h = 0;
        //private void Lv_SizeChanged(object sender, EventArgs e)
        //{
        //    h = lv.Height;
        //}

        //private void Lv_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    DisplayAlert("D", lv.RowHeight.ToString(), "C");
        //}

        //private void Button_Clicked(object sender, EventArgs e)
        //{
        //    lv.HeightRequest = h;
        //}


        //private async void ButtonTrack_Clicked(object sender, EventArgs e)
        //{
        //	try
        //	{
        //		var hasPermission = await Utils.CheckPermissions(Permission.Location);
        //		if (!hasPermission)
        //			return;

        //		if (tracking)
        //		{
        //			CrossGeolocator.Current.PositionChanged -= CrossGeolocator_Current_PositionChanged;
        //			CrossGeolocator.Current.PositionError -= CrossGeolocator_Current_PositionError;
        //		}
        //		else
        //		{
        //			CrossGeolocator.Current.PositionChanged += CrossGeolocator_Current_PositionChanged;
        //			CrossGeolocator.Current.PositionError += CrossGeolocator_Current_PositionError;
        //		}

        //		if (CrossGeolocator.Current.IsListening)
        //		{
        //			await CrossGeolocator.Current.StopListeningAsync();
        //			labelGPSTrack.Text = "Stopped tracking";
        //			ButtonTrack.Text = "Start Tracking";
        //			tracking = false;
        //			count = 0;
        //		}
        //		else
        //		{
        //			Positions.Clear();
        //			if (await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(TrackTimeout.Value), TrackDistance.Value,
        //				TrackIncludeHeading.IsToggled, new ListenerSettings
        //				{
        //					ActivityType = (ActivityType)ActivityTypePicker.SelectedIndex,
        //					AllowBackgroundUpdates = AllowBackgroundUpdates.IsToggled,
        //					DeferLocationUpdates = DeferUpdates.IsToggled,
        //					DeferralDistanceMeters = DeferalDistance.Value,
        //					DeferralTime = TimeSpan.FromSeconds(DeferalTIme.Value),
        //					ListenForSignificantChanges = ListenForSig.IsToggled,
        //					PauseLocationUpdatesAutomatically = PauseLocation.IsToggled
        //				}))
        //			{
        //				labelGPSTrack.Text = "Started tracking";
        //				ButtonTrack.Text = "Stop Tracking";
        //				tracking = true;
        //			}
        //		}
        //	}
        //	catch (Exception ex)
        //	{
        //		await DisplayAlert("Uh oh", "Something went wrong, but don't worry we captured for analysis! Thanks.", "OK");
        //	}
        //}

        //void CrossGeolocator_Current_PositionChanged(object sender, PositionEventArgs e)
        //{

        //	Device.BeginInvokeOnMainThread(() =>
        //	{
        //		var position = e.Position;
        //		Positions.Add(position);
        //		count++;
        //		LabelCount.Text = $"{count} updates";
        //		labelGPSTrack.Text = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
        //			position.Timestamp, position.Latitude, position.Longitude,
        //			position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

        //	});
        //}

        //void CrossGeolocator_Current_PositionError(object sender, PositionErrorEventArgs e)
        //{
        //	//labelGPSTrack.Text = "Location error: " + e.Error.ToString();
        //}
    }


    //public ICommand PostCommand
    //{
    //    get
    //    {
    //        return new Command<List<DieuXe>>(async (postlich) =>
    //        {
    //            var accessToken = Settings.AccessToken;
    //            var client = new HttpClient();
    //            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
    //            var json = JsonConvert.SerializeObject(postlich);
    //            HttpContent content = new StringContent(json);
    //            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    //            var response = await client.PutAsync("https://apidieuxe20200508212151.azurewebsites.net/api/guilichdieuxe/", content);
    //            if (response.IsSuccessStatusCode)
    //            {
    //                await Application.Current.MainPage.DisplayAlert("Notify?", "cập nhật thành công !", "OK");
    //                await Application.Current.MainPage.Navigation.PushAsync(new xemlichdadangky());
    //            }
    //            else
    //            {
    //                await Application.Current.MainPage.DisplayAlert("Error", "Delete failed 😞", "Again");
    //            }
    //        });
    //    }
    //}
    //private void submit_Clicked(object sender, EventArgs e)
    //{
    //    var button = sender as Button;
    //    var lich = button.CommandParameter as List<DangKyLichChiTiet>;
    //    var vm = BindingContext as xemlichdadangkyViemodel;
    //    vm.PostCommand.Execute(lich);


    //    //if (selectedItem != null)
    //    //{
    //    //    Debug.WriteLine("name:>." + selectedItem.SelectItem);
    //    //}
    //    foreach (lich i in xemlich.ItemsSource)
    //    {
    //        Debug.WriteLine(i.SelectItem == null ? "null" : "giá trị " + i.SelectItem.TenLienHe);
    //    }

    //}

}