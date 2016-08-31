using System.Linq;
using System.Web.Mvc;

namespace X.PagedList.Mvc.Example.Controllers
{
    public class AjaxController : BaseController
	{
		// Ajax Paging
		public ViewResult Index()
		{
			return View();
		}

		// Ajax Paging (cont'd)
		public ActionResult AjaxPage(int? page)
		{
			var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
			if (listPaged == null)
				return HttpNotFound();

			return Json(new
			{
				names = listPaged.ToArray(),
				pager = listPaged.GetMetaData()
			}, JsonRequestBehavior.AllowGet);
		}
    }
}
