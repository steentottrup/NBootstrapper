using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Web.Routing;
using System.Data.Linq;
using System.Globalization;
using System.Web.Mvc;

namespace NBootstrapper.Mvc.Html
{
    // TODO: DocComments

    public static class InputExtensions
    {
        #region Checkbox
        public static MvcHtmlString CheckBox(this HtmlHelper htmlHelper, string name, bool isChecked = false, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            return CheckBoxHelper(htmlHelper, null, name, isChecked, dictionary);
        }

        public static MvcHtmlString CheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, object htmlAttributes = null)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");

            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            var modelMetadata = ModelMetadata.FromLambdaExpression<TModel, bool>(expression, htmlHelper.ViewData);

            var isChecked = new bool?();
            bool value;
            if (modelMetadata.Model != null && bool.TryParse(modelMetadata.Model.ToString(), out value))
                isChecked = value;

            return CheckBoxHelper(htmlHelper, modelMetadata, ExpressionHelper.GetExpressionText(expression), isChecked, dictionary);
        }

        private static MvcHtmlString CheckBoxHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, bool? isChecked, IDictionary<string, object> htmlAttributes)
        {
            bool hasValue = isChecked.HasValue;
            if (hasValue)
                htmlAttributes.Remove("checked");
            return InputHelper(htmlHelper, InputType.CheckBox, metadata, name, "true", !hasValue, isChecked ?? false, true, false, htmlAttributes);
        }
        #endregion

        #region Hidden
        public static MvcHtmlString Hidden(this HtmlHelper htmlHelper, string name, object value = null, object htmlAttributes = null)
        {
            return HiddenHelper(htmlHelper, null, value, value == null, name, htmlHelper.AutomaticHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString HiddenFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            return HiddenHelper(htmlHelper, modelMetadata, modelMetadata.Model, false, ExpressionHelper.GetExpressionText(expression), htmlHelper.AutomaticHtmlAttributes(htmlAttributes));
        }

        private static MvcHtmlString HiddenHelper(HtmlHelper htmlHelper, ModelMetadata metadata, object value, bool useViewData, string expression, IDictionary<string, object> htmlAttributes)
        {
            var binary = value as Binary;
            if (binary != null)
                value = binary.ToArray();

            var array = value as byte[];
            if (array != null)
                value = Convert.ToBase64String(array);

            return InputHelper(htmlHelper, InputType.Hidden, metadata, expression, value, useViewData, false, true, true, htmlAttributes);
        }
        #endregion

        #region Password
        public static MvcHtmlString Password(this HtmlHelper htmlHelper, string name, object value = null, object htmlAttributes = null)
        {
            return PasswordHelper(htmlHelper, null, name, value, htmlHelper.AutomaticHtmlAttributes(htmlAttributes));
        }

        public static MvcHtmlString PasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            if (expression == null)
                throw new ArgumentNullException("expression");
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            return PasswordHelper(htmlHelper, ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData), ExpressionHelper.GetExpressionText(expression), null, dictionary);
        }

        private static MvcHtmlString PasswordHelper(HtmlHelper htmlHelper, ModelMetadata metadata, string name, object value, IDictionary<string, object> htmlAttributes)
        {
            return InputHelper(htmlHelper, InputType.Password, metadata, name, value, false, false, true, true, htmlAttributes);
        }
        #endregion

        #region RadioButton
        public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, object value = null, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            var valueAsString = Convert.ToString(value, (IFormatProvider)CultureInfo.CurrentCulture);
            var isChecked = !string.IsNullOrEmpty(name) && string.Equals(htmlHelper.EvalString(name), valueAsString, StringComparison.OrdinalIgnoreCase);
            if (dictionary.ContainsKey("checked"))
                return InputHelper(htmlHelper, InputType.Radio, (ModelMetadata)null, name, value, false, false, true, true, dictionary);
            else
                return RadioButton(htmlHelper, name, value, isChecked, htmlAttributes);
        }

        public static MvcHtmlString RadioButton(this HtmlHelper htmlHelper, string name, object value = null, bool isChecked = false, object htmlAttributes = null)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            dictionary.Remove("checked");
            return InputHelper(htmlHelper, InputType.Radio, (ModelMetadata)null, name, value, false, isChecked, true, true, dictionary);
        }

        public static MvcHtmlString RadioButtonFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object value = null, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            return RadioButtonHelper((HtmlHelper)htmlHelper, metadata, metadata.Model, ExpressionHelper.GetExpressionText((LambdaExpression)expression), value, null, dictionary);
        }

        private static MvcHtmlString RadioButtonHelper(HtmlHelper htmlHelper, ModelMetadata metadata, object model, string name, object value, bool? isChecked, IDictionary<string, object> htmlAttributes)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            var hasValue = isChecked.HasValue;
            if (hasValue)
            {
                htmlAttributes.Remove("checked");
            }
            else
            {
                string valueAsString = Convert.ToString(value, CultureInfo.CurrentCulture);
                isChecked = new bool?(model != null && !string.IsNullOrEmpty(name) && string.Equals(model.ToString(), valueAsString, StringComparison.OrdinalIgnoreCase));
            }
            return InputExtensions.InputHelper(htmlHelper, InputType.Radio, metadata, name, value, false, isChecked ?? false, true, true, htmlAttributes);
        }
        #endregion

        #region TextBox
        public static MvcHtmlString TextBox(this HtmlHelper htmlHelper, string name, object value = null, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            return InputExtensions.InputHelper(htmlHelper, InputType.Text, null, name, value, value == null, false, true, true, dictionary);
        }

        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null)
        {
            var dictionary = htmlHelper.AutomaticHtmlAttributes(htmlAttributes);
            var modelMetadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, htmlHelper.ViewData);
            return htmlHelper.TextBoxHelper(modelMetadata, modelMetadata.Model, ExpressionHelper.GetExpressionText(expression), dictionary);
        }

        private static MvcHtmlString TextBoxHelper(this HtmlHelper htmlHelper, ModelMetadata metadata, object model, string expression, IDictionary<string, object> htmlAttributes)
        {
            return InputExtensions.InputHelper(htmlHelper, InputType.Text, metadata, expression, model, false, false, true, true, htmlAttributes);
        }
        #endregion

        private static MvcHtmlString InputHelper(HtmlHelper htmlHelper, InputType inputType, ModelMetadata metadata, string name, object value, bool useViewData, bool isChecked, bool setId, bool isExplicitValue, IDictionary<string, object> htmlAttributes)
        {
            string fullHtmlFieldName = htmlHelper.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(name);
            if (string.IsNullOrEmpty(fullHtmlFieldName))
                throw new ArgumentNullException("name");

            var modelValueAsString = htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof(string)) as string;
            var valueAsString = Convert.ToString(value, (IFormatProvider)CultureInfo.CurrentCulture);

            var overrideByModel = false;
            switch (inputType)
            {
                case InputType.CheckBox:
                    bool? nullable = htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof(bool)) as bool?;
                    if (nullable.HasValue)
                    {
                        isChecked = nullable.Value;
                        overrideByModel = true;
                    }
                    goto case InputType.Radio;
                case InputType.Radio:
                    if (!overrideByModel)
                    {
                        if (modelValueAsString != null)
                        {
                            isChecked = string.Equals(modelValueAsString, valueAsString, StringComparison.Ordinal);
                            overrideByModel = true;
                        }
                    }
                    if (!overrideByModel && useViewData)
                        isChecked = (htmlHelper.GetModelStateValue(fullHtmlFieldName, typeof(bool)) as bool?).GetValueOrDefault();
                    break;
            }

            if (isExplicitValue)
                valueAsString = valueAsString ?? (useViewData ? htmlHelper.EvalString(fullHtmlFieldName) : valueAsString);
            if (inputType == InputType.Password && !isExplicitValue)
                valueAsString = null;

            return BootstrapHelperConfiguration.HtmlRenderer.Input(htmlHelper, inputType, fullHtmlFieldName, name, valueAsString, isChecked, setId, htmlAttributes);
        }
    }
}
