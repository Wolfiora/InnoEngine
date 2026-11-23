using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;

namespace Inno.Testing.Discovery;

/// <summary>
/// Records discovered sources and process-isolated contracts from a single VSTest discovery session.
/// </summary>
[ExtensionUri("logger://inno/discovery/v1")]
[FriendlyName("InnoDiscovery")]
public sealed class DiscoveryLogger : ITestLoggerWithParameters
{
    /// <exception cref="ArgumentException">
    /// The logger requires the parameterized overload with an explicit receipt path.
    /// </exception>
    /// <inheritdoc/>
    public void Initialize(
        TestLoggerEvents events,
        string testRunDirectory
    ) => throw new ArgumentException("Discovery requires an explicit receipt path.");

    /// <exception cref="ArgumentException">
    /// The Receipt parameter is missing or does not identify an absolute output path.
    /// </exception>
    /// <inheritdoc/>
    public void Initialize(
        TestLoggerEvents events,
        Dictionary<string, string?> parameters
    ) {
        if (!parameters.TryGetValue("Receipt", out string? receipt) || !Path.IsPathFullyQualified(receipt ?? string.Empty))
        {
            throw new ArgumentException("Discovery requires an absolute receipt path.");
        }

        var owners = new Dictionary<string, Owner>(
            OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal
        );
        List<string> errors = [];
        events.DiscoveryMessage += (
            _,
            message
        ) =>
        {
            if (message.Level == TestMessageLevel.Error)
            {
                errors.Add(message.Message ?? "Discovery emitted an unspecified error.");
            }
        };
        events.DiscoveredTests += (
            _,
            batch
        ) =>
        {
            foreach (TestCase test in batch.DiscoveredTestCases ?? throw new InvalidDataException("Discovery omitted its case batch."))
            {
                string source = Path.GetFullPath(test.Source);
                if (!owners.TryGetValue(source, out Owner? owner))
                {
                    owners.Add(source, owner = new Owner());
                }
                owner.count++;
                if (test.Traits.Any(trait => trait.Name == "ProcessIsolation" && trait.Value == "Required"))
                {
                    owner.contracts.Add(test.FullyQualifiedName);
                }
            }
        };
        events.DiscoveryComplete += (
            _,
            completion
        ) =>
        {
            using var output = new FileStream(receipt!, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            JsonSerializer.Serialize(output, new
            {
                completed = !completion.IsAborted && errors.Count == 0
                            && completion.PartiallyDiscoveredSources?.Count is null or 0
                            && completion.NotDiscoveredSources?.Count is null or 0
                            && completion.SkippedDiscoveredSources?.Count is null or 0,
                errors,
                discovered = completion.TotalCount,
                owners = owners.Select(pair => new { source = pair.Key, count = pair.Value.count, contracts = pair.Value.contracts })
            });
        };
    }

    private sealed class Owner
    {
        internal int count { get; set; }
        internal SortedSet<string> contracts { get; } = new(StringComparer.Ordinal);
    }
}
