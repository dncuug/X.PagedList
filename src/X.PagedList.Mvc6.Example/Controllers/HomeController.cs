using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using PagedList;
using X.PagedList;

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

        public IActionResult StaticContact(int? page = 1)
        {
            ViewData["Message"] = "Your static contact page.";

            var pageIndex = page ?? 1;
            var pageSize = 10;

            var toSkip = pageIndex * pageSize - pageSize;

            // filter contacts
            var filteredContacts = Contacts.Skip(toSkip).Take(pageSize);

            int totalContactsCount = Contacts.Count;

            StaticPagedList<string> contactsAsIPagedList = new StaticPagedList<string>(filteredContacts, pageIndex, pageSize, totalContactsCount);

            return View(contactsAsIPagedList);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
