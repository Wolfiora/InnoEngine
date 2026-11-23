using System.Collections.Generic;

namespace Inno.Editor.Panel.Settings;

internal static class BuildSettingsPresentation
{
    internal static IReadOnlyList<BuildSettingsField> fields { get; } =
    [
        new BuildSettingsField(
            BuildSettingsKey.GameProductName,
            "Build/Game/Product Name",
            "Game Export Defaults",
            "Player-facing product name copied into each game export."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GamePersistentDataPath,
            "Build/Game/Persistent Data Folder",
            "Game Export Defaults",
            "Folder below local application data. Empty uses the Project ID; use portable names separated by '/'."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GameStartupScene,
            "Build/Game/Startup Scene",
            "Game Export Defaults",
            "Mount-qualified Scene path loaded when the exported Player starts."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GameWindowWidth,
            "Build/Game/Initial Window Width",
            "Game Export Defaults",
            "Initial logical Player window width."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GameWindowHeight,
            "Build/Game/Initial Window Height",
            "Game Export Defaults",
            "Initial logical Player window height."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GameTarget,
            "Build/Game/Target",
            "Game Export Defaults",
            "Platform target selected when the game export window opens."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GameManagedDeployment,
            "Build/Game/Managed Deployment",
            "Game Export Defaults",
            "Selects the managed publisher independently of the platform. AOT compiles code before execution."
        ),
        new BuildSettingsField(
            BuildSettingsKey.GameOutputDirectory,
            "Build/Game/Output Directory",
            "Game Export Defaults",
            "Game output directory. Relative paths resolve from the project root."
        ),
        new BuildSettingsField(
            BuildSettingsKey.PluginDisplayName,
            "Build/Plugin/Display Name",
            "Plugin Export Defaults",
            "User-facing Plugin name copied into each Plugin export."
        ),
        new BuildSettingsField(
            BuildSettingsKey.PluginOutputPath,
            "Build/Plugin/Destination IPlugin",
            "Plugin Export Defaults",
            "Package destination ending in .iplugin. Relative paths resolve from the project root."
        ),
        new BuildSettingsField(
            BuildSettingsKey.IncludePluginDependencies,
            "Build/Plugin/Include Dependencies",
            "Plugin Export Defaults",
            "Whether dependency Plugin packages are embedded by default. Export changes remain temporary."
        )
    ];
}

internal enum BuildSettingsKey
{
    GameProductName,
    GamePersistentDataPath,
    GameStartupScene,
    GameWindowWidth,
    GameWindowHeight,
    GameTarget,
    GameManagedDeployment,
    GameOutputDirectory,
    PluginDisplayName,
    PluginOutputPath,
    IncludePluginDependencies
}
