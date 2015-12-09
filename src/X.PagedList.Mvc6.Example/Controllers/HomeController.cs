using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PagedList;

namespace WebApplication3.Controllers
{
    public class HomeController : Controller
    {
        public List<string> Contacts { get; set; }

        public HomeController()
        {
            Contacts = Enumerable.Range(1, 50).Select(i => "Contact " + i).ToList();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact(int? page = 1)
        {
            var model = Contacts.ToPagedList(page.GetValueOrDefault(1), 10);
            ViewData["Message"] = "Your contact page.";

            return View(model);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
