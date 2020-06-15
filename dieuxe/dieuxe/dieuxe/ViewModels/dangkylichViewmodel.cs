using dieuxe.Helpers;
using dieuxe.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace dieuxe.ViewModels
{
    class dangkylichViewmodel : INotifyPropertyChanged
    {
        private string _ngaydangky { get; set; }
        private int _songuoi { get; set; }
        private string _noidi { get; set; }
        private string _noiden { get; set; }
        private string _gioden { get; set; }
        private string _giove { get; set; }
        
        public ICommand dangkylichCommand
        {
            get
            {
                return new Command(async () =>
                {
                    var keyValues = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("ngaydangky",DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        new KeyValuePair<string, string>("noidi",noidi),
                        new KeyValuePair<string, string>("noiden",noiden),
                        new KeyValuePair<string, string>("gioden",gioden),
                        new KeyValuePair<string, string>("giove",giove),
                        new KeyValuePair<string, string>("songuoi",songuoi.ToString()),
                    };
                    var accessToken = Settings.AccessToken;
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://apidieuxe20200508212151.azurewebsites.net/api/lichdangki");
                    request.Content = new FormUrlEncodedContent(keyValues);
                    var client = new HttpClient();
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    var response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Notify?", "đăng ký thành công", "OK");
                       // await Application.Current.MainPage.Navigation.PushModalAsync(new dangkythongtin());
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "đăng ký thất bại :(", "Again");
                    }

                });
            }

        }

        public dangkylichViewmodel()
        {
          
        }
        public string ngaydangky
        {
            get { return _ngaydangky; }
            set
            {
                _ngaydangky = value;
                OnPropertyChanged();
            }
        }
        public int songuoi
        {
            get { return _songuoi; }
            set
            {
                _songuoi = value;
                OnPropertyChanged();
            }
        }
        public string noidi
        {
            get { return _noidi; }
            set
            {
                _noidi = value;
                OnPropertyChanged();
            }
        }
        public string noiden
        {
            get { return _noiden; }
            set
            {
                _noiden = value;
                OnPropertyChanged();
            }
        }
        public string giove
        {
            get { return _giove; }
            set
            {
                _giove = value;
                OnPropertyChanged();
            }
        }
        public string gioden
        {
            get { return _gioden; }
            set
            {
                _gioden = value;
                OnPropertyChanged();
            }
        }



        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    
    public ICommand LogoutCommand
        {
            get
            {
                return new Command(async () =>
                {
                    Settings.AccessToken = "";
                });

            }
        }
    }
}
