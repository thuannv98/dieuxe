using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace dieuxe.Models
{
    public class LichSuTimKiem : INotifyPropertyChanged
    {
        string _LienHeId;
        public string LienHeId
        {
            get { return _LienHeId; }
            set
            {
                _LienHeId = value;
                OnPropertyChanged("LienHeId");
            }
        }
        string _diadiem;
        public string diadiem
        {
            get { return _diadiem; }
            set
            {
                _diadiem = value;
                OnPropertyChanged("diadiem");
            }
        }
        string _NgayLuu;
        public string NgayLuu
        {
            get { return _NgayLuu; }
            set
            {
                _NgayLuu = value;
                OnPropertyChanged("NgayLuu");
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
