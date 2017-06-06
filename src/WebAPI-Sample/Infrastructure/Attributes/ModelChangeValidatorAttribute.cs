using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Infrastructure.Attributes
{
    public class ModelChangeValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}