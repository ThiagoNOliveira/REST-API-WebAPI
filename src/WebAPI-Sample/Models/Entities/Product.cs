using System.Runtime.Serialization;
using WebApiSample.Infrastructure;

namespace WebApiSample.Models.Entities
{
    [DataContract]
    [ModelUpdate]
    public class Product
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        [ValidPropertyUpdate]
        public string Name { get; set; }

        [DataMember]
        [ValidPropertyUpdate]
        public string Category { get; set; }

        [DataMember]
        public decimal? Price { get; set; }
    }
}