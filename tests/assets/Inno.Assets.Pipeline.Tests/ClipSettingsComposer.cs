using System.Collections.Generic;
using Inno.Core.Settings;

namespace Inno.Assets.Pipeline.Tests;

[ProjectSettingComposer("tests.settings.composed-clip")]
internal sealed class ClipSettingsComposer : ProjectSettingComposer<ComposedClipSettings, ClipContribution>
{
    protected override ClipContribution CaptureContribution(
        ComposedClipSettings baseline,
        ComposedClipSettings value
    ) => new() { clip = value.clip };

    protected override bool IsEmpty(ClipContribution contribution) => contribution.clip is null;

    protected override void Compose(
        ComposedClipSettings target,
        IReadOnlyList<ProjectSettingContribution<ClipContribution>> contributions
    ) {
        foreach (ProjectSettingContribution<ClipContribution> contribution in contributions)
        {
            target.clip = contribution.value.clip;
        }
    }
}
