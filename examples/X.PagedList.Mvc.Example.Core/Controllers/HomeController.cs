using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace X.PagedList.Mvc.Example.Core.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index(int page = 1)
        {
            ViewBag.Names = GetPagedNames(page);
            return View();
        }

        public IActionResult AjaxIndex(int page = 1)
        {
            var listPaged = GetPagedNames(page);
            ViewBag.Names = listPaged;
            return View();
        }

        public IActionResult GetOnePageOfNames(int page = 1)
        {
            var listPaged = GetPagedNames(page);
            ViewBag.Names = listPaged;
            return PartialView("_NameListPartial", ViewBag.Names);
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
            var sampleData = System.IO.File.ReadAllText("Names.txt");
            return sampleData.Split('\n');
        }
    }
}