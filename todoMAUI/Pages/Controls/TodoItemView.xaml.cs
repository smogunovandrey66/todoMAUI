using Microsoft.Extensions.Logging;
using todoMAUI.Models;

namespace todoMAUI.Pages.Controls;

public partial class TodoItemView
{
    private readonly ILogger? _logger;

	public TodoItemView()
	{
		InitializeComponent();
        _logger = IPlatformApplication.Current?.Services.GetService<ILogger<TodoItemView>>();
	}

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        //CheckBoxDone.IsChecked = !CheckBoxDone.IsChecked;
        _logger?.LogDebug($"TapGestureRecognizer_Tapped in TodoItem code behind{sender}");
    }

    private void CheckBoxDone_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        var pageModel = GetTodoItemsModel();
        //_logger?.LogDebug($"checkBoxDone_CheckedChanged in TodoItem code behind{sender}");
    }

    private ContentPage GetParentPage()
    {
        Element parent = this;
        while (parent != null && parent is not ContentPage)
        {
            parent = parent.Parent;
        }

        return parent as ContentPage;
    }

    private TodoItemsModel GetTodoItemsModel()
    {
        var page = this.GetParentPage();

        return page?.BindingContext as TodoItemsModel;
    }
}