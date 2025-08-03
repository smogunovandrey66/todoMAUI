using CommunityToolkit.Mvvm.Input;
using todoMAUI.Models;

namespace todoMAUI.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}