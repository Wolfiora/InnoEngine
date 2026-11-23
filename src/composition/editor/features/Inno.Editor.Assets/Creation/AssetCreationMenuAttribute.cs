using System;
using Inno.Assets;

namespace Inno.Editor.Assets;

/// <summary>
/// Declares the source name, extension, and menu placement that cannot be inferred from an
/// <see cref="AssetObject"/> type alone.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AssetCreationMenuAttribute : Attribute
{
    /// <summary>
    /// Creates one asset creation menu declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable template identifier.
    /// </param>
    /// <param name="menuPath">
    /// Slash-delimited path below the File Browser Create menu.
    /// </param>
    /// <param name="extension">
    /// Native source extension, including its leading dot.
    /// </param>
    /// <param name="defaultName">
    /// Default source name without an extension or numeric suffix.
    /// </param>
    /// <param name="groupOrder">
    /// Stable order of the top-level category below Create.
    /// </param>
    /// <param name="itemOrder">
    /// Stable order of the leaf within its immediate category.
    /// </param>
    /// <param name="separatorBeforeGroup">
    /// Whether the top-level category starts after a separator.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when an identifier, path, source name, or native extension is invalid.
    /// </exception>
    public AssetCreationMenuAttribute(
        string id,
        string menuPath,
        string extension,
        string defaultName,
        int groupOrder = 0,
        int itemOrder = 0,
        bool separatorBeforeGroup = false
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentException.ThrowIfNullOrWhiteSpace(menuPath);
        ArgumentException.ThrowIfNullOrWhiteSpace(extension);
        ArgumentException.ThrowIfNullOrWhiteSpace(defaultName);
        if (extension.Length == 0 || extension[0] != '.'
            || extension.IndexOfAny(['/', '\\']) >= 0
        ) {
            throw new ArgumentException(
                "A native source extension beginning with a dot is required.",
                nameof(extension)
            );
        }

        string normalizedPath = menuPath.Replace('\\', '/').Trim('/');
        if (normalizedPath.Length == 0
            || Array.Exists(normalizedPath.Split('/'), static segment => string.IsNullOrWhiteSpace(segment))
        ) {
            throw new ArgumentException("A valid menu path is required.", nameof(menuPath));
        }
        if (defaultName.IndexOfAny(['/', '\\']) >= 0)
        {
            throw new ArgumentException("The default name cannot contain a path separator.", nameof(defaultName));
        }

        this.id = id;
        this.menuPath = normalizedPath;
        this.extension = extension.ToLowerInvariant();
        this.defaultName = defaultName.Trim();
        this.groupOrder = groupOrder;
        this.itemOrder = itemOrder;
        this.separatorBeforeGroup = separatorBeforeGroup;
    }

    /// <summary>
    /// Gets the globally stable template identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the slash-delimited path below the File Browser Create menu.
    /// </summary>
    public string menuPath { get; }

    /// <summary>
    /// Gets the native source extension, including its leading dot.
    /// </summary>
    public string extension { get; }

    /// <summary>
    /// Gets the default source name without an extension or numeric suffix.
    /// </summary>
    public string defaultName { get; }

    /// <summary>
    /// Gets the stable order of the top-level category below Create.
    /// </summary>
    public int groupOrder { get; }

    /// <summary>
    /// Gets the stable order of the leaf within its immediate category.
    /// </summary>
    public int itemOrder { get; }

    /// <summary>
    /// Gets whether the top-level category starts after a separator.
    /// </summary>
    public bool separatorBeforeGroup { get; }
}
