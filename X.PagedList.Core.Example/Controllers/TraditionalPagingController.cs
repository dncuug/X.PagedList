using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace X.PagedList.Core.Example.Controllers
{
    public class TraditionalPagingController : BaseController
    {
        // Traditional Paging
        public ActionResult Index(int? page)
        {
            var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
            if (listPaged == null)
                return NotFound();

            // pass the paged list to the view and render
            ViewBag.Names = listPaged;
            return View();
        }
    }
}
