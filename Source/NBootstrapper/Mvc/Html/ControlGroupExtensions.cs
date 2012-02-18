using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
using System.IO;
using System.Threading;
using System.Web.Mvc.Html;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents extensions methods for <see cref="HtmlHelper"/> for control groups.
    /// </summary>
    public static class ControlGroupExtensions
    {
        /// <summary>
        /// Writes evaluatedValueString control group to the view and returns an object that can be used to
        /// write the closing tags for the control group.
        /// </summary>
        /// <typeparam name="TModel">The type of the model object.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="title">The title.</param>
        /// <param name="expression">The expression that returns the property value.</param>
        /// <returns>
        /// An object that can be used to write the closing tags for the control group.
        /// </returns>
        public static IDisposable ControlGroupFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string title = null, object htmlAttributes = null)
        {
            var expr = ExpressionHelper.GetExpressionText(expression);
            return ControlGroupForImpl(htmlHelper, ModelMetadata.FromStringExpression(expr, htmlHelper.ViewData), title, expr, htmlHelper.AutomaticHtmlAttributes(htmlAttributes));
        }

        private static IDisposable ControlGroupForImpl(HtmlHelper htmlHelper, ModelMetadata metadata, string title, string expression, IDictionary<string, object> htmlAttributes)
        {
            var writer = htmlHelper.ViewContext.Writer;
            var result = new SimpleGroupHelper(htmlHelper, "</div></div>");

            // Check for field.
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);
            title = Common.NameOf(metadata, fullHtmlFieldName, title);

            if (!htmlHelper.ViewData.ModelState.ContainsKey(fullHtmlFieldName))
            {
                WriteHeader(writer, null, fullHtmlFieldName, title, htmlAttributes);
                return result;
            }

            // Grab the errors.
            var modelState = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            var modelErrorCollection = (modelState == null) ? null : modelState.Errors;
            var hasErrors = modelErrorCollection != null && modelErrorCollection.Count != 0;

            if (hasErrors)
                WriteHeader(writer, "error", fullHtmlFieldName, title, htmlAttributes);
            else if (htmlHelper.ViewContext.RequestContext.HttpContext.Request.HttpMethod != "GET")
                WriteHeader(writer, "success", fullHtmlFieldName, title, htmlAttributes);
            else
                WriteHeader(writer, null, fullHtmlFieldName, title, htmlAttributes);

            return result;
        }

        private static void WriteHeader(TextWriter writer, string cls, string htmlElemName, string title, IDictionary<string, object> htmlAttributes)
        {
            var cgBuilder = new TagBuilder("div");
            cgBuilder.MergeAttributes(htmlAttributes);
            cgBuilder.AddCssClass("control-group");
            if (!string.IsNullOrEmpty(cls))
                cgBuilder.AddCssClass(cls);

            var labelBuilder = new TagBuilder("label");
            labelBuilder.AddCssClass("control-label");
            labelBuilder.MergeAttribute("for", htmlElemName);
            labelBuilder.SetInnerText(title);

            var controlsBuilder = new TagBuilder("div");
            controlsBuilder.AddCssClass("controls");

            writer.Write(cgBuilder.ToString(TagRenderMode.StartTag));
            writer.Write(labelBuilder.ToString(TagRenderMode.Normal));
            writer.Write(controlsBuilder.ToString(TagRenderMode.StartTag));
        }
    }
}