# What is this?

PagedList is a library that enables you to easily take an IEnumerable/IQueryable, chop it up into "pages", and grab a specific "page" by an index. PagedList.Mvc allows you to take that "page" and display a pager control that has links like "Previous", "Next", etc.

# How do I use it?

1. Install ["PagedList.Mvc"](http://nuget.org/List/Packages/PagedList.Mvc) via [NuGet](http://nuget.org) - that will automatically install ["PagedList"](http://nuget.org/List/Packages/PagedList) as well.
2. In your controller code, call **ToPagedList** off of your IEnumerable/IQueryable passing in the page size and which page you want to view.
3. Pass the result of **ToPagedList** to your view where you can enumerate over it - its still an IEnumerable, but only contains a subset of the original data.
4. Call **Html.PagedListPager**, passing in the instance of the PagedList and a function that will generate URLs for each page to see a paging control.

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

&lt;!-- loop through each of your products and display it however you want. we're just printing the name here --&gt;
&lt;h2&gt;List of Products&lt;/h2&gt;
&lt;ul&gt;
	@foreach(var product in ViewBag.OnePageOfProducts){
		&lt;li&gt;@product.Name&lt;/li&gt;
	}
&lt;/ul&gt;

&lt;!-- output a paging control that lets the user navigation to the previous page, next page, etc --&gt;
@Html.PagedListPager( (IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page }) )
</pre>

# Out-of-the-box Pager Configurations

![Out-of-the-box Pager Configurations](https://github.com/TroyGoode/PagedList/raw/master/misc/DefaultPagingControlStyles.png)

**Code to generate the above configurations:**

<pre>
&lt;h3&gt;Default Paging Control&lt;/h3&gt;
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page =&gt; Url.Action("Index", new { page = page }))

&lt;h3&gt;Minimal Paging Control&lt;/h3&gt;
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page =&gt; Url.Action("Index", new { page = page }), PagedListRenderOptions.Minimal)

&lt;h3&gt;Minimal Paging Control w/ Page Count Text&lt;/h3&gt;
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page =&gt; Url.Action("Index", new { page = page }), PagedListRenderOptions.MinimalWithPageCountText)

&lt;h3&gt;Minimal Paging Control w/ Item Count Text&lt;/h3&gt;
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page =&gt; Url.Action("Index", new { page = page }), PagedListRenderOptions.MinimalWithItemCountText)

&lt;h3&gt;Page Numbers Only&lt;/h3&gt;
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page =&gt; Url.Action("Index", new { page = page }), PagedListRenderOptions.PageNumbersOnly)
</pre>

You can instantiate [**PagedListRenderOptions**](https://github.com/TroyGoode/PagedList/blob/master/src/PagedList.Mvc/PagedListRenderOptions.cs) yourself to create custom configurations. All elements/links have discrete CSS classes applied to make styling easier as well.

# Custom Pager Configurations

<pre>

</pre>

# License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).