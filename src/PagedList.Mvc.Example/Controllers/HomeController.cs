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

		// Ajax Paging
		public ViewResult Ajax()
		{
			return View();
		}

		// Ajax Paging
		public ViewResult Infinite()
		{
			return View();
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
			            		pager = new Pager(listPaged)
			            	}, JsonRequestBehavior.AllowGet);
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
			var listPaged = listUnpaged.ToPagedList(page.HasValue ? page.Value - 1 : 0, pageSize);

			// return a 404 if user browses to pages beyond last page. special case first page if no items exist
			if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
				return null;

			return listPaged;
		}

		// in this case we return IEnumerable<int>, but in most
		// - DB situations you'll want to return IQueryable<T>
		private IEnumerable<string> GetStuffFromDatabase()
		{
			var sampleData = new StreamReader(Server.MapPath("~/App_Data/Names.txt")).ReadToEnd();
			return sampleData.Split('\n');
		}

		#region Nested type: Pager

		public class Pager : IPagedList
		{
			public Pager(IPagedList pagedList)
			{
				PageCount = pagedList.PageCount;
				TotalItemCount = pagedList.TotalItemCount;
				PageIndex = pagedList.PageIndex;
				PageNumber = pagedList.PageNumber;
				PageSize = pagedList.PageSize;
				HasPreviousPage = pagedList.HasPreviousPage;
				HasNextPage = pagedList.HasNextPage;
				IsFirstPage = pagedList.IsFirstPage;
				IsLastPage = pagedList.IsLastPage;
				FirstItemOnPage = pagedList.FirstItemOnPage;
				LastItemOnPage = pagedList.LastItemOnPage;
			}

			#region IPagedList Members

			public int PageCount { get; private set; }
			public int TotalItemCount { get; private set; }
			public int PageIndex { get; private set; }
			public int PageNumber { get; private set; }
			public int PageSize { get; private set; }
			public bool HasPreviousPage { get; private set; }
			public bool HasNextPage { get; private set; }
			public bool IsFirstPage { get; private set; }
			public bool IsLastPage { get; private set; }
			public int FirstItemOnPage { get; private set; }
			public int LastItemOnPage { get; private set; }

			#endregion
		}

		#endregion
	}
}