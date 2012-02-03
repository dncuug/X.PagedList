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