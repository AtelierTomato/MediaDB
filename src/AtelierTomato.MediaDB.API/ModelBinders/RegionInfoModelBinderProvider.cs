using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Globalization;

namespace AtelierTomato.MediaDB.API.ModelBinders
{
	public class RegionInfoModelBinderProvider : IModelBinderProvider
	{
		public IModelBinder? GetBinder(ModelBinderProviderContext context)
		{
			if (context.Metadata.ModelType == typeof(RegionInfo))
			{
				return new BinderTypeModelBinder(typeof(RegionInfo));
			}

			return null;
		}
	}
}
