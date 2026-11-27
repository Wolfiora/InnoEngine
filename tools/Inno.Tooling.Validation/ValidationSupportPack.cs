using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;

namespace Inno.Tooling.Validation;

internal static class ValidationSupportPack
{
    internal static void Check(string archive)
    {
        using ZipArchive zip = ZipFile.OpenRead(archive);
        ZipArchiveEntry runtime = Library("native/");
        ZipArchiveEntry link = Library("PlayerLink/native/");
        using Stream runtimeStream = runtime.Open();
        using Stream linkStream = link.Open();
        if (runtime.Name != link.Name || !SHA256.HashData(runtimeStream).AsSpan().SequenceEqual(SHA256.HashData(linkStream)))
        {
            throw new InvalidDataException("Runtime and PlayerLink must contain the same MiniAudio library.");
        }

        ZipArchiveEntry Library(string prefix)
        {
            ZipArchiveEntry[] entries = zip.Entries.Where(entry => (
                entry.FullName.StartsWith(prefix, StringComparison.Ordinal)
                && entry.Name.Contains("miniaudio", StringComparison.OrdinalIgnoreCase)
                && (entry.Name.EndsWith(".dll", StringComparison.OrdinalIgnoreCase)
                    || entry.Name.EndsWith(".dylib", StringComparison.OrdinalIgnoreCase))
            )).ToArray();
            return entries.Length == 1
                   ? entries[0]
                   : throw new InvalidDataException("Missing or repeated MiniAudio library under " + prefix);
        }
    }
}
