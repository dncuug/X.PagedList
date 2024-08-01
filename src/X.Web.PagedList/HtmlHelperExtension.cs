﻿using System;
using System.Web;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace X.Web.PagedList;

///<summary>
///	Extension methods for generating paging controls that can operate on instances of IPagedList.
///</summary>
[PublicAPI]
public static class HtmlHelperExtension
{
    ///<summary>
    ///	Displays a configurable paging control for instances of PagedList.
    ///</summary>
    ///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
    ///<param name = "list">The PagedList to use as the data source.</param>
    ///<param name = "generatePageUrl">A function that takes the page number of the desired page and returns a URL-string that will load that page.</param>
    ///<returns>Outputs the paging control HTML.</returns>
    public static HtmlString PagedListPager(this IHtmlHelper html,
        IPagedList? list,
        Func<int, string?> generatePageUrl)
    {
        return PagedListPager(html, list, generatePageUrl, new PagedListRenderOptions());
    }

    ///<summary>
    ///	Displays a configurable paging control for instances of PagedList.
    ///</summary>
    ///<param name = "html">This method is meant to hook off HtmlHelper as an extension method.</param>
    ///<param name = "list">The PagedList to use as the data source.</param>
    ///<param name = "generatePageUrl">A function that takes the page number  of the desired page and returns a URL-string that will load that page.</param>
    ///<param name = "options">Formatting options.</param>
    ///<returns>Outputs the paging control HTML.</returns>
    public static HtmlString PagedListPager(this IHtmlHelper html,
        IPagedList? list,
        Func<int, string?> generatePageUrl,
        PagedListRenderOptions options)
    {
        HtmlHelper htmlHelper = new HtmlHelper(new TagBuilderFactory());
        string? htmlString = htmlHelper.PagedListPager(list, generatePageUrl, options);

        htmlString = HttpUtility.HtmlDecode(htmlString);

        return new HtmlString(htmlString);
    }

    ///<summary>
    /// Displays a configurable "Go To Page:" form for instances of PagedList.
    ///</summary>
    ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
    ///<param name="list">The PagedList to use as the data source.</param>
    ///<param name="formAction">The URL this form should submit the GET request to.</param>
    ///<returns>Outputs the "Go To Page:" form HTML.</returns>
    public static HtmlString PagedListGoToPageForm(this IHtmlHelper html, IPagedList list, string formAction)
    {
        return PagedListGoToPageForm(html, list, formAction, "page");
    }

    ///<summary>
    /// Displays a configurable "Go To Page:" form for instances of PagedList.
    ///</summary>
    ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
    ///<param name="list">The PagedList to use as the data source.</param>
    ///<param name="formAction">The URL this form should submit the GET request to.</param>
    ///<param name="inputFieldName">The querystring key this form should submit the new page number as.</param>
    ///<returns>Outputs the "Go To Page:" form HTML.</returns>
    public static HtmlString PagedListGoToPageForm(this IHtmlHelper html,
        IPagedList list,
        string formAction,
        string inputFieldName)
    {
        return PagedListGoToPageForm(html, list, formAction, new GoToFormRenderOptions(inputFieldName));
    }

    ///<summary>
    /// Displays a configurable "Go To Page:" form for instances of PagedList.
    ///</summary>
    ///<param name="html">This method is meant to hook off HtmlHelper as an extension method.</param>
    ///<param name="list">The PagedList to use as the data source.</param>
    ///<param name="formAction">The URL this form should submit the GET request to.</param>
    ///<param name="options">Formatting options.</param>
    ///<returns>Outputs the "Go To Page:" form HTML.</returns>
    public static HtmlString PagedListGoToPageForm(this IHtmlHelper html,
        IPagedList list,
        string formAction,
        GoToFormRenderOptions options)
    {
        HtmlHelper htmlHelper = new HtmlHelper(new TagBuilderFactory());
        string htmlString = htmlHelper.PagedListGoToPageForm(list, formAction, options);

        return new HtmlString(htmlString);
    }
}
