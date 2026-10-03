using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;

namespace Inno.Build.SupportPacks;

internal static class PlayerSupportPackFiles
{
    internal static void CopyReferences(
        string buildOutput,
        string destination
    ) {
        string[] files = Directory.Exists(buildOutput)
            ? Directory.EnumerateFiles(buildOutput, "Inno.*.dll")
                .Where(static file => !Path.GetFileName(file).StartsWith("Inno.Plugin.", StringComparison.Ordinal)
                    && Path.GetFileName(file) is not "Inno.GameScripts.dll" and not "Inno.EditorScripts.dll" and not "Inno.Player.Browser.dll")
                .Order(StringComparer.Ordinal).ToArray()
            : [];
        if (files.Length == 0)
            throw new InvalidDataException("Player build produced no target compilation references.");
        Directory.CreateDirectory(destination);
        foreach (string file in files)
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
    }
}
