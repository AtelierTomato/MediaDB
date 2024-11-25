using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Globalization;

namespace AtelierTomato.MediaDB.API.ModelBinders
{
	public class CultureInfoModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder? GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType == typeof(CultureInfo))
			{
				return new BinderTypeModelBinder(typeof(CultureInfo));
			}

			return null;
		}
	}
}
