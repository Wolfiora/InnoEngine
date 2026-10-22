using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class ManagedInputEvaluatorPreparation
{
    internal sealed class Result(
        string directory,
        string fingerprint,
        BuildArtifactReadLease reading
    ) : IDisposable {
        internal string directory { get; } = directory;
        internal string fingerprint { get; } = fingerprint;
        /// <inheritdoc />
        public void Dispose() => reading.Dispose();
    }

    internal static async ValueTask<Result> PrepareAsync(
        NativeBuildContext context,
        DotNetSdkDescriptor sdk,
        CancellationToken cancellation
    ) {
        using Stream source = typeof(ManagedInputEvaluatorPreparation).Assembly.GetManifestResourceStream(
            "Inno.Build.Toolchains.Managed.ManagedInputGraphTask.cs.txt")
            ?? throw new InvalidOperationException("The managed graph evaluator source resource is absent.");
        using var reader = new StreamReader(source);
        string code = await reader.ReadToEndAsync(cancellation).ConfigureAwait(false);
        using Stream compilerInputs = typeof(ManagedInputEvaluatorPreparation).Assembly.GetManifestResourceStream(
            "Inno.Build.Toolchains.Managed.ManagedCompilerInputs.targets")
            ?? throw new InvalidOperationException("The compiler input target resource is absent.");
        using var compilerInputsReader = new StreamReader(compilerInputs);
        string compilerInputsCode = await compilerInputsReader.ReadToEndAsync(cancellation).ConfigureAwait(false);
        string sdkRoot = Path.GetDirectoryName(sdk.cliPath)!;
        string[] assemblies = ["Microsoft.Build.dll", "Microsoft.Build.Framework.dll", "Microsoft.Build.Utilities.Core.dll", "NuGet.Frameworks.dll"];
        NativeBuildInput[] inputs = assemblies.Select(name => NativeBuildInput.FromPath(context.engineRoot, Path.Combine(sdkRoot, name)))
            .Concat(new[] { sdk.cliPath, sdk.hostPath, Path.Combine(sdkRoot, "Roslyn", "bincore"),
                Path.Combine(sdkRoot, "Microsoft.NETCoreSdk.BundledVersions.props") }
                .Select(path => NativeBuildInput.FromPath(context.engineRoot, path))).ToArray();
        string[] declarations = ["managed-input-evaluator", "net10.0", "Release", sdk.sdkIdentity, code, compilerInputsCode];
        string fingerprint = NativeBuildFingerprint.Create(context, declarations, inputs, cancellation);
        string directory = Path.Combine(context.engineRoot, "artifacts", "build-tools", "managed-inputs", "evaluators", fingerprint);
        var cache = new BuildCachePublisher(Path.GetDirectoryName(directory)!, BuildCacheArtifactRole.ImmutableArtifact);
        BuildCacheOwnerCatalog.Register(context.engineRoot, Path.GetDirectoryName(directory)!, "managed-evaluators", cache.role, cancellation);
        using FileLease activity = await cache.AcquireUseAsync(cancellation).ConfigureAwait(false);
        BuildArtifactReadLease? existing = await BuildArtifactReadLease.TryAcquireAsync(directory, Path.GetDirectoryName(directory)!, fingerprint,
            ["Outputs"], context, cancellation).ConfigureAwait(false);
        if (existing is not null)
        {
            try
            {
                cache.Record(directory, directory + ".read.lock", [], sdk.sdkIdentity, null, cancellation);
                return new(directory, fingerprint, existing);
            }
            catch
            {
                existing.Dispose();
                throw;
            }
        }
        using (FileLease generation = await FileLease.AcquireAsync(directory + ".generation.lock",
            Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false))
        {
            Verify("managed-evaluator-lock");
            if (!BuildArtifactManifest.IsComplete(directory, fingerprint, ["Outputs"], context))
            {
                using ProcessOwnedDirectory candidateOwner = await ProcessOwnedDirectory.CreateAsync(Path.GetDirectoryName(directory)!, cancellation).ConfigureAwait(false);
                string candidate = candidateOwner.directory;
                try
                {
                    Directory.CreateDirectory(candidate);
                    string project = Path.Combine(candidate, "ManagedInputGraphTask.csproj");
                    var definition = new XElement("Project", new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                        new XElement("PropertyGroup", new XElement("TargetFramework", "net10.0"), new XElement("LangVersion", "14.0"), new XElement("OutputType", "Library"),
                            new XElement("ImplicitUsings", "disable"), new XElement("Nullable", "disable"),
                            new XElement("ImportDirectoryBuildProps", "false"), new XElement("ImportDirectoryBuildTargets", "false"),
                            new XElement("GenerateAssemblyInfo", "false"), new XElement("DebugType", "None"),
                            new XElement("DebugSymbols", "false")),
                        new XElement("ItemGroup", assemblies.Select(name => new XElement("Reference",
                            new XAttribute("Include", Path.GetFileNameWithoutExtension(name)),
                            new XElement("HintPath", Path.Combine(sdkRoot, name)), new XElement("Private", "false")))));
                    await File.WriteAllTextAsync(project, definition.ToString(), cancellation).ConfigureAwait(false);
                    await File.WriteAllTextAsync(Path.Combine(candidate, "ManagedInputGraphTask.cs"), code, cancellation).ConfigureAwait(false);
                    context.RecordManagedProcess();
                    await ToolchainEnvironment.RunAsync(sdk.hostPath,
                        [sdk.cliPath, "build", project, "--configuration", "Release", "--output", Path.Combine(candidate, "Outputs"),
                            "--disable-build-servers", "-m:1", "-nodeReuse:false", "--nologo", "-v:quiet"],
                        context.engineRoot, cancellation, DotNetSdkEnvironment.Create(sdk.hostPath)).ConfigureAwait(false);
                    Verify("managed-evaluator-build");
                    File.Delete(project);
                    File.Delete(Path.Combine(candidate, "ManagedInputGraphTask.cs"));
                    string intermediate = Path.Combine(candidate, "obj");
                    if (Directory.Exists(intermediate))
                        Directory.Delete(intermediate, recursive: true);
                    BuildArtifactManifest.Write(candidate, fingerprint, ["Outputs"], context);
                    using BuildArtifactWriteLease mutation = await BuildArtifactWriteLease.AcquireAsync(directory, Path.GetDirectoryName(directory)!, cancellation).ConfigureAwait(false);
                    try
                    {
                        AtomicDirectory.Install(candidate, directory);
                    }
                    catch (DirectoryCleanupException cleanupFailure)
                    {
                        Console.Error.WriteLine(cleanupFailure.Message);
                    }
                }
                finally
                {
                    candidateOwner.Dispose();
                }
            }
        }
        BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(directory, Path.GetDirectoryName(directory)!,
            fingerprint, ["Outputs"], cancellation, context).ConfigureAwait(false);
        try
        {
            cache.Record(directory, directory + ".read.lock", [], sdk.sdkIdentity, null, cancellation);
            return new(directory, fingerprint, reading);
        }
        catch
        {
            reading.Dispose();
            throw;
        }

        void Verify(string phase)
        {
            if (NativeBuildFingerprint.Create(context.BeginInputVerification(phase), declarations, inputs, cancellation) != fingerprint)
                throw new InvalidOperationException("The managed evaluator's selected SDK changed; no candidate was published.");
        }
    }
}
