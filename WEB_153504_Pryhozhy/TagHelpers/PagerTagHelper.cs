using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("Pager")]
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
        divTag.InnerHtml.AppendHtml(CreatePageLink("&laquo;", CurrentPage - 1, CurrentPage == 1));

        // Add page links for each page number
        for (var pageNum = 1; pageNum <= TotalPages; pageNum++)
        {
            divTag.InnerHtml.AppendHtml(CreatePageLink(pageNum.ToString(), pageNum, pageNum == CurrentPage));
        }

        // Add the next page link
        divTag.InnerHtml.AppendHtml(CreatePageLink("&raquo;", CurrentPage + 1, CurrentPage == TotalPages));

        // Set the result as the inner HTML of the custom-pagination tag
        output.TagName = "ul";
        output.Content.SetHtmlContent(divTag);
    }

    private TagBuilder CreatePageLink(string text, int pageNo, bool isDisabled)
    {
        var liTag = new TagBuilder("li");
        liTag.AddCssClass("page-item");
        liTag.AddCssClass(isDisabled ? "disabled" : "");

        var aTag = new TagBuilder("a");
        aTag.AddCssClass("page-link");

        // Hardcode the controller name here (e.g., "pizza")
        var controller = "pizza";

        // Generate the URL with the hardcoded controller
        aTag.Attributes.Add("href", $"/{controller}?pageNo={pageNo}");
        aTag.InnerHtml.AppendHtml(text);

        liTag.InnerHtml.AppendHtml(aTag);

        return liTag;
    }
}
