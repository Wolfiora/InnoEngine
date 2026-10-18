using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Framework;

namespace Inno.Build.TaskHosting;

/// <summary>
/// Claims bootstrap storage before SDK compilation without depending on the full task runtime.
/// </summary>
public sealed class TaskRuntimeBootstrapOwner : Microsoft.Build.Utilities.Task
{
    /// <summary>
    /// Gets or sets the absolute publisher root managed by the standard distribution.
    /// </summary>
    [Required]
    public string Root { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current operation identity already propagated through the project graph.
    /// </summary>
    [Required]
    public string Identity { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the freshly built Foundation assembly selected by this SDK bootstrap.
    /// </summary>
    [Required]
    public string FoundationAssembly { get; set; } = string.Empty;

    /// <summary>
    /// Gets the new process-owned directory whose claim precedes all bootstrap payloads.
    /// </summary>
    [Output]
    public string Directory { get; private set; } = string.Empty;

    /// <summary>
    /// Establishes ownership through Core.IO; no SDK object or lease enters the Build registry.
    /// </summary>
    /// <returns>
    /// True after a new claim, or false with an error before payload compilation.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            // The SDK inline compiler targets netstandard. Invoke only the public Foundation
            // entry so its current runtime contract does not become a duplicate bootstrap writer.
            Assembly foundation = Assembly.LoadFrom(FoundationAssembly);
            Type owner = foundation.GetType("Inno.Core.IO.ProcessOwnedDirectory", throwOnError: true)!;
            MethodInfo create = owner.GetMethod("CreateForProcessAsync", BindingFlags.Public | BindingFlags.Static,
                null, [typeof(string), typeof(string), typeof(CancellationToken)], null)!;
            object pending = create.Invoke(null, [Root, Identity, CancellationToken.None])!;
            var operation = (Task<string>)pending.GetType().GetMethod("AsTask", BindingFlags.Public | BindingFlags.Instance)!.Invoke(pending, null)!;
            Directory = operation.GetAwaiter().GetResult();
            return true;
        }
        catch (Exception failure)
        {
            Log.LogErrorFromException(failure, showStackTrace: true);
            return false;
        }
    }
}
