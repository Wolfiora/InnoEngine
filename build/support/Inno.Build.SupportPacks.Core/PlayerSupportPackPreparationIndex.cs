using System;
using System.IO;
using System.Text;
using Inno.Core.IO;

namespace Inno.Build.SupportPacks;

internal static class PlayerSupportPackPreparationIndex
{
    internal static string? Read(
        string root,
        string target,
        string preparation
    ) {
        string path = GetPath(root, target, preparation);
        if (!File.Exists(path))
            return null;
        string fingerprint = File.ReadAllText(path);
        return IsIdentity(fingerprint) ? fingerprint : null;
    }

    internal static void Write(
        string root,
        string target,
        string preparation,
        string content
    ) {
        if (!IsIdentity(content))
            throw new InvalidDataException("A prepared Pack index requires a complete content identity.");
        string path = GetPath(root, target, preparation);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        PathBoundary.RequireUnlinkedPath(root, path);
        AtomicFile.WriteAllBytes(path, Encoding.UTF8.GetBytes(content));
    }

    private static string GetPath(
        string root,
        string target,
        string preparation
    ) {
        if (!IsIdentity(preparation))
            throw new InvalidDataException("A prepared Pack index requires a complete source identity.");
        return PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, target + "/.preparations/" + preparation));
    }

    private static bool IsIdentity(string identity) => identity.Length == 64
        && !System.Linq.Enumerable.Any(identity, static character => !char.IsAsciiHexDigitLower(character));
}
