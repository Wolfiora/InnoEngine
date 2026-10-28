using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.Settings;

[EditorAction(SettingsInteractionIds.C_OPEN)]
[EditorMenu(SettingsInteractionIds.C_MAIN_MENU_AREA, "Edit/Settings...", order: 1000)]
internal sealed class OpenSettingsAction(SettingsWindowModule window) : EditorAction
{
    /// <inheritdoc />
    protected override EditorActionState Query(EditorActionContext context)
        => new(true, window.canOpen, unavailableReason: window.unavailableReason);

    /// <summary>
    /// Applies the editor action to the supplied interaction context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void Execute(EditorActionContext context) => window.Open();
}
