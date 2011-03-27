# What is this?

PagedList is a library that enables you to easily take an IEnumerable/IQueryable, chop it up into "pages", and grab a specific "page" by an index. PagedList.Mvc allows you to take that "page" and display a pager control that has links like "Previous", "Next", etc.

# How do I use it?

1. Install "PagedList.Mvc" via NuGet.
2. In your controller code, call **ToPagedList** off of your IEnumerable/IQueryable passing in the page size and which page you want to view.
3. Pass the result of **ToPagedList** to your view where you can enumerate over it - its still IEnumerable, but only contains a subset of the original data.
4. Call Html.PagedListPager, passing in the instance of the PagedList and a function that will generate URLs for each page to see a paging control.

# Example

**/Controllers/ProductController.cs**
<pre>
public class ProductController : Controller
{
	public object Index(int? page)
	{
		var products = MyProductDataSource.FindAllProducts(); //returns IQueryable<Product>

		var pageIndex = page ?? 0; // if no page was specified in the querystring, default to page 0
		var pageSize = 25;
		var onePageOfProducts = products.ToPagedList(pageIndex, pageSize);
		
		ViewBag.OnePageOfProducts = onePageOfProducts;
		return View();
	}
}
</pre>

**/Views/Products/Index.cshtml**
<pre>
@{
	ViewBag.Title = "Product Listing"
}
@using PagedList.Mvc; //import this so we get our HTML Helper
@using PagedList; //import this so we can cast our list to IPagedList (only necessary because ViewBag is dynamic)

&lt;-- import the included stylesheet for some (very basic) default styling --&gt;
&lt;link href="/Content/PagedList.css" rel="stylesheet" type="text/css" /&gt;

&lt;-- loop through each of your products and display it however. we're just printing then name here --&gt;
&lt;h2&gt;List of Products&lt;/h2&gt;
&lt;ul&gt;
	@foreach(var product in ViewBag.OnePageOfProducts){
		&lt;li&gt;@product.Name&lt;/li&gt;
	}
&lt;/ul&gt;

&lt;-- output a paging control that lets the user navigation to the previous page, next page, etc --&gt;
@Html.PagedListPager( (IPagedList)ViewBag.Numbers, page => Url.Action("Index", new { page }) )
</pre>

# Using PagedList<T>
<pre>
using System;
using System.Linq;

namespace PagedList.Tests
{
	public class PagedListExample
	{
		public static void Main(string args)
		{
			// create a list of integers from 1 to 200
			var list = Enumerable.Range(1, 200);

			var firstPage = list.ToPagedList(0, 20); // first page, page size = 20

			Console.WriteLine("Is first page? {0}", firstPage.IsFirstPage); // true
			Console.WriteLine("Is last page? {0}", firstPage.IsLastPage); // false
			Console.WriteLine("First value on page? {0}", firstPage[0]); // 1
			Console.WriteLine();

			var anotherPage = list.ToPagedList(4, 20); // fifth page, page size = 20
			
			Console.WriteLine("Is first page? {0}", anotherPage.IsFirstPage); // false
			Console.WriteLine("Is last page? {0}", anotherPage.IsLastPage); // false
			Console.WriteLine("Total integers in list? {0}", anotherPage.TotalItemCount); // 200
			Console.WriteLine("Integers on this page? {0}", anotherPage.Count); // 20
			Console.WriteLine("First value on page? {0}", anotherPage[0]); // 81
			Console.WriteLine();

			var lastPage = list.ToPagedList(9, 20); // last (tenth) page, page size = 20
			
			Console.WriteLine("Is first page? {0}", lastPage.IsFirstPage); // false
			Console.WriteLine("Is last page? {0}", lastPage.IsLastPage); // true
			Console.WriteLine("First value on page? {0}", lastPage[0]); // 181
			Console.ReadKey(false);
		}
	}
}
</pre>

# Using StaticPagedList<T>
<pre>
using System;
using System.Linq;

namespace PagedList.Tests
{
	public class StaticPagedListExample
	{
		public static void Main(string[] args)
		{
			// create a list of integers from 1 to 20
			var list = Enumerable.Range(1, 20);

			var firstPage = new StaticPagedList&lt;int&gt;(list, 0, 20, 100); // first page
			
			Console.WriteLine("Is first page? {0}", firstPage.IsFirstPage); // true
			Console.WriteLine("Is last page? {0}", firstPage.IsLastPage); // false
			Console.WriteLine("First value on page? {0}", firstPage[0]); // 1
			Console.WriteLine();

			var middlePage = new StaticPagedList&lt;int&gt;(list, 2, 20, 100); // third page, same values
			
			Console.WriteLine("Is first page? {0}", middlePage.IsFirstPage); // false
			Console.WriteLine("Is last page? {0}", middlePage.IsLastPage); // false
			Console.WriteLine("First value on page? {0}", middlePage[0]); // 1
			Console.WriteLine();

			var lastPage = new StaticPagedList&lt;int&gt;(list, 4, 20, 100); // fifth page, same values
			
			Console.WriteLine("Is first page? {0}", lastPage.IsFirstPage); // false
			Console.WriteLine("Is last page? {0}", lastPage.IsLastPage); // true
			Console.WriteLine("First value on page? {0}", lastPage[0]); // 1
			Console.ReadKey(false);
		}
	}
}
</pre>

# License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).