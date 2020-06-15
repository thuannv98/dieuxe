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
using dieuxe.Helpers;
using dieuxe.Models;

namespace dieuxe.ViewModels
{
     class xemlichdadangkyViemodel : INotifyPropertyChanged
    {
        private int _sodangky { get; set; }
        private string _ngaydangky { get; set; }
        private int _songuoi { get; set; }
        private string _noidi { get; set; }
        private string _noiden { get; set; }
        private string _gioden { get; set; }
        private string _giove { get; set; }
        private List<lich> _listLich;
        public List<lich> listLich
        {
            get { return _listLich; }
            set
            {
                _listLich = value;
                OnPropertyChanged();
            }
        }
        public xemlichdadangkyViemodel()
        {
            var accessToken = Settings.AccessToken;
            GetlichAsync("https://apidieuxe20200508212151.azurewebsites.net/api/user/lichdangky", accessToken);
        }

        public async void GetlichAsync(string path, string accessToken)
        //public async void GetUserAsync(string path)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.GetAsync(path);

            System.Diagnostics.Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                listLich = JsonConvert.DeserializeObject<List<lich>>(content);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("error", "you should be login first :( ", "OK");
                listLich = null;
            }
        }
        
        public int sodangky
        {
            get { return _sodangky; }
            set
            {
                _sodangky = value; OnPropertyChanged();
            }
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

    }
}
