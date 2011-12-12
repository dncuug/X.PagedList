using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace PagedList.Mvc.Example.Controllers
{
	public class HomeController : Controller
	{
		// Traditional Paging
		public ActionResult Index(int? page)
		{
			var listPaged = GetPagedNames(page);
			if (listPaged == null)
				return HttpNotFound();

			// pass the paged list to the view and render
			ViewBag.Names = listPaged;
			return View();
		}

		// Traditional Paging (+Twitter Bootstrap)
		public ActionResult TwitterBootstrap(int? page)
		{
			return Index(page);
		}

		// Ajax Paging (cont'd)
		public ActionResult AjaxPage(int? page)
		{
			var listPaged = GetPagedNames(page);
			if (listPaged == null)
				return HttpNotFound();

			return Json(new
			            	{
			            		names = listPaged,
			            		pager = listPaged.GetMetaData()
			            	}, JsonRequestBehavior.AllowGet);
		}

		// Ajax Paging
		public ViewResult Ajax()
		{
			return View();
		}

		// Ajax Paging (+Twitter Bootstrap)
		public ViewResult TwitterBootstrapAjax()
		{
			return View();
		}

		// Ajax Paging (Infinite)
		public ViewResult Infinite()
		{
			return View();
		}

		private IPagedList<string> GetPagedNames(int? page)
		{
			// return a 404 if user browses to before the first page
			if (page.HasValue && page < 1)
				return null;

			// retrieve list from database/whereverand
			var listUnpaged = GetStuffFromDatabase();

			// page the list
			const int pageSize = 20;
			var listPaged = listUnpaged.ToPagedList(page ?? 1, pageSize);

			// return a 404 if user browses to pages beyond last page. special case first page if no items exist
			if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
				return null;

			return listPaged;
		}

		// in this case we return IEnumerable<string>, but in most
		// - DB situations you'll want to return IQueryable<string>
		private IEnumerable<string> GetStuffFromDatabase()
		{
			var sampleData = new StreamReader(Server.MapPath("~/App_Data/Names.txt")).ReadToEnd();
			return sampleData.Split('\n');
		}
	}
}