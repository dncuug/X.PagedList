using System;

namespace X.PagedList.Mvc.Core
{
    ///<summary>
    /// Options for configuring the output of <see cref = "HtmlHelper" />.
    ///</summary>
    public class GoToFormRenderOptions
    {
        ///<summary>
        /// The default settings, with configurable querystring key (input field name).
        ///</summary>
        public GoToFormRenderOptions(string inputFieldName)
        {
            LabelFormat = "Go to page:";
            SubmitButtonFormat = "Go";
            InputFieldName = inputFieldName;
            InputFieldType = "number";
        }

        ///<summary>
        /// The default settings.
        ///</summary>
        public GoToFormRenderOptions() : this("page")
        {
        }

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
        public int SubmitButtonWidth { get; set; }

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
        public int InputWidth { get; set; }

        /// <summary>
        /// Class that will be applied for input field
        /// </summary>
        public String InputFieldClass { get; set; }

        /// <summary>
        /// Class that will be applied for submit button
        /// </summary>
        public string SubmitButtonClass { get; set; }
    }
}