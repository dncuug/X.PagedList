using System.Collections.Generic;
using System.Web.Mvc;
using System.IO;

namespace PagedList.Mvc.Example.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(int? page)
		{
			// return a 404 if user browses to before the first page
			if (page.HasValue && page < 1)
				return HttpNotFound();

			// retrieve list from database/whereverand
			var listUnpaged = GetStuffFromDatabase();

			// page the list
			const int pageSize = 20;
			var listPaged = listUnpaged.ToPagedList(page.HasValue ? page.Value - 1 : 0, pageSize);

			// return a 404 if user browses to pages beyond last page. special case first page if no items exist
			if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
				return HttpNotFound();

			// pass the paged list to the view and render
			ViewBag.Names = listPaged;
			return View();
		}

		// in this case we return IEnumerable<int>, but in most
		// - DB situations you'll want to return IQueryable<T>
		private IEnumerable<string> GetStuffFromDatabase()
		{
			var sampleData = new StreamReader(Server.MapPath("~/SampleData.txt")).ReadToEnd();
			return sampleData.Split('\n');
		}
	}
}