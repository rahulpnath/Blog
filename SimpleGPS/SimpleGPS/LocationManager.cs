using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace SimpleGPS
{
   public class LocationManager
    {
       public static string bingApiKey = "BING MAPS API KEY";

       public static void GetLocationName(Action<String> callback, string latitude, string longitude)
       {
           string url = string.Format("http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?o=json&key={2}",
                       latitude, longitude, bingApiKey);
           WebClient webClient = new WebClient();
           webClient.DownloadStringCompleted += (p, q) =>
           {
               if (q.Error == null)
               {
                   // do something
                   DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(LocationQueryResponse));
                   MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(q.Result));

                   LocationQueryResponse obj = (LocationQueryResponse)json.ReadObject(stream);
                   string message = String.Empty;
                   if(obj.resourceSets!= null && obj.resourceSets.Length > 0 && obj.resourceSets[0].resources.Length > 0)
                   {
                      message= obj.resourceSets[0].resources[0].name;
                   }
                   callback(message);
               }
               else
               {
                  
               }
           };
           webClient.DownloadStringAsync(new Uri(url));
       

       }

       public static void GetLocationCordinates(Action<SimpleGPS.LocationQueryResponse.ResourceSet[]> callback,  string name)
       {
           string url = "http://dev.virtualearth.net/REST/v1/Locations?q={0}&o=json&key={1}";
           url = string.Format(url, name, bingApiKey);
           WebClient webClient = new WebClient();
           webClient.DownloadStringCompleted += (p, q) =>
           {
               if (q.Error == null)
               {
                   // do something
                   DataContractJsonSerializer json = new DataContractJsonSerializer(typeof(LocationQueryResponse));
                   MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(q.Result));

                   LocationQueryResponse obj = (LocationQueryResponse)json.ReadObject(stream);
                   callback(obj.resourceSets);
               }
               else
               {

               }
           };
           webClient.DownloadStringAsync(new Uri(url));
       }
    }
}
