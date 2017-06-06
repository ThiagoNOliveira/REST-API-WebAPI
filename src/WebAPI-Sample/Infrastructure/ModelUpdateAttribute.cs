using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Infrastructure
{
    public class ModelUpdateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}