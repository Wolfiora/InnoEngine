using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Core.Settings;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scripting.Compiler;

namespace Inno.Editor.PlayMode.Tests;

internal sealed class SettingsPanelServices
{
    internal SettingsPanelServices(
        string root,
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        EngineHost engine
    ) {
        string host = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var compiler = new ScriptCompiler(new ScriptCompilerOptions
        {
            projectRootDirectory = root,
            compilationEnvironment = new SdkScriptCompilationEnvironment(host, root)
        }, assets, plugins);
        var target = new FixtureTarget();
        var pipeline = new BuildPipeline(assets, plugins, settings, engine.serialization, engine.modules.generations,
            compiler, Path.Combine(root, "SupportPacks"), [new GameBuildTargetBinding(target, target)],
            new ManagedDeploymentCatalog([new FixtureDeployment()]));
        var build = new BuildSettingsStore(Path.Combine(root, "Settings.Build.inno"), engine.serialization,
            BuildSettings.CreateDefault("Settings acceptance", string.Empty, target.id));
        services = [pipeline, build];
    }

    internal IReadOnlyList<object> services { get; }

    private sealed class FixtureTarget : IGameBuildTarget, IGameContentCompiler
    {
        public BuildTargetId id => new("tests-ui-settings");
        public BuildTargetId target => id;
        public ManagedDeploymentId defaultManagedDeployment => new("tests-ui-managed");
        public string runtimeIdentifier => "tests-ui-rid";
        public string displayName => "Settings fixture";
        public void Validate(string directory) => throw new NotSupportedException("Settings acceptance does not package players.");
        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException("Settings acceptance does not package players.");
        public ValueTask CompileAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException("Settings acceptance does not compile content.");
    }

    private sealed class FixtureDeployment : IManagedDeploymentCompiler
    {
        public ManagedDeploymentId id => new("tests-ui-managed");
        public ManagedDeploymentCapabilities capabilities => new(["tests-ui-rid"], true, false, false);
        public ValueTask<ManagedDeploymentResult> CompileAsync(
            ManagedDeploymentRequest request,
            CancellationToken cancellationToken = default
        ) => throw new NotSupportedException("Settings acceptance does not compile players.");
    }
}
