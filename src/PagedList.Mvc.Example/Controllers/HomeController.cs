using System.Linq;
using System.Web.Mvc;

namespace PagedList.Mvc.Example.Controllers
{
	public class HomeController : Controller
	{
		public object Index(int? page)
		{
			// return a 404 if user browses to before the first page
			if (page.HasValue && page < 1)
				return HttpNotFound();

			// retrieve list from database/wherever (in this case Enumerable.Range) and page it
			const int pageSize = 20;
			var listAll = Enumerable.Range(1, 500);
			var listPaged = listAll.ToPagedList(page.HasValue ? page.Value - 1 : 0, pageSize);

			// return a 404 if user browses to pages beyond last page. special case first page if no items exist
			if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
				return HttpNotFound();

			// pass the paged list to the view and render
			ViewBag.Numbers = listPaged;
			return View();
		}
	}
}