﻿using dieuxe.CustomControl;
using dieuxe.Helpers;
using dieuxe.Models;
using dieuxe.Services;
using dieuxe.ViewModels;
using Plugin.Geolocator;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;
using Distance = Xamarin.Forms.Maps.Distance;
using Polyline = Xamarin.Forms.Maps.Polyline;

namespace dieuxe.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BanDo : ContentPage , INotifyPropertyChanged
    {


        #region INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            if (PropertyChanged == null)
                return;

            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        private static readonly CompositeDisposable EventSubscriptions = new CompositeDisposable();
        private readonly PanGestureRecognizer _panGesture = new PanGestureRecognizer();

        double Height_TieuDeFrame = 50;
        double Height_TieuDeFrameSteps = 100;
        double YMax_Frame;
        double YMin_Frame;
        double Width_Frame;
        double Height_Frame;

        int direction;
        double layoutBoundsHeight;

        dangkylichViewmodel svm;
        Pin pin = null;
        dangkylichViewmodel dklvm;
        public string Loai { get; set; }

        public BanDo()
        {
            InitializeComponent();

            //map.IsShowingUser = true;
            DisplayCurLocation();
        }
        public BanDo(dangkylichViewmodel dklvm)//dang ky lich
        {
            InitializeComponent();
            Title = "Chọn địa điểm";
            this.dklvm = dklvm;
            svm = new dangkylichViewmodel();
            //map.IsShowingUser = true;
            BindingContext = svm;
            searchFrame.IsVisible = true;
            select.IsVisible = true;
            map.MapClicked += OnMapClicked;
            this.DisplayCurLocation();
        }

        public BanDo(Dieuxe chuyenXe)
        {
            InitializeComponent();
            //Title = "";
            steps.IsVisible = true;

            //map.IsShowingUser = true;
            DisplayCurLocation();

            ChiTietChuyenDiViewModel chiTietChuyenDiViewModel = new ChiTietChuyenDiViewModel(chuyenXe);
            BindingContext = chiTietChuyenDiViewModel;

            var listPoint = MapFunction.Decode(chiTietChuyenDiViewModel.LayTuyenDuong()).ToList();
            Polyline polyline = new Polyline
            {
                StrokeWidth = 10,
                StrokeColor = Color.FromHex("#5c96f1"),
                //FillColor = Color.FromHex("#881BA1E2"),
                Geopath = { }
            };
            foreach (Models.Coordinate point in listPoint)
            {
                polyline.Geopath.Add(new Position(point.Latitude, point.Longitude));
            }
            map.MapElements.Add(polyline);

            foreach (var p in chiTietChuyenDiViewModel.DanhSachDiemDung)
            {
                Position position = new Position(Convert.ToDouble(p.lat), Convert.ToDouble(p.lng));
                Pin pin = new Pin
                {
                    Address = p.DiaChi,
                    Type = PinType.Place,
                    Position = position,
                    Label = p.Equals(chiTietChuyenDiViewModel.DanhSachDiemDung.First()) ? "Khởi hành" 
                            : p.Equals(chiTietChuyenDiViewModel.DanhSachDiemDung.Last()) ? "Kết thúc" : "Điểm dừng"
                };
                map.Pins.Add(pin);
            }

            double maxLat = listPoint[0].Latitude, minLat = listPoint[0].Latitude;
            double maxLng = listPoint[0].Longitude, minLng = listPoint[0].Longitude;
            foreach (var point in listPoint)
            {
                if (point.Latitude < minLat)
                    minLat = point.Latitude;
                if (point.Latitude > maxLat)
                    maxLat = point.Latitude;

                if (point.Longitude < minLng)
                    minLng = point.Longitude;
                if (point.Longitude > maxLng)
                    maxLng = point.Longitude;
            }
            // góc nhọn tại góc trái màn hình (góc dưới)
            //double gocA = Math.Asin(Content.Width / (Math.Sqrt(Math.Pow(Content.Width, 2) + Math.Pow(Content.Height, 2))));
            
            Position NorthEast = new Position(maxLat, maxLng);
            Position SouthWest = new Position(minLat, minLng);
            Position Center = new Position((maxLat + minLat) / 2, (maxLng + minLng) / 2);
            Vector vectorSouthWest_NorthEast = MapFunction.layVector(new Coordinate(SouthWest.Latitude, SouthWest.Longitude), new Coordinate(NorthEast.Latitude, NorthEast.Longitude));
            Vector vectorEast = new Vector { Lat = Center.Latitude, Lng = 0 };
            double gocA = MapFunction.timgoc(vectorSouthWest_NorthEast, vectorEast);
            double canhHuyen = Distance.BetweenPositions(NorthEast, SouthWest).Kilometers;
            double canhGocA = canhHuyen * Math.Cos(gocA * Math.PI / 180);
            double radius = canhGocA;
            map.MoveToRegion(MapSpan.FromCenterAndRadius(Center, Distance.FromKilometers(radius)));
            map.MoveToRegion(MapSpan.FromCenterAndRadius(Center, Distance.FromKilometers(radius)));
            steps.SizeChanged += Content_SizeChanged;

            MessagingCenter.Subscribe<ChiTietChuyenDiViewModel>(this, "Cập nhật chuyến đi thất bại", async (sender) =>
            {
                var action = await DisplayAlert("Đã xảy ra sự cố", "Không thể xác nhận hoàn thành lộ trình. Báo cáo vấn đề ngay?", "Báo cáo", "Bỏ qua");
                if(action)
                {
                    //báo cáo vấn đề cho quản lý ...
                }    
            });


        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // tài xế
            YMax_Frame = Content.Height - Height_TieuDeFrame;
            YMin_Frame = Content.Height - steps.Height;
            if (YMin_Frame < 0)
                YMin_Frame = 0;
            Height_Frame = steps.Height;
            Width_Frame = steps.Width;
        }
        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            //listkq.IsVisible = false;
            kq.IsVisible = false;
            System.Diagnostics.Debug.WriteLine($"MapClick:{e.Position.Latitude}, {e.Position.Longitude}");
            var position = new Position(e.Position.Latitude, e.Position.Longitude);
            if (pin == null)
            {
                addmark(position);
            }
            else
            {
                removepin(pin);
                addmark(position);
            }

        }
        public void addmark(Position position)
        {
            pin = new Pin
            {
                Type = PinType.Place,
                Position = position,
                Label = "Điểm Tọa độ",
                Address = ""
            };
            map.Pins.Add(pin);
        }
        private void removepin(Pin pin)
        {
            map.Pins.Remove(pin);
        }
        private async void OnTextChanged(object sender, TextChangedEventArgs eventArgs)
        {
            //listSection.IsVisible = true;
            //listkq.IsVisible = true;
            kq.IsVisible = true;
            if (!string.IsNullOrWhiteSpace(svm.searchvalue))
            {
                await svm.GetPlacesPredictionsAsync();
            }

        }
        private async void Listkq_ItemSelected(object sender, ItemTappedEventArgs e)
        {
            //kq.IsVisible = false;
            //listkq.IsVisible = false;
            var details = e.Item as placesearchdetail.AddressInfo;
            var location = await mahoadiachi(details.Address);
            DisplayselectedLocation(location, details.Address);
            timkiem.Text = details.Address;
            kq.IsVisible = false;
            //listSection.IsVisible = false;

        }
        public async Task<Location> mahoadiachi(string address)
        {
            var locations = await Geocoding.GetLocationsAsync(address);
            var location = locations?.FirstOrDefault();
            if (location == null)
            {
                await DisplayAlert("Error", "cannot find location", "OK");
                location = null;
            }

            Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
            return location;
        }
        public async void DisplayselectedLocation(Location location, string address)
        {
            try
            {
                if (location != null)
                {
                    Position position = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(.444));
                    map.MoveToRegion(mapSpan);
                    //addmark(position);
                    if (pin == null)
                    {
                        pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = position,
                            Label = position.Longitude + "," + position.Latitude,
                            Address = address
                        };
                        map.Pins.Add(pin);
                    }
                    else
                    {
                        removepin(pin);
                        pin = new Pin
                        {
                            Type = PinType.Place,
                            Position = position,
                            Label = position.Longitude + "," + position.Latitude,
                            Address = address
                        };
                        map.Pins.Add(pin);
                    }
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}");
                }
                else
                {
                    await DisplayAlert("Permission Denied", "dell kết nối đc gps", "OK");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }
        private async void select_Clicked(object sender, EventArgs e)
        {
            kq.IsVisible = false;
            //listkq.IsVisible = false;
            Pin pinselect = pin;
            if (pin == null)
            {
                await DisplayAlert("thông báo:", "Bạn chưa chọn điểm đến", "ok");
            }
            else
            {
                if (!string.IsNullOrEmpty(Loai))
                {
                    if (String.IsNullOrEmpty(pin.Address))
                    {
                        pin.Address = await svm.GetAddressAsync(pin.Position.Latitude, pin.Position.Longitude);
                    }
                    if (Loai == "noidi")
                    {
                        await DisplayAlert("đã đăng ký", pinselect.Address, "ok");
                        dklvm.NoiDi = pin.Address;
                        await Navigation.PopAsync();
                        MessagingCenter.Send<BanDo, string>(this, "Đã chọn địa điểm", pinselect.Address);
                    }
                    else if (Loai == "noiden")
                    {
                        await DisplayAlert("đã đăng ký", pinselect.Address, "ok");
                        dklvm.NoiDen = pin.Address;
                        await Navigation.PopAsync();
                        MessagingCenter.Send<BanDo, string>(this, "Đã chọn địa điểm", pinselect.Address);
                    }
                }
            }
        }

        private void Content_SizeChanged(object sender, EventArgs e)
        {
            AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, Content.Height - step.Height, 1, AbsoluteLayout.AutoSize));
        }

        private async void nutBatDauLaiXe_Clicked(object sender, EventArgs e)
        {
            //
            var hasPermission = await Utils.CheckPermissions(Permission.Location);
            if (!hasPermission)
                return;

            var locator = CrossGeolocator.Current;
            var position = await locator.GetPositionAsync();

            var viewmodel = this.BindingContext as ChiTietChuyenDiViewModel;
            var diemkhoihanh = viewmodel.DanhSachDiemDung.FirstOrDefault();
            var DiemKhoiHanh = new Position(Convert.ToDouble(diemkhoihanh.lat), Convert.ToDouble(diemkhoihanh.lng));

            var stringpolyline = await MapFunction.GetPolyline(new Coordinate(position.Latitude, position.Longitude), new Coordinate(DiemKhoiHanh.Latitude, DiemKhoiHanh.Longitude));
            var listPoint = MapFunction.Decode(stringpolyline).ToList();
            Polyline polyline = new Polyline
            {
                StrokeWidth = 10,
                StrokeColor = Color.FromHex("#5c96f1"),
                Geopath = { }
            };
            foreach (Models.Coordinate point in listPoint)
            {
                polyline.Geopath.Add(new Position(point.Latitude, point.Longitude));
            }
            polyline.Geopath.Add(DiemKhoiHanh);

            map.MapElements.Add(polyline);

            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude, position.Longitude), Distance.FromMeters(300)));
            grid_steps.RowDefinitions[0].Height = Height_TieuDeFrameSteps;
            var delta = Height_TieuDeFrameSteps - Height_TieuDeFrame;

            YMax_Frame -= delta;
            YMin_Frame -= delta;
            if (YMin_Frame < 0)
                YMin_Frame = 0;
            Height_Frame += delta;
            DayXuong();

            nutBatDauLaiXe.IsVisible = false;
        }

        Label lbSoKQ = new Label();
        int indexKQHienTai = 0;
        public BanDo(List<DangKyLichChiTiet> danhsachdangky, QuanLyVM bindingContext)
        {
            InitializeComponent();
            this.Title = "Phân tích chuyến đi";
            MapSpan mapSpan = new MapSpan(new Position(10.832282, 106.778043), 0.01, 0.01);
            map.MoveToRegion(mapSpan);
            Pin pin = new Pin
            {
                Label = "CTY Quốc tế Phong Phú - PPJ",
                Address = "48 Đường Tăng Nhơn Phú, Tăng Nhơn Phú B, Quận 9",
                Type = PinType.Place,
                Position = new Position(10.832282, 106.778043)
            };
            map.Pins.Add(pin);

            BindingContext = bindingContext;
            nutPhanTich.IsVisible = true;
            lbSoKQ.SetBinding(Label.TextProperty, "SoKq");

            ListView lvKetQua = new ListView();
            lvKetQua.SetBinding(ListView.ItemsSourceProperty, "DanhSachChuyenDi");
            content.Children.Add(lvKetQua);
            lvKetQua.IsVisible = false;

            var list = lvKetQua.ItemsSource as ObservableCollection<ChuyenDi>;
            if (list != null)
                list.CollectionChanged += ListViewBehaviors_CollectionChanged;
        }

        //start khu vực quản lý
        private void nutphantich_Clicked(object sender, EventArgs e)
        {
            activityChoxuly.IsRunning = true;
            contentChoxuly.IsVisible = true;
        }

        private void ListViewBehaviors_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            indexKQHienTai++;
            if(indexKQHienTai != (this.BindingContext as QuanLyVM).SoKq)
            {
                return;
            }
            HienThiKetQua((this.BindingContext as QuanLyVM).DanhSachChuyenDi);
        }
        List<Pin> listPinOrgDes = new List<Pin>();
        List<List<Pin>> listPin = new List<List<Pin>>();
        List<Polyline> listPolyline = new List<Polyline>();
        MapSpan mapSpanChung;
        List<MapSpan> listMapSpan = new List<MapSpan>();
        async void HienThiKetQua(ObservableCollection<ChuyenDi> DsChuyenDi)
        {
            var d = (this.BindingContext as QuanLyVM).DsDieuXe;
            listChiTietKetQua.HeightRequest = DsChuyenDi.Count * 70;
            lvDsDieuXe.HeightRequest = (DsChuyenDi.Count * lvDsDieuXe.RowHeight) < Content.Height - 100 ? DsChuyenDi.Count * lvDsDieuXe.RowHeight : Content.Height - 100;

            int stt = 0, ind = 0;
            string[] colors = new string[] { "#1BA1E2", "#009973", "#558000", "#ff9900", "#990099", "#003300", "#999966", "#3399ff", "#ff3399" };
            double? maxLat = null, minLat = null, maxLng = null, minLng = null;

            foreach (var chuyen in DsChuyenDi)
            {
                stt++;
                var random = new Random();
                string color = ind >= colors.Length ? String.Format("#{0:X6}", random.Next(0x1000000)) : colors[ind];
                double maxlat = chuyen.dsDiem[0].location.Latitude, minlat = chuyen.dsDiem[0].location.Latitude;
                double maxlng = chuyen.dsDiem[0].location.Longitude, minlng = chuyen.dsDiem[0].location.Longitude;

                var property_list = QuanLyVM.get_ORG_DES_WAY(chuyen.dsDiem);
                Coordinate origin = property_list.origin;
                Coordinate destination = property_list.destination;
                List<Coordinate> waypoints = property_list.waypoints;

                Pin OrgPin = new Pin
                {
                    Label = "Điểm khởi hành chuyến đi: " + stt,
                    Position = new Position(origin.Latitude, origin.Longitude),
                    Address = chuyen.dsDiem.First().diachi
                };
                Pin DesPin = new Pin
                {
                    Label = "Điểm cuối chuyến đi: " + stt,
                    Position = new Position(destination.Latitude, destination.Longitude),
                    Address = chuyen.dsDiem.Last().diachi
                };
                //map.Pins.Add(pin);
                //map.Pins.Add(pin1);
                listPinOrgDes.Add(OrgPin);
                listPinOrgDes.Add(DesPin);

                listPin.Add(new List<Pin>());
                foreach(var point in chuyen.dsDiem)
                {
                    var pin = new Pin
                    {
                        Label = "Điểm dừng",
                        Position = new Position(point.location.Latitude, point.location.Longitude),
                        Address = point.diachi
                    };
                    pin.InfoWindowClicked += Pin_Clicked;
                    listPin.Last().Add(pin);
                }

                MapDirectionResponse mapDirectionResponse = await MapFunction.GetDirectionJsonAsync(origin, destination, waypoints);
                if (mapDirectionResponse.routes.Count > 0)
                {
                    var listpoint = MapFunction.GetOverviewPolylinePoints(mapDirectionResponse);
                    if (!maxLat.HasValue)
                        maxLat = listpoint.First().Latitude;
                    if (!minLat.HasValue)
                        minLat = listpoint.First().Latitude;
                    if (!maxLng.HasValue)
                        maxLng = listpoint.First().Longitude;
                    if (!minLng.HasValue)
                        minLng = listpoint.First().Longitude;

                    Polyline polyline = new Polyline
                    {
                        StrokeWidth = 8,
                        StrokeColor = Color.FromHex(color),
                        //FillColor = Color.FromHex("#881BA1E2"),
                        Geopath = { }
                    };
                    foreach (Coordinate point in listpoint)
                    {
                        polyline.Geopath.Add(new Position(point.Latitude, point.Longitude));

                        //if (point.Latitude < minLat)      //ban đầu
                        //    minLat = point.Latitude;
                        //if (point.Latitude > maxLat)
                        //    maxLat = point.Latitude;

                        //if (point.Longitude < minLng)
                        //    minLng = point.Longitude;
                        //if (point.Longitude > maxLng)
                        //    maxLng = point.Longitude;

                        if (point.Latitude < minlat)
                            minlat = point.Latitude;
                        if (point.Latitude > maxlat)
                            maxlat = point.Latitude;

                        if (point.Longitude < minlng)
                            minlng = point.Longitude;
                        if (point.Longitude > maxlng)
                            maxlng = point.Longitude;
                    }
                    listPolyline.Add(polyline);
                    map.MapElements.Add(polyline);
                    //Polyline dd = (Polyline)map.MapElements.ElementAt(1);
                    //double d = 0;
                    //foreach (Leg leg in mapDirectionResponse.routes[0].legs)
                    //{
                    //    d += leg.distance.value;
                    //}
                    //System.Console.WriteLine("tổng quãng đường: {0}", d);
                    ind++;

                    minLat = minlat < minLat ? minlat : minLat;
                    maxLat = maxlat > maxLat ? maxlat : maxLat;
                    minLng = minlng < minLng ? minlng : minLng;
                    maxLng = maxlng > maxLng ? maxlng : maxLng;

                }
                listMapSpan.Add(CreateMapSpan(maxlat, minlat, maxlng, minlng));

            }
            // góc nhọn tại góc trái màn hình (góc dưới)
            //double gocA = Math.Asin(Content.Width / (Math.Sqrt(Math.Pow(Content.Width, 2) + Math.Pow(Content.Height, 2))));

            //Position NorthEast = new Position(maxLat.Value, maxLng.Value);
            //Position SouthWest = new Position(minLat.Value, minLng.Value);
            //Position Center = new Position((maxLat.Value + minLat.Value) / 2, (maxLng.Value + minLng.Value) / 2);
            //Vector vectorSouthWest_NorthEast = MapFunction.layVector(new Coordinate(SouthWest.Latitude, SouthWest.Longitude), new Coordinate(NorthEast.Latitude, NorthEast.Longitude));
            //Vector vectorEast = new Vector { Lat = Center.Latitude, Lng = 0 };
            //double gocA = MapFunction.timgoc(vectorSouthWest_NorthEast, vectorEast);
            //double canhHuyen = Distance.BetweenPositions(NorthEast, SouthWest).Kilometers;
            //double canhGocA = canhHuyen * Math.Cos(gocA * Math.PI / 180);
            //double radius = canhGocA;
            //map.MoveToRegion(MapSpan.FromCenterAndRadius(Center, Distance.FromKilometers(radius)));
            mapSpanChung = CreateMapSpan(maxLat.Value, minLat.Value, maxLng.Value, minLng.Value);
            map.MoveToRegion(mapSpanChung);

            YMax_Frame = Content.Height - Height_TieuDeFrame;
            YMin_Frame = Content.Height - FrameDsChuyenDi.Height;
            if(YMin_Frame < 0)
                YMin_Frame = 0;
            Height_Frame = FrameDsChuyenDi.Height;
            Width_Frame = FrameDsChuyenDi.Width;

            FrameDsChuyenDi.IsVisible = true;
            AbsoluteLayout.SetLayoutBounds(FrameDsChuyenDi, new Rectangle(0, YMax_Frame, 1, AbsoluteLayout.AutoSize));
            await map.LayoutTo(new Rectangle(0, 0, map.Width, YMax_Frame));
            DayLen();

            gridButton.IsVisible = true;
            map.MapClicked += Map_MapClicked;

            nutPhanTich.IsVisible = false;
            activityChoxuly.IsRunning = false;
            contentChoxuly.IsVisible = false;

            var listviewHeight = lvDsDieuXe.RowHeight * DsChuyenDi.Count;
            lvDsDieuXe.HeightRequest = listviewHeight + stackchontaixe.Y + 55 > Content.Height ? 
                Content.Height - (stackchontaixe.Y + 55) : listviewHeight;      // 55: height + margin của nút xác nhận + padding frame
        }

        private void nutPostdieuxe_Clicked(object sender, EventArgs e)
        {
            foreach (var dieuxe in (List<Dieuxe>)lvDsDieuXe.ItemsSource)
            {
                if (dieuxe.SelectItem == null)
                {
                    int index = ((List<Dieuxe>)lvDsDieuXe.ItemsSource).IndexOf(dieuxe);
                    var option = DisplayAlert("Chưa chọn đủ tài xế", "Hãy chọn tài xế cho chuyến đi " + index + "!", "Đã hiểu");
                    return;
                }
            }
            var vm = BindingContext as QuanLyVM;
            vm.PostCommand.Execute((this.BindingContext as QuanLyVM).DsDieuXe);
        }

        private void nutPhantaixe_Clicked(object sender, EventArgs e)
        {
            hienPopUp(stackchontaixe);
        }

        MapSpan CreateMapSpan(double maxLat, double minLat, double maxLng, double minLng)
        {
            Position NorthEast = new Position(maxLat, maxLng);
            Position SouthWest = new Position(minLat, minLng);
            Position Center = new Position((maxLat + minLat) / 2, (maxLng + minLng) / 2);
            Vector vectorSouthWest_NorthEast = MapFunction.layVector(new Coordinate(SouthWest.Latitude, SouthWest.Longitude), new Coordinate(NorthEast.Latitude, NorthEast.Longitude));
            Vector vectorEast = new Vector { Lat = Center.Latitude, Lng = 0 };
            double gocA = MapFunction.timgoc(vectorSouthWest_NorthEast, vectorEast);
            double canhHuyen = Distance.BetweenPositions(NorthEast, SouthWest).Kilometers;
            double canhGocA = canhHuyen * Math.Cos(gocA * Math.PI / 180);
            double radius = canhGocA;
            return MapSpan.FromCenterAndRadius(Center, Distance.FromKilometers(radius));
        }

        string _gioden_luotdi;
        public string gioden_luotdi
        {
            get { return _gioden_luotdi; }
            set
            {
                _gioden_luotdi = value;
                OnPropertyChanged("gioden_luotdi");
            }
        }
        string _gioden_luotve;
        public string gioden_luotve
        {
            get { return _gioden_luotve; }
            set
            {
                _gioden_luotve = value;
                OnPropertyChanged("gioden_luotve");
            }
        }
        List<point_detail> dsLuotDi;
        public List<point_detail> DsLuotDi
        {
            get { return dsLuotDi; }
            set
            {
                dsLuotDi = value;
                OnPropertyChanged("DsLuotDi");
            }
        }
        List<point_detail> dsLuotVe;
        public List<point_detail> DsLuotVe
        {
            get { return dsLuotVe; }
            set
            {
                dsLuotVe = value;
                OnPropertyChanged("DsLuotVe");
            }
        }
        private void Pin_Clicked(object sender, EventArgs e)
        {
            var pin = sender as Pin;
            foreach (var point in (this.BindingContext as QuanLyVM).DanhSachChuyenDi[indexChuyenDi].dsDiem)
            {
                if(point.location.IsSame(new Coordinate(pin.Position.Latitude, pin.Position.Longitude)))
                {
                    diachidiadiem.Text = point.diachi;
                    gioden_luotdi = point.thoigianxeden_luotdi;
                    gioden_luotve = point.thoigianxeden_luotve;
                    DsLuotDi = point.dsdiemcungtoado.Where(w => w.thoigianden != null).ToList();
                    DsLuotVe = point.dsdiemcungtoado.Where(w => w.thoigianden != null).ToList();

                    lenxe_xuongxe.IsVisible = ((listluotdi.IsVisible == true && dsLuotDi.Count == 0) || (listluotdi.IsVisible == false && dsLuotVe.Count == 0)) ? false : true;
                    //lbdiemdi.IsVisible = !lblenxe_xuongxe.IsVisible;
                    //im.IsVisible = !lblenxe_xuongxe.IsVisible;
                    listluotdi.HeightRequest = listluotdi.RowHeight * dsLuotDi.Count;
                    listluotve.HeightRequest = listluotve.RowHeight * dsLuotVe.Count;
                }
            }
            lbGioxeden.Text = String.Format("Xe đến lúc: {0:HH:mm}", gioden_luotdi);
            listluotdi.IsVisible = true;

            chitietdiadiem.IsVisible = true;
            this.popuplayout_chitietdiadiem.AnchorX = 0.5;
            this.popuplayout_chitietdiadiem.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => this.popuplayout_chitietdiadiem.Scale = f,
                0.5,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => this.popuplayout_chitietdiadiem.Opacity = f,
                0.2,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(this.popuplayout_chitietdiadiem, "popupScaleAnimation", 250);
            fadeAnimation.Commit(this.popuplayout_chitietdiadiem, "popupFadeAnimation", 250);

            map.MapClicked += (s, EventArgs) => { chitietdiadiem.IsVisible = false; };
        }
        private void btnChuyenLuotDi_Clicked(object sender, EventArgs e)
        {
            listluotdi.IsVisible = !listluotdi.IsVisible;
            if (listluotdi.IsVisible)
                lbGioxeden.Text = String.Format("Xe đến lúc: {0:HH:mm}", gioden_luotdi);
            else
                lbGioxeden.Text = String.Format("Xe đến lúc: {0:HH:mm}", gioden_luotve);
        }
        private async void close_chitietdiadiem(object sender, EventArgs e)
        {
            await Task.WhenAny<bool>(this.popuplayout_chitietdiadiem.FadeTo(0, 200, Easing.SinInOut));
            chitietdiadiem.IsVisible = false;
        }

        bool frameOTren = false;
        async void DayLen()
        {
            if (frameOTren)
                return;

            if (steps.IsVisible)
            {
                // frame tài xế
                up_downFrameTx.Source = "down.png";
                await steps.LayoutTo(new Rectangle(0, YMin_Frame, Width_Frame, Height_Frame));
                AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, YMin_Frame, 1, Height_Frame));
            }
            else
            {
                // frame quản lý
                up_downFramQl.Source = "down.png";
                await FrameDsChuyenDi.LayoutTo(new Rectangle(0, YMin_Frame, Width_Frame, Height_Frame));
                await map.LayoutTo(new Rectangle(0, 0, map.Width, YMin_Frame));
            }

            frameOTren = true;
        }
        async void DayXuong()
        {
            if (!frameOTren)
                return;

            if (steps.IsVisible)
            {
                // frame tài xế
                up_downFrameTx.Source = "up.png";
                await steps.LayoutTo(new Rectangle(0, YMax_Frame, Width_Frame, Height_Frame));

                AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, YMax_Frame, 1, Height_Frame));
            }
            else
            {
                // frame quản lý
                await map.LayoutTo(new Rectangle(0, 0, map.Width, YMax_Frame));
                up_downFramQl.Source = "up.png";
                await FrameDsChuyenDi.LayoutTo(new Rectangle(0, YMax_Frame, Width_Frame, Height_Frame));
            }
            
            frameOTren = false;
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            DayXuong();
            //map.MoveToRegion(mapSpanChung);
        }

        private void XemChiTietDsChuyenDi(object sender, EventArgs e)
        {
            if (frameOTren)
                DayXuong();
            else
                DayLen();
        }

        private async void FramDsChuyenDi_PanUpdated(object sender, PanUpdatedEventArgs e)
        {
            double YFramDsChuyenDi = frameOTren ? YMin_Frame : YMax_Frame;
            double YKeCuoi = YFramDsChuyenDi;
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    layoutBoundsHeight = AbsoluteLayout.GetLayoutBounds(steps).Y;
                    break;
                case GestureStatus.Running:
                    direction = e.TotalY < 0 ? 1 : -1;

                    var TranslationY = YFramDsChuyenDi + e.TotalY;
                    if ((TranslationY < YMax_Frame) & (TranslationY > YMin_Frame))
                    {
                        if (steps.IsVisible)
                        {
                            await steps.LayoutTo(new Rectangle(0, TranslationY, Width_Frame, Height_Frame));
                        }
                        else
                        {
                            FrameDsChuyenDi.LayoutTo(new Rectangle(0, TranslationY, Width_Frame, Height_Frame));
                            map.LayoutTo(new Rectangle(0, 0, map.Width, TranslationY));
                        }
                        
                    }

                    break;
                case GestureStatus.Completed:
                    if (direction > 0)
                    {
                        DayLen();
                    }
                    else
                    {
                        DayXuong();
                    }
                    break;
            }
        }

        int indexChuyenDi = 0;
        private void ChiTietKetQua_Tapped(object sender, ItemTappedEventArgs e)
        {
            listPolyline[indexChuyenDi].StrokeWidth -= 3;
            indexChuyenDi = ((ObservableCollection<ChuyenDi>)listChiTietKetQua.ItemsSource).IndexOf(e.Item as ChuyenDi);
            map.Pins.Clear();
            foreach(var pin in listPin[indexChuyenDi])
            {
                map.Pins.Add(pin);
            }
            map.MoveToRegion(listMapSpan[indexChuyenDi]);
            for (int i = 0; i < (listChiTietKetQua.ItemsSource as ObservableCollection<ChuyenDi>).Count; i++)
            {
                if(indexChuyenDi == i)
                {
                    listPolyline[i].StrokeWidth += 3;
                }
                else
                {
                    Color color = listPolyline[i].StrokeColor;
                    listPolyline[i].StrokeColor = Color.FromRgba(color.R, color.G, color.B, 125);
                }
            }
            DayXuong();
        }
        //end khu vực quản lý

        public async void DisplayCurLocation()
        {
            try
            {

                var request = new GeolocationRequest(GeolocationAccuracy.Best);
                var location = await Geolocation.GetLocationAsync(request);



                if (location != null)
                {
                    Position p = new Position(location.Latitude, location.Longitude);
                    MapSpan mapSpan = MapSpan.FromCenterAndRadius(p, Distance.FromKilometers(.444));

                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
                else
                {
                    await DisplayAlert("Permission Denied", "dell kết nối đc gps", "OK");
                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        private void XemChiTiet(object sender, EventArgs e)
        {
            if (frameOTren)
                DayXuong();
            else
                DayLen();

            //
            //if (stepsHeightMax == 0)
            //{
            //    foreach (var item in stepscontent.Children)
            //    {
            //        stepsHeight += item.Height;
            //    }
            //    stepsHeightMax = stepsHeight;

            //    //foreach (var item in listChiTietDiemDon.ItemsSource)
            //    //{
            //    //    stepsHeightMax += item.h
            //    //}
            //    //foreach (var item in listChiTietDiemDon.ItemsSource)
            //    //{
            //    //    stepsHeightMax += listChiTietDiemDon.RowHeight;
            //    //}

            //}
            //if (steps.Y == Content.Height - stepsHeightMin)
            //{
            //    AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, Content.Height - stepsHeightMax, 1, stepsHeightMax));
            //    //labelDiaChiKeTiep.IsVisible = false;
            //    //btnHoanTat.IsVisible = false;
            //    //tieude.IsVisible = false;
            //    //labelGioDenYeuCau.IsVisible = false;
            //    //labelThuNho.IsVisible = true;
            //    up_down.Source = "down_arrow.png";
            //}
            //else
            //if (steps.Y == Content.Height - stepsHeightMax)
            //{
            //    AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, Content.Height - stepsHeightMin, 1, stepsHeightMax));
            //    //labelDiaChiKeTiep.IsVisible = true;
            //    //btnHoanTat.IsVisible = true;
            //    //tieude.IsVisible = true;
            //    //labelGioDenYeuCau.IsVisible = true;
            //    //labelThuNho.IsVisible = false;
            //    up_down.Source = "up_arrow.png";
            //}

            //animate();
            //double yt = steps.Height;
            //double ys = steps.Height + ChiTietDiemDon.Height;
            //steps.TranslateTo(steps.X, ys);
            //Animation scaleAnimation = new Animation(
            //    f => this.steps.HeightRequest = f,
            //    steps.Height,
            //    steps.Height + ChiTietDiemDon.Height);

            //Animation fadeAnimation = new Animation(
            //    f => this.steps.Opacity = f,
            //    0.2,
            //    1);

            //scaleAnimation.Commit(this.steps, "popupScaleAnimation", 250);
            //fadeAnimation.Commit(this.steps, "popupFadeAnimation", 250);


        }
        void animate()
        {

            Animation scaleAnimation = new Animation(
                f => this.steps.HeightRequest = f,
                steps.Height,
                steps.Height + 500);

            Animation fadeAnimation = new Animation(
                f => this.steps.Opacity = f,
                0.2,
                1);

            scaleAnimation.Commit(this.steps, "popupScaleAnimation", 250);
            fadeAnimation.Commit(this.steps, "popupFadeAnimation", 250);
        }

        double? layoutHeight;
        //double layoutBoundsHeight;
        //int direction;
        //private void PanGestureRecognizer_PanUpdated(object sender, PanUpdatedEventArgs e)
        //{


        //    if (stepsHeightMax == 0)
        //    {
        //        foreach (var item in stepscontent.Children)
        //        {
        //            stepsHeight += item.Height;
        //        }
        //        stepsHeightMax = stepsHeight;
        //    }

        //    //height = steps.Height;
        //    ////layoutPropHeightMax = height < content.Height ? height : content.Height;
        //    //layoutHeight = layoutHeight ?? content.Height;
        //    switch (e.StatusType)
        //    {
        //        case GestureStatus.Started:
        //            layoutBoundsHeight = AbsoluteLayout.GetLayoutBounds(steps).Y;
        //            break;
        //        case GestureStatus.Running:
        //            //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.All);
        //            direction = e.TotalY < 0 ? 1 : -1;
        //            //var translateY = layoutBoundsHeight + (-e.TotalY / (double)layoutHeight);
        //            //if ((yProp > layoutPropHeightMin) & (yProp < layoutPropHeightMax))
        //            //    AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, 1, 1, yProp));
        //            ////var translateY = Math.Max(Math.Min(0, y + e.TotalY), -Math.Abs((Height * .15) - Height));
        //            ////steps.TranslateTo(steps.X, yProp, 10);
        //            ///

        //            var TranslationY = layoutBoundsHeight + e.TotalY;
        //            if ((TranslationY < Content.Height - stepsHeightMin) & (TranslationY > Content.Height - stepsHeightMax))
        //            {
        //                AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, TranslationY, 1, AbsoluteLayout.AutoSize));
        //                //steps.HeightRequest = translateY;
        //                //steps.TranslateTo(0, content.Height - steps.Height + e.TotalY, 250);
        //            }

        //            //MethodLockedSync(() =>
        //            //{
        //            //    Device.BeginInvokeOnMainThread(() =>
        //            //    {
        //            //        //steps.TranslationY = Math.Max(0,steps.TranslationY + e.TotalY);
        //            //        steps.TranslateTo(steps.X, steps.TranslationY + e.TotalY);
        //            //    });
        //            //}, 2);

        //            break;
        //        case GestureStatus.Completed:
        //            if (direction > 0) // snap to max/min, you could use an animation....
        //            {
        //                //steps.TranslateTo(steps.X, Content.Height - steps.Height);
        //                //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.XProportional);
        //                //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.YProportional);
        //                //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.WidthProportional);
        //                AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, Content.Height - stepsHeightMax, 1, AbsoluteLayout.AutoSize));
        //                //labelDiaChiKeTiep.IsVisible = false;
        //                //btnHoanTat.IsVisible = false;
        //                //tieude.IsVisible = false;
        //                //labelGioDenYeuCau.IsVisible = false;
        //                //labelThuNho.IsVisible = true;
        //                up_down.Source = "down_arrow.png";
        //                //swipeLabel.Text = "Swipe me down";
        //            }
        //            else
        //            {
        //                //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.XProportional);
        //                //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.YProportional);
        //                //AbsoluteLayout.SetLayoutFlags(steps, AbsoluteLayoutFlags.WidthProportional);
        //                AbsoluteLayout.SetLayoutBounds(steps, new Rectangle(0, Content.Height - stepsHeightMin, 1, AbsoluteLayout.AutoSize));
        //                //labelDiaChiKeTiep.IsVisible = true;
        //                //btnHoanTat.IsVisible = true;
        //                //tieude.IsVisible = true;
        //                //labelGioDenYeuCau.IsVisible = true;
        //                //labelThuNho.IsVisible = false;
        //                up_down.Source = "up_arrow.png";
        //                //swipeLabel.Text = "Swipe me up";
        //            }
        //            break;
        //    }
        //}

        private void GoiDienThoai(object sender, EventArgs e)
        {
            ImageButton imageButton = sender as ImageButton;
            var Info = imageButton.BindingContext as NguoiDi;
            string SoDienThoai = Info.SdtLienLac;
            try
            {
                PhoneDialer.Open(SoDienThoai);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }

        private void ChiTietNguoiDi(object sender, ItemTappedEventArgs e)
        {
            var Item = e.Item as ThongTinDiemDung;
            var DsNguoiDi = Item.DsNguoiDi;

            var DsNguoiLen = DsNguoiDi.Where(w => w.LenXe == true);
            var DsNguoiXuong = DsNguoiDi.Where(w => w.XuongXe == true);
            lvNguoiLen.HeightRequest = DsNguoiLen.Count() * lvNguoiLen.RowHeight + 10;
            lvNguoiXuong.HeightRequest = DsNguoiXuong.Count() * lvNguoiXuong.RowHeight + 10;
            lvNguoiLen.ItemsSource = DsNguoiLen;
            lvNguoiXuong.ItemsSource = DsNguoiXuong;

            lbLenxe.IsVisible = DsNguoiLen.Any();
            lbXuongxe.IsVisible = DsNguoiXuong.Any();

            popupcontent.IsVisible = true;
            this.popuplayout.AnchorX = 0.5;
            this.popuplayout.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => this.popuplayout.Scale = f,
                0.5,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => this.popuplayout.Opacity = f,
                0.2,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(this.popuplayout, "popupScaleAnimation", 250);
            fadeAnimation.Commit(this.popuplayout, "popupFadeAnimation", 250);

            if (e.Item == null) return;
            Task.Delay(500);
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private async void close(object sender, EventArgs e)
        {
            await Task.WhenAny<bool>(this.popuplayout.FadeTo(0, 200, Easing.SinInOut));
            popupcontent.IsVisible = false;
        }

        private void listNguoiDi_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null) return;
            Task.Delay(500);
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Page1());
        }

        private async void xacnhantaixe_Clicked(object sender, EventArgs e)
        {
            foreach (var dieuxe in (List<Dieuxe>)lvDsDieuXe.ItemsSource)
            {
                if(dieuxe.SelectItem == null)
                {
                    int index = ((List<Dieuxe>)lvDsDieuXe.ItemsSource).IndexOf(dieuxe);
                    var option = await DisplayAlert("Thiếu thông tin", "Hãy chọn tài xế cho chuyến đi " + index +"!", "Đã hiểu", "Chọn sau");
                    if (!option)
                        tatPopUp(stackchontaixe);
                    break;
                }
            }
        }

        void hienPopUp(StackLayout stack)
        {
            stack.IsVisible = true;
            var popuplayout = stack.Children[0] as Frame;
            popuplayout.AnchorX = 0.5;
            popuplayout.AnchorY = 0.5;

            Animation scaleAnimation = new Animation(
                f => popuplayout.Scale = f,
                0.5,
                1,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => popuplayout.Opacity = f,
                0.2,
                1,
                Easing.SinInOut);

            scaleAnimation.Commit(popuplayout, "popupScaleAnimation", 250);
            fadeAnimation.Commit(popuplayout, "popupFadeAnimation", 250);
        }
        async void tatPopUp(StackLayout stack)
        {
            var popuplayout = stack.Children[0] as Frame;
            await Task.WhenAny<bool>(popuplayout.FadeTo(0, 200, Easing.SinInOut));
            stack.IsVisible = false;
        }

        private void btnNhapChiPhi_Clicked(object sender, EventArgs e)
        {

        }

        private async void tatframeCapNhatChiPhi(object sender, EventArgs e)
        {
            //await Task.WhenAny<bool>(this.popuplayout.FadeTo(0, 200, Easing.SinInOut));

            

            Animation scaleAnimation = new Animation(
                f => frameCapNhatChiPhi.Scale = f,
                1,
                0,
                Easing.SinInOut);

            Animation fadeAnimation = new Animation(
                f => frameCapNhatChiPhi.Opacity = f,
                1,
                0,
                Easing.SinInOut);

            scaleAnimation.Commit(frameCapNhatChiPhi, "popupScaleAnimation", 250);
            fadeAnimation.Commit(frameCapNhatChiPhi, "popupFadeAnimation", 250);

            //popuplayout.IsVisible = false;
        }

        private void xacNhanChiPhi(object sender, EventArgs e)
        {

        }

        private void hienFrame(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Equals("False"))
            {
                frameCapNhatChiPhi.IsVisible = true;
                frameCapNhatChiPhi.AnchorX = 0;
                frameCapNhatChiPhi.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => frameCapNhatChiPhi.Scale = f,
                    0,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => frameCapNhatChiPhi.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(frameCapNhatChiPhi, "popupScaleAnimation", 250);
                fadeAnimation.Commit(frameCapNhatChiPhi, "popupFadeAnimation", 250);
            }
        }
    }
}