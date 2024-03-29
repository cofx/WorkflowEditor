﻿using Microsoft.AspNetCore.Components;

namespace Blazor.WorkflowEditor.Components.Modals;

public interface IModal {
    Task<ModalResult> Show();
    ModalResult ModalResult { get; }
}

public class ModalObject {
    public CancellationTokenSource RenderCancelationToken { get; set; } = new CancellationTokenSource();
    public Type ModalType { get; internal set; }
    public Dictionary<string, object> Parameters { get; internal set; }
    public object? ObjRef { get; set; }

    readonly TaskCompletionSource task = new();

    public ModalObject(Type modalType, Dictionary<string, object> parameters) {
        ModalType = modalType;
        Parameters = parameters;
    }

    public void AfterFirstRender() {
        task.SetResult();
    }

    public Task GetDialog() => task.Task;
}

public interface IModalService {
    Task<TModal> ShowModal<TModal>(Dictionary<string, object> parameters) where TModal : class;
}

public partial class ModalService : IModalService {
    public List<ModalObject> Modals { get; set; }

    public ModalService() {
        Modals = new List<ModalObject>();
    }

    public async Task<TModal> ShowModal<TModal>(Dictionary<string, object> parameters) where TModal : class {
        var modalObj = new ModalObject(typeof(TModal), parameters) ?? throw new SystemException("Can`t create modal");
        Modals.Add(modalObj);
        StateHasChanged();

        await modalObj.GetDialog();
        var modal = (modalObj.ObjRef as DynamicComponent)?.Instance as TModal;
        if (modal != null && (modal is IModal) == true) {
            if (modal is IModal imodal) {
                await imodal.Show();
            }
        } else {
            throw new SystemException("Modal instance is null");
        }
        return modal;
    }

}
