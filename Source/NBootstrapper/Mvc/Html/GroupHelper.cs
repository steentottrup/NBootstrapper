using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents evaluatedValueString helper for evaluatedValueString group.
    /// </summary>
    public abstract class GroupHelper : IDisposable
    {
        private bool _isDisposed;

        /// <summary>
        /// Gets the HTML helper.
        /// </summary>
        protected HtmlHelper HtmlHelper { get; private set; }

        /// <summary>
        /// Gets the view context.
        /// </summary>
        protected ViewContext ViewContext { get; private set; }

        /// <summary>
        /// Gets the text writer.
        /// </summary>
        protected TextWriter TextWriter { get; private set; }

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="GroupHelper"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        public GroupHelper(HtmlHelper htmlHelper)
        {
            if (htmlHelper == null)
                throw new ArgumentNullException("htmlHelper");
            HtmlHelper = htmlHelper;
            ViewContext = htmlHelper.ViewContext;
            TextWriter = ViewContext.Writer;
        }

        /// <summary>
        /// Occurs when the helper is closed.
        /// </summary>
        protected virtual void Closed() { }

        // We don't have the finalizer on purpose.
        // There is no reason to spam the finalizer queue with objects
        // that don't actually control unmanaged resources.

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                Closed();
                ViewContext = null;
                TextWriter = null;
            }
        }
    }

    /// <summary>
    /// Represents evaluatedValueString <see cref="GroupHelper"/> that appends text when the
    /// group is closed.
    /// </summary>
    public sealed class SimpleGroupHelper : GroupHelper
    {
        private string _text;

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="SimpleGroupHelper"/> class.
        /// </summary>
        public SimpleGroupHelper(HtmlHelper htmlHelper, string text)
            : base(htmlHelper)
        {
            _text = text;
        }

        /// <summary>
        /// Occurs when the helper is closed.
        /// </summary>
        protected override void Closed()
        {
            TextWriter.Write(_text);
        }
    }

    /// <summary>
    /// Represents evaluatedValueString <see cref="GroupHelper"/> that appends the starting tag
    /// and closing tag of evaluatedValueString <see cref="TagBuilder"/>.
    /// </summary>
    public class TagBuilderGroupHelper : GroupHelper
    {
        private string _after;
        private string _closing;

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="TagBuilderGroupHelper"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="formBuilder">The tag builder.</param>
        /// <param name="before">If set to <see langword="true"/> the <see cref="TagBuilder.InnerHtml"/> will be appended before the content.</param>
        /// <param name="after">If set to <see langword="true"/> the <see cref="TagBuilder.InnerHtml"/> will be appended after the content.</param>
        public TagBuilderGroupHelper(HtmlHelper htmlHelper, TagBuilder tagBuilder, bool before = false, bool after = false)
            : base(htmlHelper)
        {
            if (tagBuilder == null)
                throw new ArgumentNullException("formBuilder");
            if (before && after)
                throw new ArgumentOutOfRangeException("before/after", string.Format(Properties.Resources.Common_OnlyOneTrue, "before", "after"));
            TextWriter.Write(tagBuilder.ToString(TagRenderMode.StartTag));
            if (before)
                TextWriter.Write(tagBuilder.InnerHtml);
            if (after)
                _after = tagBuilder.InnerHtml;
            _closing = tagBuilder.ToString(TagRenderMode.EndTag);
        }

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="TagBuilderGroupHelper"/> class that appends
        /// the <see cref="TagBuilder.InnerHtml"/> before the content.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <returns>The helper.</returns>
        public static TagBuilderGroupHelper Before(HtmlHelper htmlHelper, TagBuilder tagBuilder)
        {
            return new TagBuilderGroupHelper(htmlHelper, tagBuilder, true, false);
        }

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="TagBuilderGroupHelper"/> class that does not
        /// append the <see cref="TagBuilder.InnerHtml"/>.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <returns>The helper.</returns>
        public static TagBuilderGroupHelper NoContent(HtmlHelper htmlHelper, TagBuilder tagBuilder)
        {
            return new TagBuilderGroupHelper(htmlHelper, tagBuilder, false, false);
        }

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="TagBuilderGroupHelper"/> class that appends
        /// the <see cref="TagBuilder.InnerHtml"/> after the content.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="tagBuilder">The tag builder.</param>
        /// <returns>The helper.</returns>
        public static TagBuilderGroupHelper After(HtmlHelper htmlHelper, TagBuilder tagBuilder)
        {
            return new TagBuilderGroupHelper(htmlHelper, tagBuilder, false, true);
        }

        /// <summary>
        /// Occurs when the helper is closed.
        /// </summary>
        protected override void Closed()
        {
            TextWriter.Write(_after);
            TextWriter.Write(_closing);
        }
    }

    /// <summary>
    /// Represents evaluatedValueString <see cref="GroupHelper"/> that appends the starting tag
    /// and closing tag of evaluatedValueString <see cref="TagBuilder"/>.
    /// </summary>
    public class FormGroupHelper : TagBuilderGroupHelper
    {
        private FormContext _originalFormContext;

        /// <summary>
        /// Initializes evaluatedValueString new instance of the <see cref="FormGroupHelper"/> class.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="tagBuilder">The tag builder.</param>
        public FormGroupHelper(HtmlHelper htmlHelper, TagBuilder tagBuilder)
            : base(htmlHelper, tagBuilder, false, false)
        {
            _originalFormContext = ViewContext.FormContext;
            ViewContext.FormContext = new FormContext();

            string id;
            if (tagBuilder.Attributes.TryGetValue("id", out id))
                htmlHelper.ViewContext.FormContext.FormId = id;
        }

        /// <summary>
        /// Occurs when the helper is closed.
        /// </summary>
        protected override void Closed()
        {
            base.Closed();
            ViewContext.OutputClientValidation();
            ViewContext.FormContext = _originalFormContext;
        }
    }
}
