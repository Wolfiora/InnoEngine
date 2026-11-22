using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

[EditorAction("tests.interaction")]
public sealed class InteractionAction : EditorPresentationAction<InteractionTarget, InteractionPresentation>
{
    private string m_value = string.Empty;

    protected override void Execute(EditorActionContext<InteractionTarget> context)
    {
        m_value = "Initial";
        context.target.validationMessage = null;
        Activate(context);
    }

    protected override bool Present(EditorActionContext<InteractionTarget, InteractionPresentation> context)
    {
        InteractionPresentation presentation = context.argument;
        if (presentation.cancel)
        {
            Cancel();
            return true;
        }

        m_value = presentation.value;
        if (!presentation.submit)
        {
            return true;
        }
        if (string.IsNullOrWhiteSpace(m_value))
        {
            context.target.validationMessage = "A name is required.";
            return true;
        }

        context.target.committedValue = m_value;
        context.target.validationMessage = null;
        Complete();
        return true;
    }

    protected override void OnCancelled() => m_value = string.Empty;
}
