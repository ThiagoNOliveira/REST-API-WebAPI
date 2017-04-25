using System.Collections.Generic;
using System.Linq;
using WebApiSample.Models.Entities;

namespace WebApiSample.Models.Repository
{
    public sealed class Products
    {
        private static volatile Products _instance = new Products();
        private static readonly object SyncRoot = new object();
        private static readonly IList<Product> _products = new List<Product>();

        public static Products Instance
        {
            get
            {
                if (_instance != null) return _instance;

                lock (SyncRoot)
                {
                    _instance = new Products();
                }

                return _instance;
            }
        }

        public static void Add(Product product)
        {
            _products.Add(product);
        }

        public static Product Get(long id)
        {
            return _products.FirstOrDefault(x => x.Id == id);
        }

        public static void Update(Product product)
        {
            _products.Remove(_products.FirstOrDefault(x => x.Id == product.Id));
            _products.Add(product);
        }

        public static void Remove(long id)
        {
            _products.Remove(_products.FirstOrDefault(x => x.Id == id));
        }

        public static IList<Product> Find(string name, string category)
        {
            var products = _products.Where(x => x.Name == name || x.Category == category).ToList();

            return products.Any() ? products : _products;
        }

        public static IList<Product> All()
        {
            return _products;
        }
    }
}