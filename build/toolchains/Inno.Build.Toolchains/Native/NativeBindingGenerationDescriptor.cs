using System;
using System.IO;
using System.Text.Json;

namespace Inno.Build.Toolchains;

/// <summary>
/// Describes the complete binding generation selected by one native build request.
/// </summary>
public sealed class NativeBindingGenerationDescriptor
{
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
}
