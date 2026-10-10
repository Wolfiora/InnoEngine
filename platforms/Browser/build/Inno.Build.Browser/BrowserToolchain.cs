using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.Browser;

/// <summary>
/// Aggregates explicitly selected backend static recipes using a frozen Emscripten SDK.
/// </summary>
public static class BrowserToolchain
{
    /// <summary>
    /// Gets the sole owner of Browser aggregation, ABI constraints and SDK link support.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "browser",
        "platforms/Browser/build/Inno.Build.Browser/Inno.Build.Browser.csproj",
        "platforms/Browser/build/Inno.Build.Browser/Inno.Build.Browser.csproj");

    /// <summary>
    /// Publishes one exact static closure without discovering components or reselecting tools.
    /// </summary>
    /// <param name="context">
    /// The owned checkout and previously resolved Emscripten selection.
    /// </param>
    /// <param name="plan">
    /// The frozen product component graph with backend-owned static definitions.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels generation and drains active compilation before returning.
    /// </param>
    /// <returns>
    /// The immutable complete native closure and corresponding managed binding selection.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The tools, target or static component capability are incompatible.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A component produces an incomplete or foreign archive closure.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation was canceled before publication.
    /// </exception>
    public static async Task<BrowserNativeArtifacts> BuildAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(plan);
        cancellationToken.ThrowIfCancellationRequested();
        NativeToolchainSelection tools = context.RequireToolchain();
        if (tools.targetId != BuildTargetId.browserWasm.value || context.configuration != "release")
            throw new InvalidOperationException("Browser static publication requires an explicit Release wasm32 selection.");
        foreach (ProductNativeBuildStep step in plan.steps)
            if (step.options.libraryKind != NativeLibraryKind.Static || step.component.staticBuild is null
                || !step.component.staticBuild.targetIds.Contains(tools.targetId, StringComparer.Ordinal))
                throw new InvalidOperationException($"Component '{step.id}' has no selected static build capability.");

        IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> generations = await NativeBindingPreparation.PrepareAsync(
            context, plan.steps.Select(static step => step.component).ToArray(), cancellationToken).ConfigureAwait(false);
        context = context.WithBindings(generations).WithComponentOptions(componentDescriptor, new(NativeLibraryKind.Static));
        string owner = componentDescriptor.GetToolchainRoot(context.engineRoot);
        string[] inputs = plan.steps.SelectMany(step => step.component.staticBuild!.inputPaths
                .Append(step.component.staticBuild.cmakeFile)
                .Select(path => Path.GetFullPath(path, context.engineRoot))
                .Append(step.component.GetToolchainRoot(context.engineRoot))
                .Concat(step.options.inputPaths.Select(path => Path.GetFullPath(path, context.engineRoot))))
            .Concat(generations.Values.Select(static generation => generation.bindingsPath))
            .Concat(generations.Values.Where(static generation => generation.bridgeDirectory.Length != 0)
                .Select(static generation => generation.bridgeDirectory))
            .Append(Path.Combine(owner, "Native", "CMakeLists.txt"))
            .Distinct(StringComparer.Ordinal).ToArray();
        string[] declarations = plan.steps.Select(step => step.id + "=" + step.component.staticBuild!.cmakeFile)
            .Concat(plan.steps.SelectMany(static step => new[] { step.id + ":library=" + step.options.libraryKind }
                .Concat(step.options.cmakeArguments.Select(argument => step.id + ":argument=" + argument))))
            .Concat(generations.OrderBy(static pair => pair.Key, StringComparer.Ordinal)
                .Select(static pair => pair.Key + "=" + pair.Value.fingerprint))
            .Append("product=" + plan.productId).ToArray();
        NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(
            context, componentDescriptor, "browser", tools.targetId, inputs, declarations,
            [Path.Combine(owner, "BrowserToolchain.cs"), Path.Combine(owner, "BrowserNativeArtifacts.cs")]);
        NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context, recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string intermediate = scoped.GetNativeBuildRoot(componentDescriptor);
                Directory.CreateDirectory(intermediate);
                string selection = Path.Combine(intermediate, "Components.cmake");
                NativeCMakeSource sources = await NativeCMakeExecutor.PrepareSourceAsync(
                    scoped, componentDescriptor, token).ConfigureAwait(false);
                WriteComponentSelection(selection, scoped.engineRoot, sources, plan, generations);
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    Path.Combine(owner, "Native"), null,
                    ["-DINNO_COMPONENT_SELECTION=" + selection, "-DCMAKE_INSTALL_PREFIX=" + output,
                        "-DPython3_EXECUTABLE=" + tools.ResolveExecutable("python")], token).ConfigureAwait(false);
                await ToolchainEnvironment.RunAsync(scoped, tools.ResolveExecutable("cmake"),
                    ["--install", build, "--component", "Inno"], scoped.engineRoot, token,
                    tools.environment).ConfigureAwait(false);
                ValidateNativeOutputs(output, tools.targetId, plan);
                NativeBindingGenerationDescriptor.WriteSelection(Path.Combine(output, "Metadata", "BindingSelection.props"), generations,
                    plan.steps.Where(step => generations.ContainsKey(step.component.nativeProject))
                        .ToDictionary(static step => step.component.nativeProject, static step => step.options.libraryKind, StringComparer.Ordinal));
            }, cancellationToken).ConfigureAwait(false);
        return new BrowserNativeArtifacts(product.fingerprint, Path.Combine(product.directory, "Outputs"));
    }

    private static void WriteComponentSelection(
        string path,
        string engineRoot,
        NativeCMakeSource sources,
        ProductNativeBuildPlan plan,
        IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> generations
    ) {
        var text = new StringBuilder();
        int index = 0;
        foreach (ProductNativeBuildStep step in plan.steps)
        {
            string scope = "inno_component_" + index++;
            text.AppendLine("function(" + scope + ")");
            text.AppendLine("set(INNO_LIBRARY_KIND STATIC)");
            foreach (string argument in step.options.cmakeArguments)
            {
                string definition = sources.ResolveDefinition(argument);
                int separator = definition.IndexOf('=');
                string name = definition[2..separator].Split(':')[0];
                text.AppendLine("set(" + name + " " + CMakeLiteral(definition[(separator + 1)..]) + ")");
            }
            text.AppendLine("set(INNO_COMPONENT_ID " + CMakeLiteral(step.id) + ")");
            text.AppendLine("set(INNO_COMPONENT_NATIVE " + CMakeLiteral(sources.ResolvePath(step.component.GetNativeRoot(engineRoot))) + ")");
            string bridge = generations[step.component.nativeProject].bridgeDirectory;
            text.AppendLine("set(INNO_COMPONENT_BRIDGE " + CMakeLiteral(bridge.Length == 0 ? string.Empty : sources.ResolvePath(bridge)) + ")");
            text.AppendLine("include(" + CMakeLiteral(sources.ResolvePath(Path.GetFullPath(step.component.staticBuild!.cmakeFile, engineRoot))) + ")");
            text.AppendLine("endfunction()");
            text.AppendLine(scope + "()");
        }
        File.WriteAllText(path, text.ToString());
    }

    private static void ValidateNativeOutputs(
        string directory,
        string targetId,
        ProductNativeBuildPlan plan
    ) {
        var expected = new HashSet<string>(StringComparer.Ordinal);
        foreach (ProductNativeBuildStep step in plan.steps)
        {
            foreach (string archive in step.component.staticBuild!.archiveNames)
            {
                string path = Path.Combine(directory, step.id, targetId, archive);
                if (!File.Exists(path) || new FileInfo(path).Length == 0)
                    throw new InvalidDataException($"Component '{step.id}' did not produce its required archive '{archive}'.");
                expected.Add(path);
            }
        }
        if (!expected.SetEquals(Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)))
            throw new InvalidDataException("The static candidate contains outputs outside its frozen component closure.");
    }

    private static string CMakeLiteral(string value)
    {
        string portable = value.Replace('\\', '/');
        int delimiter = 0;
        while (portable.Contains("]" + new string('=', delimiter) + "]", StringComparison.Ordinal))
            delimiter++;
        string equals = new('=', delimiter);
        return "[" + equals + "[" + portable + "]" + equals + "]";
    }
}
