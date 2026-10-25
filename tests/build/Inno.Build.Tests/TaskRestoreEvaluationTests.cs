using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

using Inno.Build.TaskHosting;
using Inno.Build.Toolchains;

using Xunit;

namespace Inno.Build.Tests;

public sealed class TaskRestoreEvaluationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RestoreDoesNotReusePackageContentItemsDuringCompilation(bool ownedRestore)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoRestoreEvaluation", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string oldPayload = Path.Combine(root, "old.payload");
            string newPayload = Path.Combine(root, "new.payload");
            File.WriteAllText(oldPayload, "old-package-content");
            File.WriteAllText(newPayload, "new-package-content");
            string imports = Path.Combine(root, "restored.props");
            new XDocument(new XElement("Project", new XElement("PropertyGroup",
                new XElement("PayloadPath", oldPayload)))).Save(imports);
            string updatedImports = new XElement("Project", new XElement("PropertyGroup",
                new XElement("PayloadPath", newPayload))).ToString(SaveOptions.DisableFormatting);
            string project = Path.Combine(root, "Runtime.csproj");
            new XDocument(new XElement("Project",
                new XElement("Import", new XAttribute("Project", imports),
                    new XAttribute("Condition", "'$(ExcludeRestorePackageImports)' != 'true'")),
                new XElement("Target", new XAttribute("Name", "Restore"),
                    new XElement("WriteLinesToFile", new XAttribute("File", imports),
                        new XAttribute("Lines", updatedImports), new XAttribute("Overwrite", "true"))),
                new XElement("Target", new XAttribute("Name", "PublishRuntime"),
                    new XAttribute("Returns", "@(HashedRuntime)"),
                    new XElement("MakeDir", new XAttribute("Directories", "$(ArtifactsPath)/bin")),
                    new XElement("Copy", new XAttribute("SourceFiles", "$(PayloadPath)"),
                        new XAttribute("DestinationFiles", "$(ArtifactsPath)/bin/Inno.Build.Tasks.dll")),
                    new XElement("ItemGroup", new XElement("RuntimeFile",
                        new XAttribute("Include", "$(ArtifactsPath)/bin/Inno.Build.Tasks.dll"),
                        new XElement("RelativePath", "Inno.Build.Tasks.dll"))),
                    new XElement("GetFileHash", new XAttribute("Files", "@(RuntimeFile)"),
                        new XElement("Output", new XAttribute("TaskParameter", "Items"),
                            new XAttribute("ItemName", "HashedRuntime")))))).Save(project);

            string artifacts = Path.Combine(root, "workspaces", "shared");
            XElement operation = ownedRestore
                ? new XElement("Inno.Build.TaskHosting.RestoreOwnedProjectTask",
                    new XAttribute("Projects", project), new XAttribute("LockPath", artifacts + ".lock"),
                    new XAttribute("Properties", "ArtifactsPath=" + artifacts))
                : new XElement("Inno.Build.TaskHosting.BuildTaskRuntimeTask",
                    new XAttribute("ProjectFile", project), new XAttribute("ArtifactsDirectory", artifacts),
                    new XAttribute("LoadDirectory", Path.Combine(root, "loads", "reader", "Debug")),
                    new XAttribute("Targets", "Restore;PublishRuntime"));
            XElement run = new("Target", new XAttribute("Name", "Run"), operation);
            if (ownedRestore)
                run.Add(new XElement("MSBuild", new XAttribute("Projects", project),
                    new XAttribute("Targets", "PublishRuntime"),
                    new XAttribute("Properties", "ArtifactsPath=" + artifacts)));
            string parent = Path.Combine(root, "Parent.proj");
            new XDocument(new XElement("Project",
                new XElement("UsingTask", new XAttribute("TaskName", operation.Name),
                    new XAttribute("AssemblyFile", typeof(BuildTaskRuntimeTask).Assembly.Location)), run)).Save(parent);

            var start = new ProcessStartInfo(ToolchainEnvironment.ResolveExecutable("dotnet"))
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = root
            };
            foreach (string argument in new[] { "msbuild", parent, "-t:Run", "-m:1", "-nr:false", "-v:minimal" })
                start.ArgumentList.Add(argument);
            using Process process = Process.Start(start)!;
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> errors = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await output + await errors);
            Assert.Equal("new-package-content", File.ReadAllText(Path.Combine(artifacts, "bin", "Inno.Build.Tasks.dll")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
