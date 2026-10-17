namespace Inno.Core.IO;

/// <summary>
/// Describes a recorded temporary directory without acquiring its producer's rights.
/// </summary>
public sealed class ProcessOwnedDirectoryDescriptor
{
    internal ProcessOwnedDirectoryDescriptor(
        string directory,
        string leasePath,
        ProcessOwnerState ownerState
    ) {
        this.directory = directory;
        this.leasePath = leasePath;
        this.ownerState = ownerState;
    }

    /// <summary>
    /// Gets the absolute path proven by the current ownership record.
    /// </summary>
    public string directory { get; }

    /// <summary>
    /// Gets the persistent producer and retirement coordination file.
    /// </summary>
    public string leasePath { get; }

    /// <summary>
    /// Gets Active or Unknown while deletion must remain blocked, or Exited for retired owners.
    /// </summary>
    public ProcessOwnerState ownerState { get; }
}
