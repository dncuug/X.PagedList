# What is this?

PagedList is a library that enables you to easily take an IEnumerable/IQueryable, chop it up into "pages", and grab a specific "page" by an index. PagedList.Mvc allows you to take that "page" and display a pager control that has links like "Previous", "Next", etc.

# How do I use it?

1. Install ["PagedList.Mvc"](http://nuget.org/List/Packages/PagedList) via [NuGet](http://nuget.org) - that will automatically install ["PagedList"](http://nuget.org/List/Packages/PagedList) as well.
2. In your controller code, call **ToPagedList** off of your IEnumerable/IQueryable passing in the page size and which page you want to view.
3. Pass the result of **ToPagedList** to your view where you can enumerate over it - its still an IEnumerable, but only contains a subset of the original data.
4. Call Html.PagedListPager, passing in the instance of the PagedList and a function that will generate URLs for each page to see a paging control.

# Example

**/Controllers/ProductController.cs**
<pre>
public class ProductController : Controller
{
	public object Index(int? page)
	{
		var products = MyProductDataSource.FindAllProducts(); //returns IQueryable&lt;Product&gt; representing an unknown number of products. a thousand maybe?

		var pageIndex = page ?? 0; // if no page was specified in the querystring, default to page 0
		var onePageOfProducts = products.ToPagedList(pageIndex, 25); // will only contain 25 products max because of the pageSize
		
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

&lt;!-- import the included stylesheet for some (very basic) default styling --&gt;
&lt;link href="/Content/PagedList.css" rel="stylesheet" type="text/css" /&gt;

&lt;!-- loop through each of your products and display it however. we're just printing then name here --&gt;
&lt;h2&gt;List of Products&lt;/h2&gt;
&lt;ul&gt;
	@foreach(var product in ViewBag.OnePageOfProducts){
		&lt;li&gt;@product.Name&lt;/li&gt;
	}
&lt;/ul&gt;

&lt;!-- output a paging control that lets the user navigation to the previous page, next page, etc --&gt;
@Html.PagedListPager( (IPagedList)ViewBag.Numbers, page => Url.Action("Index", new { page }) )
</pre>

# License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).