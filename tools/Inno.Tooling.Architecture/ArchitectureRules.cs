using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static partial class ArchitectureRules
{
    private static readonly string[] S_SCAN_ROOTS = ["src", "backends", "platforms", "build", "tests"];
    private static readonly string[] S_FORBIDDEN_PLAYER_PROJECT_FRAGMENTS =
    [
        "Inno.Editor.",
        "Inno.Build",
        "Inno.Scripting.Compiler",
        "Inno.Scripting.Reload",
        "Inno.Assets.Pipeline",
        "Inno.Rendering.Assets.Authoring",
        "Inno.Rendering.Shaders",
        "Inno.Adapter.Authoring",
        "Inno.Plugins.Authoring",
        "Inno.Adapter.Modules.DotNet",
        "Inno.Adapter.Serialization.DotNet",
        "Toolchains"
    ];
    private static readonly string[] S_FORBIDDEN_IMPLEMENTATION_WORDS =
    [
        "Legacy",
        "Compatibility",
        "Migration",
        "Former",
        "Deprecated"
    ];
    private static readonly string[] S_REMOVED_PROJECT_NAMES =
    [
        "Inno.Core.Framework",
        "Inno.Core.Assemblies",
        "Inno.Core.Reflection",
        "Inno.Core.Scripting",
        "Inno.Engine.Scene",
        "Inno.Audio.Scene",
        "Inno.Rendering.Core",
        "Inno.Native.Dll",
        "Inno.Adapter.Input.Sdl3",
        "Inno.Editor.Application", "Inno.Player", "Inno.Build.Toolchains.Host", "Inno.Build.Toolchains.Browser",
        "Inno.Build.SupportPacks", "Inno.Build.Platform.Windows", "Inno.Build.Platform.MacOS", "Inno.Build.Platform.Browser"
    ];
    private static readonly string[] S_CONCRETE_ADAPTER_MARKERS =
    [
        "Inno.Adapter.Platform.Sdl3",
        "Inno.Adapter.Storage.FileSystem",
        "Inno.Adapter.Rendering.Bgfx",
        "Inno.Adapter.Audio.MiniAudio",
        "Inno.Adapter.Presentation.ImGui",
        "Sdl3Platform",
        "FileSystemApplicationStorage",
        "BgfxDevice",
        "MiniAudioDevice",
        "PlatformImGuiContext"
    ];

    internal static void Validate(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        ValidateHostIsolation(repositoryRoot, failures);
        PlatformOwnershipValidator.Validate(repositoryRoot, failures);
        ValidateBindingTargetOutputs(repositoryRoot, failures);
        ValidateRepositorySources(repositoryRoot, failures);
        Dictionary<string, ProjectNode> graph = LoadProjectGraph(repositoryRoot, failures);
        ValidateCycles(repositoryRoot, graph, failures);
        ValidateConceptualLayerReferences(graph, failures);
        ValidateCompositionShellBoundaries(graph, failures);
        ValidatePlayerClosure(repositoryRoot, graph, failures);
        ValidateRemovedProjects(repositoryRoot, failures);
    }

    internal static bool IsForbiddenPlayerDependency(string assemblyName) => S_FORBIDDEN_PLAYER_PROJECT_FRAGMENTS
        .Any(fragment => assemblyName.Contains(fragment, StringComparison.Ordinal))
        || assemblyName.StartsWith("Inno.Integration.", StringComparison.Ordinal)
        && assemblyName.EndsWith(".Bgfx", StringComparison.Ordinal);

    private static void ValidateHostIsolation(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        var executables = new HashSet<string>(StringComparer.Ordinal)
        {
            "Inno.Editor.Windows", "Inno.Editor.MacOS", "Inno.Player.Windows", "Inno.Player.MacOS", "Inno.Player.Browser", "Inno.Build.Cli"
        };
        foreach (string rootName in new[] { "src", "backends", "platforms", "build", "tools" })
        {
            foreach (string project in EnumerateFiles(Path.Combine(repositoryRoot, rootName), "*.csproj"))
            {
                XDocument document = XDocument.Load(project);
                if (document.Descendants("OutputType").Any(static value => value.Value == "Exe")
                    && !executables.Contains(Path.GetFileNameWithoutExtension(project)))
                    failures.Add($"{Relative(repositoryRoot, project)}: engine utilities must be libraries composed by Inno.Build.Cli.");
                if (Path.GetFileNameWithoutExtension(project).StartsWith("Inno.Native.", StringComparison.Ordinal)
                    && Path.GetFileNameWithoutExtension(project).EndsWith(".Browser", StringComparison.Ordinal))
                    failures.Add($"{Relative(repositoryRoot, project)}: native targets must use binding profiles, not duplicate projects.");
            }
        }
        foreach (string rootName in new[] { "src/foundation", "src/composition/shell", "src/composition/player/Inno.Player.Runtime" })
        {
            string root = Path.Combine(repositoryRoot, rootName);
            if (!Directory.Exists(root))
                continue;
            foreach (string sourcePath in EnumerateFiles(root, "*.cs"))
            {
                if (File.ReadAllText(sourcePath).Contains("OperatingSystem.IsBrowser", StringComparison.Ordinal))
                    failures.Add($"{Relative(repositoryRoot, sourcePath)}: shared code must receive host capabilities through contracts.");
            }
        }
        string browserProject = Path.Combine(repositoryRoot, "platforms/Browser/player/Inno.Player.Browser/Inno.Player.Browser.csproj");
        if (File.Exists(browserProject) &&
            XDocument.Load(browserProject).Descendants("Compile").Any(static item => item.Attribute("Link") is not null))
            failures.Add("Inno.Player.Browser: shared Player source must be consumed through Inno.Player.Runtime.");
    }

    private static void ValidateBindingTargetOutputs(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        string backendRoot = Path.Combine(repositoryRoot, "backends");
        if (!Directory.Exists(backendRoot))
            return;
        foreach (string owner in EnumerateFiles(backendRoot, "*.csproj")
            .Where(static path => Path.GetFileName(path).StartsWith("Inno.Native.", StringComparison.Ordinal))
            .Select(static path => Path.GetDirectoryName(path)!))
        {
            string bindings = Path.Combine(owner, "Bindings");
            if (!Directory.Exists(bindings))
                continue;
            foreach (string hostConfig in Directory.EnumerateFiles(bindings, "*.json"))
            {
                using JsonDocument host = JsonDocument.Parse(File.ReadAllText(hostConfig));
                if (host.RootElement.ValueKind != JsonValueKind.Object
                    || !host.RootElement.TryGetProperty("outputPath", out JsonElement hostOutput))
                    continue;
                string hostPath = Path.GetFullPath(Path.Combine(bindings, hostOutput.GetString()!));
                string profilePattern = Path.GetFileNameWithoutExtension(hostConfig) + ".*.json";
                foreach (string profileConfig in Directory.EnumerateFiles(bindings, profilePattern))
                {
                    using JsonDocument profile = JsonDocument.Parse(File.ReadAllText(profileConfig));
                    if (!profile.RootElement.TryGetProperty("outputPath", out JsonElement profileOutput))
                    {
                        failures.Add($"{Relative(repositoryRoot, profileConfig)}: target profiles must declare an isolated output root.");
                        continue;
                    }
                    string profilePath = Path.GetFullPath(Path.Combine(bindings, profileOutput.GetString()!));
                    if (!profilePath.StartsWith(owner + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        failures.Add($"{Relative(repositoryRoot, profileConfig)}: target output must stay inside its native owner.");
                    if (string.Equals(hostPath, profilePath, StringComparison.OrdinalIgnoreCase)
                        || profilePath.StartsWith(hostPath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                        || hostPath.StartsWith(profilePath + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        failures.Add($"{Relative(repositoryRoot, profileConfig)}: host and target generated outputs must not overlap.");
                }
            }
        }
    }

    private static void ValidateRepositorySources(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        foreach (string rootName in S_SCAN_ROOTS)
        {
            string root = Path.Combine(repositoryRoot, rootName);
            if (!Directory.Exists(root))
                continue;
            foreach (string path in EnumerateFiles(root, "*.cs"))
            {
                string relative = Relative(repositoryRoot, path);
                string source = File.ReadAllText(path);
                if (IsGenerated(source))
                    continue;
                ValidateForbiddenImplementationNames(relative, source, failures);
                if ((relative.StartsWith("tests/", StringComparison.Ordinal) || relative.Contains("/tests/", StringComparison.Ordinal)))
                    ValidateTestSource(relative, source, failures);
                else
                    ValidateProductionSource(relative, source, failures);
            }
        }
    }

    private static void ValidateForbiddenImplementationNames(
        string relative,
        string source,
        ICollection<string> failures
    ) {
        string fileName = Path.GetFileNameWithoutExtension(relative);
        foreach (string word in S_FORBIDDEN_IMPLEMENTATION_WORDS)
        {
            if (fileName.Contains(word, StringComparison.OrdinalIgnoreCase))
                failures.Add($"{relative}: implementation file names cannot contain '{word}'.");
        }
        foreach (Match match in DeclaredTypePattern().Matches(source))
        {
            string name = match.Groups[2].Value;
            foreach (string word in S_FORBIDDEN_IMPLEMENTATION_WORDS)
            {
                if (name.Contains(word, StringComparison.OrdinalIgnoreCase))
                    failures.Add($"{relative}: declared implementation '{name}' cannot contain '{word}'.");
            }
        }
    }

    private static void ValidateProductionSource(
        string relative,
        string source,
        ICollection<string> failures
    ) {
        if (relative.StartsWith("tools/Inno.Tooling.Architecture/", StringComparison.Ordinal))
            return;
        if (relative.StartsWith("src/services/platform/", StringComparison.Ordinal) &&
            (source.Contains("PlatformNativeHandles", StringComparison.Ordinal) ||
             source.Contains("PlatformNativeHandleId", StringComparison.Ordinal) ||
             source.Contains("INativeWindowSurface", StringComparison.Ordinal)))
        {
            failures.Add($"{relative}: native surface ABI belongs to platform adapters, not the shared platform service.");
        }
        if (source.Contains(".With<IAssetReferenceResolver>", StringComparison.Ordinal) &&
            !relative.EndsWith(
                "src/content/assets/Inno.Assets/Serialization/AssetSerializationContext.cs",
                StringComparison.Ordinal))
        {
            failures.Add(
                $"{relative}: asset-aware serialization must use the owner-complete AssetSerializationContext factory.");
        }
        if (StaticManagerDeclarationPattern().IsMatch(source))
            failures.Add($"{relative}: process-wide static Manager ownership is forbidden.");
        if (StaticLogFacadeCallPattern().IsMatch(source))
            failures.Add($"{relative}: engine implementation must use an explicitly owned Logger.");
        if ((relative.StartsWith("src/composition/editor/", StringComparison.Ordinal) ||
             relative.StartsWith("src/composition/player/Inno.Player/", StringComparison.Ordinal)) &&
            source.Contains("_ = typeof(", StringComparison.Ordinal))
        {
            failures.Add(
                $"{relative}: composition roots must discover engine modules from their declared dependency closure, not typeof anchors.");
        }
        if ((relative.StartsWith("src/composition/player/Inno.Player.Runtime/", StringComparison.Ordinal) ||
             relative.StartsWith("src/composition/shell/", StringComparison.Ordinal)) &&
            S_CONCRETE_ADAPTER_MARKERS.Any(source.Contains))
        {
            failures.Add(
                $"{relative}: product composition must use Shell and backend-neutral adapter contracts instead of concrete backend types.");
        }
        if (relative.StartsWith("src/adapters/audio/Inno.Adapter.Audio.MiniAudio/", StringComparison.Ordinal) &&
            (source.Contains("AudioContentProvider", StringComparison.Ordinal) ||
             source.Contains("AudioMixerExtension", StringComparison.Ordinal) ||
             source.Contains("AudioMixerFeature", StringComparison.Ordinal) ||
             source.Contains("TypeCatalog", StringComparison.Ordinal) ||
             source.Contains("System.Reflection", StringComparison.Ordinal)))
        {
            failures.Add(
                $"{relative}: the real-time MiniAudio adapter cannot retain managed extension-generation objects or reflection state.");
        }
        if (relative.StartsWith("src/services/audio/", StringComparison.Ordinal) &&
            relative.EndsWith("/Properties/ScriptingApi.cs", StringComparison.Ordinal) &&
            (source.Contains("typeof(IAudioDevice)", StringComparison.Ordinal) ||
             source.Contains("typeof(AudioDevice)", StringComparison.Ordinal) ||
             source.Contains("Inno.Native", StringComparison.Ordinal) ||
             source.Contains("typeof(MiniAudio", StringComparison.Ordinal)))
        {
            failures.Add($"{relative}: scripting API cannot export audio devices, native bindings, or backend adapters.");
        }
        int lineNumber = 0;
        using var reader = new StringReader(source);
        string? line;
        while ((line = reader.ReadLine()) is not null)
        {
            lineNumber++;
            string trimmed = line.TrimStart();
            if (trimmed.StartsWith("///", StringComparison.Ordinal) ||
                trimmed.StartsWith("//", StringComparison.Ordinal))
            {
                continue;
            }
            if (PublicNativeSignaturePattern().IsMatch(line) &&
                !relative.StartsWith("native/", StringComparison.Ordinal))
            {
                failures.Add($"{relative}:{lineNumber}: public or protected API leaks a native implementation type.");
            }
        }
    }

    private static void ValidateTestSource(
        string relative,
        string source,
        ICollection<string> failures
    ) {
        if (source.Contains("InternalsVisibleTo", StringComparison.Ordinal))
            failures.Add($"{relative}: tests cannot introduce friend-assembly access.");
        if (NonPublicReflectionPattern().IsMatch(source))
            failures.Add($"{relative}: tests cannot penetrate non-public implementation state through reflection.");
    }

    private static Dictionary<string, ProjectNode> LoadProjectGraph(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        var graph = new Dictionary<string, ProjectNode>(StringComparer.OrdinalIgnoreCase);
        foreach (string path in EnumerateFiles(repositoryRoot, "*.csproj"))
        {
            string relative = Relative(repositoryRoot, path);
            if (relative.StartsWith("extern/", StringComparison.Ordinal) ||
                relative.StartsWith("demo/", StringComparison.Ordinal))
            {
                continue;
            }
            XDocument document = XDocument.Load(path);
            string name = document.Descendants("AssemblyName").Select(static value => value.Value.Trim())
                .FirstOrDefault(static value => value.Length > 0)
                ?? Path.GetFileNameWithoutExtension(path);
            var node = new ProjectNode(path, relative, name);
            graph.Add(Path.GetFullPath(path), node);
        }
        foreach (ProjectNode node in graph.Values)
        {
            XDocument document = XDocument.Load(node.path);
            ValidateProjectProperties(node, document, failures);
            foreach (XElement reference in document.Descendants("ProjectReference"))
            {
                if (string.Equals((string?)reference.Attribute("ReferenceOutputAssembly"), "false", StringComparison.OrdinalIgnoreCase))
                    continue;
                string? include = reference.Attribute("Include")?.Value;
                if (string.IsNullOrWhiteSpace(include))
                    continue;
                if (include.StartsWith("$(BindGenRoot)/", StringComparison.Ordinal))
                {
                    bool permitted = node.relative switch
                    {
                        "backends/ImGui/native/Inno.Native.ImGui/Bindings/Extension/Inno.Native.ImGui.BindingExtension.csproj"
                            => include is "$(BindGenRoot)/src/BGCS/BGCS.csproj"
                                or "$(BindGenRoot)/src/BGCS.Core/BGCS.Core.csproj",
                        "build/bindings/Inno.Build.Bindings/Inno.Build.Bindings.csproj"
                            => include is "$(BindGenRoot)/src/BGCS/BGCS.csproj"
                                or "$(BindGenRoot)/src/BGCS.Cpp2C/BGCS.Cpp2C.csproj",
                        _ => false
                    };
                    string bindingProject = Path.Combine(Path.GetDirectoryName(repositoryRoot)!, "BindGen-CS",
                        include["$(BindGenRoot)/".Length..]);
                    if (!permitted || !File.Exists(bindingProject))
                        failures.Add($"{node.relative}: external binding dependency '{include}' is unavailable or outside its generation boundary.");
                    continue;
                }
                if (include == "$(BGCSRuntimeProject)")
                {
                    if (!node.relative.StartsWith("backends/", StringComparison.Ordinal))
                        failures.Add($"{node.relative}: the interop runtime dependency belongs to a backend boundary.");
                    continue;
                }
                string normalizedInclude = include
                    .Replace('\\', Path.DirectorySeparatorChar)
                    .Replace('/', Path.DirectorySeparatorChar);
                string targetPath = Path.GetFullPath(normalizedInclude, Path.GetDirectoryName(node.path)!);
                if (!graph.TryGetValue(targetPath, out ProjectNode? target))
                {
                    failures.Add($"{node.relative}: project reference '{include}' does not resolve to a repository project.");
                    continue;
                }
                if (node.references.Contains(target))
                {
                    failures.Add($"{node.relative}: project reference '{include}' is duplicated.");
                    continue;
                }
                node.references.Add(target);
                ValidateReferenceBoundary(node, target, failures);
            }
        }
        return graph;
    }

    private static void ValidateProjectProperties(
        ProjectNode project,
        XDocument document,
        ICollection<string> failures
    ) {
        foreach (XElement noWarn in document.Descendants("NoWarn"))
        {
            string value = noWarn.Value;
            if (value.Contains("CS1572", StringComparison.OrdinalIgnoreCase) ||
                value.Contains("CS1573", StringComparison.OrdinalIgnoreCase) ||
                value.Contains("CS1591", StringComparison.OrdinalIgnoreCase))
            {
                failures.Add($"{project.relative}: public XML contract warnings cannot be suppressed.");
            }
        }
    }

    private static bool IsNativeProject(string path) => path.StartsWith("backends/", StringComparison.Ordinal)
        && path.Contains("/native/", StringComparison.Ordinal);

    private static void ValidateReferenceBoundary(
        ProjectNode project,
        ProjectNode target,
        ICollection<string> failures
    ) {
        string sourcePath = project.relative;
        string targetPath = target.relative;
        bool foundationTarget = targetPath.StartsWith("src/foundation/", StringComparison.Ordinal);
        if ((project.name is "Inno.Rendering" or "Inno.Content") && !foundationTarget)
            failures.Add($"{sourcePath}: neutral mechanisms may only reference Foundation contracts, not {targetPath}.");
        if (project.name == "Inno.Rendering.Assets" && target.name is
            "Inno.Rendering.Runtime" or "Inno.Rendering.Shaders" or "Inno.Rendering.Assets.Authoring" or "Inno.Assets.Pipeline")
            failures.Add($"{sourcePath}: runtime asset definitions cannot reference authoring or runtime owners in {targetPath}.");
        if (project.name == "Inno.Rendering.Runtime" && target.name is
            "Inno.Rendering.Assets.Authoring" or "Inno.Assets.Pipeline" or "Inno.Rendering.Shaders")
            failures.Add($"{sourcePath}: rendering runtime cannot include authoring implementation {targetPath}.");
        if (project.name == "Inno.Rendering.Shaders" && target.name == "Inno.Rendering.Runtime")
            failures.Add($"{sourcePath}: shader authoring cannot depend on the runtime owner {targetPath}.");
        if (project.name == "Inno.Adapter.Input" && (IsNativeProject(targetPath)
            || IsConcreteAdapter(target.name)))
            failures.Add($"{sourcePath}: shared event input cannot depend on native or platform adapters in {targetPath}.");
        if (project.name == "Inno.Build" && (targetPath.StartsWith("build/composition/", StringComparison.Ordinal)
            || targetPath.StartsWith("build/pipeline/Inno.Build.Platform.", StringComparison.Ordinal)
            || target.name == "Inno.Build.Managed.DotNet"))
            failures.Add($"{sourcePath}: the build pipeline must receive providers instead of referencing {targetPath}.");
        if (IsNativeProject(sourcePath) &&
            !IsNativeProject(targetPath))
        {
            failures.Add($"{sourcePath}: Native cannot reference upper-layer project {targetPath}.");
        }
        if (sourcePath.StartsWith("src/foundation/", StringComparison.Ordinal) &&
            !targetPath.StartsWith("src/foundation/", StringComparison.Ordinal))
        {
            failures.Add($"{sourcePath}: Core cannot reference upper-layer project {targetPath}.");
        }
        if (sourcePath.StartsWith("build/", StringComparison.Ordinal) &&
            !sourcePath.StartsWith("build/cli/", StringComparison.Ordinal) &&
            targetPath.StartsWith("src/composition/editor/", StringComparison.Ordinal))
        {
            failures.Add($"{sourcePath}: Build cannot reference Editor project {targetPath}.");
        }
        if ((string.Equals(project.name, "Inno.Rendering", StringComparison.Ordinal) ||
             string.Equals(project.name, "Inno.Rendering.Runtime", StringComparison.Ordinal)) &&
            (targetPath.StartsWith("build/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/adapters/", StringComparison.Ordinal) ||
             IsNativeProject(targetPath)))
        {
            failures.Add($"{sourcePath}: backend-neutral Rendering cannot reference implementation project {targetPath}.");
        }
        if (string.Equals(project.name, "Inno.Build", StringComparison.Ordinal) &&
            targetPath.StartsWith("build/support/", StringComparison.Ordinal))
        {
            failures.Add($"{sourcePath}: Build pipeline must depend on the provisioner contract, not Support Pack implementations.");
        }
        if (sourcePath.Contains("Inno.Rendering/", StringComparison.Ordinal) &&
            (targetPath.Contains("MaterialGraph", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/content/scene/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/composition/editor/", StringComparison.Ordinal)))
        {
            failures.Add($"{sourcePath}: backend-neutral Rendering cannot reference {targetPath}.");
        }
        if (string.Equals(project.name, "Inno.Audio", StringComparison.Ordinal) &&
            (targetPath.StartsWith("src/services/audio/Inno.Audio.Runtime/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/adapters/audio/Inno.Adapter.Audio.MiniAudio/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/content/scene/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/runtime/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/composition/editor/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/services/platform/", StringComparison.Ordinal) ||
             IsNativeProject(targetPath)))
        {
            failures.Add($"{sourcePath}: backend-neutral Audio cannot reference {targetPath}.");
        }
        if (string.Equals(project.name, "Inno.Animation", StringComparison.Ordinal) &&
            (targetPath.StartsWith("src/runtime/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/content/scene/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/services/rendering/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/services/audio/", StringComparison.Ordinal) ||
             targetPath.StartsWith("src/composition/editor/", StringComparison.Ordinal) ||
             IsNativeProject(targetPath)))
        {
            failures.Add($"{sourcePath}: backend-neutral Animation cannot reference {targetPath}.");
        }
        if (target.name.Contains("Inno.Native.Bgfx", StringComparison.Ordinal) &&
            !IsAllowedBgfxConsumer(project.name))
        {
            failures.Add($"{sourcePath}: BGFX native code is restricted to BGFX adapters and toolchains.");
        }
        if (target.name.Contains("Inno.Native.Sdl3", StringComparison.OrdinalIgnoreCase) &&
            !IsAllowedSdlConsumer(project.name))
        {
            failures.Add($"{sourcePath}: SDL3 native code is restricted to SDL3 adapters and toolchains.");
        }
        if (target.name.Contains("Inno.Native.MiniAudio", StringComparison.Ordinal) &&
            !IsAllowedMiniAudioConsumer(project.name))
        {
            failures.Add($"{sourcePath}: miniaudio native code is restricted to the MiniAudio adapter and toolchain.");
        }
        if (target.name.Contains("Inno.Native.UI", StringComparison.Ordinal) &&
            !IsAllowedUiNativeConsumer(project.name))
        {
            failures.Add($"{sourcePath}: RmlUi native code is restricted to the RmlUi adapter and UI toolchain.");
        }
        if (IsAdapterContract(project.name) && IsConcreteAdapter(target.name))
        {
            failures.Add($"{sourcePath}: adapter contract projects cannot reference concrete implementation {targetPath}.");
        }
    }

    private static void ValidateCycles(
        string repositoryRoot,
        IReadOnlyDictionary<string, ProjectNode> graph,
        ICollection<string> failures
    ) {
        var states = new Dictionary<ProjectNode, VisitState>();
        var stack = new List<ProjectNode>();
        foreach (ProjectNode node in graph.Values)
            Visit(node);

        void Visit(ProjectNode node)
        {
            if (states.TryGetValue(node, out VisitState state))
            {
                if (state == VisitState.Visiting)
                {
                    int start = stack.IndexOf(node);
                    string cycle = string.Join(" -> ", stack.Skip(start).Append(node).Select(static value => value.name));
                    failures.Add($"{Relative(repositoryRoot, node.path)}: project reference cycle detected: {cycle}.");
                }
                return;
            }
            states[node] = VisitState.Visiting;
            stack.Add(node);
            foreach (ProjectNode target in node.references)
                Visit(target);
            stack.RemoveAt(stack.Count - 1);
            states[node] = VisitState.Visited;
        }
    }

    private static void ValidatePlayerClosure(
        string repositoryRoot,
        IReadOnlyDictionary<string, ProjectNode> graph,
        ICollection<string> failures
    ) {
        ProjectNode? player = graph.Values.SingleOrDefault(static value => value.name == "Inno.Player.Runtime");
        if (player is null)
        {
            failures.Add("src/composition/player/Inno.Player: Player composition project is missing.");
            return;
        }
        var visited = new HashSet<ProjectNode>();
        var pending = new Stack<ProjectNode>();
        pending.Push(player);
        while (pending.Count > 0)
        {
            ProjectNode current = pending.Pop();
            if (!visited.Add(current))
                continue;
            if (!ReferenceEquals(current, player) &&
                S_FORBIDDEN_PLAYER_PROJECT_FRAGMENTS.Any(fragment =>
                    current.name.Contains(fragment, StringComparison.OrdinalIgnoreCase)))
            {
                failures.Add($"{Relative(repositoryRoot, player.path)}: Player dependency closure contains forbidden project {current.name}.");
            }
            foreach (ProjectNode target in current.references)
                pending.Push(target);
        }
    }

    private static void ValidateRemovedProjects(
        string repositoryRoot,
        ICollection<string> failures
    ) {
        string solution = File.ReadAllText(Path.Combine(repositoryRoot, "InnoEngine.sln"));
        foreach (string removed in S_REMOVED_PROJECT_NAMES)
        {
            if (solution.Contains("= \"" + removed + "\",", StringComparison.Ordinal))
                failures.Add($"InnoEngine.sln: removed project '{removed}' remains in the solution.");
            foreach (string projectPath in EnumerateFiles(repositoryRoot, "*.csproj"))
            {
                if (string.Equals(
                        Path.GetFileNameWithoutExtension(projectPath),
                        removed,
                        StringComparison.Ordinal))
                {
                    failures.Add(
                        $"{Relative(repositoryRoot, projectPath)}: removed project '{removed}' remains in the repository.");
                }
            }
        }
    }

    private static void ValidateConceptualLayerReferences(
        IReadOnlyDictionary<string, ProjectNode> graph,
        ICollection<string> failures
    ) {
        foreach (ProjectNode project in graph.Values)
        {
            ConceptualLayer? sourceLayer = ClassifyConceptualLayer(project);
            if (sourceLayer is null)
                continue;
            foreach (ProjectNode target in project.references)
            {
                ConceptualLayer? targetLayer = ClassifyConceptualLayer(target);
                if (targetLayer is null || targetLayer <= sourceLayer)
                    continue;
                failures.Add(
                    $"{project.relative}: {sourceLayer} project cannot reference upper {targetLayer} project {target.relative}.");
            }
        }
    }

    private static void ValidateCompositionShellBoundaries(
        IReadOnlyDictionary<string, ProjectNode> graph,
        ICollection<string> failures
    ) {
        ValidateHost("Inno.Player.Runtime");
        ValidateHost("Inno.Editor.Hosting");

        void ValidateHost(string projectName)
        {
            ProjectNode? host = graph.Values.SingleOrDefault(value =>
                string.Equals(value.name, projectName, StringComparison.Ordinal));
            if (host is null)
            {
                failures.Add($"{projectName}: product composition project is missing.");
                return;
            }
            if (!host.references.Any(static target =>
                    string.Equals(target.name, "Inno.Shell", StringComparison.Ordinal)))
            {
                failures.Add($"{host.relative}: product composition must inherit the common Inno.Shell lifecycle.");
            }
            foreach (ProjectNode target in host.references.Where(target =>
                         projectName == "Inno.Player.Runtime" && IsConcreteAdapter(target.name)))
            {
                failures.Add(
                    $"{host.relative}: product composition cannot reference concrete adapter project {target.relative}; use Inno.Adapter.Default and neutral contracts.");
            }
        }
    }

    private static ConceptualLayer? ClassifyConceptualLayer(ProjectNode project)
    {
        if (!project.relative.StartsWith("src/", StringComparison.Ordinal))
            return null;
        if (project.relative.StartsWith("src/foundation/", StringComparison.Ordinal))
        {
            return ConceptualLayer.Foundation;
        }
        if (project.relative.StartsWith("src/composition/", StringComparison.Ordinal))
        {
            return ConceptualLayer.Composition;
        }
        if (project.name.StartsWith("Inno.Adapter", StringComparison.Ordinal))
        {
            return ConceptualLayer.Adapter;
        }
        return ConceptualLayer.EngineCapability;
    }

    private static bool IsAdapterContract(string name)
        => string.Equals(name, "Inno.Adapter", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Platform", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Input", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Storage", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Rendering", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Audio", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Text", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.UI", StringComparison.Ordinal) ||
           string.Equals(name, "Inno.Adapter.Presentation", StringComparison.Ordinal);

    private static bool IsConcreteAdapter(string name)
        => name.StartsWith("Inno.Adapter.Content.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Platform.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Input.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Storage.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Rendering.", StringComparison.Ordinal) &&
           !string.Equals(name, "Inno.Adapter.Rendering.Authoring", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Audio.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Text.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.UI.", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Presentation.", StringComparison.Ordinal);

    private static bool IsAllowedBgfxConsumer(string name)
        => name.StartsWith("Inno.Adapter.Rendering.Bgfx", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Presentation.ImGui.Bgfx", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Build.Toolchains.Bgfx", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Native.Bgfx", StringComparison.Ordinal) ||
           name.EndsWith(".Tests", StringComparison.Ordinal);

    private static bool IsAllowedSdlConsumer(string name)
        => name.StartsWith("Inno.Adapter.Platform.Sdl3", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Adapter.Presentation.ImGui.Sdl3", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Build.Toolchains.Sdl3", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Native.Sdl3", StringComparison.Ordinal) ||
           name is "Inno.Integration.Windows.Sdl3" or "Inno.Integration.MacOS.Sdl3" or "Inno.Integration.Browser.Sdl3" ||
           name.EndsWith(".Tests", StringComparison.Ordinal);

    private static bool IsAllowedMiniAudioConsumer(string name)
        => name.StartsWith("Inno.Adapter.Audio.MiniAudio", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Build.Toolchains.MiniAudio", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Native.MiniAudio", StringComparison.Ordinal);

    private static bool IsAllowedUiNativeConsumer(string name)
        => name.StartsWith("Inno.Adapter.UI.RmlUi", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Build.Toolchains.UI", StringComparison.Ordinal) ||
           name.StartsWith("Inno.Native.UI", StringComparison.Ordinal) ||
           name.EndsWith(".Tests", StringComparison.Ordinal);

    private static IEnumerable<string> EnumerateFiles(
        string root,
        string pattern
    ) => RepositorySourceInventory.Files(root, pattern);

    private static bool IsGenerated(string source)
        => source.Contains("<auto-generated>", StringComparison.OrdinalIgnoreCase) ||
           source.Contains("[GeneratedCode", StringComparison.Ordinal);

    private static string Relative(
        string repositoryRoot,
        string path
    ) => Path.GetRelativePath(repositoryRoot, path).Replace('\\', '/');

    [GeneratedRegex(@"\b(class|struct|interface|enum|record)\s+([A-Za-z_]\w*)")]
    private static partial Regex DeclaredTypePattern();

    [GeneratedRegex(@"\bstatic\s+class\s+(DiagnosticManager|LogManager|IdentityManager|JobSystemManager|AssemblyManager|TypeCacheManager|SerializationManager|ProjectSettingsManager|AssetManager|PluginManager|Shell)\b")]
    private static partial Regex StaticManagerDeclarationPattern();

    [GeneratedRegex(@"\bLog\.(Debug|Info|Warn|Error|Fatal)\s*\(")]
    private static partial Regex StaticLogFacadeCallPattern();

    [GeneratedRegex(@"\b(public|protected)\b[^\r\n;{]*(Inno\.Native\.|bgfx_|SDL[A-Z]|ImGuiPtr|Ma[A-Z]\w*)")]
    private static partial Regex PublicNativeSignaturePattern();

    [GeneratedRegex(@"BindingFlags\s*\.[^\r\n;]*(NonPublic|Private)|(GetField|GetMethod|GetProperty|GetConstructor)\s*\([^\r\n;]*BindingFlags\s*\.[^\r\n;]*(NonPublic|Private)")]
    private static partial Regex NonPublicReflectionPattern();

    private sealed class ProjectNode(
        string path,
        string relative,
        string name
    ) {
        internal string path { get; } = path;
        internal string relative { get; } = relative;
        internal string name { get; } = name;
        internal List<ProjectNode> references { get; } = [];
    }

    private enum VisitState
    {
        Visiting,
        Visited
    }

    private enum ConceptualLayer
    {
        Foundation,
        EngineCapability,
        Adapter,
        Composition
    }
}
