using System.Linq;
using System.Threading.Tasks;
using Example.DAL;
using Microsoft.AspNetCore.Mvc;
using X.PagedList.EF;

namespace Example.Website.Controllers;

public class EFController : Controller
{
    private const int PageSize = 10;

    private readonly DatabaseContext _databaseContext;

    public EFController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        // return a 404 if user browses to before the first page
        if (page < 1)
        {
            return NotFound();
        }

        var records = await _databaseContext.Animals
            .Select(o => o.Name)
            .ToPagedListAsync(page, PageSize);

        // return a 404 if user browses to pages beyond last page. special case first page if no items exist
        if (records.PageNumber != 1 && page > records.PageCount)
        {
            return NotFound();
        }

        ViewBag.Names = records;

        return View();
    }
}