using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Routing;
using System.Globalization;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    // Internal stuff.

    static class Common
    {
        internal static string NameOf(ModelMetadata metadata, string htmlFieldName, string labelText = null)
        {
            string text = labelText;
            if (labelText == null && (text = metadata.DisplayName) == null && (text = metadata.PropertyName) == null)
            {
                var index = htmlFieldName.LastIndexOf('.');
                return htmlFieldName.Substring(index + 1);
            }
            else
            {
                return text;
            }
        }

        internal static IDictionary<string, object> AutomaticHtmlAttributes(this HtmlHelper helper, object htmlAttributes)
        {
            var attrDictionary = htmlAttributes as IDictionary<string, object>;
            if (attrDictionary == null)
                return HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
            return attrDictionary;
        }

        internal static RouteValueDictionary AutomaticRouteValuesDictionary(this HtmlHelper helper, object routeValues)
        {
            if (routeValues == null)
                return new RouteValueDictionary();

            var routeDictionary = routeValues as RouteValueDictionary;
            if (routeDictionary == null)
            {
                var idic = routeValues as IDictionary<string, object>;
                if (idic == null)
                    return new RouteValueDictionary(routeValues);
                else
                    return new RouteValueDictionary(idic);
            }
            return routeDictionary;
        }

        internal static ViewDataDictionary AutomaticViewDataDictionary(this HtmlHelper helper, object routeValues)
        {
            if (routeValues == null)
                return helper.ViewData;

            var viewDictionary = routeValues as ViewDataDictionary;
            if (viewDictionary == null)
            {
                var idic = routeValues as IDictionary<string, object>;
                if (idic == null)
                    return new ViewDataDictionary(routeValues);
                else
                    return new ViewDataDictionary(idic);
            }
            return viewDictionary;
        }

        internal static string EvalString(this HtmlHelper htmlHelper, string key)
        {
            return Convert.ToString(htmlHelper.ViewData.Eval(key), CultureInfo.CurrentCulture);
        }

        internal static object GetModelStateValue(this HtmlHelper htmlHelper, string key, Type destinationType)
        {
            System.Web.Mvc.ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(key, out modelState) && modelState.Value != null)
                return modelState.Value.ConvertTo(destinationType, (CultureInfo)null);
            else
                return (object)null;
        }
    }
}
