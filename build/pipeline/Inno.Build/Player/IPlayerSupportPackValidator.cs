namespace Inno.Build;

/// <summary>
/// Validates platform-owned runtime or link inputs without coupling the pack catalog to target names.
/// </summary>
public interface IPlayerSupportPackValidator
{
    /// <summary>
    /// Rejects an incomplete or incompatible platform closure.
    /// </summary>
    /// <param name="directory">
    /// The prepared or installed Support Pack directory.
    /// </param>
    /// <exception cref="System.IO.InvalidDataException">
    /// A required platform input is absent or incompatible.
    /// </exception>
    void Validate(string directory);
}
