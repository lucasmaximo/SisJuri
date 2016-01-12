using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Sisjuri.Helpers
{
    public static class HtmlHelpers
    {

        public static MvcHtmlString FileBox(this HtmlHelper htmlHelper, string name)
        {
            return htmlHelper.FileBox(name, (object)null);
        }

        public static MvcHtmlString FileBox(this HtmlHelper htmlHelper, string name, object htmlAttributes)
        {
            return htmlHelper.FileBox(name, new RouteValueDictionary(htmlAttributes));
        }

        public static MvcHtmlString FileBox(this HtmlHelper htmlHelper, string name, IDictionary<String, Object> htmlAttributes)
        {
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.MergeAttribute("type", "file", true);
            tagBuilder.MergeAttribute("name", name, true);
            tagBuilder.GenerateId(name);

            ModelState modelState;
            if (htmlHelper.ViewData.ModelState.TryGetValue(name, out modelState))
            {
                if (modelState.Errors.Count > 0)
                {
                    tagBuilder.AddCssClass(HtmlHelper.ValidationInputCssClassName);
                }
            }

            return MvcHtmlString.Create(tagBuilder.ToString(TagRenderMode.SelfClosing));

        }
    }
}