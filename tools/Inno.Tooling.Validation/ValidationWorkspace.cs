using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Tooling.Validation;

internal static class ValidationWorkspace
{
    internal static void RecordCompleted(
        ValidationPlan plan,
        CancellationToken cancellationToken
    ) {
        var publisher = new BuildCachePublisher(
            Path.GetDirectoryName(plan.artifacts)!,
            BuildCacheArtifactRole.ReusableWorkspace
        );
        publisher.Record(plan.artifacts, plan.artifacts + ".lock", [], null, null, cancellationToken);
    }

    internal static async Task<CoordinatedFileLease> AcquireReadAsync(
        ValidationPlan plan,
        CancellationToken cancellationToken
    ) {
        string parent = Path.GetDirectoryName(plan.artifacts)
                        ?? throw new InvalidDataException("A validation output owner requires a parent directory.");
        ValidationInputs.Resolve(plan.root, Path.GetRelativePath(plan.root, parent));
        string owner = Path.Combine(parent, "owner.lock");
        string leaf = plan.artifacts + ".lock";
        if (!File.Exists(owner) || !File.Exists(leaf))
        {
            throw new InvalidDataException("Required validation output ownership is absent; evidence cannot adopt an unknown directory.");
        }
        return await CoordinatedFileLease.AcquireAsync(
            owner,
            leaf,
            Timeout.InfiniteTimeSpan,
            shared: true,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
    }

    internal static async Task<CoordinatedFileLease> AcquireAsync(
        ValidationPlan plan,
        CancellationToken cancellationToken
    ) {
        string parent = Path.GetDirectoryName(plan.artifacts)
                        ?? throw new InvalidDataException("A validation output owner requires a parent directory.");
        ValidationInputs.Resolve(plan.root, Path.GetRelativePath(plan.root, parent));
        string key = Convert.ToHexStringLower(
            SHA256.HashData(Encoding.UTF8.GetBytes(Path.GetRelativePath(plan.root, parent).Replace('\\', '/')))
        );
        var publisher = new BuildCachePublisher(parent, BuildCacheArtifactRole.ReusableWorkspace);
        BuildCacheOwnerCatalog.Register(plan.root, parent, "validation-workspaces-" + key, publisher.role, cancellationToken);
        using FileLease activity = await publisher.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        CoordinatedFileLease ownership = await CoordinatedFileLease.AcquireAsync(
            Path.Combine(parent, "owner.lock"),
            plan.artifacts + ".lock",
            Timeout.InfiniteTimeSpan,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
        try
        {
            publisher.BeginWorkingDirectory(plan.artifacts, plan.artifacts + ".lock", null, cancellationToken);
            return ownership;
        }
        catch
        {
            ownership.Dispose();
            throw;
        }
    }
}
