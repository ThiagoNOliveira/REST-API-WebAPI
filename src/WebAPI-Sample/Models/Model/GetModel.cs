using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace WebApiSample.Models.Model
{
    public class GetModel
    {
        #region Properties

        public string Filter { get; set; }

        //Implement persistent/temporary filters
        public long? FilterKey { get; set; }

        public int Offset { get; set; } = 0;
        public int Limit { get; set; } = 20;
        public string Sort { get; set; }

        public string Fields { get; set; }

        //Implement this if you need to create different views with fixed fields specific
        //Restfull Web Services Cookbook - 8.1, pg 138
        public string View { get; set; }

        #endregion

        #region Methods

        public string[] GetFields()
        {
            return Fields?.Split(',');
        }

        public Dictionary<string, string> GetFilter()
        {
            return Filter?.Split(',').Select(filter => filter?.Split('=')).ToDictionary(f => f[0], f => f[1]);
        }

        public Dictionary<string, SortOrder> GetSort()
        {
            var sortDictionary = new Dictionary<string, SortOrder>();

            if (Sort == null) return sortDictionary;

            foreach (var field in Sort?.Split(','))
            {
                if (field.StartsWith("-"))
                {
                    sortDictionary.Add(field.Replace("-", ""), SortOrder.Descending);

                    continue;
                }
                sortDictionary.Add(field, SortOrder.Ascending);
            }

            return sortDictionary;
        }

        #endregion
    }
}