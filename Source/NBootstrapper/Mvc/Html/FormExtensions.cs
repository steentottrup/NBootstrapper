using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents extensions methods for <see cref="HtmlHelper"/> for forms.
    /// </summary>
    public static class FormExtensions
    {
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
        public static IDisposable BeginForm(this HtmlHelper htmlHelper, string actionName = null, string controllerName = null, FormMethod method = FormMethod.Post, object routeValues = null, object htmlAttributes = null)
        {
            var routeDictionary = htmlHelper.AutomaticRouteValuesDictionary(routeValues);
            var attrDictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);

            return BootstrapHelperConfiguration.HtmlRenderer.BeginForm(htmlHelper, actionName, controllerName, method, routeDictionary, attrDictionary);
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
        /// <returns>An opening form tag.</returns>
        public static IDisposable BeginRouteForm(this HtmlHelper htmlHelper, string routeName, FormMethod method = FormMethod.Post, object routeValues = null, object htmlAttributes = null)
        {
            var routeDictionary = htmlHelper.AutomaticRouteValuesDictionary(routeValues);
            var attrDictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);

            return BootstrapHelperConfiguration.HtmlRenderer.BeginRouteForm(htmlHelper, routeName, method, routeDictionary, attrDictionary);
        }
    }
}
