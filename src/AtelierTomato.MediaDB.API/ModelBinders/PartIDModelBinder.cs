using AtelierTomato.MediaDB.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace AtelierTomato.MediaDB.API.ModelBinders
{
	public class PartIDModelBinder : IModelBinder
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
				var partID = PartID.Parse(value);
				bindingContext.Result = ModelBindingResult.Success(partID);
			}
			catch
			{
				bindingContext.Result = ModelBindingResult.Failed();
			}

			return Task.CompletedTask;
		}
	}
}
