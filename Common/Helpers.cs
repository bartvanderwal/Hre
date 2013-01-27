using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using System.Runtime.InteropServices;
using System.Threading;

namespace HRE.Common {

    public static class Helpers {
        /// <summary>
        /// Show an image.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="action"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        /// <param name="imagePath"></param>
        /// <param name="alt"></param>
        /// <param name="cls"></param>
        /// <returns></returns>
        public static MvcHtmlString ActionImage(this HtmlHelper html, string action, string controllerName, object routeValues, string imagePath, string alt, string cls) {
            var url = new UrlHelper(html.ViewContext.RequestContext);

            // Build the <img> image tag.
            var imgBuilder = new TagBuilder("img");
            imgBuilder.MergeAttribute("src", url.Content(imagePath));
            if (alt != null) {
                imgBuilder.MergeAttribute("alt", alt);
                imgBuilder.MergeAttribute("title", alt);
            }
            if (cls != null)
                imgBuilder.MergeAttribute("class", cls);
            string imgHtml = imgBuilder.ToString(TagRenderMode.SelfClosing);

            // Build the <a> anchor tag.
            var anchorBuilder = new TagBuilder("a");
            if (routeValues == null)
                anchorBuilder.MergeAttribute("href", url.Action(action, controllerName));
            else
                anchorBuilder.MergeAttribute("href", url.Action(action, controllerName, routeValues));
            anchorBuilder.InnerHtml = imgHtml; // include the <img> tag inside
            string imageHtml = anchorBuilder.ToString(TagRenderMode.Normal);

            return MvcHtmlString.Create(imageHtml);
        }


        /// <summary>
        /// Shows the text with an optional read more link.
        /// </summary>
        /// <param name="html"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        public static MvcHtmlString TextWithReadMoreLink(this HtmlHelper html, string text) {
            int maxLenght = HreSettings.CharacterLengthForReadMore;
            
            // If there is no text, the text is not long enough, or the settings specifies readmore link should not be used then just return the text.
            if (string.IsNullOrEmpty(text) || text.Length <= maxLenght || !HreSettings.UseReadMoreLinks) {
                return MvcHtmlString.Create(text);
            }
            
            var outerSpan = new TagBuilder("span");
            
            // Make a span with the initially visible text.
            var initialSpan = new TagBuilder("span");
            initialSpan.MergeAttribute("class", "initial-text");
            initialSpan.InnerHtml = text.Substring(0, Math.Max(text.Length, maxLenght));
            
            // Make the read more link.
            var readMoreLink = new TagBuilder("a");
            readMoreLink.MergeAttribute("class", "read-more-link");
            readMoreLink.InnerHtml = "Lees meer...";
                
            var hiddenSpan = new TagBuilder("span");
            hiddenSpan.MergeAttribute("class", "read-more-text");
            hiddenSpan.InnerHtml = text.Substring(maxLenght, text.Length-maxLenght);
            
            outerSpan.InnerHtml = initialSpan.ToString() + readMoreLink.ToString() + hiddenSpan.ToString();
            
            return MvcHtmlString.Create(outerSpan.ToString());
        }


        public static MvcHtmlString DescriptionValueFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression) {
            var metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var description = metadata.Description;

            return MvcHtmlString.Create(string.Format(@"{0}", description));
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="listOfValues"></param>
        /// <returns></returns>
        public static MvcHtmlString RadioButtonsForList<TModel, TProperty>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression,
            IEnumerable<SelectListItem> listOfValues) {
            var metaData = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var ul = new TagBuilder("ul");

            if (listOfValues != null) {
                // Create a radio button for each item in the list 
                foreach (SelectListItem item in listOfValues) {
                    // Generate an id to be given to the radio button field 
                    var id = string.Format("{0}_{1}", metaData.PropertyName, item.Value);

                    // Create and populate a radio button using the existing html helpers 
                    var label = htmlHelper.Label(id, HttpUtility.HtmlEncode(item.Text));
                    var radio = htmlHelper.RadioButtonFor(expression, item.Value, new { id = id }).ToHtmlString();

                    // Create the html string that will be returned to the client 
                    // e.g. <input data-val="true" data-val-required="You must select an option" id="TestRadio_1" name="TestRadio" type="radio" value="1" /><label for="TestRadio_1">Line1</label> 
                    ul.InnerHtml += string.Format("<li class=\"RadioButton\">{0}{1}</li>", radio, label);
                }
            }

            return MvcHtmlString.Create(ul.ToString());
        }


        /// <summary>
        /// Overwrite of the TextBoxFor method to automatically use the StringLenght attribute of a model property (e.g. for validation) als as the html maxlength property (e.g. for entry limitation).
        /// This is done using reflexion.
        /// Source: http://stackoverflow.com/questions/2386365/maxlength-attribute-of-a-text-box-from-the-dataannotations-stringlength-in-mvc2
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <param name="htmlAttributes">Default: empty</param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxWithMaxLengthFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
                Expression<Func<TModel, TProperty>> expression, object htmlAttributes = null, int maxWidth = 400) {
            var member = expression.Body as MemberExpression;
            var stringLength = member.Member.GetCustomAttributes(typeof(StringLengthAttribute), false).FirstOrDefault() as StringLengthAttribute;

            var attributes = (IDictionary<string, object>)new RouteValueDictionary(htmlAttributes);
            if (stringLength != null) {
                // Add the maxLength attribute to prevent overflow in the database.
                attributes.Add("maxlength", stringLength.MaximumLength);
                const int pixelsPerCharachter = 7;
                // Add a width based on the number of charachters, but only IF the htmlAttributes does NOT contain 'style' yet.
                if (!attributes.ContainsKey("style")) {
                    // Use a maximum to prevent layout issues for very loosely defined fields.
                    attributes.Add("style", "width: " + Math.Min(stringLength.MaximumLength * pixelsPerCharachter, maxWidth) + "px;");
                }
            }
            return htmlHelper.TextBoxFor(expression, attributes);
        }


        /// <summary>
        /// Create a radio button list from a Selectlist of options and a lambda referencing a
        /// model property.
        /// </summary>
        /// <typeparam name="TModel">Data entity type of your Model</typeparam>
        /// <typeparam name="TProperty">Data type of the property</typeparam>
        /// <param name="helper"></param>
        /// <param name="property">Lambda referencing the property of the model that will be the selected item/output of the selection</param>
        /// <param name="list">SelectList of properties</param>
        /// <param name="separator">Html to put between radio button/label pairs</param>
        /// <param name="inputhtmlAttributes">Styling for the button</param>
        /// <param name="labelhtmlAttributes">Styling for the label</param>
        /// <returns></returns>
        /// Not used yet... Check also: http://stackoverflow.com/questions/2512809/has-anyone-implement-radiobuttonlistfort-for-asp-net-mvc
        /// Based on source from: http://mindlesspassenger.wordpress.com/2010/10/06/missing-html-helpers/
        public static MvcHtmlString CustomRadioButtonListFor<TModel, TProperty> (
                this HtmlHelper<TModel> helper,
                Expression<Func<TModel, TProperty>> property,
                SelectList list,
                String separator = "",
                object inputhtmlAttributes = null,
                object labelhtmlAttributes = null) {
            // get the model metadata and verify we found something
            var modelmetadata = ModelMetadata.FromLambdaExpression(property, helper.ViewData);
            if (null == modelmetadata) throw new ArgumentException(property.ToString() + " not found");

            // get the property value and name from the metadata.
            var value = modelmetadata.Model ?? default(TProperty);
            var name = modelmetadata.PropertyName;

            // convert the value to a String for comparison with the SelectList contents.
            String selected = (null != value) ? value.ToString() : (null != list.SelectedValue) ? list.SelectedValue.ToString() : String.Empty;


            // build the output html for the radiobuttons and their labels.
            StringBuilder outstr = new StringBuilder();
            int counter = 0;
            foreach (SelectListItem item in list) {
                string id = name + "_" + counter.ToString();
                counter++;

                // the radio button
                TagBuilder input = new TagBuilder("input");
                input.Attributes.Add("id", id);
                input.Attributes.Add("name", name);
                input.Attributes.Add("type", "radio");
                input.Attributes.Add("value", item.Value);
                if (selected == item.Value || selected == item.Text)
                    input.Attributes.Add("checked", "checked");
                if (null != inputhtmlAttributes)
                    input.MergeAttributes(new RouteValueDictionary(inputhtmlAttributes));
                outstr.Append(input.ToString(TagRenderMode.SelfClosing));

                // the label
                TagBuilder label = new TagBuilder("label");
                label.Attributes.Add("for", id);
                label.InnerHtml = item.Text;
                if (null != labelhtmlAttributes)
                    label.MergeAttributes(new RouteValueDictionary(labelhtmlAttributes));
                outstr.Append(label.ToString(TagRenderMode.Normal));

                // add the separator
                outstr.Append(separator);
            }

            return MvcHtmlString.Create(outstr.ToString());
        }

    }
}