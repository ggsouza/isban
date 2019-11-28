using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace API_ISBAN.Model
{
    public static class HackNewsWrapper
    {
        private static Locker _Lock = new Locker();
        private static long _TickCount = 0;
        private static IEnumerable<dynamic> _CachedData = null; 

        private static void _GetHotData()
        {
            //geting stories list
            var xTempData = new Collection<dynamic>();
            var xStoriesList = WebInvoke.Get<int[]>("https://hacker-news.firebaseio.com/v0/beststories.json");
            var iLock = new Locker();

            //geting storie obj
            Parallel.ForEach(xStoriesList, (iStorie) =>
                                            {
                                                var iUrlStorie = $"https://hacker-news.firebaseio.com/v0/item/{iStorie}.json";
                                                var iStoreObj = WebInvoke.Get<dynamic>(iUrlStorie);

                                                lock(iLock)
                                                {
                                                    xTempData.Add(iStoreObj);
                                                }

                                            });


            _CachedData = xTempData.OrderByDescending(x => x.score).Take(20);
            _TickCount = Environment.TickCount;
        }

        private static IEnumerable<dynamic> _GetCachedData(int aAgeCacheSec)
        {
            IEnumerable<dynamic> xResult = null;

            try
            {
                lock (_Lock)
                {
                    var xAgeCached = Environment.TickCount - _TickCount;

                    if ((_CachedData == null) || (xAgeCached >= (aAgeCacheSec * 1000)))
                    {
                        _GetHotData();
                    }

                    xResult = _CachedData;
                }
            }
            catch
            {
                // do nothing
            }

            return xResult;
        }

        public static IEnumerable<dynamic> GetData(int aAgeCacheSec = 300)
        {
            return _GetCachedData(aAgeCacheSec);
        }
    }

    public static class WebInvoke
    {
        public static T Get<T>(string aUri)
        {
            T xResult;
            var xRequest = WebRequest.CreateHttp(aUri);

            using (var xMenStream = new MemoryStream())
            {
                using (var xStreamResponse = xRequest.GetResponse().GetResponseStream())
                {
                    xStreamResponse.CopyTo(xMenStream);
                }

                var xJson = Encoding.UTF8.GetString(xMenStream.ToArray());
                xResult = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(xJson);
            }

            return xResult;
        }
    }

    public class Locker
    {

    }
}
