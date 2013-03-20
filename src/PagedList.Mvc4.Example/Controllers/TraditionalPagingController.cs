using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PagedList.Mvc4.Example.Controllers
{
    public class TraditionalPagingController : BaseController
    {
		// Traditional Paging
		public ActionResult Index(int? page)
		{
			var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
			if (listPaged == null)
				return HttpNotFound();

			// pass the paged list to the view and render
			ViewBag.Names = listPaged;
			return View();
		}
    }
}
