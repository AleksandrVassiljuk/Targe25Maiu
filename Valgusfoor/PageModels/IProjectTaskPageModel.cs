using CommunityToolkit.Mvvm.Input;
using Valgusfoor.Models;

namespace Valgusfoor.PageModels
{
    public interface IProjectTaskPageModel
    {
        IAsyncRelayCommand<ProjectTask> NavigateToTaskCommand { get; }
        bool IsBusy { get; }
    }
}