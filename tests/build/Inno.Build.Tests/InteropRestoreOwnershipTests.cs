using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Build.Tests;

public sealed class InteropRestoreOwnershipTests
{
    [Theory]
    [InlineData("backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj", 1)]
    [InlineData("backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Inno.Adapter.Presentation.ImGui.Sdl3.csproj", 1)]
    [InlineData("src/composition/editor/panels/Inno.Editor.Panel.FileBrowser/Inno.Editor.Panel.FileBrowser.csproj", 0)]
    public async Task OnlyDirectReferencesOwnTheIsolatedRestore(
        string project,
        int expectedOwners
    ) {
        DirectoryInfo? root = new(AppContext.BaseDirectory);
        while (root is not null && !File.Exists(Path.Combine(root.FullName, "InnoEngine.sln")))
            root = root.Parent;
        Assert.NotNull(root);
        string dotnet = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(dotnet)
        {
            WorkingDirectory = root.FullName,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach (string argument in new[]
        {
            "msbuild", project, "-nologo", "-p:DesignTimeBuild=true",
            "-target:IncludeTransitiveProjectReferences",
            "-getItem:ProjectReference,_InnoDirectInteropRuntimeReference"
        })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("MSBuild did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        string diagnostics = await error;
        string evaluated = await output;
        Assert.True(process.ExitCode == 0, evaluated + diagnostics);
        using JsonDocument document = JsonDocument.Parse(evaluated);
        JsonElement items = document.RootElement.GetProperty("Items");
        JsonElement owners = items.GetProperty("_InnoDirectInteropRuntimeReference");
        Assert.Equal(expectedOwners, owners.GetArrayLength());
        Assert.Contains(items.GetProperty("ProjectReference").EnumerateArray(), static reference =>
            reference.GetProperty("Filename").GetString() == "BGCS.Runtime");
        foreach (JsonElement owner in owners.EnumerateArray())
        {
            Assert.Equal("BGCS.Runtime", owner.GetProperty("Filename").GetString());
            Assert.Contains("ArtifactsPath=", owner.GetProperty("AdditionalProperties").GetString());
        }
    }
}
