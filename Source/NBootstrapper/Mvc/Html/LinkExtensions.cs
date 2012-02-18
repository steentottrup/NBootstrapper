using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents extensions methods for <see cref="HtmlHelper"/> for hyperlinks.
    /// </summary>
    public static class LinkExtensions
    {
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
        /// <returns>An anchor element (evaluatedValueString element).</returns>
        public static MvcHtmlString ActionLink(this HtmlHelper htmlHelper, string linkText, string actionName = null, string controllerName = null, string protocol = null, string hostName = null, string fragment = null, object routeValues = null, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(linkText))
                throw new ArgumentNullException("linkText");

            var routeDictionary = htmlHelper.AutomaticRouteValuesDictionary(routeValues);
            var attrDictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);

            return BootstrapHelperConfiguration.HtmlRenderer.ActionLink(htmlHelper, linkText, actionName, controllerName, protocol, hostName, fragment, routeDictionary, attrDictionary);
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
        /// <returns>An anchor element (evaluatedValueString element).</returns>
        public static MvcHtmlString RouteLink(this HtmlHelper htmlHelper, string linkText, string routeName = null, string protocol = null, string hostName = null, string fragment = null, object routeValues = null, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(linkText))
                throw new ArgumentNullException("linkText");

            var routeDictionary = htmlHelper.AutomaticRouteValuesDictionary(routeValues);
            var attrDictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);

            return BootstrapHelperConfiguration.HtmlRenderer.RouteLink(htmlHelper, linkText, routeName, protocol, hostName, fragment, routeDictionary, attrDictionary);
        }
    }
}
