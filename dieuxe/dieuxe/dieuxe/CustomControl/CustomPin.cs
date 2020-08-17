using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace dieuxe.CustomControl
{
    public class CustomPin : Pin
    {
        public string Name { get; set; }
        public string Url { get; set; }

        public static BindableProperty TrangThaiProperty = BindableProperty.Create(
           propertyName: "TrangThai",
           returnType: typeof(int),
           declaringType: typeof(CustomPin),
           defaultValue: -1,
           defaultBindingMode: BindingMode.TwoWay,
           propertyChanged: TrangThaiPropertyChanged);

        public int TrangThai
        {
            get { return (int)base.GetValue(TrangThaiProperty); }
            set { base.SetValue(TrangThaiProperty, value); }
        }

        private static void TrangThaiPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //var control = bindable as TitleWithIcon;
            //control._Icon.Source = ImageSource.FromFile(newValue.ToString());
        }
    }
}