namespace Inno.Core.IO;

/// <summary>
/// Describes whether an exact operating-system process lifetime can still own files.
/// </summary>
public enum ProcessOwnerState
{
    /// <summary>
    /// The matching process lifetime is still running.
    /// </summary>
    Active,

    /// <summary>
    /// The process exited or its identifier now belongs to another lifetime.
    /// </summary>
    Exited,

    /// <summary>
    /// Ownership cannot be verified; dependent files must remain protected.
    /// </summary>
    Unknown
}
