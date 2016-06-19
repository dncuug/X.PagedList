using System.Web.Mvc;

namespace X.PagedList.Mvc.Example.Controllers
{
    public class UnobtrusiveAjaxController : BaseController
    {
		// Unobtrusive Ajax
		public ActionResult Index(int? page)
		{
			var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
			if (listPaged == null)
				return HttpNotFound();

			ViewBag.Names = listPaged;
			return Request.IsAjaxRequest()
				? (ActionResult)PartialView("UnobtrusiveAjax_Partial")
				: View();
		}
    }
}
