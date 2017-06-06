using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Infrastructure
{
    public class ValidPropertyUpdateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}