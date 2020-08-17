using dieuxe.Services;
using dieuxe.ViewModels;
using Plugin.Badge.Abstractions;
using System;
using dieuxe.Helpers;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using dieuxe.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class notifyPage : ContentPage, INotifyPropertyChanged
    {// thong báo
        notifyViewModel ntf;
        private string _count;
        public string count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }
        public notifyPage()
        {
            InitializeComponent();
            ntf = new notifyViewModel();
            BindingContext = ntf;


        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            count = ntf.count.ToString();
            if (!string.IsNullOrEmpty(count) && count != "0") { TabBadge.SetBadgeText(tb, count); }
        }

    }
}