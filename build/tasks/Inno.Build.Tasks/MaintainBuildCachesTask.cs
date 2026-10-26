using System;
using System.Threading;
using Microsoft.Build.Framework;
using Inno.Build.Toolchains;
using BuildTask = Microsoft.Build.Utilities.Task;

namespace Inno.Build.Tasks;

/// <summary>
/// Runs bounded cache maintenance after a successful root Editor operation.
/// </summary>
public sealed class MaintainBuildCachesTask : BuildTask
{
    /// <summary>
    /// Gets or sets the absolute checkout owning the registered cache catalog.
    /// </summary>
    [Required]
    public string EngineRoot { get; set; } = string.Empty;

    /// <summary>
    /// Attempts maintenance without changing the successful product build outcome.
    /// </summary>
    /// <returns>
    /// True after completed or explicitly deferred automatic maintenance.
    /// </returns>
    public override bool Execute()
    {
        try
        {
            BuildMaintenanceApplication.CleanAsync(EngineRoot, new BuildCachePolicy(), false, false, null,
                CancellationToken.None).GetAwaiter().GetResult();
            Log.LogMessage(MessageImportance.High, "INNO-CACHE-MAINTENANCE root=Editor budgetSeconds=5");
        }
        catch (Exception failure)
        {
            Log.LogWarning("The product build succeeded; automatic cache maintenance was deferred: {0}", failure.Message);
        }
        return true;
    }
}
