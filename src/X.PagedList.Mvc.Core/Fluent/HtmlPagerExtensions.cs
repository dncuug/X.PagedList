using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using X.PagedList.Extensions;

namespace X.PagedList.Mvc.Core.Fluent;

[PublicAPI]
public static class HtmlPagerExtensions
{
    public static IHtmlContent Pager(this IHtmlHelper htmlHelper)
    {
        return new HtmlPagerBuilder(htmlHelper, Enumerable.Empty<object?>().ToPagedList()).Build();
    }

    public static IHtmlPagerBuilder Pager(this IHtmlHelper htmlHelper, IPagedList list)
    {
        return new HtmlPagerBuilder(htmlHelper, list);
    }
}
