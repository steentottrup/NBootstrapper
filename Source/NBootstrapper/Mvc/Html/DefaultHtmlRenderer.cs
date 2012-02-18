using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents the default HTML renderer.
    /// </summary>
    public class DefaultHtmlRenderer : IHtmlRenderer
    {
        private static readonly object _lastFormNumKey = new object();

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="DefaultHtmlRenderer"/> class.
        /// </summary>
        public DefaultHtmlRenderer()
        {

        }

        #region Validation
        /// <summary>
        /// Returns the HTML markup for evaluatedValueString validation-error message for each data field
        /// that is represented by the specified expression, using the specified message
        /// and HTML attributes.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="validationMessage">The message to display if the specified field contains an error.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        /// <returns>
        /// If the property or object is valid, an empty string; otherwise, evaluatedValueString span element that contains an error message.
        /// </returns>
        public virtual MvcHtmlString ValidationMessageFor(HtmlHelper htmlHelper, string elementName, string title, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            var spanBuilder = new TagBuilder("span");
            spanBuilder.MergeAttributes(htmlAttributes);
            spanBuilder.AddCssClass("help-inline");
            spanBuilder.SetInnerText(validationMessage);
            return new MvcHtmlString(spanBuilder.ToString(TagRenderMode.Normal));
        }

        /// <summary>
        /// Returns an unordered list (ul element) of validation messages that are in
        /// the System.Web.Mvc.ModelStateDictionary object and optionally displays only
        /// model-level errors.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="excludePropertyErrors"><c>true</c> to have the summary display model-level errors only, or <c>false</c> to have the summary display all errors.</param>
        /// <param name="title">The to use for the alert.</param>
        /// <param name="message">The message to display with the validation summary.</param>
        /// <param name="errorsList">The list of errors. This will be <c>null</c> if the model is valid.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        /// <returns>
        /// A string that contains an unordered list (ul element) of validation messages.
        /// </returns>
        public virtual MvcHtmlString ValidationSummary(HtmlHelper htmlHelper, bool excludePropertyErrors, string title, string message, IEnumerable<Tuple<ModelError, string>> errorsList, IDictionary<string, object> htmlAttributes)
        {
            if (errorsList == null)
                return null;

            var body = new StringBuilder();
            var bodyBuilder = new TagBuilder("h4");
            bodyBuilder.AddCssClass("alert-heading");
            bodyBuilder.SetInnerText(title);
            body.AppendLine(bodyBuilder.ToString(TagRenderMode.Normal));

            if (!string.IsNullOrEmpty(message))
            {
                bodyBuilder = new TagBuilder("p");
                bodyBuilder.SetInnerText(message);
                body.AppendLine(bodyBuilder.ToString(TagRenderMode.Normal));
            }

            var listBodyBuilder = new StringBuilder();
            var listBuilder = new TagBuilder("ul");

            foreach (var error in errorsList)
            {
                if (!string.IsNullOrEmpty(error.Item2))
                {
                    var listItemBuilder = new TagBuilder("li");
                    listItemBuilder.SetInnerText(error.Item2);
                    listBodyBuilder.AppendLine(listItemBuilder.ToString(TagRenderMode.Normal));
                }
            }

            if (listBodyBuilder.Length == 0)
                listBodyBuilder.AppendLine("<li style=\"display:none\"></li>");

            listBuilder.InnerHtml = listBodyBuilder.ToString();
            body.Append(listBuilder.ToString(TagRenderMode.Normal));

            var outerDivBuilder = new TagBuilder("div");
            outerDivBuilder.MergeAttributes<string, object>(htmlAttributes);
            outerDivBuilder.AddCssClass(htmlHelper.ViewData.ModelState.IsValid ? "alert alert-success" : "alert alert-error");
            outerDivBuilder.InnerHtml = body.ToString();

            return new MvcHtmlString(outerDivBuilder.ToString(TagRenderMode.Normal));
        }
        #endregion

        #region Links
        /// <summary>
        /// Returns an anchor element (evaluatedValueString element) that contains the virtual path of the
        /// specified action.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="actionName">The name of the action.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="routeValues">An object that contains the parameters for evaluatedValueString route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An anchor element (evaluatedValueString element).
        /// </returns>
        public virtual MvcHtmlString ActionLink(HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(HtmlHelper.GenerateLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, null, actionName, controllerName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }

        /// <summary>
        /// Returns an anchor element (evaluatedValueString element) that contains the virtual path of the
        /// specified action.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="linkText">The inner text of the anchor element.</param>
        /// <param name="routeName">The name of the route that is used to return evaluatedValueString virtual path.</param>
        /// <param name="protocol">The protocol for the URL, such as "http" or "https".</param>
        /// <param name="hostName">The host name for the URL.</param>
        /// <param name="fragment">The URL fragment name (the anchor name).</param>
        /// <param name="routeValues">An object that contains the parameters for evaluatedValueString route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An anchor element (evaluatedValueString element).
        /// </returns>
        public MvcHtmlString RouteLink(HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            return MvcHtmlString.Create(HtmlHelper.GenerateRouteLink(htmlHelper.ViewContext.RequestContext, htmlHelper.RouteCollection, linkText, routeName, protocol, hostName, fragment, routeValues, htmlAttributes));
        }
        #endregion

        #region Partials
        /// <summary>
        /// Renders the specified partial view as an HTML-encoded string.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data dictionary for the partial view.</param>
        /// <returns>
        /// The partial view that is rendered as an HTML-encoded string.
        /// </returns>
        public MvcHtmlString Partial(HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData)
        {
            using (var stringWriter = new StringWriter(CultureInfo.CurrentCulture))
            {
                RenderPartial(htmlHelper, partialViewName, model, viewData, stringWriter);
                return MvcHtmlString.Create(stringWriter.ToString());
            }
        }

        /// <summary>
        /// Renders the specified partial view, replacing the partial view's ViewData
        /// property with the specified System.Web.Mvc.ViewDataDictionary object and
        /// setting the Model property of the view data to the specified model.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data for the partial view.</param>
        public void RenderPartial(HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData)
        {
            RenderPartial(htmlHelper, partialViewName, model, viewData, htmlHelper.ViewContext.Writer);
        }

        /// <summary>
        /// Renders the specified partial view, replacing the partial view's ViewData
        /// property with the specified System.Web.Mvc.ViewDataDictionary object and
        /// setting the Model property of the view data to the specified model.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data for the partial view.</param>
        /// <param name="writer">The writer to text render the partial to.</param>
        public virtual void RenderPartial(HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData, TextWriter writer)
        {
            viewData = htmlHelper.ViewData;

            var viewContext = new ViewContext(htmlHelper.ViewContext, htmlHelper.ViewContext.View, viewData, htmlHelper.ViewContext.TempData, writer);
            var view = FindPartialView(viewContext, partialViewName, ViewEngines.Engines);
            view.Render(viewContext, writer);
        }

        /// <summary>
        /// Searches for evaluatedValueString partial view in the given <see cref="ViewEngineCollection"/>.
        /// </summary>
        /// <param name="viewContext">The view context.</param>
        /// <param name="partialViewName">The partial view name.</param>
        /// <param name="viewEngineCollection">The view engine collection.</param>
        /// <returns>
        /// The partial view.
        /// </returns>
        public virtual IView FindPartialView(ViewContext viewContext, string partialViewName, ViewEngineCollection viewEngineCollection)
        {
            if (string.IsNullOrEmpty(partialViewName))
                throw new ArgumentNullException("partialViewName");

            ViewEngineResult viewEngineResult = viewEngineCollection.FindPartialView(viewContext, partialViewName);
            if (viewEngineResult.View != null)
            {
                return viewEngineResult.View;
            }
            StringBuilder stringBuilder = new StringBuilder();
            foreach (string current in viewEngineResult.SearchedLocations)
            {
                stringBuilder.AppendLine();
                stringBuilder.Append(current);
            }
            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Properties.Resources.Common_PartialViewNotFound, partialViewName, stringBuilder));
        }
        #endregion

        #region Forms
        /// <summary>
        /// Writes an opening form tag to the response. When the user submits the form,
        /// the request will be processed by an action method.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="actionName">The name of the action method.</param>
        /// <param name="controllerName">The name of the controller.</param>
        /// <param name="method">The HTTP method for processing the form, either GET or POST.</param>
        /// <param name="routeValues">An object that contains the parameters for evaluatedValueString route.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>
        /// An opening form tag.
        /// </returns>
        public virtual IDisposable BeginForm(HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string formAction = UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, true);
            return CreateForm(htmlHelper, formAction, method, htmlAttributes);
        }

        /// <summary>
        /// Writes an opening form tag to the response. When the user submits the form,
        /// the request will be processed by the route target.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="routeName">The name of the route to use to obtain the form-post URL.</param>
        /// <param name="method">The HTTP method for processing the form, either GET or POST.</param>
        /// <param name="routeValues">An object that contains the parameters for evaluatedValueString route.</param>
        /// <param name="htmlAttributes">The HTTP method for processing the form, either GET or POST.</param>
        /// <returns>
        /// An opening form tag.
        /// </returns>
        public virtual IDisposable BeginRouteForm(HtmlHelper htmlHelper, string routeName, FormMethod method, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes)
        {
            string formAction = UrlHelper.GenerateUrl(routeName, null, null, routeValues, htmlHelper.RouteCollection, htmlHelper.ViewContext.RequestContext, false);
            return CreateForm(htmlHelper, formAction, method, htmlAttributes);
        }

        /// <summary>
        /// Creates and writes the starting tag for evaluatedValueString form element, and then returns
        /// the object that can be used to write the closing tag.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="formAction">The form action.</param>
        /// <param name="method">The method.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// The object that can be used to write the closing tag.
        /// </returns>
        public virtual IDisposable CreateForm(HtmlHelper htmlHelper, string formAction, FormMethod method, IDictionary<string, object> htmlAttributes)
        {
            var formBuilder = new TagBuilder("form");
            formBuilder.MergeAttributes<string, object>(htmlAttributes);
            formBuilder.MergeAttribute("action", formAction);
            formBuilder.MergeAttribute("method", HtmlHelper.GetFormMethodString(method), true);

            if (htmlHelper.ViewContext.ClientValidationEnabled)
                formBuilder.GenerateId(GenerateFormId(htmlHelper));

            return new FormGroupHelper(htmlHelper, formBuilder);
        }

        /// <summary>
        /// Generates evaluatedValueString new ID for evaluatedValueString form that is unique for the
        /// duration of the request.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <returns>The form ID.</returns>
        public virtual string GenerateFormId(HtmlHelper htmlHelper)
        {
            var items = htmlHelper.ViewContext.HttpContext.Items;
            var obj = items[_lastFormNumKey];
            var num = (obj != null) ? ((int)obj + 1) : 0;
            items[_lastFormNumKey] = num;

            // Not going to be the same one as MVC.
            // We need to use evaluatedValueString new unique name - so put in evaluatedValueString sneaky dash in there.
            return string.Format("form-{0}", num);
        }
        #endregion

        #region Input
        /// <summary>
        /// Returns an input element.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="inputType">The type of input control.</param>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="name">The name.</param>
        /// <param name="valueAsString">The value as string.</param>
        /// <param name="isChecked">If set to <see langword="true"/> either the radio or checkbox is checked.</param>
        /// <param name="setId">If set to <see langword="true"/> the ID should be set.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns>
        /// The input tag.
        /// </returns>
        public virtual MvcHtmlString Input(HtmlHelper htmlHelper, InputType inputType, string elementName, string name, string valueAsString, bool isChecked, bool setId, IDictionary<string, object> htmlAttributes)
        {
            var inputBuilder = new TagBuilder("input");
            inputBuilder.MergeAttributes(htmlAttributes);
            inputBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(inputType));
            inputBuilder.MergeAttribute("name", elementName, true);
            if (setId)
                inputBuilder.GenerateId(elementName);

            switch (inputType)
            {
                case InputType.CheckBox:
                case InputType.Radio:
                    if (isChecked)
                        inputBuilder.MergeAttribute("checked", "checked");
                    break;
            }

            if (valueAsString != null)
                inputBuilder.MergeAttribute("value", valueAsString);

            if (inputType != InputType.CheckBox)
                return new MvcHtmlString(inputBuilder.ToString(TagRenderMode.SelfClosing));

            var combinationBuilder = new StringBuilder(inputBuilder.ToString(TagRenderMode.SelfClosing));

            var hiddenBuilder = new TagBuilder("input");
            hiddenBuilder.MergeAttribute("type", HtmlHelper.GetInputTypeString(InputType.Hidden));
            hiddenBuilder.MergeAttribute("name", elementName);
            hiddenBuilder.MergeAttribute("value", "false");
            combinationBuilder.Append(hiddenBuilder.ToString(TagRenderMode.SelfClosing));

            return MvcHtmlString.Create(combinationBuilder.ToString());
        } 
        #endregion
    }
}
