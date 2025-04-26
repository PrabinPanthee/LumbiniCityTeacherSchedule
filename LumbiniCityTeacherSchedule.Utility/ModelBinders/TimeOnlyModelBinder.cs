
using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace LumbiniCityTeacherSchedule.Utility.ModelBinders
{
    public class TimeOnlyModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var value = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).FirstValue;
            if (TimeOnly.TryParse(value, out var result))
            {
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            else
            {
                bindingContext.ModelState.TryAddModelError(bindingContext.ModelName, "Invalid Time Format");
            }
            return Task.CompletedTask;

        }
    }
}
