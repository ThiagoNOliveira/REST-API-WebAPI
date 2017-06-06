using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Infrastructure
{
    public class CanChangeValueAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}