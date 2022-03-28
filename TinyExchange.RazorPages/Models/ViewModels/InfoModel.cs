namespace TinyExchange.RazorPages.Models.ViewModels;

public class InfoModel
{
    public string Title { get; set; }
    public string Message { get; set; }
    public string ReturnController { get; set; }
    public string ReturnAction { get; set; }


    public InfoModel(string title, string message, string returnController, string returnAction)
    {
        Title = title;
        Message = message;
        ReturnController = returnController;
        ReturnAction = returnAction;
    }
}