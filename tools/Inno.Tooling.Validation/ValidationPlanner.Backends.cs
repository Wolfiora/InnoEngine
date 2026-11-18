using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal sealed record BackendInputPlan(
    string commit,
    string target,
    string configuration,
    Dictionary<string, string> projects,
    string[] required,
    string[] tests
);

internal static partial class ValidationPlanner
{
    internal static async Task<ValidationPlan> BindBackendsAsync(
        ValidationPlan plan,
        string packages,
        CancellationToken cancellation
    ) {
        if (plan.backendStore is null || plan.backendPackages is not null || plan.nativeDependencies is null)
            throw new InvalidDataException("Only a pending backend plan can bind platform packages.");
        BackendPackageReceipt[] receipts = JsonSerializer.Deserialize<BackendPackageReceipt[]>(File.ReadAllBytes(packages))
            ?? throw new InvalidDataException("Missing backend acquisition receipts.");
        if (receipts.Any(static item => item is null || string.IsNullOrEmpty(item.target)
            || !item.target.All(static c => char.IsAsciiLetterOrDigit(c) || c == '-') || !Digest(item.sha256) || !Digest(item.manifestSha256))
            || receipts.Select(static receipt => receipt.target).Distinct(StringComparer.Ordinal).Count() != receipts.Length)
            throw new InvalidDataException("Incomplete or duplicate backend acquisition receipts.");
        string store = PathBoundary.RequireUnlinkedPath(plan.root, PathBoundary.Resolve(plan.root, plan.backendStore));
        var assets = new JsonArray();
        var aggregates = new JsonArray();
        var managed = new Dictionary<(string target, string configuration, string owner), string>();
        foreach (BackendPackageReceipt receipt in receipts)
        {
            string directory = PathBoundary.Resolve(store, receipt.target);
            string installed = PathBoundary.RequireUnlinkedPath(store, Path.Combine(directory, "receipt.json"));
            if (JsonSerializer.Deserialize<BackendPackageReceipt>(File.ReadAllBytes(installed)) != receipt)
                throw new InvalidDataException("Installed backend payload belongs to a different archive: " + receipt.target);
            byte[] manifest = File.ReadAllBytes(PathBoundary.RequireUnlinkedPath(store, Path.Combine(directory, "manifest.json")));
            if (Convert.ToHexStringLower(SHA256.HashData(manifest)) != receipt.manifestSha256)
                throw new InvalidDataException("Installed backend inventory changed: " + receipt.target);
            using JsonDocument document = JsonDocument.Parse(manifest);
            JsonElement files = document.RootElement.GetProperty("files");
            JsonElement selection = files.EnumerateArray().Single(item => item.GetProperty("path").GetString() == "native/selection.json");
            byte[] native = File.ReadAllBytes(PathBoundary.RequireUnlinkedPath(store, Path.Combine(directory, "native/selection.json")));
            if (Convert.ToHexStringLower(SHA256.HashData(native)) != selection.GetProperty("sha256").GetString())
                throw new InvalidDataException("The native selection changed after installation: " + receipt.target);
            JsonNode catalog = JsonNode.Parse(native) ?? throw new InvalidDataException("Missing native selection.");
            foreach (var family in new[] { ("assets", assets), ("aggregates", aggregates) })
            foreach (JsonNode? item in catalog[family.Item1]!.AsArray())
            {
                if (item?["target"]?.GetValue<string>() != receipt.target)
                    throw new InvalidDataException("A native selection names another platform.");
                family.Item2.Add(item.DeepClone());
            }
            if (document.RootElement.GetProperty("target").GetString() != receipt.target)
                throw new InvalidDataException("The backend inventory declares a different platform.");
            foreach (JsonElement file in files.EnumerateArray().Where(item => item.GetProperty("path").GetString()!.StartsWith("managed/", StringComparison.Ordinal)))
            {
                string owner = file.GetProperty("owner").GetString()!;
                string input = file.GetProperty("input").GetString()!;
                var variant = (receipt.target, file.GetProperty("configuration").GetString()!, owner);
                if (managed.TryGetValue(variant, out string? prior) && prior != input)
                    throw new InvalidDataException("A backend owner has conflicting compiler inputs: " + owner);
                managed[variant] = input;
            }
        }
        string selectionPath = PathBoundary.RequireUnlinkedPath(plan.root, PathBoundary.Resolve(plan.root, plan.nativeDependencies));
        byte[] selectionBytes = JsonSerializer.SerializeToUtf8Bytes(new { layout = "platform-packages", assets, aggregates });
        string host = plan.backendHost ?? "";
        var requests = plan.nativeRequests ?? [];
        var contexts = requests.Select(static request => (request.target, request.configuration))
            .Concat(plan.contexts.Values.Select(context => (host, context.properties.GetValueOrDefault("Configuration", plan.configuration))))
            .Distinct();
        foreach (var (target, configuration) in contexts)
        {
            BackendInputPlan expected = await BackendInputsAsync(plan, target, cancellation, selectionBytes, configuration).ConfigureAwait(false);
            foreach (string owner in expected.required)
                if (!managed.TryGetValue((target, configuration, owner), out string? actual) || actual != expected.projects[owner])
                    throw new InvalidDataException($"Missing or stale backend owner {owner} for {target}/{configuration}.");
        }
        foreach (var request in requests)
            if (!assets.Concat(aggregates).Any(asset => asset?["target"]?.GetValue<string>() == request.target
                && string.Equals(asset["configuration"]?.GetValue<string>(), request.configuration, StringComparison.OrdinalIgnoreCase) && asset["product"]?.GetValue<string>() == request.product))
                throw new InvalidDataException($"Missing native product {request.product} for {request.target}/{request.configuration}.");
        AtomicFile.WriteAllBytes(selectionPath, selectionBytes);
        return plan with { backendPackages = receipts.OrderBy(static receipt => receipt.target, StringComparer.Ordinal).ToArray() };

        static bool Digest(string? value) => value is { Length: 64 } && value.All(static c => char.IsAsciiHexDigitLower(c));
    }

    private sealed record BackendNativeProduct(string product, string target, string configuration);

    internal static async Task<BackendInputPlan> BackendInputsAsync(
        ValidationPlan plan,
        string target,
        CancellationToken cancellation,
        byte[]? selectionBytes = null,
        string? configuration = null
    ) {
        configuration ??= plan.configuration;
        ValidationContext graph = configuration == plan.configuration ? plan.contexts["managed"]
            : plan.contexts.Values.FirstOrDefault(context => context.properties.GetValueOrDefault("Configuration") == configuration)
                ?? throw new InvalidDataException("The SDK graph has no captured configuration " + configuration);
        var owners = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        var properties = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        var tools = new Dictionary<string, HashSet<BackendNativeProduct>>(StringComparer.Ordinal);
        // All project roots and their evaluated product variants are in the primary SDK capture.
        // Additional execution contexts depend on the selected checks and cannot define package identity.
        foreach (JsonElement snapshot in graph.sdkGraph.EnumerateArray())
        {
            ValidationProject project = ValidationProject.Read(plan.root, snapshot, target);
            if (!owners.TryGetValue(project.path, out HashSet<string>? sources))
            {
                sources = owners[project.path] = new(StringComparer.Ordinal);
                properties[project.path] = new(StringComparer.Ordinal);
                tools[project.path] = [];
            }
            sources.UnionWith(project.inputs);
            foreach (JsonElement variant in snapshot.GetProperty("Variants").EnumerateArray().Append(snapshot))
            {
                JsonElement effective = variant.GetProperty("Properties");
                string? variantTarget = effective.GetProperty("InnoNativeTarget").GetString();
                if (!string.IsNullOrEmpty(variantTarget) && variantTarget != target)
                    continue;
                foreach (JsonElement tool in variant.GetProperty("Items").GetProperty("InnoNativeDependency").EnumerateArray())
                {
                    string? hostTarget = tool.GetProperty("TargetId").GetString();
                    string? toolConfiguration = tool.GetProperty("Configuration").GetString();
                    tools[project.path].Add(new(tool.GetProperty("Identity").GetString()!,
                        string.IsNullOrEmpty(hostTarget) ? plan.backendHost ?? target : hostTarget,
                        (string.IsNullOrEmpty(toolConfiguration) ? configuration : toolConfiguration).ToLowerInvariant()));
                }
                foreach (string name in new[] { "AssemblyName", "TargetFramework", "RuntimeIdentifier", "DefineConstants", "Nullable", "AllowUnsafeBlocks", "LangVersion", "NETCoreSdkVersion" })
                    properties[project.path].Add(name + "=" + effective.GetProperty(name).GetString());
                foreach (JsonElement package in variant.GetProperty("Items").GetProperty("PackageReference").EnumerateArray())
                    properties[project.path].Add("package:" + package.GetProperty("Identity").GetString() + "=" + package.GetProperty("Version").GetString());
                foreach (JsonElement pack in variant.GetProperty("FrameworkPacks").EnumerateArray())
                {
                    var installed = new DirectoryInfo(pack.GetString()!);
                    properties[project.path].Add("framework:" + installed.Parent!.Name + "=" + installed.Name);
                }
            }
        }

        if (selectionBytes is null && plan.nativeDependencies is not null && File.Exists(Path.Combine(plan.root, plan.nativeDependencies)))
            selectionBytes = File.ReadAllBytes(Path.Combine(plan.root, plan.nativeDependencies));
        if (selectionBytes is not null)
        {
            using JsonDocument dependencies = JsonDocument.Parse(selectionBytes);
            foreach (string family in new[] { "assets", "aggregates" })
            foreach (JsonElement asset in dependencies.RootElement.GetProperty(family).EnumerateArray())
            {
                var product = new BackendNativeProduct(asset.GetProperty("product").GetString()!, asset.GetProperty("target").GetString()!,
                    asset.GetProperty("configuration").GetString()!.ToLowerInvariant());
                var consumers = tools.Where(pair => pair.Value.Contains(product)).Select(static pair => pair.Key).ToHashSet(StringComparer.Ordinal);
                if (product.target == target && string.Equals(product.configuration, configuration, StringComparison.OrdinalIgnoreCase))
                    consumers.UnionWith(family == "assets"
                        ? new[] { asset.GetProperty("nativeProject").GetString()!, asset.GetProperty("toolchainProject").GetString()! }
                        : asset.GetProperty("bindings").EnumerateObject().Select(static binding => binding.Name));
                foreach (string owner in consumers.Where(owners.ContainsKey))
                {
                    owners[owner].UnionWith(asset.GetProperty("sources").EnumerateObject().Select(static source => source.Name));
                    properties[owner].Add("native:" + asset.GetProperty("component").GetString() + "/" + asset.GetProperty("fingerprint").GetString());
                }
            }
        }

        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveDirectoryAsync(plan.dotnet, plan.root, cancellation).ConfigureAwait(false);
        string sdkRoot = Path.GetDirectoryName(sdk.cliPath)!;
        var common = new Dictionary<string, string>(StringComparer.Ordinal) { ["target"] = target, ["sdk"] = sdk.sdkIdentity, ["configuration"] = configuration };
        foreach (string compiler in new[] { "Microsoft.Build.dll", "Roslyn/bincore/csc.dll", "Roslyn/bincore/Microsoft.CodeAnalysis.dll", "Roslyn/bincore/Microsoft.CodeAnalysis.CSharp.dll" })
        {
            using FileStream file = File.OpenRead(Path.Combine(sdkRoot, compiler));
            common[compiler] = Convert.ToHexStringLower(await SHA256.HashDataAsync(file, cancellation).ConfigureAwait(false));
        }
        foreach (var pair in plan.inputs.Where(static pair => pair.Key is "global.json" or ".editorconfig" or "Directory.Packages.props"
            || pair.Key.Equals("NuGet.Config", StringComparison.OrdinalIgnoreCase)))
            common[pair.Key] = pair.Value;

        string[] submodules = plan.inputSets["native-sources"];
        var direct = new Dictionary<string, string>(StringComparer.Ordinal);
        var references = new Dictionary<string, string[]>(StringComparer.Ordinal);
        foreach (var pair in owners)
        {
            cancellation.ThrowIfCancellationRequested();
            var current = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (string source in pair.Value)
            {
                string? key = plan.inputs.ContainsKey(source) ? source : submodules.FirstOrDefault(prefix => source.StartsWith(prefix + "/", StringComparison.Ordinal));
                if (key is null)
                    continue;
                string value = plan.inputs[key];
                current[key] = value.StartsWith("unavailable:", StringComparison.Ordinal) ? value["unavailable:".Length..] : value;
            }
            foreach (string property in properties[pair.Key])
                current["@" + property] = "";
            direct[pair.Key] = ValidationInputs.Identity(current);
            references[pair.Key] = pair.Value.Where(owners.ContainsKey).ToArray();
        }
        var identities = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string project in owners.Keys.Where(IsBackend).Order(StringComparer.Ordinal))
        {
            var inputs = new Dictionary<string, string>(common, StringComparer.Ordinal);
            foreach (string dependency in Closure(project))
                inputs[dependency] = direct[dependency];
            identities.Add(project, ValidationInputs.Identity(inputs));
        }

        var required = new HashSet<string>(StringComparer.Ordinal);
        ValidationCheck[] checks = plan.checks.Where(check => check.exclusion is null && (check.context is null
            ? configuration == plan.configuration : plan.contexts[check.context].properties.GetValueOrDefault("Configuration", plan.configuration) == configuration)).ToArray();
        foreach (ValidationCheck check in checks.Where(check => owners.ContainsKey(check.owner)))
            required.UnionWith(Closure(check.owner).Where(IsBackend));
        foreach (string project in checks.SelectMany(static check => check.projects ?? []).Where(owners.ContainsKey))
            required.UnionWith(Closure(project).Where(IsBackend));
        const string C_BUILD = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
        if ((plan.backendHost is null || target == plan.backendHost) && plan.nativeRequests is { Length: > 0 } && configuration == plan.configuration && owners.ContainsKey(C_BUILD))
            required.UnionWith(Closure(C_BUILD).Where(IsBackend));
        return new(plan.commit, target, configuration, identities, required.Order(StringComparer.Ordinal).ToArray(),
            checks.Where(static check => check.kind == "test" && check.owner.StartsWith("tests/backends/", StringComparison.Ordinal))
                .Select(static check => check.owner).Order(StringComparer.Ordinal).ToArray());

        IEnumerable<string> Closure(string project)
        {
            var visited = new HashSet<string>(StringComparer.Ordinal);
            var pending = new Stack<string>();
            pending.Push(project);
            while (pending.TryPop(out string? current))
                if (visited.Add(current))
                    foreach (string dependency in references[current])
                        pending.Push(dependency);
            return visited;
        }

        static bool IsBackend(string path) => path.StartsWith("backends/", StringComparison.Ordinal);
    }
}
