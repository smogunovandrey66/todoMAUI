using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using todoMAUI.Models;

namespace todoMAUI.PageModels
{
    public partial class TodoItemsModel : ObservableObject
    {
        private TodoItemsRepository _repository;

        private ILogger _logger;

        [ObservableProperty]
        private string _title = string.Empty;

        [ObservableProperty]
        private ObservableCollection<TodoItem> _todoItems = [];

        [RelayCommand]
        private void AddItem(/* нерекомендуемый способ передачи параметров, но можно) string titleNotRecomendForMVVM*/)
        {
            _logger.LogDebug($"AddItem in viewmodel {Title}");
            TodoItem item = new TodoItem();
            item.Title = Title;

            _repository.Save(item);

            TodoItems.Insert(0, item);

            _logger.LogDebug($"AddItem with result={item.ToString()}, todoItems={TodoItems.Count}");
            Title = "";
        }

        public TodoItemsModel(ILogger<TodoItemsModel> logger, TodoItemsRepository repository)
        {
            _repository = repository;
            _logger = logger;
            _logger.LogDebug($"TodoItemsModel constructor");

            //TodoItems.Add(new TodoItem { Title = "Для теста" });
            //_logger.LogDebug("Title first=" + TodoItems.First<TodoItem>().Title);
        }

        private void Refresh()
        {
            foreach (var todoItem in _repository.GetTodoItems())
            {
                TodoItems.Add(todoItem);
            }
        }

        [RelayCommand]
        private void Appearing()
        {
            Refresh();
        }

        [RelayCommand]
        private void ItemClick(TodoItem obj)
        {
            obj.IsDone = !obj.IsDone;
            _repository.ToggleItem(obj);
            _logger.LogDebug($"ItemClick In model {obj.ToString()}");
        }

        [RelayCommand]
        private void DeleteItem(TodoItem obj)
        {
            if (_repository.RemoveItem(obj))
                TodoItems.Remove(obj);
        }
    }
}
