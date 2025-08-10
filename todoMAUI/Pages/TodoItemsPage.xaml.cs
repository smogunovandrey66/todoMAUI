using Microsoft.Extensions.Logging;

namespace todoMAUI.Pages

{
	public partial class TodoItemsPage : ContentPage
	{
		private readonly ILogger _logger;
		public TodoItemsPage(TodoItemsModel model, ILogger<TodoItemsPage> logger)
		{
			InitializeComponent();
			BindingContext = model;
			_logger = logger;
		}

		void OnAddClicked(object sender, EventArgs e)
		{
			var text = taskEntry.Text;
			_logger.LogDebug($"Add item click {text}");
		}

		private void ItemClick(object sender, EventArgs e)
		{
			_logger.LogDebug($"ItemClick code behind{sender}");
		}

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
			_logger.LogDebug($"TapGestureRecognizer_Tapped code behind{sender}");
        }
    }
}