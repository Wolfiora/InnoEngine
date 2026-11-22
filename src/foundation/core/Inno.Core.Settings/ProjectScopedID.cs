using System;

namespace Inno.Core.Settings;

/// <summary>
/// Combines a mutable project namespace with a stable project-independent local identity.
/// </summary>
public readonly record struct ProjectScopedID
{
    /// <summary>
    /// Creates a qualified project identity.
    /// </summary>
    /// <param name="projectID">
    /// The current project namespace.
    /// </param>
    /// <param name="name">
    /// The stable local identity.
    /// </param>
    public ProjectScopedID(
        ProjectID projectID,
        ProjectLocalID name
    ) {
        if (!projectID.isValid)
        {
            throw new ArgumentException("A valid Project ID is required.", nameof(projectID));
        }
        if (string.IsNullOrEmpty(name.value))
        {
            throw new ArgumentException("A valid project-local ID is required.", nameof(name));
        }
        this.projectID = projectID;
        this.name = name;
    }

    /// <summary>
    /// Gets the project namespace.
    /// </summary>
    public ProjectID projectID { get; }

    /// <summary>
    /// Gets the project-independent local identity.
    /// </summary>
    public ProjectLocalID name { get; }

    /// <summary>
    /// Gets the canonical <c>projectId.name</c> representation.
    /// </summary>
    public string value => $"{projectID.value}.{name.value}";

    /// <summary>
    /// Formats the canonical qualified identity.
    /// </summary>
    /// <returns>
    /// The <c>projectId.name</c> representation.
    /// </returns>
    public override string ToString() => value;
}
