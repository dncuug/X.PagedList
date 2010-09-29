using Xunit;
using Xunit.Extensions;

namespace PagedList.Tests
{
	public class StaticPagedListFacts
	{
		[Theory,
		InlineData(0, true, false),
		InlineData(1, false, false),
		InlineData(2, false, true)]
		public void StaticPagedList_uses_supplied_totalItemCount_to_determine_subsets_position_within_superset(int index, bool shouldBeFirstPage, bool shouldBeLastPage)
		{
			//arrange
			var subset = new[] {1, 1, 1};

			//act
			var list = new StaticPagedList<int>(subset, index, 3, 9);

			//assert
			Assert.Equal(index, list.PageIndex);
			Assert.Equal(shouldBeFirstPage, list.IsFirstPage);
			Assert.Equal(shouldBeLastPage, list.IsLastPage);
		}
	}
}