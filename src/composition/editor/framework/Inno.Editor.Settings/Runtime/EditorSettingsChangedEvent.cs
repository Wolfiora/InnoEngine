using Inno.Core.Events;

namespace Inno.Editor.Settings;
internal sealed class EditorSettingsChangedEvent(EditorSettings settings) : Event
{
    internal EditorSettings settings { get; } = settings;
}
