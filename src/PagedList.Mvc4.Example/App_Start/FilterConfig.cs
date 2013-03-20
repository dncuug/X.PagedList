using System.Web;
using System.Web.Mvc;

namespace PagedList.Mvc4.Example
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}