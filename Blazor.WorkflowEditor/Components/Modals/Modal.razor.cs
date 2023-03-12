using Microsoft.AspNetCore.Components;

namespace Blazor.WorkflowEditor.Components.Modals;

public enum ModalResult {
    None,
    Applied,
    Canceled,
    Showing
}

public partial class Modal : ComponentBase, IModal, IDisposable {
    private readonly TaskCompletionSource<ModalResult> _taskCompletionSource = new();
    private bool _isShowed = false;
    public bool IsShowed {
        get {
            return _isShowed;
        }
        internal set {
            if (value != _isShowed) {
                _isShowed = value;
                StateHasChanged();
            }
        }
    }
    public ModalResult ModalResult { get; internal set; } = ModalResult.None;
    [Parameter]
    public RenderFragment? Header { get; set; } = default;
    [Parameter]
    public RenderFragment? Body { get; set; } = default;
    [Parameter]
    public RenderFragment? Footer { get; set; } = default;
    [Parameter]
    public bool? ApplyButtonVisible { get; set; } = true;
    [Parameter]
    public string? ApplyButtonText { get; set; } = "Apply";
    [Parameter]
    public string? ApplyButtonClass { get; set; } = "btn btn-primary";
    [Parameter]
    public bool? CancelButtonVisible { get; set; } = true;
    [Parameter]
    public string? CancelButtonText { get; set; } = "Cancel";
    [Parameter]
    public string? CancelButtonClass { get; set; } = "btn btn-secondary";
    [Parameter]
    public bool? CloseButtonVisible { get; set; } = true;
    [Parameter]
    public string? CloseButtonClass { get; set; } = "btn-close";
    [Parameter]
    public string? TitleText { get; set; } = string.Empty;
    [Parameter]
    public EventCallback OnStateChanged { get; set; }

    public Modal() : base() {
    }

    public Task<ModalResult> Show() {
        IsShowed = true;
        return _taskCompletionSource.Task;
    }

    public void Hide() {
        UpdateState(ModalResult.Canceled);
    }

    public void Close() {
        UpdateState(ModalResult.Canceled);
    }

    public void Apply() {
        UpdateState(ModalResult.Applied);
    }

    public void Dispose() {
        UpdateState(ModalResult.Canceled);
        GC.SuppressFinalize(this);
    }
    private void UpdateState(ModalResult result) {
        ModalResult = result;
        if (!_taskCompletionSource.Task.IsCompleted) {
            _taskCompletionSource.SetResult(result);
        }
        IsShowed = false;
    }
}
