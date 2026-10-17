using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Inno.Core.IO;

namespace Inno.Build.TaskHosting;

internal static class TaskRuntimeBuildScope
{
    private const string C_PREFIX = "Inno.TaskRuntime.Build/";

    internal static ITaskItem[]? Read(
        IBuildEngine4 engine,
        string identity,
        string load,
        bool verifyOutputs = true,
        CancellationToken cancellation = default
    ) {
        if (engine.GetRegisteredTaskObject(C_PREFIX + identity, RegisteredTaskObjectLifetime.Build) is not string[][] rows)
            return null;
        ITaskItem[] outputs = Restore(rows);
        string assembly = outputs.Single(static item => item.GetMetadata("RelativePath") == "Inno.Build.Tasks.dll").ItemSpec;
        string runtime = Path.GetDirectoryName(assembly)!;
        string publication = Path.GetDirectoryName(runtime)!;
        string owner = Path.GetDirectoryName(publication)!;
        if (!Directory.Exists(publication))
            return null;
        try
        {
            using FileLease activity = FileLease.AcquireSharedAsync(Path.Combine(owner, "owner.lock"), TimeSpan.Zero, cancellation)
                .AsTask().GetAwaiter().GetResult();
            using FileLease reading = FileLease.AcquireSharedAsync(publication + ".lock", TimeSpan.Zero, cancellation)
                .AsTask().GetAwaiter().GetResult();
            if (verifyOutputs && !TaskRuntimePreparation.IsComplete(runtime, TaskRuntimePreparation.FreezeFiles(outputs), cancellation))
                return null;
            TaskRuntimePreparation.RegisterReader(load, assembly);
            return outputs;
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    internal static void Register(
        IBuildEngine4 engine,
        string identity,
        ITaskItem[] outputs
    ) {
        string[][] rows = outputs.Select(static output => new[] { output.ItemSpec }
            .Concat(output.CloneCustomMetadata().Keys.Cast<string>().Order(StringComparer.Ordinal)
                .SelectMany(name => new[] { name, output.GetMetadata(name) })).ToArray()).ToArray();
        // Only BCL data crosses private task load boundaries. No leases or SDK owners enter the registry.
        engine.UnregisterTaskObject(C_PREFIX + identity, RegisteredTaskObjectLifetime.Build);
        engine.RegisterTaskObject(C_PREFIX + identity, rows, RegisteredTaskObjectLifetime.Build, allowEarlyCollection: false);
    }

    private static ITaskItem[] Restore(string[][] rows)
    {
        var outputs = new List<ITaskItem>(rows.Length);
        foreach (string[] row in rows)
        {
            var item = new TaskItem(row[0]);
            for (int index = 1; index < row.Length; index += 2)
                item.SetMetadata(row[index], row[index + 1]);
            outputs.Add(item);
        }
        return outputs.ToArray();
    }
}
