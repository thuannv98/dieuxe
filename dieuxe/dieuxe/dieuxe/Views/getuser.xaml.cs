using dieuxe.Helpers;
using dieuxe.Services;
using dieuxe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class getuser : ContentPage
    {
        public Action<ContentPage> PushPage;

        void NavigateToPage(ContentPage page)
        {
            PushPage?.Invoke(page);
        }

        readonly INotificationRegistrationService _notificationRegistrationService;

        public getuser()
        {
            InitializeComponent();
            this.BindingContext = new getuserViewModels();

            _notificationRegistrationService = ServiceContainer.Resolve<INotificationRegistrationService>();
        }
        private async void dangxuat_Clicked(object sender, EventArgs e)
        {
            Settings.AccessToken = "";
            await _notificationRegistrationService.DeregisterDeviceAsync().ContinueWith((task)
                => {
               //     ShowAlert(task.IsFaulted ?
               //task.Exception.Message :
               //$"Device deregistered");
                });
            Application.Current.MainPage = new NavigationPage(new Login());
        }
        private void doimk_Clicked(object sender, EventArgs e)
        {
            NavigateToPage(new ChangePWPage());
        }
        private void xemlichdangky_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new xemlichdadangky());
        }
        private void profile_Clicked(object sender, EventArgs e)
        {
            NavigateToPage(new profile());

        }
        void ShowAlert(string message)
            => MainThread.BeginInvokeOnMainThread(()
                => DisplayAlert("PushDemo", message, "OK").ContinueWith((task)
                    => { if (task.IsFaulted) throw task.Exception; }));
    }
}