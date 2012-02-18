using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents extensions methods for <see cref="HtmlHelper"/> for partials.
    /// </summary>
    public static class PartialExtensions
    {
        /// <summary>
        /// Renders the specified partial view as an HTML-encoded string.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data dictionary for the partial view.</param>
        /// <returns>The partial view that is rendered as an HTML-encoded string.</returns>
        public static MvcHtmlString Partial(this HtmlHelper htmlHelper, string partialViewName, object model = null, object viewData = null)
        {
            if (partialViewName == null)
                throw new ArgumentNullException("partialViewName");
            
            return BootstrapHelperConfiguration.HtmlRenderer.Partial(htmlHelper, partialViewName, model, htmlHelper.AutomaticViewDataDictionary(viewData));
        }

        // Originally in RenderPartialExtensions.
        /// <summary>
        /// Renders the specified partial view, replacing the partial view's ViewData
        /// property with the specified System.Web.Mvc.ViewDataDictionary object and
        /// setting the Model property of the view data to the specified model.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="partialViewName">The name of the partial view.</param>
        /// <param name="model">The model for the partial view.</param>
        /// <param name="viewData">The view data for the partial view.</param>
        public static void RenderPartial(this HtmlHelper htmlHelper, string partialViewName, object model = null, object viewData = null)
        {
            if (partialViewName == null)
                throw new ArgumentNullException("partialViewName");

            BootstrapHelperConfiguration.HtmlRenderer.RenderPartial(htmlHelper, partialViewName, model, htmlHelper.AutomaticViewDataDictionary(viewData));
        }
    }
}
