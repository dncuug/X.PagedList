using System.Web.Mvc;

namespace X.PagedList.Mvc.Example.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

			return View();
		}
	}
}
