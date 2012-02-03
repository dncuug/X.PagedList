# What is this?

PagedList is a library that enables you to easily take an IEnumerable/IQueryable, chop it up into "pages", and grab a specific "page" by an index. PagedList.Mvc allows you to take that "page" and display a pager control that has links like "Previous", "Next", etc.

# How do I use it?

1. Install ["PagedList.Mvc"](http://nuget.org/List/Packages/PagedList.Mvc) via [NuGet](http://nuget.org) - that will automatically install ["PagedList"](http://nuget.org/List/Packages/PagedList) as well.
2. In your controller code, call **ToPagedList** off of your IEnumerable/IQueryable passing in the page size and which page you want to view.
3. Pass the result of **ToPagedList** to your view where you can enumerate over it - its still an IEnumerable, but only contains a subset of the original data.
4. Call **Html.PagedListPager**, passing in the instance of the PagedList and a function that will generate URLs for each page to see a paging control.

# Example

**/Controllers/ProductController.cs**

```csharp
public class ProductController : Controller
{
	public object Index(int? page)
	{
		var products = MyProductDataSource.FindAllProducts(); //returns IQueryable<Product> representing an unknown number of products. a thousand maybe?

		var pageNumber = page ?? 1; // if no page was specified in the querystring, default to the first page (1)
		var onePageOfProducts = products.ToPagedList(pageNumber, 25); // will only contain 25 products max because of the pageSize
		
		ViewBag.OnePageOfProducts = onePageOfProducts;
		return View();
	}
}
```

**/Views/Products/Index.cshtml**

```html
@{
	ViewBag.Title = "Product Listing"
}
@using PagedList.Mvc; //import this so we get our HTML Helper
@using PagedList; //import this so we can cast our list to IPagedList (only necessary because ViewBag is dynamic)

<!-- import the included stylesheet for some (very basic) default styling -->
<link href="/Content/PagedList.css" rel="stylesheet" type="text/css" />

<!-- loop through each of your products and display it however you want. we're just printing the name here -->
<h2>List of Products</h2>
<ul>
	@foreach(var product in ViewBag.OnePageOfProducts){
		<li>@product.Name</li>
	}
</ul>

<!-- output a paging control that lets the user navigation to the previous page, next page, etc -->
@Html.PagedListPager( (IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page }) )
```

# Example 2: Manual Paging

**/Controllers/ProductController.cs**

In some cases you do not have access something capable of creating an IQueryable, such as when using .Net's built-in [MembershipProvider's GetAllUsers](http://msdn.microsoft.com/en-us/library/system.web.security.membershipprovider.getallusers.aspx) method. This method offers paging, but not via IQueryable. Luckily PagedList still has your back (note the use of **StaticPagedList**):

```csharp
public class UserController : Controller
{
	public object Index(int? page)
	{
		var pageIndex = (page ?? 1) - 1; //MembershipProvider expects a 0 for the first page
		var pageSize = 10;
		int totalUserCount; // will be set by call to GetAllUsers due to _out_ paramter :-|

		var users = Membership.GetAllUsers(pageIndex, pageSize, out totalUserCount);
		var usersAsIPagedList = new StaticPagedList<MembershipUser>(users, pageIndex + 1, pageSize, totalUserCount);

		ViewBag.OnePageOfUsers = usersAsIPagedList;
		return View();
	}
}
```

# Pager Configurations

![Out-of-the-box Pager Configurations](https://github.com/TroyGoode/PagedList/raw/master/misc/DefaultPagingControlStyles.png)

**Code to generate the above configurations:**

## Out-of-the-box Pager Configurations

```html
<h3>Default Paging Control</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }))

<h3>Minimal Paging Control</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), PagedListRenderOptions.Minimal)

<h3>Minimal Paging Control w/ Page Count Text</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), PagedListRenderOptions.MinimalWithPageCountText)

<h3>Minimal Paging Control w/ Item Count Text</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), PagedListRenderOptions.MinimalWithItemCountText)

<h3>Page Numbers Only</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), PagedListRenderOptions.PageNumbersOnly)

<h3>Only Show Five Pages At A Time</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), PagedListRenderOptions.OnlyShowFivePagesAtATime)
```

## Custom Pager Configurations

You can instantiate [**PagedListRenderOptions**](https://github.com/TroyGoode/PagedList/blob/master/src/PagedList.Mvc/PagedListRenderOptions.cs) yourself to create custom configurations. All elements/links have discrete CSS classes applied to make styling easier as well.

```html
<h3>Custom Wording (<em>Spanish Translation Example</em>)</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), new PagedListRenderOptions { LinkToFirstPageFormat = "<< Primera", LinkToPreviousPageFormat = "< Anterior", LinkToNextPageFormat = "Siguiente >", LinkToLastPageFormat = "&Uacute;ltima >>" })

<h3>Show Range of Items For Each Page</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), new PagedListRenderOptions { FunctionToDisplayEachPageNumber = page => ((page - 1) * ViewBag.Names.PageSize + 1).ToString() + "-" + (((page - 1) * ViewBag.Names.PageSize) + ViewBag.Names.PageSize).ToString(), MaximumPageNumbersToDisplay = 5 })

<h3>With Delimiter</h3>
@Html.PagedListPager((IPagedList)ViewBag.OnePageOfProducts, page => Url.Action("Index", new { page = page }), new PagedListRenderOptions { DelimiterBetweenPageNumbers = "|" })
```

## Split and Partition

You can split an enumerable up into <em>n</em> equal-sized objects using the .Split extension method:

```csharp
var deckOfCards = new DeckOfCards(); //there are 52 cards in the deck
var splitDeck = deckOfCards.Split(2).ToArray();

Assert.Equal(26, splitDeck[0].Count());
Assert.Equal(26, splitDeck[1].Count());
```

You can split an enumerable up into <em>n</em> pages, each with a maximum of <em>m</em> items using the .Partition extension method:

```csharp
var deckOfCards = new DeckOfCards(); //52 cards
var hands = deckOfCards.Partition(5).ToArray();

Assert.Equal(11, hands.Count());
Assert.Equal(5, hands.First().Count());
Assert.Equal(2, hands.Last().Count()); //10 hands have 5 cards, last hand only has 2 cards
```

# License

Licensed under the [MIT License](http://www.opensource.org/licenses/mit-license.php).