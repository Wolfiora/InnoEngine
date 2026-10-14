using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Xml.Linq;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class ManagedBuildInputReader
{
    internal static async ValueTask<ManagedBuildInputCapture> CaptureAsync(
        NativeBuildContext context,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellation
    ) {
        string requests = Path.Combine(context.engineRoot, "artifacts", "build-tools", "managed-inputs", "requests");
        using ProcessOwnedDirectory requestOwner = await ProcessOwnedDirectory.CreateAsync(requests, cancellation).ConfigureAwait(false);
        string request = requestOwner.directory;
        string manifest = Path.Combine(request, "Inputs.xml");
        string entry = Path.Combine(request, "Capture.proj");
        Directory.CreateDirectory(request);
        try
        {
            using ManagedInputEvaluatorPreparation.Result evaluator = await ManagedInputEvaluatorPreparation.PrepareAsync(
                context, sdk, cancellation).ConfigureAwait(false);
            using Stream template = typeof(ManagedBuildInputReader).Assembly.GetManifestResourceStream(
                "Inno.Build.Toolchains.Managed.ManagedBuildInputs.targets")
                ?? throw new InvalidOperationException("The managed graph evaluator resource is absent.");
            XElement document = XElement.Load(template);
            document.AddFirst(new XElement("PropertyGroup",
                new XElement("InnoInputProject", project), new XElement("InnoInputOutput", manifest),
                new XElement("InnoInputEvaluator", Path.Combine(evaluator.directory, "Outputs", "ManagedInputGraphTask.dll"))));
            document.Add(new XElement("ItemGroup", properties.OrderBy(static pair => pair.Key, StringComparer.OrdinalIgnoreCase)
                .Select(static pair => new XElement("InnoInputProperty", new XAttribute("Include", pair.Key),
                    new XElement("Value", pair.Value)))));
            await File.WriteAllTextAsync(entry, document.ToString(), cancellation).ConfigureAwait(false);
            context.RecordManagedProcess();
            await ToolchainEnvironment.RunAsync(sdk.hostPath,
                [sdk.cliPath, "msbuild", entry, "-t:CaptureManagedInputs", "-nologo", "-v:quiet", "-m:1", "-nodeReuse:false"],
                context.engineRoot, cancellation, DotNetSdkEnvironment.Create(sdk.hostPath, context.toolchain?.environment)).ConfigureAwait(false);
            XElement captured = XElement.Load(manifest);
            var inputs = captured.Elements("Input").Select(element => NativeBuildInput.FromPath(context.engineRoot,
                (string?)element.Attribute("Path") ?? throw new InvalidDataException("A managed input has no physical location."))).ToList();
            foreach (XElement assets in captured.Elements("Assets"))
                CollectPackageInputs(context, assets, inputs, cancellation);
            inputs.AddRange(new[] { sdk.hostPath, sdk.cliPath,
                Path.Combine(Path.GetDirectoryName(sdk.cliPath)!, "Roslyn", "bincore") }
                .Select(path => NativeBuildInput.FromPath(context.engineRoot, path)));
            string[] declarations = captured.Elements("Declaration").Select(static element => element.Value)
                .Prepend("sdk=" + sdk.sdkIdentity).Prepend("project=" + project).ToArray();
            // Evaluator implementation participates without depending on unrelated tool assembly MVIDs.
            declarations = declarations.Append("evaluator=" + evaluator.fingerprint).ToArray();
            return new ManagedBuildInputCapture(NativeBuildFingerprint.Create(context, declarations, inputs, cancellation), declarations);
        }
        finally
        {
            requestOwner.Dispose();
        }
    }

    private static void CollectPackageInputs(
        NativeBuildContext context,
        XElement selected,
        ICollection<NativeBuildInput> inputs,
        CancellationToken cancellation
    ) {
        string assets = (string)selected.Attribute("Path")!;
        if (!File.Exists(assets))
            throw new FileNotFoundException("Managed input planning requires the selected project's restored dependency graph.", assets);
        inputs.Add(NativeBuildInput.FromPath(context.engineRoot, assets));
        using JsonDocument document = JsonDocument.Parse(File.ReadAllText(assets));
        JsonElement root = document.RootElement;
        JsonElement targets = root.GetProperty("targets");
        string selectedTarget = (string)selected.Attribute("Target")!;
        if (!targets.TryGetProperty(selectedTarget, out JsonElement target))
            throw new InvalidDataException($"Restored graph '{assets}' does not contain target '{selectedTarget}'. "
                + "Available targets: " + string.Join(", ", targets.EnumerateObject().Select(static item => item.Name)) + ".");
        string[] folders = root.GetProperty("packageFolders").EnumerateObject().Select(static folder => folder.Name).ToArray();
        JsonElement libraries = root.GetProperty("libraries");
        foreach (JsonProperty dependency in target.EnumerateObject())
        {
            cancellation.ThrowIfCancellationRequested();
            if (dependency.Value.GetProperty("type").GetString() != "package")
                continue;
            JsonElement library = libraries.GetProperty(dependency.Name);
            string location = library.GetProperty("path").GetString()
                ?? throw new InvalidDataException("A restored package has no physical location.");
            string directory = folders.Select(folder => PathBoundary.Resolve(folder, location)).FirstOrDefault(Directory.Exists)
                ?? throw new DirectoryNotFoundException($"The restored package '{dependency.Name}' is unavailable.");
            var files = new HashSet<string>(StringComparer.Ordinal);
            foreach (string group in new[] { "compile", "runtime", "native", "resource", "runtimeTargets", "build",
                "buildMultiTargeting", "buildTransitive", "contentFiles" })
                if (dependency.Value.TryGetProperty(group, out JsonElement entries))
                    foreach (JsonProperty entry in entries.EnumerateObject())
                        files.Add(entry.Name);
            foreach (JsonElement entry in library.GetProperty("files").EnumerateArray())
            {
                string file = entry.GetString()!;
                if (file.StartsWith("analyzers/", StringComparison.Ordinal)
                    && !file.Contains("/vb/", StringComparison.Ordinal) && !file.Contains("/fs/", StringComparison.Ordinal))
                    files.Add(file);
            }
            foreach (string file in files)
            {
                if (Path.GetFileName(file) == "_._")
                    continue;
                string path = PathBoundary.Resolve(directory, file);
                if (!File.Exists(path))
                    throw new FileNotFoundException($"The restored package input '{dependency.Name}/{file}' is absent.", path);
                inputs.Add(NativeBuildInput.FromPath(context.engineRoot, path));
            }
        }
    }
}
