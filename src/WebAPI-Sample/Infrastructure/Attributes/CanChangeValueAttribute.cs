using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Infrastructure.Attributes
{
    public class CanChangeValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}