using System.Linq;
using Xunit;

namespace PagedList.Tests
{
	public class SplitAndPartitionFacts
	{
		[Fact]
		public void Partition_Works()
		{
			//arrange
			var list = Enumerable.Range(1, 9999);

			//act
			var splitList = list.Partition(1000);

			//assert
			Assert.Equal(10, splitList.Count());
			Assert.Equal(1000, splitList.First().Count());
			Assert.Equal(999, splitList.Last().Count());
		}
		
		[Fact]
		public void Paritiion_Returns_Enumerable_With_One_Item_When_Count_Less_Than_Page_Size()
		{
			//arrange
			var list = Enumerable.Range(1,10);
			
			//act
			var partitionList = list.Partition(1000);
			
			//assert
			Assert.Equal(1, splitList.Count());
			Assert.Equal(10, splitList.First().Count());
		}

		[Fact]
		public void Split_Works()
		{
			//arrange
			var list = Enumerable.Range(1, 9999);

			//act
			var splitList = list.Split(10);

			//assert
			Assert.Equal(10, splitList.Count());
			Assert.Equal(1000, splitList.First().Count());
			Assert.Equal(999, splitList.Last().Count());
		}
	}
}