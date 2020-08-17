﻿using dieuxe.CustomControl;
using dieuxe.Helpers;
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
        public Action<ContentPage> PushPage;

        void NavigateToPage(ContentPage page)
        {
            PushPage?.Invoke(page);
        }

        TaiXeVM vm;
        public TaiXeView()
        {
            InitializeComponent();
            vm = new TaiXeVM();
            this.BindingContext = vm;

            //while (vm.checkdata == true)
            //{
            //    LabelThongBaoListChuaDiNull.IsVisible = vm.DanhSachChuyenChuaDi.Count == 0 ? true : false;
            //    LabelThongBaoListDaDiNull.IsVisible = vm.DanhSachChuyenDaDi.Count == 0 ? true : false;
            //}
            ten.Text = Settings.TenLienHe;
            SelectTab(chuyenchuadi);
            UnselectTab(chuyendadi);
            hrChuyendadi.IsVisible = false;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            noidung.Children.Add(tabs, Constraint.RelativeToParent(parent =>
            {
                return parent.X;
            }), Constraint.RelativeToView(tabnames, (parent, sibling) =>
            {
                return sibling.Y + sibling.Height + 20;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Width;
            }), Constraint.RelativeToParent((parent) =>
            {
                return parent.Height - tabs.Y;
            }));
        }

        private void tabTapped(object sender, EventArgs e)
        {
            var d = sender as StackLayout;
            var label = d.Children[0] as TitleWithIcon;
            string name = label.Title;
            switch (name)
            {
                case "Chuyến chưa đi":
                    SelectTab(chuyenchuadi);
                    UnselectTab(chuyendadi);
                    hrChuyenchuadi.IsVisible = true;
                    hrChuyendadi.IsVisible = false;
                    tab1.IsVisible = true;
                    tab2.IsVisible = false;
                    break;
                case "Chuyến đã đi":
                    SelectTab(chuyendadi);
                    UnselectTab(chuyenchuadi);
                    hrChuyendadi.IsVisible = true;
                    hrChuyenchuadi.IsVisible = false;
                    tab1.IsVisible = false;
                    tab2.IsVisible = true;
                    break;
                default:
                    SelectTab(chuyenchuadi);
                    UnselectTab(chuyendadi);
                    hrChuyenchuadi.IsVisible = true;
                    hrChuyendadi.IsVisible = false;
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
            tab.Opacity = 0.5;
        }

        static void SelectTab(StackLayout tab)
        {
            tab.Opacity = 1.0;
        }

        private void itemTapped(object sender, ItemTappedEventArgs e)
        {
            var d = e.Item as Dieuxe;
            NavigateToPage(new BanDo(d));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Settings.AccessToken = "";
            Application.Current.MainPage = new NavigationPage(new Login());
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Equals("True"))
            {
                LabelThongBaoListChuaDiNull.IsVisible = vm.DanhSachChuyenChuaDi.Count == 0 ? true : false;
                LabelThongBaoListDaDiNull.IsVisible = vm.DanhSachChuyenDaDi.Count == 0 ? true : false;
            }
        }
    }
}