using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Collections.ObjectModel;
using System.Text;
using System.Runtime.CompilerServices;
using dieuxe.Models;
using dieuxe.Helpers;
using System.Net.Http.Headers;
using Xamarin.Forms;
using System.Windows.Input;

namespace dieuxe.ViewModels
{
    class getuserViewModels : INotifyPropertyChanged
    {
    
        private string _tennv;
        private Users _Users;
        public Users User
        {
            get { return _Users; }
            set
            {
                _Users = value;
                OnPropertyChanged();
            }
        }
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
        public getuserViewModels()
        {
            var accessToken = Settings.AccessToken;
            GetUserAsync("https://localhost:44328/api/user/profile", accessToken);
        }
        public async void GetUserAsync(string path, string accessToken)
        //public async void GetUserAsync(string path)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessToken);
            HttpResponseMessage response = await client.GetAsync(path);

            System.Diagnostics.Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                User = JsonConvert.DeserializeObject<Users>(content);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("error", "you should be login first :( ", "OK");
                User = null;
            }
          
        }
        
        public string tennv
        {
            get { return _tennv; }
            set
            {
                _tennv = value;
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
