using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace Inno.Build.Toolchains;

/// <summary>
/// Describes the complete binding generation selected by one native build request.
/// </summary>
public sealed class NativeBindingGenerationDescriptor
{
    /// <summary>
    /// Writes the exact native binding identities for a subsequent managed build without reselecting generations.
    /// </summary>
    /// <param name="path">
    /// The operation-owned MSBuild selection document.
    /// </param>
    /// <param name="generations">
    /// Explicit checkout-relative native project paths and their complete selected generations.
    /// </param>
    /// <param name="linkage">
    /// Explicit linkage of each selected component, used by its managed native initialization boundary.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A project name is unsafe or repeated, or component linkage is invalid or incomplete.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The generation or linkage closure is null.
    /// </exception>
    public static void WriteSelection(
        string path,
        IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> generations,
        IReadOnlyDictionary<string, NativeLibraryKind> linkage
    ) {
        XDocument document = CreateSelection(generations, linkage);
        string destination = Path.GetFullPath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        document.Save(destination);
    }

    /// <summary>
    /// Rejects publication when the managed selection differs from the complete native generations to deploy.
    /// </summary>
    /// <param name="path">
    /// The operation-owned selection consumed during managed compilation.
    /// </param>
    /// <param name="generations">
    /// The complete current native project closure and its selected generations.
    /// </param>
    /// <param name="linkage">
    /// The exact component linkage used to build the native closure.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// The selection differs in project closure, fingerprint, source location, linkage, or structure.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The current generation closure has an unsafe or repeated project name.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// The current generation closure is null.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// The managed selection is no longer available.
    /// </exception>
    /// <exception cref="System.Xml.XmlException">
    /// The selection is not a valid XML document.
    /// </exception>
    public static void ValidateSelection(
        string path,
        IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> generations,
        IReadOnlyDictionary<string, NativeLibraryKind> linkage
    ) {
        if (!XNode.DeepEquals(XDocument.Load(path), CreateSelection(generations, linkage)))
            throw new InvalidDataException("Native publication must preserve the exact binding selection used by managed compilation.");
    }

    /// <summary>
    /// Gets the immutable input identity assigned to this target generation.
    /// </summary>
    public required string fingerprint { get; init; }

    /// <summary>
    /// Gets the absolute managed source path selected by the component project.
    /// </summary>
    public required string bindingsPath { get; init; }

    /// <summary>
    /// Gets the complete native bridge directory, or an empty string for a direct C binding.
    /// </summary>
    public required string bridgeDirectory { get; init; }

    /// <summary>
    /// Reads and validates the result of a completed component generation request.
    /// </summary>
    /// <param name="path">
    /// The request-owned descriptor file written after successful generation.
    /// </param>
    /// <returns>
    /// The selected generation whose managed source and optional bridge are present.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// The descriptor is missing required output identities or contains unavailable outputs.
    /// </exception>
    /// <exception cref="JsonException">
    /// The descriptor cannot be read as the current contract.
    /// </exception>
    public static NativeBindingGenerationDescriptor Load(string path)
    {
        NativeBindingGenerationDescriptor descriptor = JsonSerializer.Deserialize<NativeBindingGenerationDescriptor>(
            File.ReadAllText(path)) ?? throw new InvalidDataException("The binding generation descriptor is empty.");
        if (string.IsNullOrWhiteSpace(descriptor.fingerprint)
            || string.IsNullOrWhiteSpace(descriptor.bindingsPath) || !Path.IsPathFullyQualified(descriptor.bindingsPath)
            || !File.Exists(descriptor.bindingsPath) || descriptor.bridgeDirectory is null
            || descriptor.bridgeDirectory.Length > 0 && (!Path.IsPathFullyQualified(descriptor.bridgeDirectory)
                || !Directory.Exists(descriptor.bridgeDirectory)))
            throw new InvalidDataException("The binding generation descriptor does not identify complete outputs.");
        return descriptor;
    }

    /// <summary>
    /// Atomically writes this completed generation to a request-owned descriptor file.
    /// </summary>
    /// <param name="path">
    /// The descriptor destination; a unique path keeps concurrent requests independent.
    /// </param>
    public void Write(string path)
    {
        string destination = Path.GetFullPath(path);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        string staging = destination + ".staging-" + Guid.NewGuid().ToString("N");
        try
        {
            File.WriteAllText(staging, JsonSerializer.Serialize(this));
            File.Move(staging, destination, overwrite: true);
        }
        finally
        {
            if (File.Exists(staging))
                File.Delete(staging);
        }
    }

    private static XDocument CreateSelection(
        IReadOnlyDictionary<string, NativeBindingGenerationDescriptor> generations,
        IReadOnlyDictionary<string, NativeLibraryKind> linkage
    ) {
        ArgumentNullException.ThrowIfNull(generations);
        ArgumentNullException.ThrowIfNull(linkage);
        if (generations.Count != linkage.Count || generations.Keys.Any(key => !linkage.ContainsKey(key)))
            throw new ArgumentException("Binding selections require exact component linkage coverage.", nameof(linkage));
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var document = new XElement("Project");
        foreach (var entry in generations.OrderBy(static entry => entry.Key, StringComparer.Ordinal))
        {
            string name = Path.GetFileNameWithoutExtension(entry.Key);
            if (string.IsNullOrWhiteSpace(name) || name.Any(static value => !char.IsAsciiLetterOrDigit(value)
                && value is not ('.' or '_' or '-')) || !names.Add(name))
                throw new ArgumentException("Binding selections require unique portable project names.", nameof(generations));
            if (!Enum.IsDefined(linkage[entry.Key]))
                throw new ArgumentException("A selected native component requires supported linkage.", nameof(linkage));
            document.Add(new XElement("PropertyGroup",
                new XAttribute("Condition", "'$(MSBuildProjectName)' == '" + name + "'"),
                new XElement("BindGenExpectedFingerprint", entry.Value.fingerprint),
                new XElement("InnoNativeLibraryKind", linkage[entry.Key]),
                new XElement("BindGenOutputDirectory", Path.GetDirectoryName(entry.Value.bindingsPath))));
        }
        return new XDocument(document);
    }
}
