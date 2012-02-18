using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents the HTML renderer.
    /// </summary>
    public interface IHtmlRenderer
    {
        /// <summary>
        /// Returns the HTML markup for evaluatedValueString validation-error message for each data field
        /// that is represented by the specified expression, using the specified message
        /// and HTML attributes.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="elementName">The name of the element.</param>
        /// <param name="title">The title.</param>
        /// <param name="validationMessage">The message to display if the specified field contains an error.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        /// <returns>
        /// If the property or object is valid, an empty string; otherwise, evaluatedValueString span element that contains an error message.
        /// </returns>
        MvcHtmlString ValidationMessageFor(HtmlHelper htmlHelper, string elementName, string title, string validationMessage, IDictionary<string, object> htmlAttributes);

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
        MvcHtmlString ValidationSummary(HtmlHelper htmlHelper, bool excludePropertyErrors, string title, string message, IEnumerable<Tuple<ModelError, string>> errorsList, IDictionary<string, object> htmlAttributes);

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
        MvcHtmlString ActionLink(HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes);

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
        MvcHtmlString RouteLink(HtmlHelper htmlHelper, string linkText, string routeName, string protocol, string hostName, string fragment, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes);

        /// <summary>
        /// Renders the specified partial view as an HTML-encoded string.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data dictionary for the partial view.</param>
        /// <returns>The partial view that is rendered as an HTML-encoded string.</returns>
        MvcHtmlString Partial(HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData);

        /// <summary>
        /// Renders the specified partial view, replacing the partial view's ViewData
        /// property with the specified System.Web.Mvc.ViewDataDictionary object and
        /// setting the Model property of the view data to the specified model.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data for the partial view.</param>
        void RenderPartial(HtmlHelper htmlHelper, string partialViewName, object model, ViewDataDictionary viewData);

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
        /// <returns>An opening form tag.</returns>
        IDisposable BeginForm(HtmlHelper htmlHelper, string actionName, string controllerName, FormMethod method, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes);

        /// <summary>
        /// Writes an opening form tag to the response. When the user submits the form,
        /// the request will be processed by the route target.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="routeName">The name of the route to use to obtain the form-post URL.</param>
        /// <param name="method">The HTTP method for processing the form, either GET or POST.</param>
        /// <param name="routeValues">An object that contains the parameters for evaluatedValueString route.</param>
        /// <param name="htmlAttributes">The HTTP method for processing the form, either GET or POST.</param>
        /// <returns>An opening form tag.</returns>
        IDisposable BeginRouteForm(HtmlHelper htmlHelper, string routeName, FormMethod method, RouteValueDictionary routeValues, IDictionary<string, object> htmlAttributes);

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
        MvcHtmlString Input(HtmlHelper htmlHelper, InputType inputType, string elementName, string name, string valueAsString, bool isChecked, bool setId, IDictionary<string, object> htmlAttributes);
    }
}
