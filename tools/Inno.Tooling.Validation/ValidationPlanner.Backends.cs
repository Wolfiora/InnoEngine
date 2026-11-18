using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

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
    internal static async Task<BackendInputPlan> BackendInputsAsync(
        ValidationPlan plan,
        string target,
        CancellationToken cancellation
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(target);
        var owners = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        var properties = new Dictionary<string, HashSet<string>>(StringComparer.Ordinal);
        // All project roots and their evaluated product variants are in the primary SDK capture.
        // Additional execution contexts depend on the selected checks and cannot define package identity.
        foreach (JsonElement snapshot in plan.contexts["managed"].sdkGraph.EnumerateArray())
        {
            ValidationProject project = ValidationProject.Read(plan.root, snapshot, target);
            if (!owners.TryGetValue(project.path, out HashSet<string>? sources))
            {
                sources = owners[project.path] = new(StringComparer.Ordinal);
                properties[project.path] = new(StringComparer.Ordinal);
            }
            sources.UnionWith(project.inputs);
            foreach (JsonElement variant in snapshot.GetProperty("Variants").EnumerateArray().Append(snapshot))
            {
                JsonElement effective = variant.GetProperty("Properties");
                string? variantTarget = effective.GetProperty("InnoNativeTarget").GetString();
                if (!string.IsNullOrEmpty(variantTarget) && variantTarget != target)
                    continue;
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

        if (plan.nativeDependencies is not null && File.Exists(Path.Combine(plan.root, plan.nativeDependencies)))
        {
            using JsonDocument dependencies = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(plan.root, plan.nativeDependencies)));
            foreach (string family in new[] { "assets", "aggregates" })
            foreach (JsonElement asset in dependencies.RootElement.GetProperty(family).EnumerateArray())
            {
                if (asset.GetProperty("target").GetString() != target
                    || !string.Equals(asset.GetProperty("configuration").GetString(), plan.configuration, StringComparison.OrdinalIgnoreCase))
                    continue;
                IEnumerable<string> consumers = family == "assets"
                    ? new[] { asset.GetProperty("nativeProject").GetString()!, asset.GetProperty("toolchainProject").GetString()! }
                    : asset.GetProperty("bindings").EnumerateObject().Select(static binding => binding.Name);
                foreach (string owner in consumers.Where(owners.ContainsKey))
                {
                    owners[owner].UnionWith(asset.GetProperty("sources").EnumerateObject().Select(static source => source.Name));
                    properties[owner].Add("native:" + asset.GetProperty("component").GetString() + "/" + asset.GetProperty("fingerprint").GetString());
                }
            }
        }

        DotNetSdkDescriptor sdk = await DotNetSdkResolver.ResolveDirectoryAsync(plan.dotnet, plan.root, cancellation).ConfigureAwait(false);
        string sdkRoot = Path.GetDirectoryName(sdk.cliPath)!;
        var common = new Dictionary<string, string>(StringComparer.Ordinal) { ["target"] = target, ["sdk"] = sdk.sdkIdentity, ["configuration"] = plan.configuration };
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
        ValidationCheck[] checks = plan.checks.Where(static check => check.exclusion is null).ToArray();
        foreach (ValidationCheck check in checks.Where(check => owners.ContainsKey(check.owner)))
            required.UnionWith(Closure(check.owner).Where(IsBackend));
        foreach (string project in checks.SelectMany(static check => check.projects ?? []).Where(owners.ContainsKey))
            required.UnionWith(Closure(project).Where(IsBackend));
        const string C_BUILD = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
        if (plan.nativeRequests is { Length: > 0 } && owners.ContainsKey(C_BUILD))
            required.UnionWith(Closure(C_BUILD).Where(IsBackend));
        return new(plan.commit, target, plan.configuration, identities, required.Order(StringComparer.Ordinal).ToArray(),
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
