using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Freezes a Support Pack's source preparation identity independently of reconstructible build outputs.
/// </summary>
public sealed class PlayerSupportPackPreparationIdentity
{
    private readonly NativeBuildContext m_context;
    private readonly ProductNativeBuildPlan m_nativePlan;
    private readonly DotNetSdkDescriptor m_sdk;
    private readonly string m_project;
    private readonly IReadOnlyDictionary<string, string> m_properties;
    private readonly IReadOnlyList<string> m_inputs;
    private ManagedBuildInputSnapshot? m_managed;

    private PlayerSupportPackPreparationIdentity(
        NativeBuildContext context,
        ProductNativeBuildPlan nativePlan,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        IReadOnlyList<string> inputs
    ) {
        m_context = context;
        m_nativePlan = nativePlan;
        m_sdk = sdk;
        m_project = project;
        m_properties = properties;
        m_inputs = inputs;
    }

    /// <summary>
    /// Gets the frozen source input identity used to find a complete prepared Pack.
    /// </summary>
    public string fingerprint { get; private set; } = string.Empty;

    /// <summary>
    /// Captures Native declarations, selected binding inputs, the managed graph and explicit packaging inputs.
    /// </summary>
    /// <param name="context">
    /// The borrowed operation with a frozen SDK and binding input provider.
    /// </param>
    /// <param name="nativePlan">
    /// The exact product component closure used by preparation.
    /// </param>
    /// <param name="sdk">
    /// The selected managed SDK also used for the actual Player build.
    /// </param>
    /// <param name="project">
    /// The existing Player entry project whose graph supplies managed sources.
    /// </param>
    /// <param name="properties">
    /// The compiler and product properties used by the managed preparation build.
    /// </param>
    /// <param name="preparationInputs">
    /// The complete explicit platform layout, template, validator and preparation implementation inputs.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels input evaluation and hashing without publishing a Pack.
    /// </param>
    /// <returns>
    /// A frozen identity; no Native compilation, binding generation or Player build is executed.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// A required operation or input declaration is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The selected source, SDK or generator input graph cannot be resolved.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Input capture was canceled.
    /// </exception>
    public static async ValueTask<PlayerSupportPackPreparationIdentity> CaptureAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan nativePlan,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        IReadOnlyList<string> preparationInputs,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentNullException.ThrowIfNull(sdk);
        ArgumentNullException.ThrowIfNull(properties);
        ArgumentNullException.ThrowIfNull(preparationInputs);
        ArgumentException.ThrowIfNullOrWhiteSpace(project);
        var frozen = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(
            properties.ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.OrdinalIgnoreCase));
        var result = new PlayerSupportPackPreparationIdentity(context, nativePlan, sdk, project, frozen,
            Array.AsReadOnly(preparationInputs.ToArray()));
        result.fingerprint = await result.ComputeAsync(context, cancellationToken).ConfigureAwait(false);
        return result;
    }

    /// <summary>
    /// Creates the exclusively owned fixed managed execution planned by this preparation identity.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels materialization without changing any installed Pack.
    /// </param>
    /// <returns>
    /// Caller-owned input copies for compilation and source extraction.
    /// </returns>
    public ValueTask<ManagedBuildExecution> CreateExecutionAsync(CancellationToken cancellationToken = default)
        => (m_managed ?? throw new InvalidOperationException("Managed preparation was not captured.")).CreateExecutionAsync(cancellationToken);

    /// <summary>
    /// Freshly verifies the complete current source input closure after waiting or preparing output.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels verification before cache acceptance or publication.
    /// </param>
    /// <returns>
    /// Completion when the current graph and bytes still match the frozen preparation identity.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Preparation inputs changed; no candidate may be selected.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Verification was canceled.
    /// </exception>
    public async ValueTask VerifyAsync(CancellationToken cancellationToken = default)
    {
        string current = await ComputeAsync(m_context.BeginInputVerification("support-pack-inputs"), cancellationToken).ConfigureAwait(false);
        if (current != fingerprint)
            throw new InvalidOperationException("Support Pack inputs changed after planning; the candidate cannot be selected.");
    }

    private async ValueTask<string> ComputeAsync(
        NativeBuildContext context,
        CancellationToken cancellation
    ) {
        cancellation.ThrowIfCancellationRequested();
        IReadOnlyList<NativeBuildRecipe> recipes = m_nativePlan.CreateRecipes(context);
        var declarations = new List<string> { "support-pack-preparation", m_nativePlan.productId, context.RequireToolchain().targetId };
        foreach (NativeBuildRecipe recipe in recipes)
            declarations.Add(recipe.component + "=" + NativeBuildFingerprint.Create(context, recipe.declarations, recipe.inputs, cancellation));
        NativeComponentDescriptor[] generated = m_nativePlan.steps.Select(static step => step.component)
            .Where(static component => component.bindingDefinition is not null).DistinctBy(static component => component.nativeProject).ToArray();
        if (generated.Length > 0)
            declarations.Add("bindings=" + await context.RequireBindingGenerator().ComputeInputFingerprintAsync(context,
                new NativeBindingGenerationRequest(generated, context.RequireToolchain().targetId, NativeBindingOutputMode.TargetArtifacts,
                    componentOptions: m_nativePlan.steps.Where(static step => step.component.bindingDefinition is not null)
                        .GroupBy(static step => step.component.nativeProject, StringComparer.Ordinal)
                        .ToDictionary(static group => group.Key, static group => group.First().options, StringComparer.Ordinal)),
                cancellation).ConfigureAwait(false));
        ManagedBuildInputSnapshot managed = await ManagedBuildInputSnapshot.CaptureAsync(context, m_sdk, m_project,
            m_properties, cancellation).ConfigureAwait(false);
        m_managed ??= managed;
        declarations.Add("managed=" + managed.fingerprint);
        string root = Path.Combine(context.engineRoot, "build", "support", "Inno.Build.SupportPacks.Core");
        IEnumerable<string> inputs = m_inputs.Concat(new[] { "FilePlayerSupportPackPreparation.cs", "PlayerSupportPackFiles.cs",
            "PlayerSupportPackPreparationIdentity.cs", "PlayerSupportPackPublisher.cs", "PlayerSupportPackPreparationIndex.cs",
            "PlayerSupportPackPlan.cs", "Inno.Build.SupportPacks.Core.csproj" }.Select(file => Path.Combine(root, file)))
            .Append(Path.Combine(context.engineRoot, "build", "toolchains", "Inno.Build.Toolchains", "Native", "ProductNativeDeployment.cs"));
        return NativeBuildFingerprint.Create(context, declarations,
            inputs.Select(path => NativeBuildInput.FromPath(context.engineRoot, path)), cancellation);
    }
}
