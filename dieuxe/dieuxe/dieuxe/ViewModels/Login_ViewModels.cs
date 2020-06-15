using dieuxe.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using dieuxe.Models;
using dieuxe.Views;

namespace dieuxe.ViewModels
{
     public class Login_ViewModels : INotifyPropertyChanged
    {
       
        private string _tennv { get; set; }
        private string _email { get; set; }
        private string _matkhau { get; set; }
        private string _phone { get; set; }
       
       
        public ICommand LoginCommand
        {
            get
            {
                return new Command(async () =>
                {
                    //var accessToken=await _apiServices.Loginasync(email, password);
                    //Settings.AccessToken = accessToken;
                    
                    
                        var keyValues = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("username",email),
                        new KeyValuePair<string, string>("password",password),
                        new KeyValuePair<string, string>("grant_type","password"),
                    };
                        //var request = new HttpRequestMessage(HttpMethod.Post, "https://apidieuxe20200508212151.azurewebsites.net/token");
                    var request = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44328/token");
                    request.Content = new FormUrlEncodedContent(keyValues);
                        var client = new HttpClient();
                        var response = await client.SendAsync(request);
                        var jwt = await response.Content.ReadAsStringAsync();// wait for response access token
                        JObject jwtDynamic = JsonConvert.DeserializeObject<dynamic>(jwt);
                        var accessToken = jwtDynamic.Value<string>("access_token");
                        Settings.AccessToken = accessToken;
                        Debug.WriteLine(jwt);
                        //response.EnsureSuccessStatusCode();
                        if (response.IsSuccessStatusCode)
                        {
                            await Application.Current.MainPage.DisplayAlert("Notify?", "Login successfully", "OK");
                            await Application.Current.MainPage.Navigation.PushModalAsync(new dangkythongtin());
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert("Error", "Email or password wrong :(", "Again");
                        }
                    
                });
            }

        }
        public Login_ViewModels()
        {
            email = Settings.email;
            password = Settings.password;
        }
        public string username
        {
            get { return _tennv; }
            set
            {
                _tennv = value;
                OnPropertyChanged();
            }
        }
        public string email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }
        public string phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged();
            }
        }
        public string password
        {
            get { return _matkhau; }
            set
            {
                _matkhau = value;
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
