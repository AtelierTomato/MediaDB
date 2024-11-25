using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Globalization;

namespace AtelierTomato.MediaDB.API.ModelBinders
{
	public class CultureInfoModelBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext is null)
				throw new ArgumentNullException(nameof(bindingContext));

			var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.FieldName);
			if (valueProviderResult == ValueProviderResult.None)
			{
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

			var value = valueProviderResult.FirstValue;

			if (string.IsNullOrEmpty(value))
			{
				bindingContext.Result = ModelBindingResult.Failed();
				return Task.CompletedTask;
			}

			try
			{
				var cultureInfo = new CultureInfo(value);
				bindingContext.Result = ModelBindingResult.Success(cultureInfo);
			}
			catch
			{
				bindingContext.Result = ModelBindingResult.Failed();
			}

			return Task.CompletedTask;
		}
	}
}
