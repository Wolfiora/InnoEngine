using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Architecture;

internal static class DocumentationProducer
{
    internal static async Task<int> ExportAsync(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        if (arguments is not [string engine, string sdk, string configuration, string output])
        {
            throw new ArgumentException("Expected engine root, SDK executable, configuration and producer output directory.");
        }
        engine = Path.GetFullPath(engine);
        output = Path.GetFullPath(output);
        sdk = ToolchainEnvironment.ResolveExecutable(sdk);
        string git = ToolchainEnvironment.ResolveExecutable("git");
        string revision = await QueryAsync(git, ["rev-parse", "HEAD"]).ConfigureAwait(false);
        string tree = await QueryAsync(git, ["rev-parse", "HEAD^{tree}"]).ConfigureAwait(false);
        await VerifyCheckoutAsync().ConfigureAwait(false);
        Directory.CreateDirectory(output);
        string receipt = Path.Combine(output, "producer.json");
        if (File.Exists(receipt))
        {
            throw new IOException("Producer output already occupied: " + receipt);
        }
        string planning = Path.Combine(output, "plan.json");
        await DocumentationReferences.PlanExportAsync(
            ["--engine-root", engine, "--dotnet", sdk, "--configuration", configuration, "--output", planning],
            cancellationToken
        ).ConfigureAwait(false);
        JsonObject plan = JsonNode.Parse(File.ReadAllBytes(planning))!.AsObject();
        if ((string?)plan["schema"] != "inno-api-export-plan/1" || plan["projects"]!.AsArray().Count == 0)
        {
            throw new InvalidDataException("API preparation has no source owners.");
        }
        string host = (string)plan["hostTarget"]!;
        string managed = Path.Combine(engine, "artifacts", "documentation-export", "managed", configuration, host);
        const string C_ENTRY = "entry/InnoEngine.Build/InnoEngine.Build.csproj";
        bool compileScripts = plan["reusedScripts"]!.AsObject().Count != 2;
        List<string> projects = plan["buildProjects"]!.Deserialize<List<string>>()!;
        if (compileScripts)
        {
            projects.Add(C_ENTRY);
        }
        if (projects.Count > 0)
        {
            await PrepareAsync(
                engine,
                sdk,
                configuration,
                managed,
                Path.Combine(output, "managed-union.slnx"),
                Path.Combine(output, "managed-union.log"),
                projects,
                cancellationToken
            ).ConfigureAwait(false);
        }
        string[] sourceOptions = ["--engine-root", engine, "--dotnet", sdk, "--configuration", configuration, "--artifacts-path", managed];
        var captures = new JsonArray();
        var reused = new JsonArray();
        foreach (JsonNode? batch in plan["captures"]!.AsArray())
        {
            string target = (string)batch!["target"]!;
            string file = "source-" + target + ".json";
            string path = Path.Combine(output, file);
            string[] owners = batch["projects"]!.Deserialize<string[]>()!;
            JsonObject facts;
            if (owners.Length > 0)
            {
                DocumentationReferences.ExportApi([
                    ..sourceOptions,
                    "--target",
                    target,
                    "--output",
                    path,
                    ..owners.SelectMany(owner => new[] { "--project", owner })
                ], cancellationToken);
                facts = JsonNode.Parse(File.ReadAllBytes(path))!.AsObject();
            }
            else
            {
                facts = new JsonObject
                {
                    ["model"] = "source-clr",
                    ["configuration"] = configuration,
                    ["target"] = target,
                    ["sourceApiStatus"] = "Unverified",
                    ["projects"] = new JsonArray()
                };
            }
            if (plan["reusedOwners"]![target] is JsonObject retained)
            {
                foreach (var (owner, value) in retained)
                {
                    facts["projects"]!.AsArray().Add(Retain(owner, target, value!));
                }
            }
            facts["projects"] = new JsonArray([..facts["projects"]!
                .AsArray()
                .OrderBy(item => (string)item!["project"]!, StringComparer.Ordinal)
                .Select(item => item!.DeepClone())]);
            Capture(file, "source-clr", target, facts);
        }
        string[] sourceReferences = captures
            .SelectMany(item => new[] { "--source-reference", Path.Combine(output, (string)item!["path"]!) })
            .ToArray();
        Dictionary<string, string>? references = null;
        if (compileScripts)
        {
            string cli = await QueryAsync(sdk, [
                "msbuild",
                C_ENTRY,
                "-nologo",
                "-p:Configuration=" + configuration,
                "-p:ArtifactsPath=" + managed,
                "-getProperty:TargetPath"
            ]).ConfigureAwait(false);
            if (!File.Exists(cli))
            {
                throw new FileNotFoundException("The prepared Build entry is unavailable.", cli);
            }
            string project = Path.Combine(engine, "artifacts", "documentation-export", "scripts", configuration, host);
            Directory.CreateDirectory(Path.Combine(project, "Assets"));
            await RunLoggedAsync(engine, sdk, [
                cli,
                "scripts",
                "--engine-root",
                engine,
                "--dotnet",
                sdk,
                "--project",
                project,
                "--output",
                Path.Combine(project, "Export"),
                "--ide"
            ], Path.Combine(output, "script-projection.log"), cancellationToken).ConfigureAwait(false);
            references = LogicalReferences(Path.Combine(project, "Inno.EditorScripts.csproj"));
        }
        foreach (string model in new[] { "runtime-script", "editor-script" })
        {
            string file = model + ".json";
            JsonNode facts;
            if (references != null)
            {
                string path = Path.Combine(output, file);
                string[] editor = model == "editor-script" ? ["--editor-reference", references["editor"]] : [];
                DocumentationReferences.ExportApi([
                    ..sourceOptions,
                    "--target",
                    host,
                    "--surface",
                    model,
                    "--runtime-reference",
                    references["runtime"],
                    "--framework-project",
                    C_ENTRY,
                    ..editor,
                    ..sourceReferences,
                    "--output",
                    path
                ], cancellationToken);
                facts = JsonNode.Parse(File.ReadAllBytes(path))!;
            }
            else
            {
                facts = Retain(model, host, plan["reusedScripts"]![model]!);
            }
            Capture(file, model, host, facts);
        }
        string inventory = Path.Combine(output, "projects.json");
        DocumentationReferences.ExportProjects(["--engine-root", engine, "--output", inventory], cancellationToken);
        await VerifyCheckoutAsync().ConfigureAwait(false);
        AtomicFile.WriteAllBytes(
            receipt,
            JsonSerializer.SerializeToUtf8Bytes(new
            {
                schema = "inno-api-producer/1",
                repository = "https://github.com/Wolfiora/InnoEngine",
                revision,
                sourceTree = tree,
                toolingRevision = revision,
                hostTarget = host,
                configuration,
                sdkRuntime = plan["sdkRuntime"],
                projectManifestIdentity = Digest(inventory),
                planIdentity = Digest(planning),
                captures,
                foreignOwners = plan["foreignOwners"],
                reused
            }),
            overwrite: false
        );
        return await DocumentationReferences.PlanExportAsync(
            ["--engine-root", engine, "--record-completed", output],
            cancellationToken
        ).ConfigureAwait(false);

        void Capture(
            string file,
            string model,
            string target,
            JsonNode facts
        ) {
            string path = Path.Combine(output, file);
            AtomicFile.WriteAllBytes(path, JsonSerializer.SerializeToUtf8Bytes(facts));
            captures.Add(
                new JsonObject
                {
                    ["path"] = file,
                    ["sha256"] = Digest(path),
                    ["model"] = model,
                    ["configuration"] = configuration,
                    ["target"] = target
                }
            );
        }

        JsonNode Retain(
            string owner,
            string target,
            JsonNode retained
        ) {
            reused.Add(new JsonObject
            {
                ["owner"] = owner,
                ["target"] = target,
                ["producerRevision"] = retained["producerRevision"]!.DeepClone(),
                ["inputIdentity"] = retained["inputIdentity"]!.DeepClone(),
                ["status"] = "Reused"
            });
            return retained["fact"]!.DeepClone();
        }

        async Task VerifyCheckoutAsync()
        {
            if (await QueryAsync(git, ["rev-parse", "HEAD"]).ConfigureAwait(false) != revision
                || await QueryAsync(
                    git,
                    ["status", "--porcelain", "--untracked-files=all", "--ignore-submodules=none"]
                ).ConfigureAwait(false) != string.Empty
            ) {
                throw new InvalidDataException("API export requires unchanged committed source and submodule inputs.");
            }
        }

        async Task<string> QueryAsync(
            string executable,
            string[] options
        ) {
            using var writer = new StringWriter();
            await ToolchainEnvironment.RunAsync(
                executable,
                options,
                engine,
                executable == sdk ? DotNetSdkEnvironment.Create(sdk) : null,
                writer,
                Console.Error,
                cancellationToken
            ).ConfigureAwait(false);
            return writer.ToString().Trim();
        }
    }

    internal static Dictionary<string, string> LogicalReferences(string project)
    {
        XElement root = XDocument.Load(project).Root ?? throw new InvalidDataException("Empty IDE project.");
        XNamespace ns = root.Name.Namespace;
        var result = new Dictionary<string, string>(StringComparer.Ordinal);
        foreach (string name in new[] { "Runtime", "Editor" })
        {
            XElement reference = root.Elements(ns + "ItemGroup")
                .Elements(ns + "Reference")
                .Single(item => (string?)item.Attribute("Include") == "Inno.ScriptApi." + name);
            string hint = reference.Elements(ns + "HintPath").Single().Value;
            if (string.IsNullOrWhiteSpace(hint))
            {
                throw new InvalidDataException("Missing hint for " + name);
            }
            string path = Path.GetFullPath(hint, Path.GetDirectoryName(Path.GetFullPath(project))!);
            if (!File.Exists(path))
            {
                throw new FileNotFoundException("Missing logical reference.", path);
            }
            result.Add(name.ToLowerInvariant(), path);
        }
        return result;
    }

    internal static async Task PrepareAsync(
        string engine,
        string sdk,
        string configuration,
        string managed,
        string solutionPath,
        string log,
        IEnumerable<string> projects,
        CancellationToken cancellationToken
    ) {
        if (configuration is not ("Debug"
                                  or "Release"
                                  or "SourceDebug")
        ) {
                                      throw new ArgumentException("Unsupported managed preparation configuration.");
                                  }
        var solution = new XElement(
            "Solution",
            new XElement("Configurations", new XElement("BuildType", new XAttribute("Name", configuration)))
        );
        foreach (string project in projects.Distinct(StringComparer.Ordinal).Order(StringComparer.Ordinal))
        {
            string path = PathBoundary.Resolve(engine, project);
            if (!path.EndsWith(".csproj", StringComparison.Ordinal) || !File.Exists(path))
            {
                throw new InvalidDataException("Managed preparation requires a source project: " + project);
            }
            solution.Add(new XElement("Project", new XAttribute("Path", path)));
        }
        if (!solution.Elements("Project").Any())
        {
            throw new InvalidDataException("No managed preparation projects.");
        }
        AtomicFile.WriteAllBytes(solutionPath, System.Text.Encoding.UTF8.GetBytes(solution.ToString()));
        await RunLoggedAsync(engine, sdk, [
            "build",
            solutionPath,
            "--configuration",
            configuration,
            "--artifacts-path",
            managed,
            "-p:InnoBuildPurpose=ApiReferences",
            "-p:ShouldUnsetParentConfigurationAndPlatform=false",
            "-p:InnoTaskBootstrapOwner=" + Guid.NewGuid().ToString("N"),
            "-m:" + Math.Min(Environment.ProcessorCount, 4),
            "-nr:false",
            "--disable-build-servers"
        ], log, cancellationToken).ConfigureAwait(false);
    }

    private static async Task RunLoggedAsync(
        string engine,
        string sdk,
        string[] arguments,
        string log,
        CancellationToken cancellationToken
    ) {
        long start = Stopwatch.GetTimestamp();
        using var writer = new StreamWriter(new FileStream(log, FileMode.CreateNew, FileAccess.Write, FileShare.Read)) { AutoFlush = true };
        TextWriter synchronized = TextWriter.Synchronized(writer);
        Console.WriteLine("Starting " + log);
        try
        {
            await ToolchainEnvironment.RunAsync(
                sdk,
                arguments,
                engine,
                DotNetSdkEnvironment.Create(sdk),
                synchronized,
                synchronized,
                cancellationToken
            ).ConfigureAwait(false);
        }
        catch (Exception failure) when (failure is not OperationCanceledException)
        {
            throw new InvalidOperationException("API preparation failed. See " + log, failure);
        }
        finally
        {
            Console.WriteLine($"{Path.GetFileName(log)}: {Stopwatch.GetElapsedTime(start).TotalSeconds:F2} s");
        }
    }

    private static string Digest(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Convert.ToHexStringLower(SHA256.HashData(stream));
    }
}
