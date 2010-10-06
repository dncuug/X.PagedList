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

			var firstPage = new StaticPagedList<int>(list, 0, 20, 100); // first page
			Console.WriteLine("Is first page? {0}", firstPage.IsFirstPage); // true
			Console.WriteLine("Is last page? {0}", firstPage.IsLastPage); // false
			Console.WriteLine("First value on page? {0}", firstPage[0]); // 1
			Console.WriteLine();

			var middlePage = new StaticPagedList<int>(list, 2, 20, 100); // third page, same values
			Console.WriteLine("Is first page? {0}", middlePage.IsFirstPage); // false
			Console.WriteLine("Is last page? {0}", middlePage.IsLastPage); // false
			Console.WriteLine("First value on page? {0}", middlePage[0]); // 1
			Console.WriteLine();

			var lastPage = new StaticPagedList<int>(list, 4, 20, 100); // fifth page, same values
			Console.WriteLine("Is first page? {0}", lastPage.IsFirstPage); // false
			Console.WriteLine("Is last page? {0}", lastPage.IsLastPage); // true
			Console.WriteLine("First value on page? {0}", lastPage[0]); // 1
			Console.ReadKey(false);
		}
	}
}