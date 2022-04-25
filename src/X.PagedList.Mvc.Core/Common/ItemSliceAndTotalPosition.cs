namespace X.PagedList.Web.Common;

/// <summary>
/// A two-state enum that controls the position of ItemSliceAndTotal text within PagedList items.
/// </summary>
public enum ItemSliceAndTotalPosition
{
    /// <summary>
    /// Shows ItemSliceAndTotal info at the beginning of the PagedList items.
    /// </summary>
    Start,

    /// <summary>
    /// Shows ItemSliceAndTotal info at the end of the PagedList items.
    /// </summary>
    End
}