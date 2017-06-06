using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApiSample.Infrastructure.Attributes;

namespace WebApiSample.Infrastructure.Filters
{
    public class ModelStateValidatorFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            List<string> erros = null;

            var obj = actionContext.ActionArguments.Select(x => x.Value).Where(x => x.GetType()
                .GetCustomAttributes(typeof(ModelChangeValidatorAttribute), true).Any()).ToList();

            var valid = true;

            foreach (var o in obj)
                valid = Validator.Validate(o, out erros);

            if (valid && actionContext.ModelState.IsValid) return;

            var exception = new InvalidOperationException("Model Invalid!");
            var exceptionData = new List<string>();

            foreach (var modelState in actionContext.ModelState.Values)
                exceptionData.AddRange(
                    modelState.Errors.Select(
                        error =>
                            !string.IsNullOrWhiteSpace(error.ErrorMessage)
                                ? error.ErrorMessage
                                : error.Exception.Message));

            if (erros != null) exceptionData.AddRange(erros);

            exception.Data.Add("ModelState", exceptionData);

            throw exception;
        }
    }

    public static class Validator
    {
        public static bool Validate(object obj, out List<string> errors)
        {
            var properties = obj.GetType().GetProperties();

            var validProperties =
                properties.Where(x => x.GetCustomAttributes(typeof(CanChangeValueAttribute), true).Length > 0)
                    .Select(x => x.Name)
                    .ToArray();
            var propertiesChanged = properties.Where(y => y.GetValue(obj, null) != null).Select(x => x.Name).ToArray();

            errors = new List<string>();
            var validation = propertiesChanged.Except(validProperties).ToList();

            if (!validation.Any()) return true;

            foreach (var item in validation)
                errors.Add($" Can not change {item}");

            return false;
        }
    }
}