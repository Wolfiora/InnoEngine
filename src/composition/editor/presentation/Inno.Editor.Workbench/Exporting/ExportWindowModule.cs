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
    EditorContext editor,
    EditorExportService exports,
    BuildSettingsStore buildSettings,
    ProjectSettingsStore projectSettings
) : EditorModule
{
    private bool m_gameProgressVisible;
    private string m_formError = string.Empty;
    internal bool isPluginVisible { get; private set; }
    internal bool isGameVisible { get; private set; }
    internal bool isGameProgressVisible => m_gameProgressVisible && exports.isGameBusy;
    internal bool isPluginBusy => exports.isPluginBusy;
    internal bool isGameBusy => exports.isGameBusy;
    internal string pluginID => projectSettings.projectID.value;
    internal string pluginDisplayName { get; set; } = string.Empty;
    internal string pluginOutputPath { get; set; } = string.Empty;
    internal string gameApplicationID => projectSettings.projectID.value;
    internal string gameProductName { get; set; } = string.Empty;
    internal string gamePersistentDataPath { get; set; } = string.Empty;
    internal string gameStartupScene { get; set; } = string.Empty;
    internal string gameOutputDirectory { get; set; } = string.Empty;
    internal int gameWindowWidth { get; set; } = 1280;
    internal int gameWindowHeight { get; set; } = 720;
    internal BuildTargetID gameTarget { get; set; }
    internal ManagedDeploymentID? gameManagedDeployment { get; set; }
    internal IReadOnlyList<ManagedDeploymentID> availableManagedDeployments => exports.GetManagedDeployments(gameTarget);
    internal IReadOnlyList<BuildTargetID> availableGameTargets => exports.availableGameTargets;
    internal EditorExportProgress progress => exports.progress;
    internal string status => progress.status;
    internal string error => m_formError.Length == 0 ? exports.progress.error : m_formError;
    internal bool includePluginDependencies { get; set; }

    internal string GetGameTargetDisplayName(BuildTargetID target) => exports.GetGameTargetDisplayName(target);

    internal void OpenPlugin()
    {
        if (isPluginBusy || isGameBusy)
        {
            return;
        }
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
        {
            return;
        }
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
        exports.StartPlugin(
            new PluginBuildRequest
            {
                pluginID = pluginID,
                displayName = pluginDisplayName,
                outputPath = ResolveOutputPath(pluginOutputPath),
                includeDependencies = includePluginDependencies
            }
        );
    }

    internal void BeginGameExport()
    {
        m_formError = string.Empty;
        exports.StartGame(
            new GameBuildRequest
            {
                profile = new BuildProfile
                {
                    applicationId = gameApplicationID,
                    productName = gameProductName,
                    persistentDataPath = gamePersistentDataPath,
                    startupScene = gameStartupScene,
                    target = gameTarget,
                    managedDeployment = gameManagedDeployment,
                    windowWidth = gameWindowWidth,
                    windowHeight = gameWindowHeight
                },
                outputDirectory = ResolveOutputPath(gameOutputDirectory)
            }
        );
        isGameVisible = false;
        m_gameProgressVisible = true;
    }

    internal void CancelPluginExport(long operationID) => exports.Cancel(operationID);

    internal void CancelGameExport(long operationID) => exports.Cancel(operationID);

    internal void ClosePlugin()
    {
        if (isPluginBusy)
        {
            exports.Cancel(exports.progress.operationID);
        }
        isPluginVisible = false;
    }

    internal void CloseGame()
    {
        if (isGameBusy)
        {
            exports.Cancel(exports.progress.operationID);
        }
        isGameVisible = false;
    }

    private BuildSettings LoadBuildSettings()
    {
        try
        {
            return buildSettings.Load();
        }
        catch (Exception exception) when (exception is IOException
                                          or InvalidDataException
                                          or InvalidOperationException
                                          or UnauthorizedAccessException)
        {
            m_formError = exception.Message;
            return buildSettings.defaultSettings;
        }
    }

    private string ResolveOutputPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            return path;
        }
        return Path.IsPathFullyQualified(path) ? Path.GetFullPath(path) : Path.GetFullPath(path, editor.projectDirectory);
    }
}
