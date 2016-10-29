using System.Linq;
using Microsoft.AspNetCore.Mvc;

namespace X.PagedList.Core.Example.Controllers
{
    public class AjaxController : BaseController
	{
		// Ajax Paging
		public ViewResult Index()
		{
			return View();
		}

        // Ajax Paging (cont'd)
        [HttpGet]
        public ActionResult AjaxPage(int? page)
		{
			var listPaged = GetPagedNames(page); // GetPagedNames is found in BaseController
			if (listPaged == null)
				return NotFound();

			return Json(new
			{
				names = listPaged.ToArray(),
				pager = listPaged.GetMetaData()
			});
		}
    }
}
