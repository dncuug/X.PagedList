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