using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal sealed class BackendProductBuild
{
    private readonly string m_root;
    private readonly string m_sdk;
    private readonly string m_work;
    private readonly string m_output;
    private readonly string m_target;
    private readonly string m_host;
    private readonly string m_staging;
    private readonly string m_native;
    private readonly string m_consumption;
    private readonly string m_previous;
    private readonly JsonElement m_targetRegistration;
    private readonly JsonElement m_request;
    private readonly CancellationToken m_cancellation;

    internal BackendProductBuild(
        string root,
        string sdk,
        string request,
        CancellationToken cancellationToken
    ) {
        m_root = Path.GetFullPath(root);
        m_sdk = sdk;
        m_request = Read(request);
        m_target = Value(m_request, "target");
        m_host = Value(m_request, "hostTarget");
        JsonElement[] registrations = Read(Path.Combine(m_root, ".github/native-targets.json")).EnumerateArray().ToArray();
        m_targetRegistration = registrations.Single(item => Value(item, "target") == m_target);
        _ = registrations.Single(item => Value(item, "target") == m_host);
        m_work = PathBoundary.RequireUnlinkedPath(m_root, Path.Combine(m_root, "artifacts/bp"));
        m_output = PathBoundary.RequireUnlinkedPath(m_root, Path.Combine(m_root, "artifacts/backend-candidate"));
        m_staging = Path.Combine(m_work, "staging");
        m_native = Path.Combine(m_root, "artifacts/components", m_target, "selection.json");
        m_consumption = Path.Combine(m_root, "artifacts/components/selection.json");
        m_previous = Path.Combine(m_work, "previous");
        m_cancellation = cancellationToken;
    }

    internal async Task RunAsync()
    {
        if (Directory.Exists(m_work) || Directory.Exists(m_output))
        {
            throw new IOException("Backend production requires fresh staging.");
        }
        Directory.CreateDirectory(m_work);
        string asset = Value(m_targetRegistration, "asset");
        string? priorDigest = m_request.GetProperty("sha256").GetString();
        string[] configurations = m_targetRegistration.GetProperty("configurations").Deserialize<string[]>()!;
        JsonElement? prior = null;
        if (priorDigest != null)
        {
            await BackendPrebuiltArchive.ExtractAsync(
                Path.Combine(m_root, "artifacts/backend-seed", asset),
                priorDigest,
                m_target,
                m_previous,
                m_cancellation
            ).ConfigureAwait(false);
            prior = Read(Path.Combine(m_previous, "manifest.json"));
        }
        var production = new Dictionary<string, object>(StringComparer.Ordinal);
        List<Configuration> prepared = [];
        foreach (string configuration in configurations)
        {
            Configuration current = await CompileAsync(configuration, prior).ConfigureAwait(false);
            prepared.Add(current);
            production.Add(configuration, new { sourceOwners = current.changed, prebuiltOwners = current.matched });
        }
        string planList = Path.Combine(m_work, "input-plans.json");
        Write(planList, prepared.Select(item => Path.Combine(item.directory, "inputs.json")));
        string archive = Path.Combine(m_output, asset);
        BackendPackageInventory.Assemble(m_staging, planList, m_native, m_target, archive, m_cancellation);
        string digest = Hash(archive);
        string store = Path.Combine(m_work, "certified");
        string installed = Path.Combine(store, m_target);
        await BackendPrebuiltArchive.ExtractAsync(archive, digest, m_target, installed, m_cancellation).ConfigureAwait(false);
        string cold = PrepareConsumption(store, installed);
        List<object> checks = [];
        foreach (Configuration configuration in prepared)
        {
            await VerifyAsync(configuration, installed, cold, checks).ConfigureAwait(false);
        }
        string commit = prepared.Select(item => item.inputs.commit).Distinct(StringComparer.Ordinal).Single();
        Write(Path.Combine(m_output, "consumption.json"), new
        {
            version = 2,
            commit,
            target = m_target,
            sha256 = digest,
            bytes = new FileInfo(archive).Length,
            production,
            checks
        });
    }

    private async Task<Configuration> CompileAsync(
        string name,
        JsonElement? prior
    ) {
        string directory = Path.Combine(m_work, "plan-" + name);
        string path = Path.Combine(directory, "plan.json");
        await ValidationCommand.ExecuteAsync([
            "plan",
            "--engine-root",
            m_root,
            "--base",
            "HEAD",
            "--profile",
            "fast",
            "--dotnet",
            m_sdk,
            "--configuration",
            name,
            "--backend-store",
            Path.Combine(m_work, "identity-store"),
            "--output",
            directory
        ], m_cancellation).ConfigureAwait(false);
        ValidationPlan plan = ValidationPlanTransport.Read(path, m_root, m_sdk);
        BackendInputPlan inputs = await ValidationPlanner.BackendInputsAsync(
            plan,
            m_target,
            File.ReadAllBytes(m_consumption),
            null,
            m_cancellation
        ).ConfigureAwait(false);
        Write(Path.Combine(directory, "inputs.json"), inputs);
        if (inputs.projects.Count == 0)
        {
            throw new InvalidDataException("The platform declares no backend projects.");
        }
        string[] matched = prior.HasValue
            ? prior.Value.GetProperty("files")
                .EnumerateArray()
                .Where(file => (
                    Value(file, "configuration") == name && Value(file, "path").StartsWith("managed/", StringComparison.Ordinal)
                    && inputs.projects.GetValueOrDefault(Value(file, "owner")) == Value(file, "input")
                ))
                .Select(file => Value(file, "owner"))
                .Distinct(StringComparer.Ordinal)
                .Order(StringComparer.Ordinal)
                .ToArray()
            : [];
        string[] changed = inputs.projects.Keys.Except(matched, StringComparer.Ordinal).Order(StringComparer.Ordinal).ToArray();
        ValidationContext context = plan.contexts["managed"];
        List<Product> products = [];
        foreach (JsonElement item in context.sdkGraph.EnumerateArray())
        {
            JsonElement properties = item.GetProperty("Properties");
            if (!properties.TryGetProperty("InnoProductTarget", out JsonElement target) || target.GetString() != m_target
                || !properties.TryGetProperty("InnoProductId", out JsonElement id) || string.IsNullOrEmpty(id.GetString())
            ) {
                    continue;
                }
            string project = Value(item, "Path");
            string profile = await EvaluateAsync(project, "InnoProductBuildProperties", name).ConfigureAwait(false);
            if (!File.Exists(profile))
            {
                throw new FileNotFoundException("Missing declared product profile.", profile);
            }
            bool aggregate = Read(m_native)
                .GetProperty("aggregates")
                .EnumerateArray()
                .Any(value => (
                    Value(value, "product") == id.GetString()
                    && Value(value, "target") == m_target && Value(value, "configuration").Equals(name, StringComparison.OrdinalIgnoreCase)
                ));
            products.Add(new Product(id.GetString()!, project, profile, aggregate));
        }
        if (products.Count == 0)
        {
            throw new InvalidDataException("The SDK graph contains no selected products.");
        }
        var current = new Configuration(
            name,
            directory,
            context.properties["ShouldUnsetParentConfigurationAndPlatform"],
            inputs,
            changed,
            matched,
            products
        );
        string solution = Solution(directory, "Backends", inputs.projects.Keys);
        string sdk = Path.Combine(m_work, "sdk");
        List<string> options =
        [
            "--artifacts-path",
            sdk,
            "-p:InnoBackendExportArtifactsPath=" + sdk,
            "-p:InnoNativeDependencyManifest=" + m_consumption,
            "-p:InnoBackendExportRoot=" + m_staging
        ];
        if (prior.HasValue)
        {
            options.AddRange(
                ["-p:InnoBackendPrebuiltRoot=" + m_previous, "-p:InnoBackendCompileOwners=|" + string.Join('|', changed) + "|"]
            );
        }
        await BuildAsync(current, solution, options, solution: true).ConfigureAwait(false);
        if (inputs.developmentProperties is not null)
        {
            await BuildAsync(
                current,
                solution,
                [..options, ..ProductOptions(Path.GetFullPath(inputs.developmentProperties, m_root))],
                solution: true
            ).ConfigureAwait(false);
        }
        foreach (Product product in products)
        {
            if (!product.aggregate)
            {
                await BuildAsync(
                    current,
                    product.project,
                    [..options, ..ProductOptions(product.profile)]
                ).ConfigureAwait(false);
            }
            if (product.id == "player" && name == "Release")
            {
                await SupportPackAsync(current, true, m_consumption, prior.HasValue ? m_previous : null).ConfigureAwait(false);
            }
            else if (product.aggregate)
            {
                throw new InvalidDataException("No aggregate entry for " + product.id + "/" + name);
            }
        }
        return current;
    }

    private string PrepareConsumption(
        string store,
        string installed
    ) {
        JsonElement inventory = Read(Path.Combine(installed, "native/selection.json"));
        var merged = new Dictionary<string, object>(StringComparer.Ordinal) { ["layout"] = "platform-packages" };
        foreach (string family in new[] { "assets", "aggregates" })
        {
            List<JsonElement> items = inventory.GetProperty(family).EnumerateArray().ToList();
            if (m_target != m_host)
            {
                string hostStore = Directory.CreateDirectory(Path.Combine(store, m_host, "native")).FullName;
                foreach (JsonElement item in Read(m_consumption)
                    .GetProperty(family)
                    .EnumerateArray()
                    .Where(value => Value(value, "target") == m_host)
                ) {
                    string digest = Value(item, "sha256");
                    if (!Regex.IsMatch(digest, @"\A[a-f0-9]{64}\z"))
                    {
                        throw new InvalidDataException("Invalid host input digest.");
                    }
                    string source = Path.Combine(Path.GetDirectoryName(m_consumption)!, m_host, "native", digest + ".zip");
                    string destination = Path.Combine(hostStore, digest + ".zip");
                    if (!File.Exists(destination))
                    {
                        File.Copy(source, destination);
                    }
                    if (Hash(destination) != digest)
                    {
                        throw new InvalidDataException("Host input changed during assembly.");
                    }
                    items.Add(item);
                }
            }
            merged.Add(family, items);
        }
        string path = Path.Combine(store, "selection.json");
        Write(path, merged);
        return path;
    }

    private async Task VerifyAsync(
        Configuration current,
        string installed,
        string cold,
        List<object> checks
    ) {
        JsonElement files = Read(Path.Combine(installed, "manifest.json")).GetProperty("files");
        JsonElement requests = Read(Path.Combine(installed, "native/selection.json")).GetProperty("requests");
        JsonElement inventory = Read(cold);
        var checkedOwners = new HashSet<string>(StringComparer.Ordinal);
        foreach (Product product in current.products)
        {
            Stopwatch clock = Stopwatch.StartNew();
            string relative = Path.GetRelativePath(m_root, product.project).Replace('\\', '/');
            BackendConsumerInputs consumer = current.inputs.consumers![relative];
            var facts = new SortedDictionary<string, string>(StringComparer.Ordinal)
            {
                ["entry"] = consumer.identity,
                ["host"] = m_host,
                ["registration"] = JsonSerializer.Serialize(m_targetRegistration)
            };
            foreach (JsonElement file in files.EnumerateArray().Where(item => (
                Value(item, "configuration") == current.name
                && consumer.backendOwners.Contains(Value(item, "owner"), StringComparer.Ordinal)
            ))
            ) {
                facts.Add("file/" + Value(file, "path"), Value(file, "sha256"));
            }
            JsonElement[] products = requests.EnumerateArray().Where(item => (
                Value(item, "target") == m_target
                && Value(item, "product") == product.id
                && Value(item, "configuration").Equals(current.name, StringComparison.OrdinalIgnoreCase)
            )).ToArray();
            if (products.Length > 1)
            {
                throw new InvalidDataException("Duplicate native product request.");
            }
            foreach (string family in new[] { "assets", "aggregates" })
            {
                foreach (JsonElement item in inventory.GetProperty(family).EnumerateArray())
                {
                    string digest = Value(item, "sha256");
                    if (products.Length == 1
                        && Value(item, "target") == m_target
                        && !products[0].GetProperty("digests").Deserialize<string[]>()!.Contains(digest, StringComparer.Ordinal)
                    ) {
                            continue;
                        }
                    facts["native/" + family + "/" + digest] = digest;
                }
            }
            if (!product.aggregate)
            {
                string output = await EvaluateAsync(
                    product.project,
                    "TargetDir",
                    current.name,
                    ["-p:ArtifactsPath=" + Path.Combine(m_work, "sdk"), ..ProductOptions(product.profile)]
                ).ConfigureAwait(false);
                string[] outputs = Directory.GetFiles(output, "*", SearchOption.AllDirectories);
                if (outputs.Length == 0)
                {
                    throw new InvalidDataException("The built SDK product has no outputs.");
                }
                foreach (string file in outputs)
                {
                    facts.Add(
                        "output/" + Path.GetRelativePath(output, file).Replace('\\', '/'),
                        Hash(file).ToUpperInvariant()
                    );
                }
            }
            string identity = Convert.ToHexStringLower(SHA256.HashData(JsonSerializer.SerializeToUtf8Bytes(facts)));
            JsonElement[] trusted = m_request
                .GetProperty("consumers")
                .EnumerateArray()
                .Where(item => (
                    Value(item, "configuration") == current.name
                    && Value(item, "project") == relative && Value(item, "product") == product.id && Value(item, "identity") == identity
                ))
                .ToArray();
            if (trusted.Length > 1)
            {
                throw new InvalidDataException("Duplicate trusted SDK evidence.");
            }
            if (trusted.Length == 0)
            {
                string[] options = ["-p:InnoNativeDependencyManifest=" + cold, "-p:InnoBackendPrebuiltRoot=" + installed];
                if (current.inputs.developmentProperties != null)
                {
                    string[] owners = consumer.backendOwners.Where(checkedOwners.Add).ToArray();
                    if (owners.Length > 0)
                    {
                        string solution = Solution(current.directory, "Consume-" + product.id, owners);
                        await BuildAsync(current, solution, [
                            ..options,
                            "--artifacts-path",
                            Path.Combine(m_work, "consume-development"),
                            ..ProductOptions(Path.GetFullPath(current.inputs.developmentProperties, m_root))
                        ], solution: true).ConfigureAwait(false);
                    }
                }
                if (!product.aggregate)
                {
                    await BuildAsync(current, product.project, [
                        ..options,
                        "--artifacts-path",
                        Path.Combine(m_work, "consume"),
                        ..ProductOptions(product.profile)
                    ]).ConfigureAwait(false);
                }
                if (product.id == "player"
                    && current.name == "Release"
                ) {
                        await SupportPackAsync(current, false, cold, installed).ConfigureAwait(false);
                    }
            }
            checks.Add(new
            {
                configuration = current.name,
                product = product.id,
                project = relative,
                seconds = clock.Elapsed.TotalSeconds,
                identity,
                source = trusted.Length == 0 ? "executed" : Value(m_request, "tag")
            });
        }
    }

    private async Task SupportPackAsync(
        Configuration current,
        bool produce,
        string selection,
        string? package
    ) {
        string project = Path.Combine(m_root, "entry/InnoEngine.Build/InnoEngine.Build.csproj");
        string bootstrap = Path.Combine(m_work, "cli");
        if (produce)
        {
            await BuildAsync(
                current,
                project,
                ["--artifacts-path", bootstrap, "-p:InnoBackendPrebuiltRoot=" + m_staging]
            ).ConfigureAwait(false);
        }
        string cli = await EvaluateAsync(project, "TargetPath", "Release", ["-p:ArtifactsPath=" + bootstrap]).ConfigureAwait(false);
        var env = new Dictionary<string, string?>(StringComparer.Ordinal);
        if (package != null)
        {
            env["InnoBackendPrebuiltRoot"] = package;
        }
        if (produce)
        {
            env["InnoBackendExportProductTarget"] = m_target;
            env["InnoBackendExportRoot"] = m_staging;
            env["InnoBackendCompileOwners"] = "|" + string.Join('|', current.changed) + "|";
        }
        await ToolchainEnvironment.RunAsync(m_sdk, [
            cli,
            "support-pack",
            "--engine-root",
            m_root,
            "--configuration",
            "Release",
            "--target",
            m_target,
            "--tools-target",
            m_host,
            "--native-dependencies",
            selection,
            "--dotnet",
            m_sdk,
            "--output",
            Path.Combine(m_work, produce ? "pack-produce" : "pack-consume")
        ], m_root, env, m_cancellation).ConfigureAwait(false);
    }

    private Task BuildAsync(
        Configuration configuration,
        string path,
        IReadOnlyList<string> options,
        bool solution = false
    ) {
        string[] policy = solution
                          ? ["-m:1", "-p:Platform=Any CPU", "-p:ShouldUnsetParentConfigurationAndPlatform=" + configuration.policy]
                          : ["-m:2"];
        return ToolchainEnvironment.RunAsync(m_sdk, [
            "build",
            path,
            "--configuration",
            configuration.name,
            "--disable-build-servers",
            "-nr:false",
            ..policy,
            ..options,
            "-p:InnoTaskBootstrapOwner=" + Guid.NewGuid().ToString("N")
        ], m_root, cancellationToken: m_cancellation);
    }

    private async Task<string> EvaluateAsync(
        string project,
        string property,
        string configuration,
        string[]? options = null
    ) {
        string value = (await ToolchainEnvironment.CaptureOutputAsync(m_sdk, [
            "msbuild",
            project,
            "-nologo",
            "-p:Configuration="
            + configuration,
            ..options ?? [],
            "-getProperty:" + property
        ], m_root, cancellationToken: m_cancellation).ConfigureAwait(false)).Trim();
        if (value.Length == 0 || value.Contains('\n'))
        {
            throw new InvalidDataException("Expected one SDK property: " + property);
        }
        return value;
    }

    private string[] ProductOptions(string profile)
        => ["-p:InnoNativeTarget=" + m_target, "-p:InnoToolTarget=" + m_host, "-p:InnoProductBuildProperties=" + profile];

    private string Solution(
        string directory,
        string name,
        IEnumerable<string> projects
    ) {
        string path = Path.Combine(directory, name + ".slnx");
        new XElement(
            "Solution",
            projects
                .Order(StringComparer.Ordinal)
                .Select(project => new XElement("Project", new XAttribute("Path", Path.GetFullPath(project, m_root))))
        ).Save(path);
        return path;
    }

    private static string Hash(string path)
    {
        using FileStream input = File.OpenRead(path);
        return Convert.ToHexStringLower(SHA256.HashData(input));
    }

    private static JsonElement Read(string path) => JsonSerializer.Deserialize<JsonElement>(File.ReadAllBytes(path));

    private static string Value(
        JsonElement item,
        string key
    ) => item.GetProperty(key).GetString()!;

    private static void Write<T>(
        string path,
        T value
    ) => AtomicFile.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(value));

    private sealed record Configuration(
        string name,
        string directory,
        string policy,
        BackendInputPlan inputs,
        string[] changed,
        string[] matched,
        List<Product> products
    );

    private sealed record Product(
        string id,
        string project,
        string profile,
        bool aggregate
    );
}
