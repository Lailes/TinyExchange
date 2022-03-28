using System.Globalization;
using System.Security.Policy;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Mvc;

namespace TinyExchange.RazorPages.Infrastructure.TagHelpers;

[HtmlTargetElement("pagination")]
public class PaginationTagHelper : TagHelper
{
    public string SelectedButtonClass { get; set; } = string.Empty;
    public string UnselectedButtonClass { get; set; } = string.Empty;
    public string DivClass { get; set; } = string.Empty;
    public string ButtonStartClass { get; set; } = string.Empty;
    public string ButtonEndClass { get; set; } = string.Empty;
    public string ProcessPage { get; set; } = string.Empty;
    public PaginationModel? PageModel { get; set; }
    
    public override void Process (TagHelperContext context, TagHelperOutput output)
    {
        if (PageModel == null) return;
        
        var builder = new TagBuilder("div");
        builder.AddCssClass(DivClass);
        
        var buttonTagBuilder = new TagBuilder("div");

        var page = PageModel.PageNumber;
        var pageCount = (int) Math.Ceiling((double) PageModel.TotalItems / PageModel.ItemsPerPage);
        if (pageCount < 5)
        {
            for (var i = 1; i <= pageCount; i++)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(i == page ? SelectedButtonClass : UnselectedButtonClass);
                tag.Attributes["href"] = ProcessPage + $"?page={i - 1}";
                tag.InnerHtml.Append(i.ToString());
                buttonTagBuilder.InnerHtml.AppendHtml(tag);
            }
        }
        else
        {
            var tagFirst = new TagBuilder("a");
            tagFirst.AddCssClass(0 == page ? SelectedButtonClass : ButtonStartClass);
            tagFirst.Attributes["href"] = ProcessPage + "?page=0";
            tagFirst.InnerHtml.AppendHtml("1");
            buttonTagBuilder.InnerHtml.AppendHtml(tagFirst);

            var tempStart = PageModel.PageNumber - 3;
            var tempEnd = PageModel.PageNumber + 3;
            var start = tempStart <= 1 ? 2 : tempStart;
            var end = tempEnd >= pageCount ? pageCount - 1 : tempEnd;

            for (var i = start; i <= end; i++)
            {
                var tag = new TagBuilder("a");
                tag.AddCssClass(i == page ? SelectedButtonClass : UnselectedButtonClass);
                tag.Attributes["href"] = ProcessPage + $"?page={i}";
                tag.InnerHtml.Append(i.ToString());
                buttonTagBuilder.InnerHtml.AppendHtml(tag);
            }

            var lastTag = new TagBuilder("a");
            lastTag.AddCssClass(pageCount == page ? SelectedButtonClass : ButtonEndClass);
            lastTag.Attributes["href"] = ProcessPage + $"?page={pageCount}";
            lastTag.InnerHtml.AppendHtml(pageCount.ToString(CultureInfo.InvariantCulture));
            buttonTagBuilder.InnerHtml.AppendHtml(lastTag);
        }

        buttonTagBuilder.Attributes["style"] = "text-align:center;";
        builder.InnerHtml.AppendHtml(buttonTagBuilder);
        output.Content.AppendHtml(builder.InnerHtml);
    }
}

public class PaginationModel
{
    public int PageNumber { get; }
    public int ItemsPerPage { get; }
    public int TotalItems { get; }
    
    public PaginationModel(int pageNumber, int itemsPerPage, int totalItems)
    {
        PageNumber = pageNumber;
        ItemsPerPage = itemsPerPage;
        TotalItems = totalItems;
    }
}