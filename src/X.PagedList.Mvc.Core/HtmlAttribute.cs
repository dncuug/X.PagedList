namespace X.PagedList.Mvc.Core;

/// <summary>
/// Represents one attribute of a DOM element
/// </summary>
/// <remarks>
/// Setting <see cref="Key"/> and <see cref="Value"/> is required.
/// </remarks>
public class HtmlAttribute
{
#if NET6_0
    public string Key { get; set; } = string.Empty;
    public object Value { get; set; } = string.Empty;
#else
    public required string Key { get; set; }
    public required object Value { get; set; }
#endif
}
