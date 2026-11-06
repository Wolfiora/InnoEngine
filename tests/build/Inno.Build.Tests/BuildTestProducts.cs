using System;
using System.IO;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;

namespace Inno.Build.Tests;

internal static class BuildTestProducts
{
    internal static BuildHostDescriptor WindowsHost() => new WindowsBuildHostProvider().TryCapture()
        ?? throw new PlatformNotSupportedException("This native fixture requires a Windows build host.");
    internal static BuildDistribution Distribution() => StandardBuildDistribution.Create(new BuildCompositionContext(
        "explicit-sdk", Path.GetTempPath(), new BuildHostDescriptor("Windows", "x64"), new BuildTargetId("windows-x64"))).build;

    internal static ProductNativeBuildPlan Player() => Distribution().ResolveNativeProduct("windows-x64", "player").plan;
    internal static ProductNativeBuildPlan Editor() => Distribution().ResolveNativeProduct("windows-x64", "editor").plan;
    internal static ProductNativeBuildPlan StaticPlayer() => Distribution().ResolveNativeProduct("browser-wasm", "player").plan;

}
