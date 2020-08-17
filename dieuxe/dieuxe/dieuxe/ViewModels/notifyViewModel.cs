using dieuxe.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using dieuxe.Helpers;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Windows.Input;
using System.Linq;

namespace dieuxe.ViewModels
{
    class notifyViewModel : INotifyPropertyChanged
    {
        private int _count;
        public int count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        private int _countdadi;
        public int countdadi
        {
            get { return _countdadi; }
            set
            {
                _countdadi = value;
                OnPropertyChanged();
            }
        }
        private List<DangKyLichChiTiet> _notify { get; set; }
        public List<DangKyLichChiTiet> notify
        {
            get { return _notify; }
            set
            {
                _notify = value;
                OnPropertyChanged();
            }
        }

        private List<DangKyLichChiTiet> _dslichchuadi { get; set; }
        public List<DangKyLichChiTiet> dslichchuadi
        {
            get { return _dslichchuadi; }
            set
            {
                _dslichchuadi = value;
                OnPropertyChanged();
            }
        }

        private List<DangKyLichChiTiet> _dslichdadi { get; set; }
        public List<DangKyLichChiTiet> dslichdadi
        {
            get { return _dslichdadi; }
            set
            {
                _dslichdadi = value;
                OnPropertyChanged();
            }
        }




        private bool _isRefresh;
        public bool isRefresh { get { return _isRefresh; } set { _isRefresh = value; OnPropertyChanged(); } }
        private bool _check;
        public bool check { get { return _check; } set { _check = value; OnPropertyChanged(); } }
        public notifyViewModel()
        {
            getdata();
            //notify = new List<DangKyLichChiTiet>
            //{
            //    new DangKyLichChiTiet
            //    {
            //        NoiDi="huế",
            //        NoiDen="hải phòng"
            //    },
            //    new DangKyLichChiTiet
            //    {
            //        NoiDen="hà nội",
            //        NoiDi="Hội an"
            //    }
            //};
            //check = true;
            // count = notify.Count;
        }

        public ICommand refreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    isRefresh = true;
                    var accessToken = Settings.AccessToken;
                    await GetNotify("https://apidieuxe.azurewebsites.net/api/Notification", accessToken);
                    isRefresh = false;
                });
            }
        }
        private async Task getdata()
        {
            var accessToken = Settings.AccessToken;
            //while (check == false) {
            //    await GetNotify("https://apidieuxe.azurewebsites.net/api/Notification", accessToken);
            //}
            // Task<List<DangKyLichChiTiet>> asynctask1 =
            await GetNotify("https://apidieuxe.azurewebsites.net/api/Notification", accessToken);
            //asynctask1.Wait();
            if (_notify == null)
            {
                check = false;
            }
            else { check = true; }
        }
        public async Task<List<DangKyLichChiTiet>> GetNotify(string path, string accessToken)
        //public async void GetUserAsync(string path)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.GetAsync(path);
            List<DangKyLichChiTiet> list = new List<DangKyLichChiTiet>();
            System.Diagnostics.Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var st = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    MissingMemberHandling = MissingMemberHandling.Ignore
                };
                list = JsonConvert.DeserializeObject<List<DangKyLichChiTiet>>(content, st);
                notify = list;
                dslichdadi = list.Where(i => i.TrangThai == 2).ToList();
                dslichdadi.Reverse();
                dslichchuadi = list.Where(i => i.TrangThai != 2).ToList();
                //check = true;
                count = dslichchuadi.Count;
                countdadi = dslichdadi.Count;

            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Thông báo", "Tải dữ liệu thất bại, hãy kiêm tra két nối mạng", "OK");
                list = null;
                notify = list;
            }
            return list;
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
