namespace X.PagedList.Tests;

public interface IDbAsyncEnumerable
{
    IDbAsyncEnumerator GetAsyncEnumerator();
}

public interface IDbAsyncEnumerable<T> : IDbAsyncEnumerable
{
}