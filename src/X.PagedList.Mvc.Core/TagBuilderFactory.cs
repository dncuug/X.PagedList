using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.PagedList.Mvc.Core;

/// <summary>
/// Factory interface for creating <see cref="TagBuilder"/> instances.
/// </summary>
public interface ITagBuilderFactory
{
    /// <summary>
    /// Creates a new <see cref="TagBuilder"/> for the specified tag name.
    /// </summary>
    /// <param name="tagName">The name of the HTML tag to create.</param>
    /// <returns>A <see cref="TagBuilder"/> instance for the given tag name.</returns>
    TagBuilder Create(string tagName);
}

/// <summary>
/// Default implementation of <see cref="ITagBuilderFactory"/>.
/// Creates <see cref="TagBuilder"/> instances for specified tag names.
/// </summary>
internal sealed class TagBuilderFactory : ITagBuilderFactory
{
    /// <inheritdoc/>
    public TagBuilder Create(string tagName) => new(tagName);
}