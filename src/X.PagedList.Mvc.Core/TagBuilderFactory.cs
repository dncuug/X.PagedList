using X.PagedList.Web.Common;

namespace X.PagedList.Mvc.Core
{
    internal sealed class TagBuilderFactory : ITagBuilderFactory
    {
        public ITagBuilder Create(string tagName)
        {
            return new TagBuilder(tagName);
        }
    }
}
