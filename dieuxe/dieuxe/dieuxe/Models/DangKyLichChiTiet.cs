using System;
using System.Collections.Generic;
using System.Text;

namespace dieuxe.Models
{
    public class DangKyLichChiTiet
    {
        public int DangKyLichChiTietId { get; set; }
        public int DangKyLichId { get; set; }
        public string TenNguoiDi { get; set; }
        public string SdtLienLac { get; set; }
        public string ThoiGianDen { get; set; }
        public string ThoiGianVe { get; set; }
        public string NgayDen { get; set; }
        public string GioDen { get; set; }
        public string NgayVe { get; set; }
        public string GioVe { get; set; }
        public string NoiDi { get; set; }
        public string NoiDen { get; set; }
        public string Ghichu { get; set; }
        public string NgayDangKy { get; set; }
        public int NguoiTao { get; set; }
        public string NgayCapNhat { get; set; }
        public int NguoiCapnhat { get; set; }
        public string ToaDoDi { get; set; }
        public string ToaDoDen { get; set; }
        public int SoNguoi { get; set; }
        public int TrangThai { get; set; }
    }
}
