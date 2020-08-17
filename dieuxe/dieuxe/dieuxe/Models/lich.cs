using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace dieuxe.Models
{
    public class lich : INotifyPropertyChanged
    {
        public int DangKyLichId { get; set; }
        public string TenNguoiDi { get; set; }
        public string ThoiGianDen { get; set; }
        public string ThoiGianVe { get; set; }
        public string NgayDen { get; set; }
        public string GioDen { get; set; }
        public string NgayVe { get; set; }
        public string GioVe { get; set; }
        public string NoiDi { get; set; }
        public string NoiDen { get; set; }
        public int SoNguoi { get; set; }
        public string ToaDoDi { get; set; }
        public string ToaDoDen { get; set; }

        private LienHeUser _SelectItem;
        public LienHeUser SelectItem
        {
            get { return _SelectItem; }
            set
            {
                _SelectItem = value;
                Mataixe = _SelectItem.LienHeID;
                OnPropertyChanged();
            }
        }
        private int _Mataixe { get; set; }
        public int Mataixe
        {
            get { return _Mataixe; }
            set { _Mataixe = value; OnPropertyChanged(); }
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
