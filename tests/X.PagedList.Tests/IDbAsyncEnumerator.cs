namespace X.PagedList.Tests;

public interface IDbAsyncEnumerator<T> : IDbAsyncEnumerator { }
public interface IDbAsyncEnumerator { object? Current { get; } }
