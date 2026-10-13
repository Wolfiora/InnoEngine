using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentDependencies
{
    internal static async Task PrepareAsync(
        string root,
        CancellationToken cancellationToken
    ) {
        string status = await DevelopmentProcess.RunAsync("git", ["submodule", "status"], root,
            capture: true, cancellationToken).ConfigureAwait(false);
        var arguments = new List<string> { "submodule", "update", "--init", "--recursive", "--" };
        foreach (string line in status.Split('\n', StringSplitOptions.RemoveEmptyEntries))
        {
            if (line[0] != '-')
                continue;
            string[] fields = line.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string path = fields[1];
            string absolute = Path.GetFullPath(path, root);
            if (!absolute.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.Ordinal))
                throw new InvalidDataException("A dependency path escapes this checkout.");
            arguments.Add(path);
        }
        if (arguments.Count > 5)
        {
            Console.WriteLine("Initializing pinned source dependencies...");
            await DevelopmentProcess.RunAsync("git", arguments.ToArray(), root,
                capture: false, cancellationToken).ConfigureAwait(false);
        }
    }
}
