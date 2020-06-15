using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;
using System.Net.Http;
using dieuxe.Models;
using dieuxe.Helpers;
using GalaSoft.MvvmLight;
using Polyline = Xamarin.Forms.Maps.Polyline;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace dieuxe.ViewModels
{
    class Point
    {
        public Coordinate location { get; set; }
        public double distance { get; set; }
        public int id { get; set; }
    }
    public class QuanLyVM
    {
        List<lich> lichdangky;
        public List<lich> DanhSachDangKy
        {
            get { return lichdangky; }
            set { lichdangky = value; }
        }
        public ICommand phantichcum { get; private set; }
        public ICommand test { get; private set; }

        Map map;
        MapFunction mapfunction;
        public string response = "response";
        public QuanLyVM()
        {//key chùa: AIzaSyB5W0x1aKzeLeeX2KeFeDk09WnJ53xFQQg
            // key bản quyền: AIzaSyBkWiDHc4rH47SLkGKdtbhNHR6EgqNfzrs
            //this.test = new Command(testfunction);

            lichdangky = new List<lich>() {
                new lich { sodangky = 0, ngaydangky = "25/03/2020", nhanviendangky = "SK10", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.845966, 106.765224", gioden = "08:00 28/03/2020", giove = "16:00 28/03/2020" },
                new lich { sodangky = 1, ngaydangky = "25/03/2020", nhanviendangky = "SK06", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.838085, 106.634272", gioden = "08:00 27/03/2020", giove = "15:00 27/03/2020" },
                new lich { sodangky = 2, ngaydangky = "18/03/2020", nhanviendangky = "SK06", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.769082, 106.702199", gioden = "08:00 20/03/2020", giove = "15:00 20/03/2020" },
                new lich { sodangky = 3, ngaydangky = "15/03/2020", nhanviendangky = "SK05", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.769082, 106.702199", gioden = "08:00 18/03/2020", giove = "16:00 18/03/2020" },
                new lich { sodangky = 4, ngaydangky = "15/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.832282, 106.778043", noiden = "10.868034, 106.780647", gioden = "08:00 19/03/2020", giove = "15:00 19/03/2020" },
                new lich { sodangky = 5, ngaydangky = "16/03/2020", nhanviendangky = "SK07", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.838781, 106.672548", gioden = "08:00 18/03/2020", giove = "17:00 18/03/2020" },
                new lich { sodangky = 6, ngaydangky = "18/03/2020", nhanviendangky = "SK06", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.769082, 106.702199", gioden = "08:00 20/03/2020", giove = "15:00 20/03/2020" },
                new lich { sodangky = 7, ngaydangky = "19/03/2020", nhanviendangky = "SK05", songuoi = 2, noidi = "10.832282, 106.778043", noiden = "10.835409, 106.635244", gioden = "08:00 23/03/2020", giove = "15:00 23/03/2020" },
                new lich { sodangky = 8, ngaydangky = "21/03/2020", nhanviendangky = "SK09", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.884010, 106.586954", gioden = "08:00 24/03/2020", giove = "16:00 24/03/2020" },
                new lich { sodangky = 9, ngaydangky = "24/03/2020", nhanviendangky = "SK07", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.787637, 106.686105", gioden = "08:00 27/03/2020", giove = "17:00 27/03/2020" },
                new lich { sodangky = 10, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.832282, 106.778043", noiden = "10.803820, 106.689993", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 11, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.803109, 106.737720", noiden = "10.842808, 106.615312", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 12, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.886754, 106.759661", noiden = "10.836570, 106.654286", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 13, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.774183, 106.722131", noiden = "10.813318, 106.578647", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 14, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.848119, 106.718481", noiden = "10.806229, 106.627437", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 15, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.795682, 106.675471", noiden = "10.832169, 106.621990", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" }
            };
        }
        public QuanLyVM(Map map)
        {
            this.map = map;
            mapfunction = new MapFunction("AIzaSyB5W0x1aKzeLeeX2KeFeDk09WnJ53xFQQg");
            phantichcum = new Command(phantich);
            this.test = new Command(testfunction);

            lichdangky = new List<lich>() {
                new lich { sodangky = 0, ngaydangky = "25/03/2020", nhanviendangky = "SK10", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.845966, 106.765224", gioden = "08:00 28/03/2020", giove = "16:00 28/03/2020" },
                new lich { sodangky = 1, ngaydangky = "25/03/2020", nhanviendangky = "SK06", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.838085, 106.634272", gioden = "08:00 27/03/2020", giove = "15:00 27/03/2020" },
                new lich { sodangky = 2, ngaydangky = "18/03/2020", nhanviendangky = "SK06", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.769082, 106.702199", gioden = "08:00 20/03/2020", giove = "15:00 20/03/2020" },
                new lich { sodangky = 3, ngaydangky = "15/03/2020", nhanviendangky = "SK05", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.769082, 106.702199", gioden = "08:00 18/03/2020", giove = "16:00 18/03/2020" },
                new lich { sodangky = 4, ngaydangky = "15/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.832282, 106.778043", noiden = "10.868034, 106.780647", gioden = "08:00 19/03/2020", giove = "15:00 19/03/2020" },
                new lich { sodangky = 5, ngaydangky = "16/03/2020", nhanviendangky = "SK07", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.838781, 106.672548", gioden = "08:00 18/03/2020", giove = "17:00 18/03/2020" },
                new lich { sodangky = 6, ngaydangky = "18/03/2020", nhanviendangky = "SK06", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.769082, 106.702199", gioden = "08:00 20/03/2020", giove = "15:00 20/03/2020" },
                new lich { sodangky = 7, ngaydangky = "19/03/2020", nhanviendangky = "SK05", songuoi = 2, noidi = "10.832282, 106.778043", noiden = "10.835409, 106.635244", gioden = "08:00 23/03/2020", giove = "15:00 23/03/2020" },
                new lich { sodangky = 8, ngaydangky = "21/03/2020", nhanviendangky = "SK09", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.884010, 106.586954", gioden = "08:00 24/03/2020", giove = "16:00 24/03/2020" },
                new lich { sodangky = 9, ngaydangky = "24/03/2020", nhanviendangky = "SK07", songuoi = 1, noidi = "10.832282, 106.778043", noiden = "10.787637, 106.686105", gioden = "08:00 27/03/2020", giove = "17:00 27/03/2020" },
                new lich { sodangky = 10, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.832282, 106.778043", noiden = "10.803820, 106.689993", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 11, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.803109, 106.737720", noiden = "10.842808, 106.615312", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 12, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.886754, 106.759661", noiden = "10.836570, 106.654286", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 13, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.774183, 106.722131", noiden = "10.813318, 106.578647", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 14, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.848119, 106.718481", noiden = "10.806229, 106.627437", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" },
                new lich { sodangky = 15, ngaydangky = "24/03/2020", nhanviendangky = "SK08", songuoi = 2, noidi = "10.795682, 106.675471", noiden = "10.832169, 106.621990", gioden = "08:00 27/03/2020", giove = "16:00 27/03/2020" }
            };
        }
        private async void testfunction()
        {
            string u = "https://maps.googleapis.com/maps/api/directions/json?origin=10.832282,106.778043&destination=10.83657,106.654286&waypoints=10.845966,106.765224|10.868034,106.780647|10.886754,106.759661&mode=driving&key=AIzaSyB5W0x1aKzeLeeX2KeFeDk09WnJ53xFQQg";
            string v = await mapfunction.GetJsonAsync(u);
            MapDirectionResponse m = JsonConvert.DeserializeObject<MapDirectionResponse>(v);
            var listpoint1 = MapFunction.Decode("wkcaAmrkiS?tAdBETDDJ?rABHNLnBHdD@bBA~BCAQQgD@{@ByA`@{Bd@kBf@{Cj@aDFu@?q@?[");
            //var listpoint = mapfunction.GetPolylinePoints(m);
            var listpoint2 = MapFunction.Decode("sweaAkmjjSUVw@SqA_@a@QsCkAeCiAoDyBmCeBqBqAs@_@kB}@yBu@qC}@gIaCyEsAuN_EeLaDeAQq@AgAB}@Lc@J_C|@kBv@e@H]A_@Ca@L]TK\\?XHPVNTHXXzBzF|BpFlFpM|BtF|GxObAjCzCbHlA`Dd@fBTlBHvA@zAEhAK`BIvAH?@?{@hNSjCq@lKcAvOsA~Sa@zHMzFWfJe@vOs@|WQ~HMzEItDG|@GnCGpBMvDKdECfACBEHCVQvCErBCdCErCCjFDnHFrL@nG?`LP@?~DO@?vEFdHDzB@zDL?@p@CvCCzCSpK_@nSKxDMdMIfD?pCFt@Dh@PfAj@tBf@hAfCxFpNt[tBxEd@nA`E~IzB~EvJnThC`GvE|KpBzEh@tAFXRn@bAbCvBpFpF`OfIhUdApCnAjCnAlBtAfBbCtC|BvCJf@f@f@rD`EbAjAv@|@AND^JRRLTD\\CVONWBO?O?I?EJCbCqAlKsFxLyGzJuFvUsMtQaKbGgDh@a@zBiBjG{GnDcExDcErBuBpAu@|@_@pAa@fCk@rCi@dAYd\\{GhJoBFTzDy@v@vHd@xGZjEPvAn@dCf@`BTl@HPrDKpACzCGdA@vAEhACjBOvCQtBS@I");
            var listpoint3 = MapFunction.Decode("ytbaAoavjS_@AIVN`AAVCF\\`@z@lAb@f@P^Lx@Rt@pCpDbCvC~EvF`@h@^ZbBbAh@p@lAbCx@fBXlC?v@SbAVfDcELu@DoAPiB^mBl@iBb@wBTkAPiAn@w@f@w@RgBNcEMe@Bi@Le@ToA`Au@p@a@h@{@~Ae@n@QIuHqDgCkAU?a@FgB^aDnAeCfAq@z@gDxF_AxA{@hAeDhByBdAmCgEoAsBo@cBk@oAg@g@cAAcADcBHm@MwKeF{DeBuBgARMj@e@P[La@sEsA}DcAcGiB[QcBuAo@a@[K@e@?wAF}@XqCKs@kAeDIu@w@Sg@]cAcB}CoGqCoFkC|@_CbAgBb@YFw@@qAJmBJELw@fCYhAm@fHKzBGh@CN_IuC_RsGyK{DcJyDcA]b@_Bz@iDfCkLbCwK`@gBOE\\gBZiCRoEJ{FLaLW?AN@pDOrIGbDKzBKtAYvBqB`J{BjK{EjSeEhRqHz[}@`EcAvDk@|BkCnLuDhPeAtG{BtSm@bG@pCHnAj@`D^|A\\|AR`AdDzNnCtMf@`Cb@~AhB~HrBhJj@hEb@pDNzBb@lGdAbIbAzDfBvEhBlEnHpRjCrFx@pB|@pB`@hBtB~FjD`JK\\?XHPVNTBx@UV[Vi@~@i@bBw@bAc@t@UjAOlA?nAJpHtBvYfI`MrDrEhBrAr@vG`EpChBfBx@lAf@rCfAhCv@zBTxWEbBBtAPhBl@dJbHNNNKe@_@aFuDuAgAqAo@aAWeAOaECqI@aGBcA@mBMk@GQCPSBCCBQRc@Iu@UeA_@sCkAwAm@uC_BuBsAoFkDaBw@iBw@eDeAaI_CqLgDiL_DmF_BeCk@s@G_CHaARaE`B{@Zi@@]Ea@Fc@VKRE^P^t@^`@x@pCdHtGtObAjClGdOnBpEzDfJzApDp@vBThARbCD~BMtCMlBJ?eBhXkAxQcCv`@e@tO_@~MgApb@a@~PSpGa@rNKRQ|CS~M@pH@zBDdKBhU?|@P@?~DO@A~D@l@LjK@zDL??|DCrAOvIQpK[zNKlKMrGCbBBtAJlAN|@b@fBxBbFjP`_@zBdFdBbErFxLhFjL`FzKpIdS`BdEXbAt@hBdFzMzKpZfBbF`A|B|ApCn@~@nDhE~BzCTVJf@bAdAbErEhApABNB`@HTPNTFZ?XKT[@I@U?SZM|HeEzJiFjFwChCuAzPsJfWsN~NmIpCwB~EiFxEsFjDuDxB}B`Ao@`Bw@hD_ArE}@vA_@zTqEzK_CtCm@FTzDy@p@~GN`B~@lMNbApAhE^~@pFMrCGdDCnDQvGe@NC@I");
            //System.Console.WriteLine("kc: " + m.routes[0].legs[0].distance.value);
            Polyline polyline1 = new Polyline
            {
                StrokeWidth = 8,
                StrokeColor = Color.FromHex("#000001F"),
                //FillColor = Color.FromHex("#881BA1E2"),
                Geopath = { }
            };
            foreach (Coordinate point in listpoint1)
            {
                polyline1.Geopath.Add(new Position(point.Latitude, point.Longitude));
            }
            map.MapElements.Add(polyline1);

            //Polyline polyline2 = new Polyline
            //{
            //    StrokeWidth = 10,
            //    StrokeColor = Color.FromHex("#1BA1E2"),
            //    //FillColor = Color.FromHex("#881BA1E2"),
            //    Geopath = { }
            //};
            //foreach (Coordinate point in listpoint2)
            //{
            //    polyline2.Geopath.Add(new Position(point.Latitude, point.Longitude));
            //}
            //map.MapElements.Add(polyline2);

            //Polyline polyline3 = new Polyline
            //{
            //    StrokeWidth = 9,
            //    StrokeColor = Color.FromHex("#00FF01"),
            //    //FillColor = Color.FromHex("#881BA1E2"),
            //    Geopath = { }
            //};
            //foreach (Coordinate point in listpoint3)
            //{
            //    polyline3.Geopath.Add(new Position(point.Latitude, point.Longitude));
            //}
            //map.MapElements.Add(polyline3);

        }
        private async void phantich()
        {
            const double eps = 5;
            const int minPts = 2;
            
            //data
            //List<lich> 

            List<List<Point>> ketqua = new List<List<Point>>();
            int[] status = new int[lichdangky.Count];   //mặc định tất cả các lịch đăng ký chưa được xét


            if (lichdangky.Count > 1)
            {
                List<Coordinate> Original_point = new List<Coordinate>();
                List<Coordinate> Destination_point = new List<Coordinate>();
                foreach (lich dangky in lichdangky)
                {
                    Coordinate point1 = new Coordinate();
                    Coordinate point2 = new Coordinate();
                    double[] toadonoidi = CoordinatesToArray(dangky.noidi);
                    point1.Latitude = toadonoidi[0];
                    point1.Longitude = toadonoidi[1];
                    Original_point.Add(point1);

                    double[] toadonoiden = CoordinatesToArray(dangky.noiden);
                    point2.Latitude = toadonoiden[0];
                    point2.Longitude = toadonoiden[1];
                    Destination_point.Add(point2);

                }

                DBSCAN Original = new DBSCAN(Original_point, eps, minPts);
                DBSCAN Destination = new DBSCAN(Destination_point, eps, minPts);

                List<List<int>> OrgCluster = Original.Clusters;
                List<int> OrgNoise = Original.Noise;

                List<List<int>> DesCluster = Destination.Clusters;
                List<int> DesNoise = Destination.Noise;

                //foreach (var item in OrgCluster)
                //{
                //    System.Console.Write("\nCụm đi: ");
                //    foreach (var p in item)
                //    {
                //        System.Console.Write(p + ", ");
                //    }
                //}
                //foreach (var item in DesCluster)
                //{
                //    System.Console.Write("\nCụm đến: ");
                //    foreach (var p in item)
                //    {
                //        System.Console.Write(p + ", ");
                //    }
                //}

                foreach (int on in OrgNoise)      //here
                {
                    Pin pin = new Pin
                    {
                        Position = new Position(Original_point[on].Latitude, Original_point[on].Longitude),
                        Label = "ID ORG noise: " + on
                    };
                    map.Pins.Add(pin);
                }
                foreach (int dn in DesNoise)
                {
                    Pin pin = new Pin
                    {
                        Position = new Position(Destination_point[dn].Latitude, Destination_point[dn].Longitude),
                        Label = "ID DES noise: " + dn
                    };
                    map.Pins.Add(pin);
                }

                for (int k = 0; k < OrgCluster.Count; k++)
                {
                    List<int> Ocluster = OrgCluster[k];
                    for (int l = 0; l < DesCluster.Count; l++)
                    {
                        List<int> Dcluster = DesCluster[l];
                        int countMatch = 0;
                        List<int> id = new List<int>();

                        foreach (int Opoint in Ocluster)
                        {
                            foreach (int Dpoint in Dcluster)
                            {
                                if (Opoint == Dpoint)
                                {
                                    id.Add(Opoint);
                                    countMatch++;
                                }
                            }
                        }

                        if (countMatch >= 2)
                        {
                            List<Point> list = new List<Point>();
                            Coordinate org = new Coordinate(Original_point[id[0]].Latitude, Original_point[id[0]].Longitude);
                            List<Coordinate> originlist = new List<Coordinate>();
                            originlist.Add(org);
                            List<Coordinate> destinationlist = new List<Coordinate>();
                            list.Add(new Point
                            {
                                location = org,
                                distance = 0.0,
                                id = id[0]
                            });

                            foreach (int orgpoint in id)
                            {
                                status[orgpoint] = 1;   //đánh dấu lịch đăng ký này là đã xét

                                Coordinate newlocation = new Coordinate(Original_point[orgpoint].Latitude, Original_point[orgpoint].Longitude);
                                bool isExist = false;
                                foreach (var point in list)
                                {
                                    if (newlocation.IsSame(point.location))
                                    {
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (isExist)
                                    continue;
                                list.Add(new Point
                                {
                                    location = newlocation,
                                    distance = 0,
                                    id = orgpoint
                                });
                                destinationlist.Add(newlocation);
                            }

                            foreach (int despoint in id)
                            {
                                Coordinate newlocation = new Coordinate(Destination_point[despoint].Latitude, Destination_point[despoint].Longitude);
                                bool isExist = false;
                                foreach (var point in list)
                                {
                                    if (newlocation.IsSame(point.location))
                                    {
                                        isExist = true;
                                        break;
                                    }
                                }
                                if (isExist)
                                    continue;
                                list.Add(new Point
                                {
                                    location = newlocation,
                                    distance = 0,
                                    id = despoint + lichdangky.Count
                                });
                                destinationlist.Add(newlocation);
                                //double dist = await mapfunction.GetDistanceAsync(org, newlocation);
                                //for (int i = 0; i < list.Count; i++)
                                //{
                                //    if (dist > list[i].distance)
                                //    {
                                //        if (i == list.Count - 1)
                                //        {
                                //            list.Add(new
                                //            {
                                //                location = newlocation,
                                //                distance = dist,
                                //                id = despoint + lichdangky.Count
                                //            });
                                //            break;
                                //        }
                                //        else
                                //        {
                                //            continue;
                                //        }
                                //    }
                                //    list.Add(list[list.Count - 1]);
                                //    for (int j = list.Count - 2; j > i; j--)
                                //    {
                                //        list[j] = list[j - 1];
                                //    }
                                //    list[i] = new
                                //    {
                                //        location = new Coordinate(Destination_point[despoint].Latitude, Destination_point[despoint].Longitude),
                                //        distance = dist,
                                //        id = despoint + lichdangky.Count
                                //    };
                                //    break;
                                //}
                            }

                            //lấy ma trận khoảng cách
                            List<List<double>> distancematrix = await mapfunction.GetDistanceMatrix(originlist, destinationlist);

                            //sx list theo khoảng cách
                            List<double> distances = distancematrix[0];

                            for (int i = 0; i < distances.Count - 1; i++)
                            {
                                double mindistance = distances[i];
                                int index = i;
                                for (int j = i+1; j < distances.Count; j++)
                                {
                                    double distance = distances[j];
                                    if (distance < mindistance)
                                    {
                                        mindistance = distance;
                                        index = j;
                                    }
                                }
                                list[index+1].distance = mindistance;
                                if (index != i)
                                {
                                    var temp = list[i+1];
                                    list[i+1] = list[index+1];
                                    list[index+1] = temp;

                                    distances[index] = distances[i];
                                    distances[i] = mindistance;
                                }
                            }
                            list[list.Count - 1].distance = distances[distances.Count - 1];

                            //System.Console.WriteLine("listcount: "+list.Count);
                            //for (int i = 0; i < list.Count; i++)
                            //{
                            //    System.Console.WriteLine("id point: {0}", list[i].id);
                            //    System.Console.WriteLine("kc: {0}", list[i].distance);
                            //}

                            //int stt = 0;
                            //foreach (var item in list)
                            //{
                            //    Pin pin = new Pin
                            //    {
                            //        Position = new Position(item.location.Latitude, item.location.Longitude),
                            //        Label = "stt: " + stt
                            //    };
                            //    map.Pins.Add(pin);
                            //    stt++;

                            //}

                            //sắp xếp tiếp theo phương hướng
                            await sx(list);

                            Coordinate origin = list[0].location, destination_c1 = list[list.Count - 1].location, destination_c2 = list[list.Count - 2].location;

                            List<Coordinate> waypoints_c1 = new List<Coordinate>(), waypoints_c2 = new List<Coordinate>();
                            for (int i = 1; i < list.Count - 1; i++)
                            {
                                waypoints_c1.Add(list[i].location);
                            }

                            for (int i = 1; i < list.Count; i++)
                            {
                                if (i == (list.Count - 2))
                                    continue;
                                waypoints_c2.Add(list[i].location);
                            }

                            double dist1 = await mapfunction.GetDistanceAsync(origin, destination_c1, waypoints_c1), dist2 = await mapfunction.GetDistanceAsync(origin, destination_c2, waypoints_c2);

                            if (dist1 > dist2)
                            {
                                int length = list.Count;
                                var temp = list[length - 2];
                                list[length - 2] = list[length - 1];
                                list[length - 1] = temp;
                            }

                            ketqua.Add(list);

                            //lấy thông tin chuyến đi sau cùng
                            var property_list = get_ORG_DES_WAY(list);
                            Coordinate destination = property_list.destination;
                            List<Coordinate> waypoints = property_list.waypoints;

                            MapDirectionResponse mapDirectionResponse = await mapfunction.GetDirectionJsonAsync(origin, destination, waypoints);
                            if (mapDirectionResponse.routes.Count > 0)
                            {
                                var listpoint = mapfunction.GetPolylinePoints(mapDirectionResponse);
                                //List<Position> geopath = new List<Position>();
                                //foreach (Coordinate point in listpoint)
                                //{
                                //    geopath.Add(new Position(point.Latitude, point.Longitude));
                                //}
                                Polyline polyline = new Polyline
                                {
                                    StrokeWidth = 8,
                                    StrokeColor = Color.FromHex("#1BA1E2"),
                                    //FillColor = Color.FromHex("#881BA1E2"),
                                    Geopath = { }
                                };
                                foreach (Coordinate point in listpoint)
                                {
                                    polyline.Geopath.Add(new Position(point.Latitude, point.Longitude));
                                }

                                map.MapElements.Add(polyline);
                                Polyline dd = (Polyline)map.MapElements.ElementAt(1);
                                double d = 0;
                                foreach (Leg leg in mapDirectionResponse.routes[0].legs)
                                {
                                    d += leg.distance.value;
                                }
                                System.Console.WriteLine("tổng quãng đường: {0}", d);
                            }
                            //dùng hoán vị
                            // var pointList = [];
                            // for (const point of id) {
                            //     pointList.push( new google.maps.LatLng(Original_point[point].latitude, Original_point[point].longitude));
                            // }
                            // for (const point of id) {
                            //     pointList.push( new google.maps.LatLng(Destination_point[point].latitude, Destination_point[point].longitude));
                            // }
                            // //lấy tất cả trường hợp hoán vị
                            // k = -1;
                            // let dshoanvi = [];
                            // //permutation(pointList, 0, pointList.length, dshoanvi);
                            // alert(pointList.length);

                            //lặp từng hoán vị để tìm tối ưu
                            // let mindistance, bestOption;
                            // for (let i = 0; i < dshoanvi.length; i++) {
                            //     let order = dshoanvi[i];
                            //     let origin = order[0], destination = order[order.length-1], waypoints = [];
                            //     for (let i = 1; i < order.length-1; i++) {
                            //         waypoints.push({
                            //             location: order[i],
                            //             stopover: false
                            //         });
                            //     }
                            //     if(i == 0)
                            //         mindistance = getdistance(origin, destination, waypoints);
                            //     else{
                            //         let distance = getdistance(origin, destination, waypoints);
                            //         if(distance < mindistance){
                            //             // mindistance = distance;
                            //             bestOption = order;
                            //         }
                            //     }
                            // }

                            // let origin = bestOption[0], destination = bestOption[bestOption.length-1], waypoints = [];
                            // for (let i = 1; i < bestOption.length-1; i++) {
                            //     waypoints.push({
                            //         location: bestOption[i],
                            //         stopover: false
                            //     });
                            // }


                            //let request = {
                            //    origin: origin,
                            //    destination: destination,
                            //    waypoints: waypoints,
                            //    travelMode: 'DRIVING'
                            //};
                            //setTimeout(() =>
                            //{
                            //    directionsService.route(request, function(result, status) {
                            //        // if(status == "OVER_QUERRY_LIMIT")
                            //        //     time.sleep(2);
                            //        alert(status);
                            //        if (status == 'OK')
                            //        {
                            //            directionsRenderer.setDirections(result);

                            //            // alert(result.routes[0].legs[0].distance.value);
                            //            // waypoints.forEach(element => {
                            //            //     var uluru = {lat: element.location.lat(), lng: element.location.lng()};
                            //            //     var marker = new google.maps.Marker({position: uluru, map: map, title: "ID:"});
                            //            // });

                            //            list.forEach(element =>
                            //            {
                            //                var uluru = { lat: element.location.lat(), lng: element.location.lng()};
                            //                var marker = new google.maps.Marker({ position: uluru, map: map, title: "ID: " + element.i});
                            //            });
                            //        }
                            //    });
                            // }, 2001);

                        }
                    }
                }



                //System.Console.WriteLine("beforecon: " + list.Count);
                //thêm noise, cluster

                //for (int id = 0; id < lichdangky.Count; id++)
                //{
                //    if(status[id] != 0)  //chưa xét
                //    {
                //        await ConsiderAddTripToCluster(id);
                //    }
                //}

                //foreach (int noise in OrgNoise)
                //{
                //    System.Console.WriteLine("id: {0}", noise);
                //    list = new List<Point>(await ConsiderAddTripToCluster(noise, list));
                //}
                //foreach (int noise in DesNoise)
                //{
                //    System.Console.WriteLine("id: {0}", noise);
                //    list = new List<Point>(await ConsiderAddTripToCluster(noise, list));
                //}
                //System.Console.WriteLine("aftercon: " + list.Count);

                //here

                //for (int index = 0; index < OrgCluster.Count; index++)
                //{
                //    List<int> orgcluster = OrgCluster[index];
                //    if (orgcluster == Ocluster)
                //        continue;
                //    foreach (int idtrip in orgcluster)
                //    {
                //        //setTimeout(() =>
                //        //{
                //        ConsiderAddTripToCluster(idtrip, list, Original_point, Destination_point);
                //        //}, 1000);
                //    }
                //}

                ////lấy thông tin chuyến đi sau cùng
                //var property_list = get_ORG_DES_WAY(list);
                //Coordinate destination = property_list.destination;
                //List<Coordinate> waypoints = property_list.waypoints;

                //MapDirectionResponse mapDirectionResponse = await mapfunction.GetDirectionJsonAsync(origin, destination, waypoints);
                //if (mapDirectionResponse.routes.Count > 0)
                //{
                //    var listpoint = mapfunction.GetPolylinePoints(mapDirectionResponse);
                //    //List<Position> geopath = new List<Position>();
                //    //foreach (Coordinate point in listpoint)
                //    //{
                //    //    geopath.Add(new Position(point.Latitude, point.Longitude));
                //    //}
                //    Polyline polyline = new Polyline
                //    {
                //        StrokeWidth = 8,
                //        StrokeColor = Color.FromHex("#1BA1E2"),
                //        //FillColor = Color.FromHex("#881BA1E2"),
                //        Geopath = { }
                //    };
                //    foreach (Coordinate point in listpoint)
                //    {
                //        polyline.Geopath.Add(new Position(point.Latitude, point.Longitude));
                //    }

                //    map.MapElements.Add(polyline);
                //    double d = 0;
                //    foreach (Leg leg in mapDirectionResponse.routes[0].legs)
                //    {
                //        d += leg.distance.value;
                //    }
                //    System.Console.WriteLine("tổng quãng đường: {0}", d);
                //}


                System.Console.WriteLine("tổng số request: " + mapfunction.CountRequest);
            }


            async Task sx(List<Point> list)
            {
                System.Console.WriteLine("hàm sắp xếp");
                //PolylineOptions d 
                int listcount = list.Count;
                int datacount = lichdangky.Count;
                bool thaydoidiembatdau = false;
                for (int i = 0; i <= listcount - 3; i++)
                {
                    //giả sử các điểm tại vị trí i = 1, 2, 3 là p1->p2->p3
                    Coordinate p1 = new Coordinate(list[i].location.Latitude, list[i].location.Longitude);
                    Coordinate p2 = new Coordinate(list[i + 1].location.Latitude, list[i + 1].location.Longitude);
                    Coordinate p3 = new Coordinate(list[i + 2].location.Latitude, list[i + 2].location.Longitude);
                    if (list[i].id <= datacount - 2)
                    {
                        Vector vector21 = layVector(p2, p1), vector23 = layVector(p2, p3), vector12 = layVector(p1, p2), vector13 = layVector(p1, p3);
                        double goc2 = timgoc(vector21, vector23), goc1 = timgoc(vector12, vector13);
                        if (goc2 < goc1)
                        {
                            list.Reverse(0, i + 1);
                            thaydoidiembatdau = true;
                        }
                        if (list[i].id == datacount - 2 && thaydoidiembatdau)
                        {
                            //cập nhật khoảng cách
                            list[0].distance = 0.0;
                            List<Coordinate> orglst = new List<Coordinate>() { list[0].location };
                            List<Coordinate> deslst = new List<Coordinate>();
                            for (int ind = 0; ind < listcount; ind++)
                            {
                                deslst.Add(list[ind].location);
                            }
                            List<List<double>> distancematrix = await mapfunction.GetDistanceMatrix(orglst, deslst);
                            for (int j = 1; j < listcount; j++)
                            {
                                list[j].distance = distancematrix[0][j-1];
                            }
                        }
                    }
                    else
                        if (!thutu3diem(p1, p2, p3))
                        {   //nếu góc tại điểm p3 lớn hơn góc tại điểm p2 thì p3 nằm giữa p1 vs p2
                            if (i == listcount - 3)
                            {
                                var temp = list[i + 1];
                                list[i + 1] = list[i + 2];
                                list[i + 2] = temp;
                                break;
                            }
                            //có 2 trường hợp: 1->3->2 hoặc 2->3->1
                            Coordinate p4 = new Coordinate(list[i + 3].location.Latitude, list[i + 3].location.Longitude);
                            //TH 1->3->2, xét p3, p2, p4
                            Vector vector23 = layVector(p2, p3), vector24 = layVector(p2, p4);
                            double anpha = timgoc(vector23, vector24);
                            //TH 2->3->1, xét p3, p1, p4
                            Vector vector13 = layVector(p1, p3), vector14 = layVector(p1, p4);
                            double beta = timgoc(vector13, vector14);
                            if (anpha >= beta)
                            {//chọn TH1
                                var temp = list[i + 1];
                                list[i + 1] = list[i + 2];
                                list[i + 2] = temp;
                            }
                            else
                            {  //TH2
                                var temp = list[i];
                                list[i] = list[i + 1];
                                list[i + 1] = list[i + 2];
                                list[i + 2] = temp;
                            }
                        }
                }
            }

            bool thutu3diem(Coordinate p1, Coordinate p2, Coordinate p3)
            {
                Vector vector21 = layVector(p2, p1), vector23 = layVector(p2, p3);
                double anpha = timgoc(vector21, vector23); //góc tại điểm p2
                Vector vector31 = layVector(p3, p1), vector32 = layVector(p3, p2);
                double beta = timgoc(vector31, vector32); //góc tại điểm p3
                if (beta > anpha)
                    return false;
                else
                    return true;
            }

            double timgoc(Vector vectorU, Vector vectorV)
            {
                //u, v là vector(x, y)
                double tichvohuong = vectorU.Lat * vectorV.Lat + vectorU.Lng * vectorV.Lng;
                double DoDaiVectorU = Math.Sqrt(Math.Pow(vectorU.Lat, 2) + Math.Pow(vectorU.Lng, 2));
                double DoDaiVectorV = Math.Sqrt(Math.Pow(vectorV.Lat, 2) + Math.Pow(vectorV.Lng, 2));
                double cosx = tichvohuong / (DoDaiVectorU * DoDaiVectorV);

                return Math.Acos(cosx) * 180 / Math.PI;
            }

            Vector layVector(Coordinate p1, Coordinate p2)
            {
                return new Vector(p2.Latitude - p1.Latitude, p2.Longitude - p1.Longitude);
            }

            double[] CoordinatesToArray(string coor)
            {
                string[] result = { "", "" };
                char[] charArray = coor.ToCharArray();
                var k = 0;
                for (var i = 0; i < charArray.Length; i++)
                {
                    if (charArray[i] != ',' && charArray[i] != ' ')
                        result[k] += charArray[i];
                    else
                        k = 1;
                }
                return new double[] { Convert.ToDouble(result[0]), Convert.ToDouble(result[1]) };
            }
            //here
            dynamic get_ORG_DES_WAY(List<Point> list)
            {
                Coordinate origin = list[0].location, destination = list[list.Count - 1].location;
                List<Coordinate> waypoints = new List<Coordinate>();
                for (int i = 1; i < list.Count - 1; i++)
                {
                    waypoints.Add(list[i].location);
                }
                return new { origin, destination, waypoints };
            }

            async Task<List<Point>> ConsiderAddTripToCluster(int IDtrip, List<Point> list)
            {
                System.Console.WriteLine("id noise: " + IDtrip);

                List<Point> temp_list = new List<Point>(list);
                double[] orgcoor = CoordinatesToArray(lichdangky[IDtrip].noidi), descoor = CoordinatesToArray(lichdangky[IDtrip].noiden);
                Coordinate Org_noise_point = new Coordinate(orgcoor[0], orgcoor[1]);
                Coordinate Des_noise_point = new Coordinate(descoor[0], descoor[1]);
                await addPointToList(IDtrip, Org_noise_point, temp_list);
                System.Console.WriteLine("sau hàm thêm");
                await addPointToList(IDtrip+lichdangky.Count, Des_noise_point, temp_list);
                //System.Console.WriteLine("temlistcount sau khi thêm: " + temp_list.Count);
                await sx(temp_list);
                System.Console.WriteLine("sau hàm sắp xếp");
                var property_before_add = get_ORG_DES_WAY(list);
                var property_after_add = get_ORG_DES_WAY(temp_list);
                System.Console.WriteLine("num wp trước: " + property_before_add.waypoints.Count);
                System.Console.WriteLine("num wp sau: " + property_after_add.waypoints.Count);
                System.Console.WriteLine("trước");
                double distance_before_add = await mapfunction.GetDistanceAsync(property_before_add.origin, property_before_add.destination, property_before_add.waypoints);
                double distance_noise = await mapfunction.GetDistanceAsync(Org_noise_point, Des_noise_point);
                System.Console.WriteLine("sau");
                double distance_after_add = await mapfunction.GetDistanceAsync(property_after_add.origin, property_after_add.destination, property_after_add.waypoints);
                System.Console.WriteLine("be: {0}, noise: {1}, af: {2}, ratio: {3}", distance_before_add, distance_noise, distance_after_add, distance_before_add / distance_after_add);
                if (distance_after_add < distance_before_add + distance_noise && distance_before_add / distance_after_add >= 0.8)
                {
                    System.Console.WriteLine("TRUE");
                    return temp_list;
                }
                else
                    System.Console.WriteLine("FALSE");
                return list;

            }

            async Task addPointToList(int idPoint, Coordinate point, List<Point> list_point)
            {
                System.Console.WriteLine("add function");
                Coordinate orgpoint = list_point[0].location;
                double dist = await mapfunction.GetDistanceAsync(orgpoint, point);
                for (int i = 1; i < list_point.Count; i++)
                {
                    if (dist > list_point[i].distance)
                        if (i == list_point.Count - 1)
                        {
                            list_point.Add(new Point
                            {
                                location = point,
                                distance = dist,
                                id = idPoint
                            });
                            break;
                        }
                        else
                            continue;

                    list_point.Add(list_point[list_point.Count - 1]);
                    for (int j = list_point.Count - 2; j > i; j--)
                    {
                        list_point[j] = list_point[j - 1];
                    }
                    list_point[i] = new Point
                    {
                        location = point,
                        distance = dist,
                        id = idPoint
                    };
                    break;
                }
            }
        }
    }
}