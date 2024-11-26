using AtelierTomato.MediaDB.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace AtelierTomato.MediaDB.API.ModelBinders
{
	public class PartIDModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder? GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType == typeof(PartID))
			{
				return new BinderTypeModelBinder(typeof(PartIDModelBinder));
			}

			return null;
		}
	}
}
