using System.Runtime.Serialization;
using WebApiSample.Infrastructure;

namespace WebApiSample.Models.Entities
{
    [DataContract]
    [ModelChangeValidator]
    public class Product
    {
        [DataMember]
        public int? Id { get; set; }

        [DataMember]
        [CanChangeValue]
        public string Name { get; set; }

        [DataMember]
        [CanChangeValue]
        public string Category { get; set; }

        [DataMember]
        public decimal? Price { get; set; }
    }
}