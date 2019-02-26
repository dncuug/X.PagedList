namespace X.PagedList.Mvc.Fluent
{
    using System.Linq;
    using System.Web;

    public static class HtmlPagerExtensions
    {
        public static IHtmlString Pager(this System.Web.Mvc.HtmlHelper htmlHelper)
        {
            return new HtmlPagerBuilder(htmlHelper, Enumerable.Empty<object>().ToPagedList()).Build();
        }

        public static IHtmlPagerBuilder Pager(this System.Web.Mvc.HtmlHelper htmlHelper, IPagedList list)
        {
            return new HtmlPagerBuilder(htmlHelper, list);
        }
    }
}
