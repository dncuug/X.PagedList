using X.PagedList.Web.Common;

namespace X.PagedList.Mvc
{
    internal sealed class TagBuilderFactory : ITagBuilderFactory
    {
        public ITagBuilder Create(string tagName)
        {
            return new TagBuilder(tagName);
        }
    }
}
