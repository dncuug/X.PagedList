﻿using Example.DAL;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using X.PagedList;
using X.PagedList.Extensions;

namespace Example.Website.Controllers;

public class HomeController : Controller
{
    private const int PageSize = 10;

    private readonly DatabaseContext _databaseContext;

    public HomeController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

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

    public IActionResult Error()
    {
        return View();
    }

    private IPagedList<string> GetPagedNames(int? page)
    {
        // return a 404 if user browses to before the first page
        if (page.HasValue && page < 1)
        {
            return null;
        }

        // retrieve list from database/whereverand
        var listUnPaged = GetStuffFromFile();

        // page the list

        var listPaged = listUnPaged.ToPagedList(page ?? 1, PageSize);

        // return a 404 if user browses to pages beyond last page. special case first page if no items exist
        if (listPaged.PageNumber != 1 && page.HasValue && page > listPaged.PageCount)
        {
            return null;
        }

        return listPaged;
    }

    /// <summary>
    /// In this case we return array of string, but in most DB situations you'll want to return IQueryable
    /// </summary>
    /// <returns></returns>
    private IEnumerable<string> GetStuffFromFile()
    {
        var sampleData = System.IO.File.ReadAllText("Names.txt");
        
        return sampleData.Split('\n');
    }
}