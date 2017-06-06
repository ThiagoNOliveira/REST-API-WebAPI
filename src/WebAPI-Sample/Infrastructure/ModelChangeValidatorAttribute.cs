using System.ComponentModel.DataAnnotations;

namespace WebApiSample.Infrastructure
{
    public class ModelChangeValidatorAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return true;
        }
    }
}