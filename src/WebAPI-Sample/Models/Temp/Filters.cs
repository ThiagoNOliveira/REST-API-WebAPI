using System;
using System.Collections.Generic;
using System.Threading;

namespace WebApiSample.Models.Temp
{
    public class Filters
    {
        private static volatile Filters _instance = new Filters();
        private static readonly object SyncRoot = new object();

        private static readonly IDictionary<long, Dictionary<string, object>> _filters =
            new Dictionary<long, Dictionary<string, object>>();

        public static Filters Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    _instance = new Filters();
                }

                return _instance;
            }
        }

        public static long Add(Dictionary<string, object> filterProperties)
        {
            var key = DateTime.Now.Ticks;

            _filters.Add(key, filterProperties);

            StartExpirationTimeCount(key);

            return key;
        }

        public static Dictionary<string, object> Get(long key)
        {
            return _filters[key];
        }

        private static void StartExpirationTimeCount(long key)
        {
            ThreadPool.RegisterWaitForSingleObject(new AutoResetEvent(false), (state, timeout) => Remove(key), null,
                TimeSpan.FromSeconds(60), false);
        }

        private static void Remove(long key)
        {
            _filters.Remove(key);
        }
    }
}