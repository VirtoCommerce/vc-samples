using System.Web;
using System.Web.Mvc;

namespace VirtoCommerce.JavaScriptShoppingCart.IntegrationSample
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
