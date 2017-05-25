using System.Collections.Generic;

namespace WebApiSample.Infrastructure
{
    internal interface IValidate<T>
    {
        bool Validate(string[] properties, out List<string> errors);
    }
}