using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;

namespace X.PagedList.Mvc.Example.Controllers
{
    public abstract class BaseController : Controller
    {
		protected IPagedList<string> GetPagedNames(int? page)
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
		protected IEnumerable<string> GetStuffFromDatabase()
		{
			var sampleData = new StreamReader(Server.MapPath("~/App_Data/Names.txt")).ReadToEnd();
			return sampleData.Split('\n');
		}
    }
}
