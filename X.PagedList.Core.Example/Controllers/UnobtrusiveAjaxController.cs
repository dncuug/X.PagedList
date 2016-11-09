using Microsoft.AspNetCore.Mvc;

namespace X.PagedList.Core.Example.Controllers
{
    public class UnobtrusiveAjaxController : BaseController
    {
		// Unobtrusive Ajax
		public ActionResult Index(int? page)
		{
			var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
			if (listPaged == null)
				return NotFound();

			ViewBag.Names = listPaged;
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";

            return isAjax
                ? (ActionResult)PartialView("UnobtrusiveAjax_Partial")
				: View();
		}
    }
}
