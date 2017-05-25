using System.Collections.Generic;
using System.Linq;
using WebApiSample.Infrastructure;

namespace WebApiSample.Models.Entities
{
    public class Validator<T> : IValidate<T>
    {
        private readonly string[] _validProperties = typeof(T).GetProperties().Select(x=>x.Name).ToArray();

        public bool Validate(string[] properties, out List<string> errors)
        {
            errors = new List<string>();
            var validation  = properties.Except(_validProperties).ToList();

            if (!validation.Any()) return true;

            foreach (var item in validation)
               errors.Add($" Can not change \"{ item}\"");

            return false;
        }
    }
}