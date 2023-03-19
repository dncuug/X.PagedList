using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.PagedList.Mvc.Core;

public interface ITagBuilderFactory
{
    TagBuilder Create(string tagName);
}

internal sealed class TagBuilderFactory : ITagBuilderFactory
{
    public TagBuilder Create(string tagName)
    {
        return new TagBuilder(tagName);
    }
}