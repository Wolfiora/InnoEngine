using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Evaluates repository project inputs in one selected SDK process without executing project targets.
/// </summary>
public static class ManagedProjectGraphCapture
{
    /// <summary>
    /// Captures effective properties, references, source items and imports using the shared SDK evaluator.
    /// </summary>
    /// <param name="engineRoot">
    /// The checkout owning the evaluator and its temporary request.
    /// </param>
    /// <param name="dotnetHost">
    /// The explicitly selected managed host executable.
    /// </param>
    /// <param name="projects">
    /// Existing absolute project paths evaluated together with identical global properties.
    /// </param>
    /// <param name="properties">
    /// The complete evaluation properties, including configuration and any explicit output owner.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation or evaluation and drains child processes before releasing owners.
    /// </param>
    /// <param name="resolveReferences">
    /// Resolves compile references and implicit symbols after restore without building project references or native products.
    /// </param>
    /// <returns>
    /// A JSON array of SDK snapshots. An empty request or failed evaluation throws instead of returning partial evidence.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// No projects are supplied or a project path is not absolute.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// A project or selected SDK input is absent.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller cancels owned work.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// SDK discovery or evaluation fails.
    /// </exception>
    public static async Task<string> CaptureAsync(
        string engineRoot,
        string dotnetHost,
        IReadOnlyList<string> projects,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellationToken = default,
        bool resolveReferences = false
    ) {
        ArgumentNullException.ThrowIfNull(projects);
        ArgumentNullException.ThrowIfNull(properties);
        if (projects.Count == 0 || projects.Any(static project => !Path.IsPathFullyQualified(project)))
            throw new ArgumentException("SDK capture requires nonempty absolute project paths.", nameof(projects));
        foreach (string project in projects)
            if (!File.Exists(project))
                throw new FileNotFoundException("The SDK project input is missing.", project);
        var context = new NativeBuildContext(engineRoot, "debug");
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveDirectoryAsync(dotnetHost, engineRoot, cancellationToken).ConfigureAwait(false);
        using ManagedInputEvaluatorPreparation.Result evaluator = await ManagedInputEvaluatorPreparation.PrepareAsync(context, sdk, cancellationToken).ConfigureAwait(false);
        string requests = Path.Combine(context.engineRoot, "artifacts", "build-tools", "managed-inputs", "requests");
        BuildCacheOwnerCatalog.Register(context.engineRoot, requests, "managed-input-requests", BuildCacheArtifactRole.ProcessWorkingDirectory, cancellationToken);
        using ProcessOwnedDirectory request = await ProcessOwnedDirectory.CreateAsync(requests, cancellationToken).ConfigureAwait(false);
        string output = Path.Combine(request.directory, "Graph.json");
        string entry = Path.Combine(request.directory, "Graph.proj");
        string compilerTargets = Path.Combine(request.directory, "RepositoryReferences.targets");
        var compiler = new XElement("Project",
            new XElement("Import", new XAttribute("Project", "$(InnoOriginalCustomAfterMicrosoftCommonTargets)"),
                new XAttribute("Condition", "'$(InnoOriginalCustomAfterMicrosoftCommonTargets)' != '' and Exists('$(InnoOriginalCustomAfterMicrosoftCommonTargets)')")),
            new XElement("Target", new XAttribute("Name", "CaptureInnoRepositoryReferences"),
                new XAttribute("DependsOnTargets", "ResolveReferences;AddImplicitDefineConstants"),
                new XAttribute("Returns", "@(_InnoRepositoryCapture)"),
                new XElement("ItemGroup",
                    new[] { "ReferencePath", "Compile" }.Select(name => new XElement("_InnoRepositoryCapture",
                        new XAttribute("Include", name == "Compile" ? "@(Compile->'%(FullPath)')" : "@(ReferencePath)"), new XElement("CaptureKind", name))),
                    new[] { "TargetPath", "DefineConstants", "LangVersion", "Nullable", "AllowUnsafeBlocks" }.Select(name =>
                        new XElement("_InnoRepositoryCapture", new XAttribute("Include", name),
                            new XElement("CaptureKind", "Property"), new XElement("Value", "$(" + name + ")"))))));
        var document = new XElement("Project",
            new XElement("UsingTask", new XAttribute("TaskName", "Inno.Build.Toolchains.ManagedInputGraphTask"),
                new XAttribute("AssemblyFile", Path.Combine(evaluator.directory, "Outputs", "ManagedInputGraphTask.dll"))),
            new XElement("ItemGroup",
                projects.Select(project => new XElement("RepositoryProject", new XAttribute("Include", project))),
                properties.Select(pair => new XElement("InputProperty", new XAttribute("Include", pair.Key), new XElement("Value", pair.Value)))),
            new XElement("Target", new XAttribute("Name", "Capture"),
                new XElement("Inno.Build.Toolchains.ManagedInputGraphTask", new XAttribute("ProjectPath", projects[0]),
                    new XAttribute("OutputPath", output), new XAttribute("CompilerInputsTargets", compilerTargets),
                    new XAttribute("ResolveRepositoryReferences", resolveReferences),
                    new XAttribute("RepositoryProjects", "@(RepositoryProject)"), new XAttribute("Properties", "@(InputProperty)"))));
        Directory.CreateDirectory(request.directory);
        await File.WriteAllTextAsync(compilerTargets, compiler.ToString(), cancellationToken).ConfigureAwait(false);
        await File.WriteAllTextAsync(entry, document.ToString(), cancellationToken).ConfigureAwait(false);
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "msbuild", entry, "-t:Capture", "-nologo", "-v:minimal", "-m:1", "-nodeReuse:false"],
            context.engineRoot, cancellationToken, DotNetSdkEnvironment.Create(sdk.hostPath)).ConfigureAwait(false);
        return await File.ReadAllTextAsync(output, cancellationToken).ConfigureAwait(false);
    }
}
