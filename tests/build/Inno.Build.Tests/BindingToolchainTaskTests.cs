using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Tasks;
using Inno.Build.Toolchains;
using Microsoft.Build.Framework;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BindingToolchainTaskTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBindingTask", Guid.NewGuid().ToString("N"));

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void MissingOrCanceledDefinitionCannotCreateGenerationCandidates(bool canceled)
    {
        Directory.CreateDirectory(m_root);
        var engine = new RecordingBuildEngine();
        var task = new GenerateBindingsTask
        {
            EngineRoot = m_root,
            ComponentProject = Path.Combine(m_root, "Fixture.csproj"),
            BindingDefinition = Path.Combine(m_root, "bindings.props"),
            TargetId = "unregistered-fixture",
            OutputMode = nameof(NativeBindingOutputMode.TargetArtifacts),
            DotnetHost = "dotnet",
            BuildEngine = engine,
            DescriptorOutputPath = Path.Combine(m_root, "request.json")
        };
        if (canceled)
            task.Cancel();

        Assert.False(task.Execute());
        Assert.NotEmpty(engine.errors);
        Assert.Empty(Directory.EnumerateFileSystemEntries(m_root));
        Assert.Empty(task.GeneratedBindings);
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private sealed class RecordingBuildEngine : IBuildEngine
    {
        public List<string> errors { get; } = [];
        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode => "fixture.proj";

        public void LogErrorEvent(BuildErrorEventArgs e) => errors.Add(e.Message ?? string.Empty);
        public void LogWarningEvent(BuildWarningEventArgs e) { }
        public void LogMessageEvent(BuildMessageEventArgs e) { }
        public void LogCustomEvent(CustomBuildEventArgs e) { }

        public bool BuildProjectFile(
            string projectFileName,
            string[] targetNames,
            IDictionary globalProperties,
            IDictionary targetOutputs
        ) => throw new NotSupportedException();
    }
}
