using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.Web.PagedList;

public interface ITagBuilderFactory
{
    TagBuilder Create(string tagName);
}

internal sealed class TagBuilderFactory : ITagBuilderFactory
{
    public TagBuilder Create(string tagName) => new(tagName);
}
