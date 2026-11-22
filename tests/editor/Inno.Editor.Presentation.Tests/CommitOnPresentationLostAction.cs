using Inno.Editor.Interactions;

namespace Inno.Editor.Presentation.Tests;

[EditorAction("tests.commit-on-presentation-lost")]
public sealed class CommitOnPresentationLostAction : EditorAction<InteractionTarget>
{
    private InteractionTarget? m_target;

    protected override void Execute(EditorActionContext<InteractionTarget> context)
    {
        m_target = context.target;
        Activate(context);
    }

    protected override void OnPresentationLost()
    {
        if (m_target is not null)
        {
            m_target.committedValue = "Committed on focus loss";
        }
        m_target = null;
        Complete();
    }

    protected override void OnCancelled() => m_target = null;
}
