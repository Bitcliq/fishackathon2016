using Newtonsoft.Json.Linq;
using PCLCrypto;
using Plugin.Settings;
//using Refractored.Xam.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Services;


namespace PinFixed.BL
{
    public class StaticMethods
    {
        public static Dictionary<int, TypesWS> typesDic = new Dictionary<int, TypesWS>();

        public static Dictionary<int, PropertyWS> propertiesDic = new Dictionary<int, PropertyWS>();

        public static int NumNearest = 0;
        public static string  NumNearestError = "";

        public static bool CheckConnection()
        {
            NetworkStatus networkStatus;

            var currentDevice = Resolver.Resolve<IDevice>();
            try
            {
                networkStatus = currentDevice.Network.InternetConnectionStatus();

                if (networkStatus == NetworkStatus.NotReachable)
                {
                  

                    return false;

                }
                return true;
            }
            catch (Exception ex)
            {
                var exception = ex.Message + ": " + ex.StackTrace;
                return false;
            }

        }


        #region TYPES

        public static async Task<bool>  GetTypes()
        {

            IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
                              new BasicHttpBinding(),
                              new EndpointAddress(StaticKeys.WebServiceUrl));


            //client.GetTypesCompleted += OnGotResult;


            //client.GetTypesAsync(null);


             client.GetTypesAndSubTypesCompleted += OnGotResult;


             client.GetTypesAndSubTypesAsync(null);
            return true;

        }


        //static void OnGotResult(object sender, GetTypesCompletedEventArgs e)

        static void OnGotResult(object sender, GetTypesAndSubTypesCompletedEventArgs e)
       {
           
 
           Device.BeginInvokeOnMainThread(async () =>
           {
               string error = null;
               if (e.Error != null)
                   error = e.Error.Message;
               else if (e.Cancelled)
                   error = "Cancelled";

               if (!string.IsNullOrEmpty(error))
               {
                  
               }
               else
               {
                   typesDic = new Dictionary<int, TypesWS>();
                   var obj = JObject.Parse(e.Result);
                   var types = (JArray)obj["Data"];
                   if (types != null)
                   {
                      


                     

                       for (int i = 0; i < types.Count; i++)
                       {
                           JObject t = (JObject)types[i];
                           int? parentID = null;
                           try { 
                               if( t["ParentID"] + "" != "")
                                parentID  = Convert.ToInt32( t["ParentID"] + "");
                           }
                           catch
                           {
                           }

                           TypesWS tp = new TypesWS(Convert.ToInt32(t["ID"]), t["Name"] + "", parentID, t["ParentName"] + "");

                        

                           bool found = false;

                           foreach (var item in typesDic)
                           {

                               if (item.Value.ID == tp.ID)
                               {
                                   found = true;
                                   break;
                               }
                           }

                           if(!found)
                               typesDic.Add(i, tp);
                           
                       }
                   }

                   //resultsLabel.Text = e.Result;
               }
           });
       }


        #endregion


        #region PROPERTIES

        public static async Task<bool> GetPropeties()
        {

            IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
                              new BasicHttpBinding(),
                              new EndpointAddress(StaticKeys.WebServiceUrl));




            client.ListPropertiesCompleted += PropertiesOnGotResult;


             client.ListPropertiesAsync(null);
             return true;
        }


     
        static void PropertiesOnGotResult(object sender, ListPropertiesCompletedEventArgs e)
        {


            Device.BeginInvokeOnMainThread(async () =>
            {
                string error = null;
                if (e.Error != null)
                    error = e.Error.Message;
                else if (e.Cancelled)
                    error = "Cancelled";

                if (!string.IsNullOrEmpty(error))
                {
                    //await DisplayAlert("Error", error, "OK", "Cancel");
                }
                else
                {
                    propertiesDic = new Dictionary<int, PropertyWS>();
                    var obj = JObject.Parse(e.Result);
                    var types = (JArray)obj["Data"];
                    if (types != null)
                    {
                      
                        for (int i = 0; i < types.Count; i++)
                        {
                            JObject t = (JObject)types[i];


                            PropertyWS tp = new PropertyWS(Convert.ToInt32(t["PropertyID"]), t["PropertyName"] + "");

                            bool found = false;

                            foreach (var item in propertiesDic)
                            {

                                if (item.Value.ID == tp.ID)
                                {
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                                propertiesDic.Add(i, tp);

                        }
                    }

                }
            });
        }


        #endregion



        #region NEAREST
      


        public static void CountNearest(GPSUtils gps, double? latitude, double? longitude)
        {
           
           


                IssuesWebServiceSoapClient client = new IssuesWebServiceSoapClient(
                               new BasicHttpBinding(),
                               new EndpointAddress(StaticKeys.WebServiceUrl));



          

                  
                client.CountNearByIssuesCompleted += OnGotResultReport;




                if (latitude == null)
                {
                    if (gps != null)
                    {
                        if (gps.PositionLatitude != null)
                        {
                            try
                            {
                                latitude = Convert.ToDouble(gps.PositionLatitude);
                            }
                            catch
                            {
                               

                            }
                        }
                    }
                }

                if (longitude == null)
                {
                    if (gps != null)
                    {
                        if (gps.PositionLongitude != null)
                        {
                            try
                            {
                                longitude = Convert.ToDouble(gps.PositionLongitude);
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                int? _radius = null;
                int rd = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Radius", 100);
                string type = CrossSettings.Current.GetValueOrDefault(StaticKeys.APP_PREFIX + "Measure", "m");



                if (rd != -1)
                    _radius = rd;

                if (type != "m") // it is km
                {
                    if (_radius != null)
                        _radius = _radius * 1000;
                }

                client.CountNearByIssuesAsync(null, (double)latitude, (double)longitude, _radius);

           

        }

        static void OnGotResultReport(object sender, CountNearByIssuesCompletedEventArgs e)
        {
            //IsBusy = true;
            Device.BeginInvokeOnMainThread(async () =>
            {
                string error = null;
                if (e.Error != null)
                    error = e.Error.Message;
                else if (e.Cancelled)
                    error = "Cancelled";

                if (!string.IsNullOrEmpty(error))
                {
                    //await DisplayAlert("Error", error, "OK", "Cancel");

                    NumNearestError = error;
                }
                else
                {

                    var obj = JObject.Parse(e.Result);

                    if (obj["Data"] != null)
                    {
                        try
                        {
                            NumNearest = Convert.ToInt32(obj["Data"]);

                            

                        }
                        catch (Exception)
                        {
                            NumNearest = 0;
                        }
                    }
                    else
                    {
                        NumNearest = 0;
                    }
                }

            });
        }



        #endregion


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }





    }
}
