using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents the Bootstrap Helper configuration.
    /// </summary>
    public class BootstrapHelperConfiguration
    {
        // TODO: Read this from Configuration.

        private static IHtmlRenderer _htmlRenderer = new DefaultHtmlRenderer();
        /// <summary>
        /// Gets or sets the HTML renderer.
        /// </summary>
        /// <value>
        /// The HTML renderer.
        /// </value>
        public static IHtmlRenderer HtmlRenderer
        {
            get { return _htmlRenderer; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                _htmlRenderer = value;
            }
        }
    }
}
