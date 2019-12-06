using Nightingale.Core.Storage;
using System.Collections.ObjectModel;

namespace Nightingale.Core.Models
{
    public interface IWorkspaceRequest : IStorageItem, IHydratable
    {
        int MethodIndex { get; set; }

        string BaseUrl { get; set; }

        RequestBody Body { get; set; }

        ObservableCollection<Parameter> Queries { get; }

        ObservableCollection<Parameter> Headers { get; }

        ObservableCollection<Parameter> ChainingRules { get; }

        Authentication Authentication { get; set; }

        ObservableCollection<ApiTest> ApiTests { get; }

        WorkspaceResponse WorkspaceResponse { get; set; }

        int RequestPivotIndex { get; set; }

        int ResponsePivotIndex { get; set; }

        int Position { get; set; }
    }
}
