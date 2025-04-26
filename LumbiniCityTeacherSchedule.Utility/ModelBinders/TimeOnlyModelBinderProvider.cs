using Microsoft.AspNetCore.Mvc.ModelBinding;


namespace LumbiniCityTeacherSchedule.Utility.ModelBinders
{
    public class TimeOnlyModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType == typeof(TimeOnly))
            { 
            return new TimeOnlyModelBinder();
            }
            return null;
        }
    }
}
