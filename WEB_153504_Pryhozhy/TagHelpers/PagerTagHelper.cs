using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Routing;
using static System.Net.Mime.MediaTypeNames;

namespace WEB_153504_Pryhozhy.TagHelpers
{
    [HtmlTargetElement("Pager", Attributes = "current-page, total-pages")]
    public class PagerTagHelper : TagHelper
    {
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }

        private readonly LinkGenerator _linkGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagerTagHelper(LinkGenerator linkGenerator, IHttpContextAccessor httpContextAccessor)
        {
            _linkGenerator = linkGenerator;
            _httpContextAccessor = httpContextAccessor;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var divTag = new TagBuilder("div");
            divTag.AddCssClass("pagination justify-content-center");
            divTag.InnerHtml.AppendHtml(CreatePageLink("&laquo;", CurrentPage - 1, GetArrowClass)); // previous page link
            for (var pageNum = 1; pageNum <= TotalPages; pageNum++)
            {
                var pageLink = CreatePageLink(pageNum.ToString(), pageNum, GetPageClass);
                divTag.InnerHtml.AppendHtml(pageLink);
            }
            divTag.InnerHtml.AppendHtml(CreatePageLink("&raquo;", CurrentPage + 1, GetArrowClass)); // next page link

            output.TagName = "ul";
            output.Content.SetHtmlContent(divTag);
        }

        private string GetArrowClass(int pageNum)
        {
            if (pageNum < 1 || pageNum > TotalPages)
                return "disabled";
            else
                return "active";
        }

        private string GetPageClass(int pageNum)
        {
            return CurrentPage == pageNum ? "active" : "";
        }

        private TagBuilder CreatePageLink(string text, int targetPageNumber, Func<int, string> liTagClassProducer)
        {
            var liTag = new TagBuilder("li");
            liTag.AddCssClass("page-item");
            liTag.AddCssClass(liTagClassProducer.Invoke(targetPageNumber));

            var aTag = new TagBuilder("a");
            aTag.AddCssClass("page-link");

           var link = _linkGenerator.GetPathByPage(
                _httpContextAccessor.HttpContext,
                values: new { pageNo = targetPageNumber }
            );
            aTag.Attributes.Add("href", link);
            aTag.InnerHtml.AppendHtml(text);
            liTag.InnerHtml.AppendHtml(aTag);

            return liTag;
        }
    }

}
