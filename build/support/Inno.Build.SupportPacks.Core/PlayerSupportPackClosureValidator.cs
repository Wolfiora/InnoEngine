using System;
using System.IO;
using System.Linq;
using Inno.Core.IO;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Combines a platform's layout checks with distribution-selected managed, binding and native requirements.
/// </summary>
public sealed class PlayerSupportPackClosureValidator : IPlayerSupportPackValidator
{
    private readonly PlayerSupportPackRequirements m_requirements;
    private readonly IPlayerSupportPackValidator m_platform;
    private readonly IPlayerSupportPackValidator? m_deployment;

    /// <summary>
    /// Borrows validators while retaining only the immutable selected closure requirements.
    /// </summary>
    /// <param name="requirements">
    /// The distribution's frozen input requirements.
    /// </param>
    /// <param name="platform">
    /// The borrowed layout and ABI validator.
    /// </param>
    /// <param name="deployment">
    /// An optional implementation-owned deployment metadata validator.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Requirements or the platform validator are null.
    /// </exception>
    public PlayerSupportPackClosureValidator(
        PlayerSupportPackRequirements requirements,
        IPlayerSupportPackValidator platform,
        IPlayerSupportPackValidator? deployment
    ) {
        m_requirements = requirements ?? throw new ArgumentNullException(nameof(requirements));
        m_platform = platform ?? throw new ArgumentNullException(nameof(platform));
        m_deployment = deployment;
    }

    /// <summary>
    /// Rejects missing selected inputs, undeclared native groups and invalid deployment metadata.
    /// </summary>
    /// <param name="directory">
    /// The complete Pack directory whose content manifest is independently verified by its publisher.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A selected input or native group is missing or a library belongs to an undeclared group.
    /// </exception>
    /// <exception cref="IOException">
    /// An input escapes ownership, crosses a link or cannot be read.
    /// </exception>
    public void Validate(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        m_platform.Validate(directory);
        foreach (string relative in m_requirements.requiredFiles)
            if (!File.Exists(PathBoundary.Resolve(directory, relative)))
                throw new InvalidDataException($"Support Pack '{m_requirements.target}' lacks selected input '{relative}'.");
        string root = PathBoundary.Resolve(directory, m_requirements.nativeRoot);
        if (!Directory.Exists(root))
            throw new InvalidDataException($"Support Pack '{m_requirements.target}' has no selected native deployment.");
        string[] libraries = PathBoundary.EnumerateFiles(root)
            .Where(path => Path.GetExtension(path).Equals(m_requirements.nativeExtension, StringComparison.OrdinalIgnoreCase))
            .Select(path => Path.GetRelativePath(root, path).Replace('\\', '/')).ToArray();
        foreach (string group in m_requirements.nativeDirectories)
            if (!libraries.Any(path => path.StartsWith(group + "/", StringComparison.Ordinal)))
                throw new InvalidDataException($"Support Pack '{m_requirements.target}' lacks native component group '{group}'.");
        if (libraries.Any(path => !m_requirements.nativeDirectories.Any(group => path.StartsWith(group + "/", StringComparison.Ordinal))))
            throw new InvalidDataException($"Support Pack '{m_requirements.target}' contains an undeclared native component group.");
        if (m_requirements.nativeFiles is { } selected
            && !libraries.ToHashSet(StringComparer.Ordinal).SetEquals(selected))
            throw new InvalidDataException($"Support Pack '{m_requirements.target}' does not match its selected native file closure.");
        m_deployment?.Validate(directory);
    }
}
