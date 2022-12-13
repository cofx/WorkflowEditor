using Microsoft.AspNetCore.Components;

namespace Blazor.WorkflowEditor.Components;

public partial class GenericTypeSelectModal : IDisposable {
    public Modal? Modal { get; set; } = new()!;
    public GenericTypeSelectModal() {
    }

    public Task<ModalResult> Show() {
        return Modal!.Show();
    }

    public void Dispose() {
        Modal!.Hide();
    }
}
