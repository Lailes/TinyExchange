using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TinyExchange.RazorPages.Pages.Shared;

public class Message : PageModel
{
    public string MessageText { get; set; } = string.Empty;

    public string BackUrl { get; set; } = string.Empty;
    
    public void OnGet(string message, string backUrl) => (MessageText, BackUrl) = (message, backUrl);
}