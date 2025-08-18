namespace X.PagedList.Mvc.Core;

/// <summary>
/// Represents one attribute of a DOM element.
/// </summary>
/// <remarks>
/// Setting <see cref="Key"/> and <see cref="Value"/> is required.
/// </remarks>
/// <example>
/// <code>
/// var attr = new HtmlAttribute { Key = "class", Value = "btn btn-primary" };
/// </code>
/// </example>
public class HtmlAttribute
{
#if NET6_0
    /// <summary>
    /// The attribute name (e.g., "class", "id").
    /// </summary>
    /// <remarks>
    /// Must be specified when constructing the instance.
    /// Settable only during object initialization.
    /// </remarks>
    public string Key { get; init; } = string.Empty;

    /// <summary>
    /// The attribute value associated with <see cref="Key"/>.
    /// </summary>
    /// <remarks>
    /// Commonly a string, but may be any object (e.g., a boolean for boolean attributes).
    /// </remarks>
    public object Value { get; init; } = string.Empty;
#else
    /// <summary>
    /// The attribute name (e.g., "class", "id").
    /// </summary>
    /// <remarks>
    /// This property is required.
    /// </remarks>
    public required string Key { get; init; }

    /// <summary>
    /// The attribute value associated with <see cref="Key"/>.
    /// </summary>
    /// <remarks>
    /// This property is required. Commonly a string, but may be any object (e.g., a boolean for boolean attributes).
    /// </remarks>
    public required object Value { get; init; }
#endif
}