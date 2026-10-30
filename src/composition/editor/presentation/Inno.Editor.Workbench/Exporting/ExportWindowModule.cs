using System;
using System.Collections.Generic;
using System.IO;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Core.Settings;
using Inno.Editor.Core;
using Inno.Editor.Exporting;

namespace Inno.Editor.Workbench;
[EditorModule("export.windows", order: 310)]
internal sealed class ExportWindowModule(
    EditorContext m_editor,
    EditorExportService m_exports,
    BuildSettingsStore m_buildSettings,
    ProjectSettingsStore m_projectSettings
) : EditorModule
{
    private bool m_gameProgressVisible;
    private string m_formError = string.Empty;
    internal bool isPluginVisible { get; private set; }
    internal bool isGameVisible { get; private set; }
    internal bool isGameProgressVisible => m_gameProgressVisible && m_exports.isGameBusy;
    internal bool isPluginBusy => m_exports.isPluginBusy;
    internal bool isGameBusy => m_exports.isGameBusy;
    internal string pluginId => m_projectSettings.projectId.value;
    internal string pluginDisplayName { get; set; } = string.Empty;
    internal string pluginOutputPath { get; set; } = string.Empty;
    internal string gameApplicationId => m_projectSettings.projectId.value;
    internal string gameProductName { get; set; } = string.Empty;
    internal string gamePersistentDataPath { get; set; } = string.Empty;
    internal string gameStartupScene { get; set; } = string.Empty;
    internal string gameOutputDirectory { get; set; } = string.Empty;
    internal int gameWindowWidth { get; set; } = 1280;
    internal int gameWindowHeight { get; set; } = 720;
    internal BuildTargetId gameTarget { get; set; }
    internal ManagedDeploymentId? gameManagedDeployment { get; set; }
    internal IReadOnlyList<ManagedDeploymentId> availableManagedDeployments => m_exports.GetManagedDeployments(gameTarget);
    internal IReadOnlyList<BuildTargetId> availableGameTargets => m_exports.availableGameTargets;
    internal EditorExportProgress progress => m_exports.progress;
    internal string status => progress.status;
    internal string error => m_formError.Length == 0 ? m_exports.progress.error : m_formError;
    internal bool includePluginDependencies { get; set; }

    internal string GetGameTargetDisplayName(BuildTargetId target) => m_exports.GetGameTargetDisplayName(target);
    internal void OpenPlugin()
    {
        if (isPluginBusy || isGameBusy)
            return;
        CloseGame();
        BuildSettings defaults = LoadBuildSettings();
        pluginDisplayName = defaults.pluginDisplayName;
        pluginOutputPath = defaults.pluginOutputPath;
        includePluginDependencies = defaults.includePluginDependencies;
        isPluginVisible = true;
    }

    internal void OpenGame()
    {
        if (isPluginBusy || isGameBusy)
            return;
        ClosePlugin();
        m_gameProgressVisible = false;
        BuildSettings defaults = LoadBuildSettings();
        gameProductName = defaults.gameProductName;
        gamePersistentDataPath = defaults.gamePersistentDataPath;
        gameStartupScene = defaults.gameStartupScene;
        gameOutputDirectory = defaults.gameOutputDirectory;
        gameWindowWidth = defaults.gameWindowWidth;
        gameWindowHeight = defaults.gameWindowHeight;
        gameTarget = defaults.gameTarget;
        gameManagedDeployment = defaults.gameManagedDeployment;
        isGameVisible = true;
    }

    internal void BeginPluginExport()
    {
        m_formError = string.Empty;
        m_exports.StartPlugin(new PluginBuildRequest { pluginId = pluginId, displayName = pluginDisplayName, outputPath = ResolveOutputPath(pluginOutputPath), includeDependencies = includePluginDependencies });
    }

    internal void BeginGameExport()
    {
        m_formError = string.Empty;
        m_exports.StartGame(new GameBuildRequest { profile = new BuildProfile { applicationId = gameApplicationId, productName = gameProductName, persistentDataPath = gamePersistentDataPath, startupScene = gameStartupScene, target = gameTarget, managedDeployment = gameManagedDeployment, windowWidth = gameWindowWidth, windowHeight = gameWindowHeight }, outputDirectory = ResolveOutputPath(gameOutputDirectory) });
        isGameVisible = false;
        m_gameProgressVisible = true;
    }

    internal void CancelPluginExport(long operationId) => m_exports.Cancel(operationId);
    internal void CancelGameExport(long operationId) => m_exports.Cancel(operationId);
    internal void ClosePlugin()
    {
        if (isPluginBusy)
            m_exports.Cancel(m_exports.progress.operationId);
        isPluginVisible = false;
    }

    internal void CloseGame()
    {
        if (isGameBusy)
            m_exports.Cancel(m_exports.progress.operationId);
        isGameVisible = false;
    }

    private BuildSettings LoadBuildSettings()
    {
        try
        {
            return m_buildSettings.Load();
        }
        catch (Exception exception) when (exception is IOException or InvalidDataException or InvalidOperationException or UnauthorizedAccessException)
        {
            m_formError = exception.Message;
            return m_buildSettings.defaultSettings;
        }
    }

    private string ResolveOutputPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return path;
        return Path.IsPathFullyQualified(path) ? Path.GetFullPath(path) : Path.GetFullPath(path, m_editor.projectDirectory);
    }
}
