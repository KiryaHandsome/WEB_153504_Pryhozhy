using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using static System.Net.Mime.MediaTypeNames;

namespace WEB_153504_Pryhozhy.TagHelpers
{
    [HtmlTargetElement("Pager", Attributes = "current-page, total-pages")]
    public class PagerTagHelper : TagHelper
    {
        // Properties to configure the pagination
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Create a div element with the specified class
            var divTag = new TagBuilder("div");
            divTag.AddCssClass("pagination justify-content-center");

            // Add the previous page link
            divTag.InnerHtml.AppendHtml(CreateArrowLink("&laquo;", CurrentPage - 1));

            for (var pageNum = 1; pageNum <= TotalPages; pageNum++)
            {
                divTag.InnerHtml.AppendHtml(CreatePageLink(pageNum.ToString(), pageNum));
            }

            // Add the next page link
            divTag.InnerHtml.AppendHtml(CreateArrowLink("&raquo;", CurrentPage + 1));

            // Set the result as the inner HTML of the custom-pagination tag
            output.TagName = "ul";
            output.Content.SetHtmlContent(divTag);
        }

        private TagBuilder CreateArrowLink(string symbol, int targetPage)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");
            if (targetPage < 1 || targetPage > TotalPages)
            {
                liTag.AddCssClass("disabled");
            }
            else
            {
                liTag.AddCssClass("active");
            }

            var linkTag = new TagBuilder("a");
            linkTag.AddCssClass("page-link");
            linkTag.Attributes.Add("href", $"/catalog?pageNo={targetPage}");
            linkTag.InnerHtml.AppendHtml(symbol);
            liTag.InnerHtml.AppendHtml(linkTag);

            return liTag;
        }

        private TagBuilder CreatePageLink(string text, int pageNo)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");
            liTag.AddCssClass(CurrentPage == pageNo ? "active" : "");

            var aTag = new TagBuilder("a");
            aTag.AddCssClass("page-link");

            aTag.Attributes.Add("href", $"/catalog?pageNo={pageNo}");
            aTag.InnerHtml.AppendHtml(text);

            liTag.InnerHtml.AppendHtml(aTag);

            return liTag;
        }
    }

}
