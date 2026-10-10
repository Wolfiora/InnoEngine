using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BGCS.Core.Extensibility;
using BGCS.Facade;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Bindings;

internal static class BindingExtensionPreparation
{
    internal static async ValueTask<string> PrepareAsync(
        NativeBuildContext context,
        string project,
        string dotnetHost,
        CancellationToken cancellation
    ) {
        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveAsync(dotnetHost, project, cancellation).ConfigureAwait(false);
        string source = Path.GetDirectoryName(project)!;
        List<string> paths =
        [
            source, sdk.hostPath, sdk.cliPath,
            Path.Combine(Path.GetDirectoryName(sdk.cliPath)!, "Roslyn", "bincore")
        ];
        string[] generators =
        [
            typeof(BGCS.CppAst.Parsing.CppParserOptions).Assembly.Location,
            typeof(BGCS.Intermediate.BindingModule).Assembly.Location,
            typeof(BGCS.Language.Lexing.Lexer).Assembly.Location,
            typeof(Newtonsoft.Json.JsonConvert).Assembly.Location,
            typeof(CsCodeGenerator).Assembly.Location, typeof(IBindingPlugin).Assembly.Location
        ];
        for (DirectoryInfo? directory = new(source); directory is not null; directory = directory.Parent)
        {
            foreach (string name in new[] { "Directory.Build.props", "Directory.Build.targets", "global.json" })
            {
                string input = Path.Combine(directory.FullName, name);
                if (File.Exists(input))
                    paths.Add(input);
            }
        }
        NativeBuildInput[] inputs = paths.Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(path => NativeBuildInput.FromPath(context.engineRoot, path))
            .Concat(generators.Select(static path => new NativeBuildInput("binding-generator/" + Path.GetFileName(path), path)))
            .ToArray();
        string[] declarations = [sdk.hostPath, sdk.cliPath, sdk.sdkIdentity, "Release", "binding-extension"];
        string fingerprint = NativeBuildFingerprint.Create(context, declarations, inputs, cancellation);
        string destination = Path.Combine(context.engineRoot, "artifacts", "build-tools", "bindings", "extensions",
            Path.GetFileNameWithoutExtension(project), fingerprint);
        using FileLease lease = await FileLease.AcquireAsync(destination + ".lock", Timeout.InfiniteTimeSpan, cancellation).ConfigureAwait(false);
        Verify("extension-lock");
        string output = Path.Combine(destination, "Outputs");
        if (!BuildArtifactManifest.IsComplete(destination, fingerprint, ["Outputs"], context))
        {
            string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
            try
            {
                string candidate = Path.Combine(staging, "Outputs");
                Directory.CreateDirectory(candidate);
                string intermediates = Path.Combine(context.engineRoot, "artifacts", "build-tools", "bindings", "intermediate",
                    Path.GetFileNameWithoutExtension(project), fingerprint);
                context.RecordManagedProcess();
                await ToolchainEnvironment.RunAsync(sdk.hostPath,
                    [sdk.cliPath, "build", project, "--configuration", "Release", "--output", candidate,
                        "--artifacts-path", intermediates, "--disable-build-servers", "-m:1", "-nodeReuse:false"],
                    context.engineRoot, cancellation, DotNetSdkEnvironment.Create(sdk.hostPath)).ConfigureAwait(false);
                Verify("extension-build");
                BuildArtifactManifest.Write(staging, fingerprint, ["Outputs"], context);
                cancellation.ThrowIfCancellationRequested();
                AtomicDirectory.Install(staging, destination);
            }
            finally
            {
                if (Directory.Exists(staging))
                    Directory.Delete(staging, recursive: true);
            }
        }
        string assembly = Path.Combine(output, Path.GetFileNameWithoutExtension(project) + ".dll");
        if (!File.Exists(assembly))
            throw new InvalidDataException("The binding extension did not produce its declared assembly.");
        return assembly;

        void Verify(string phase)
        {
            if (NativeBuildFingerprint.Create(context.BeginInputVerification(phase), declarations, inputs, cancellation) != fingerprint)
                throw new InvalidOperationException("Generator extension inputs changed; no extension candidate was published.");
        }
    }
}
