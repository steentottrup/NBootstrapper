using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq.Expressions;
using System.Globalization;
using System.Web.Mvc.Html;
using System.Text;

namespace NBootstrapper.Mvc.Html
{
    /// <summary>
    /// Represents extensions methods for <see cref="HtmlHelper"/> for validation.
    /// </summary>
    public static class ValidationExtensions
    {
        #region Helpers
        private static FieldValidationMetadata ApplyFieldValidationMetadata(HtmlHelper htmlHelper, ModelMetadata modelMetadata, string modelName)
        {
            var formContext = htmlHelper.ViewContext.FormContext;
            var validationMetadataForField = formContext.GetValidationMetadataForField(modelName, true);
            var validators = ModelValidatorProviders.Providers.GetValidators(modelMetadata, htmlHelper.ViewContext);

            foreach (var current in validators.SelectMany((ModelValidator v) => v.GetClientValidationRules()))
            {
                validationMetadataForField.ValidationRules.Add(current);
            }

            return validationMetadataForField;
        }

        private static string GetInvalidPropertyValueResource(HttpContextBase httpContext)
        {
            string text = null;
            if (!string.IsNullOrEmpty(System.Web.Mvc.Html.ValidationExtensions.ResourceClassKey) && httpContext != null)
                text = (httpContext.GetGlobalResourceObject(System.Web.Mvc.Html.ValidationExtensions.ResourceClassKey, "InvalidPropertyValue", CultureInfo.CurrentUICulture) as string);
            return text ?? Properties.Resources.Common_ValueNotValidForProperty;
        }

        private static string GetUserErrorMessageOrDefault(HttpContextBase httpContext, ModelError error, ModelState modelState)
        {
            if (!string.IsNullOrEmpty(error.ErrorMessage))
                return error.ErrorMessage;
            if (modelState == null)
                return null;
            var text = (modelState.Value != null) ? modelState.Value.AttemptedValue : null;
            return string.Format(CultureInfo.CurrentCulture, GetInvalidPropertyValueResource(httpContext), text);
        }
        #endregion

        #region ValidationMessage
        /// <summary>
        /// Displays evaluatedValueString validation message if an error exists for the specified field in the System.Web.Mvc.ModelStateDictionary object.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="modelName">The name of the property or model object that is being validated.</param>
        /// <param name="validationMessage">The message to display if the specified field contains an error.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        /// <returns>
        /// If the property or object is valid, an empty string; otherwise, evaluatedValueString span element that contains an error message.
        /// </returns>
        public static MvcHtmlString ValidationMessage(this HtmlHelper htmlHelper, string modelName, string validationMessage = null, object htmlAttributes = null)
        {
            if (modelName == null)
                throw new ArgumentNullException("modelName");
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            return htmlHelper.ValidationMessageForImpl(ModelMetadata.FromStringExpression(modelName, htmlHelper.ViewContext.ViewData), modelName, validationMessage, dictionary);
        }
        #endregion

        #region ValidationMessageFor
        /// <summary>
        /// Returns the HTML markup for evaluatedValueString validation-error message for each data field
        /// that is represented by the specified expression, using the specified message
        /// and HTML attributes.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
        /// <param name="validationMessage">The message to display if the specified field contains an error.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        /// <returns>If the property or object is valid, an empty string; otherwise, evaluatedValueString span element that contains an error message.</returns>
        public static MvcHtmlString ValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string validationMessage = null, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            return htmlHelper.ValidationMessageForImpl(ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression), validationMessage, dictionary);
        }

        private static MvcHtmlString ValidationMessageForImpl(this HtmlHelper htmlHelper, ModelMetadata modelMetadata, string expression, string validationMessage, IDictionary<string, object> htmlAttributes)
        {
            var fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(expression);

            if (!htmlHelper.ViewData.ModelState.ContainsKey(fullHtmlFieldName))
                return null;

            var modelState = htmlHelper.ViewData.ModelState[fullHtmlFieldName];
            var modelErrorCollection = (modelState == null) ? null : modelState.Errors;
            var error = modelErrorCollection != null && modelErrorCollection.Count != 0 ?
                (modelErrorCollection.FirstOrDefault((ModelError m) => !string.IsNullOrEmpty(m.ErrorMessage)) ?? modelErrorCollection[0]) :
                null;

            if (error == null)
                return BootstrapHelperConfiguration.HtmlRenderer.ValidationMessageFor(htmlHelper, fullHtmlFieldName, Common.NameOf(modelMetadata, fullHtmlFieldName), null, htmlAttributes);
            else
            {
                validationMessage = validationMessage ?? GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, error, modelState);
                return BootstrapHelperConfiguration.HtmlRenderer.ValidationMessageFor(htmlHelper, fullHtmlFieldName, Common.NameOf(modelMetadata, fullHtmlFieldName), validationMessage, htmlAttributes);
            }
        }
        #endregion

        #region ValidationSummary
        /// <summary>
        /// Returns an unordered list (ul element) of validation messages that are in
        /// the System.Web.Mvc.ModelStateDictionary object and optionally displays only
        /// model-level errors.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="excludePropertyErrors"><c>true</c> to have the summary display model-level errors only, or <c>false</c> to have the summary display all errors.</param>
        /// <param name="title">The to use for the alert.</param>
        /// <param name="message">The message to display with the validation summary.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes for the element.</param>
        /// <returns>A string that contains an unordered list (ul element) of validation messages.</returns>
        public static MvcHtmlString ValidationSummary(this HtmlHelper htmlHelper, bool excludePropertyErrors = false, string title = null, string message = null, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            return htmlHelper.ValidationSummaryImpl(excludePropertyErrors, title, message, dictionary);
        }

        private static MvcHtmlString ValidationSummaryImpl(this HtmlHelper htmlHelper, bool excludePropertyErrors, string title, string message, IDictionary<string, object> htmlAttributes)
        {
            if (htmlHelper == null)
                throw new ArgumentNullException("htmlHelper");

            if (htmlHelper.ViewData.ModelState.IsValid)
                return BootstrapHelperConfiguration.HtmlRenderer.ValidationSummary(htmlHelper, excludePropertyErrors, title ?? Properties.Resources.Common_DefaultTitle, message, null, htmlAttributes);

            var listBodyBuilder = new StringBuilder();
            var listBuilder = new TagBuilder("ul");
            IEnumerable<ModelState> errors = null;
            if (excludePropertyErrors)
            {
                ModelState modelState;
                htmlHelper.ViewData.ModelState.TryGetValue(htmlHelper.ViewData.TemplateInfo.HtmlFieldPrefix, out modelState);
                if (modelState != null)
                    errors = new[] { modelState };
            }
            else
            {
                errors = htmlHelper.ViewData.ModelState.Values;
            }

            Tuple<ModelError, string>[] errorsList = null;
            if (errors != null)
            {
                errorsList = errors.SelectMany(x => x.Errors).Select(x => Tuple.Create(x, GetUserErrorMessageOrDefault(htmlHelper.ViewContext.HttpContext, x, null))).ToArray();
            }

            return BootstrapHelperConfiguration.HtmlRenderer.ValidationSummary(htmlHelper, excludePropertyErrors, title ?? Properties.Resources.Common_DefaultTitle, message, errorsList, htmlAttributes);
        }
        #endregion
    }
}