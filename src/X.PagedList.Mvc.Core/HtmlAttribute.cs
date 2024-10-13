namespace X.PagedList.Mvc.Core;

/// <summary>
/// Represents one attribute of a DOM element
/// </summary>
/// <remarks>
/// Setting <see cref="Key"/> and <see cref="Value"/> is required.
/// </remarks>
public class HtmlAttribute
{
    public string Key { get; set; } = "";
    public object? Value { get; set; }
}