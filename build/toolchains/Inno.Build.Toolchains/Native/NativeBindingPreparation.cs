using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Requests one coherent target binding closure through the shared MSBuild generator integration.
/// </summary>
public static class NativeBindingPreparation
{
    /// <summary>
    /// Generates or verifies target-isolated bindings for the explicitly selected component owners.
    /// </summary>
    /// <param name="context">
    /// The checkout and frozen tool selection owning this operation.
    /// </param>
    /// <param name="components">
    /// The exact Native owners; repeated recipe owners share one generation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels generation before publishing request results.
    /// </param>
    /// <returns>
    /// Immutable generation descriptors indexed by portable Native project location.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The tool selection lacks the managed generator host.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The completed request has an incomplete generation descriptor.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The generation request was canceled and drained.
    /// </exception>
    public static async Task<IReadOnlyDictionary<string, NativeBindingGenerationDescriptor>> PrepareAsync(
        NativeBuildContext context,
        IReadOnlyList<NativeComponentDescriptor> components,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(components);
        cancellationToken.ThrowIfCancellationRequested();
        NativeToolchainSelection tools = context.RequireToolchain();
        NativeComponentDescriptor[] owners = components.Where(static owner => owner.bindingConfig is not null)
            .DistinctBy(static owner => owner.nativeProject).ToArray();
        if (owners.Length == 0)
            return new Dictionary<string, NativeBindingGenerationDescriptor>();
        string[] configurations = owners.Select(owner => Path.ChangeExtension(
            Path.GetFullPath(owner.bindingConfig!, context.engineRoot), tools.targetId + ".json")).ToArray();
        foreach (string configuration in configurations)
            if (!File.Exists(configuration))
                throw new FileNotFoundException("The selected component has no declared target binding configuration.", configuration);
        string directory = Path.Combine(context.engineRoot, "artifacts", "bindings", "requests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(directory);
        try
        {
            string request = Path.Combine(directory, "GenerateBindings.proj");
            new XDocument(new XElement("Project",
                new XElement("ItemGroup", owners.Select((
                    owner,
                    index
                ) => new XElement("BindingProject",
                    new XAttribute("Include", Path.GetFullPath(owner.nativeProject, context.engineRoot)),
                    new XElement("AdditionalProperties", "BindGenDescriptorOutput=" + Path.Combine(directory, index + ".json")
                        + ";BindGenConfig=" + configurations[index])))),
                new XElement("Target", new XAttribute("Name", "Generate"),
                    new XElement("MSBuild", new XAttribute("Projects", "@(BindingProject)"),
                        new XAttribute("Targets", "GenerateBindings"),
                        new XAttribute("Properties", "Configuration=" + (context.configuration == "debug" ? "Debug" : "Release")
                            + ";InnoNativeTarget=" + tools.targetId)))))
                .Save(request);
            await ToolchainEnvironment.RunAsync(tools.ResolveExecutable("dotnet"),
                ["msbuild", request, "-t:Generate", "-nologo", "-m:1", "-nodeReuse:false"],
                context.engineRoot, cancellationToken,
                DotNetSdkEnvironment.Create(tools.ResolveExecutable("dotnet"), tools.environment)).ConfigureAwait(false);
            var results = new Dictionary<string, NativeBindingGenerationDescriptor>(StringComparer.Ordinal);
            for (int index = 0; index < owners.Length; index++)
                results.Add(owners[index].nativeProject, NativeBindingGenerationDescriptor.Load(Path.Combine(directory, index + ".json")));
            return new System.Collections.ObjectModel.ReadOnlyDictionary<string, NativeBindingGenerationDescriptor>(results);
        }
        finally
        {
            Directory.Delete(directory, recursive: true);
        }
    }
}
