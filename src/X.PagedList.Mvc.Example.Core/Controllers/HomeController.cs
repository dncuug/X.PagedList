using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace X.PageList.Mvc.Example.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int page = 1)
        {            
            var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
            ViewBag.Names = listPaged;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

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
