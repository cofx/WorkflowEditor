using System.Xaml;

namespace Blazor.WorkflowEditor.Activity.State;

public static class Designer {
    //public static readonly AttachableMemberIdentifier ViewStateIdentifier = new(typeof(Designer), "ViewState");
    //public static ViewState Get(object target) {
    //    AttachablePropertyServices.TryGetProperty<ViewState>(target, ViewStateIdentifier, out var value);
    //    return value;
    //}
    //public static void Set(object target, ViewState value) {
    //    AttachablePropertyServices.SetProperty(target, ViewStateIdentifier, value);
    //}

    static T? getValue<T>(object target, AttachableMemberIdentifier propery) {
        return AttachablePropertyServices.TryGetProperty<T>(target, propery, out var v) ? v : default;
    }

    static void setValue<T>(object target, AttachableMemberIdentifier propery, T value) {
        if (value is not null)
            AttachablePropertyServices.SetProperty(target, propery, value);
        else
            AttachablePropertyServices.RemoveProperty(target, propery);
    }

    public static bool HasProperty(object target) =>
        AttachablePropertyServices.GetAttachedPropertyCount(target) > 0;

    #region CenterX
    public static readonly AttachableMemberIdentifier CenterXProperty = new(typeof(Designer), "CenterX");
    public static int? GetCenterX(object target) => getValue<int?>(target, CenterXProperty);
    public static void SetCenterX(object target, int? value) => setValue(target, CenterXProperty, value);
    #endregion

    #region CenterY
    public static readonly AttachableMemberIdentifier CenterYProperty = new(typeof(Designer), "CenterY");
    public static int? GetCenterY(object target) => getValue<int?>(target, CenterYProperty);
    public static void SetCenterY(object target, int? value) => setValue(target, CenterYProperty, value);
    #endregion

}

public enum PortAlignment {
    Top = 0,
    TopRight = 1,
    Right = 2,
    BottomRight = 3,
    Bottom = 4,
    BottomLeft = 5,
    Left = 6,
    TopLeft = 7
}

/*
public class ViewState {

    //public static readonly AttachableMemberIdentifier CenterXProperty = new(typeof(ViewState), "CenterX");
    //public static int? GetCenterX(object target) =>
    //     AttachablePropertyServices.TryGetProperty<int?>(target, CenterXProperty, out var v) ? v : null;
    //public static void SetCenterX(object target, int? centerX) =>
    //     AttachablePropertyServices.SetProperty(target, CenterXProperty, centerX);


    public int? CenterX { get; set; }
    public int? CenterY { get; set; }
    public int? Widht { get; set; }
    public int? Height { get; set; }
    public int? OffcetX { get; set; }
    public int? OffcetY { get; set; }
    public string? Comment { get; set; }
    public bool? IsExpanded { get; set; }
    public double? Zoom { get; set; }
    public PortAlignment? IncomingPortAlign { get; set; }
    public PortAlignment? OutcomingPortAlign { get; set; }

    public bool IsEmpty() {
        return CenterX == null && CenterY == null &&
            Widht == null && Height == null &&
            Comment == null &&
            IsExpanded == null &&
            Zoom == null &&
            OffcetX == null && OffcetY == null &&
            IncomingPortAlign == null &&
            OutcomingPortAlign == null;
    }

}
*/
