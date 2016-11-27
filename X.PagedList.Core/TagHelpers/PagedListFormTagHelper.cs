using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace X.PagedList.Core.TagHelpers
{
    ///<summary>
    /// Displays a configurable "Go To Page:" form for instances of PagedList.
    ///</summary>
    public class PagedListFormTagHelper : TagHelper
    {
        #region Input parameters
        ///<summary>
        /// The PagedList to use as the data source.
        ///</summary>
        public IPagedList List { get; set; }

        ///<summary>
        /// The URL this form should submit the GET request to.
        ///</summary>
        public string Action { get; set; }
        
        ///<summary>
        /// The text to show in the form's input label.
        ///</summary>
        ///<example>
        /// "Go to page:"
        ///</example>
        public string LabelFormat { get; set; }

        ///<summary>
        /// The text to show in the form's submit button.
        ///</summary>
        ///<example>
        /// "Go"
        ///</example>
        public string SubmitButtonFormat { get; set; }

        /// <summary>
        /// Submit button width in px
        /// </summary>
        public string SubmitButtonWidth { get; set; }

        ///<summary>
        /// The querystring key this form should submit the new page number as.
        ///</summary>
        ///<example>
        /// "page"
        ///</example>
        public string InputFieldName { get; set; }

        ///<summary>
        /// The HTML input type for this field. Defaults to the HTML5 "number" type, but can be changed to "text" if targetting previous versions of HTML.
        ///</summary>
        ///<example>
        /// "number"
        ///</example>
        public string InputFieldType { get; set; }

        /// <summary>
        /// Input width in px
        /// </summary>
        public string InputWidth { get; set; }

        /// <summary>
        /// Class that will be applied for form
        /// </summary>
        public String FormClass { get; set; }

        /// <summary>
        /// Class that will be applied for input field
        /// </summary>
        public String InputFieldClass { get; set; }

        /// <summary>
        /// Class that will be applied for submit button
        /// </summary>
        public string SubmitButtonClass { get; set; }
        #endregion

        ///<summary>
        /// The default settings.
        ///</summary>
        public PagedListFormTagHelper()
        {
            Action = "/";
            LabelFormat = "Go to page:";
            SubmitButtonFormat = "Go";
            InputFieldName = "page";
            InputFieldType = "number";
            InputFieldClass = "form-control";
            InputWidth = "100px";
            SubmitButtonWidth = "50px";
            SubmitButtonClass = "btn btn-default";
            FormClass = "form-inline";
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagMode = TagMode.StartTagAndEndTag;
            if (List != null)
            {
                output.TagName = "form";
                output.Attributes.Add("class", FormClass);
                output.Attributes.Add("action", Action);
                output.Attributes.Add("method", "get");

                var fieldset = new TagBuilder("fieldset");

                var label = new TagBuilder("label");
                label.Attributes.Add("for", InputFieldName);
                label.InnerHtml.SetHtmlContent(LabelFormat);

                var input = new TagBuilder("input");
                input.Attributes.Add("type", InputFieldType);
                input.Attributes.Add("name", InputFieldName);
                input.Attributes.Add("value", List.PageNumber.ToString());
                input.Attributes.Add("class", InputFieldClass);
                input.Attributes.Add("style", $"width: {InputWidth}");

                var submit = new TagBuilder("input");
                submit.Attributes.Add("type", "submit");
                submit.Attributes.Add("value", SubmitButtonFormat);
                submit.Attributes.Add("class", SubmitButtonClass);
                submit.Attributes.Add("style", $"width: {SubmitButtonWidth}");

                fieldset.InnerHtml.AppendHtml(label);
                fieldset.InnerHtml.AppendHtml(input);
                fieldset.InnerHtml.AppendHtml(submit);

                output.Content.AppendHtml(fieldset);
            }
            else
            {
                output.Content.SetContent("TagHelper attribute \"list\" is required!");
            }
        }
    }
}
