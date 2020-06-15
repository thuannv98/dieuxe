using dieuxe.Helpers;
using dieuxe.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Forms;

namespace dieuxe.ViewModels
{
    public class TaiXeVM
    {
        ObservableCollection<ChuyenXe> chuyenchuadi = new ObservableCollection<ChuyenXe>();
        ObservableCollection<ChuyenXe> chuyendadi = new ObservableCollection<ChuyenXe>();
        public ObservableCollection<ChuyenXe> DanhSachChuyenChuaDi 
        { 
            get { return chuyenchuadi; }
            set
            {
                chuyenchuadi = value;
                //OnPropertyChanged();
            }
        }
        public ObservableCollection<ChuyenXe> DanhSachChuyenDaDi
        {
            get { return chuyendadi; }
            set
            {
                chuyendadi = value;
                //OnPropertyChanged();
            }
        }

        public TaiXeVM(string mataixe)
        {
            //var accessToken = Settings.AccessToken;
            //generate("https://apidieuxe20200508212151.azurewebsites.net/api/lichchayxehomnay/" + mataixe, accessToken);


            chuyenchuadi.Add(new ChuyenXe { madieuxe = "DX1016", nhanviendangky = "SK03", songuoi = 1, diemdau = "48 tăng nhơn phú", diemcuoi = "điểm cuối", tuyenduong = "ytbaAoavjS_@AEFCNJl@BRAVCF\\`@z@lAb@f@P^Lx@D\\LVbAvAlAxAbCvCfCpCvAdB`@h@^ZbBbARRT\\lAbCx@fBXlCBTC`@SbA@^TfCiBDyAFu@DoAP}@Nk@NmBl@y@To@Lw@L_AFkAPc@Re@ZYT]Pw@R_AJg@B_A?cCMe@Bi@Le@ToA`Au@p@a@h@a@n@Yn@e@n@QIuHqDgCkAU?a@FgB^aDnAeCfA_@b@QVgDxF_AxAc@p@WVUPoCvAyBdAmCgEoAsB]_AQc@Ww@SWg@g@cAAcAD", giodi = "8:00 20/2/2020", giove = "8:00 20/2/2020", taixe = "SK14", bienkiemsoat = "51A - 1223", trangthai = "chuahoantat", ghichu="" });
            chuyenchuadi.Add(new ChuyenXe { madieuxe = "DX1017", nhanviendangky = "Sk02", songuoi = 2, diemdau = "48 tăng nhơn phú", diemcuoi = "điểm cuối chưa biết", tuyenduong = "ytbaAoavjS_@AEFCNJl@BRAVCF{@u@UIo@Oa@QOMIOEQ?SHs@RaAFk@CYe@_Bi@{AkAqDa@}@W[[SaB}@a@Wo@i@]k@z@EfACVGlC{AXUdAiATYJGRK^GtAIp@CrACdB?j@NnAb@d@RbA{CzAsFdAwEp@qDZ}BHa@Na@`@]dBcAREPDd@D~A`@h@Ff@@LEtEyCbBeAhMyI|HmF^WQUqCqEeBuCsBsCyMeSyC}EuG}Jc@o@{CuEuC}DsAgB{BsBUUsDaC{@i@mEkCwC}AaFyC}FaEaCgB{AkAkCeCaBaBUa@Q_@Ku@IOOMOKSCc@BYROZC\\@J_@v@mGdHuN`OoKjLkAtA@IAQIUOQCA", giodi = "8:00 20/2/2020", giove = "8:00 20/2/2020", taixe = "SK14", bienkiemsoat = "51A - 1223", trangthai = "chuahoantat", ghichu = "" });

        }

        async void generate(string url, string accessToken)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            HttpResponseMessage response = await client.GetAsync(url);

            System.Diagnostics.Debug.WriteLine(response);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                DanhSachChuyenChuaDi = JsonConvert.DeserializeObject<ObservableCollection<ChuyenXe>>(content);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("error", "you should be login first :( ", "OK");
                chuyenchuadi = null;
            }
        }
    }
}
