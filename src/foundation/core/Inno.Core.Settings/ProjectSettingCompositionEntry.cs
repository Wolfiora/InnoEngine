namespace Inno.Core.Settings;

internal sealed record ProjectSettingCompositionEntry(
    ProjectSettingContributionContext context,
    byte[] data
);
