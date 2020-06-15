using dieuxe.Models;
using dieuxe.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TaiXeView : ContentPage
    {
        public TaiXeView()
        {
            string mataixe = "SK13";
            InitializeComponent();
            BindingContext = new TaiXeVM(mataixe);
            int sochuyenchuadi = 0;
            int sochuyendadi = 0;
            foreach (var item in listchuyenchuadi.ItemsSource)
            {
                sochuyenchuadi++;
            }
            foreach (var item in listchuyendadi.ItemsSource)
            {
                sochuyendadi++;
            }

            LabelThongBaoListChuaDiNull.IsVisible = sochuyenchuadi == 0 ? true : false;
            LabelThongBaoListDaDiNull.IsVisible = sochuyendadi == 0 ? true : false;

            SelectTab(chuyenchuadi);
            UnselectTab(chuyendadi);
        }

        private void tabTapped(object sender, EventArgs e)
        {
            var d = sender as StackLayout;
            var label = d.Children[0] as Label;
            string name = label.Text;
            switch (name)
            {
                case "Chuyến chưa đi":
                    SelectTab(chuyenchuadi);
                    UnselectTab(chuyendadi);
                    tab1.IsVisible = true;
                    tab2.IsVisible = false;
                    break;
                case "Chuyến đã đi":
                    SelectTab(chuyendadi);
                    UnselectTab(chuyenchuadi);
                    tab1.IsVisible = false;
                    tab2.IsVisible = true;
                    break;
                default:
                    SelectTab(chuyenchuadi);
                    UnselectTab(chuyendadi);
                    tab1.IsVisible = true;
                    tab2.IsVisible = false;
                    break;
            }

            //var tabname = tabnames.Children.Cast<StackLayout>().ToList();
            ////var tab = tabs.Children.Cast<StackLayout>().ToList();
            //foreach (var pi in tabname)
            //{
            //    UnselectTab(pi);
            //}

            //if (selectedIndex > -1)
            //{
            //    SelectTab(tabname[selectedIndex]);
            //}
            
        }
        static void UnselectTab(StackLayout tab)
        {
            tab.Opacity = 0.3;
        }

        static void SelectTab(StackLayout tab)
        {
            tab.Opacity = 1.0;
        }

        private async void itemTapped(object sender, ItemTappedEventArgs e)
        {
            var d = e.Item as ChuyenXe;
            await Navigation.PushAsync(new Map(d.tuyenduong));
        }
    }
}