using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using Inno.Content;
using Inno.Core.IO;

namespace Inno.Adapter.Content.FileSystem;

internal static class ContentCacheValidator
{
    internal static bool Validate(
        string root,
        ContentPackIndex index,
        CancellationToken cancellationToken
    ) {
        if (!Directory.Exists(root) || (File.GetAttributes(root) & FileAttributes.ReparsePoint) != 0)
            return false;
        HashSet<string> files = new(StringComparer.Ordinal);
        HashSet<string> directories = new(StringComparer.Ordinal);
        foreach (ContentEntry entry in index.entries)
        {
            string key = entry.key.value!;
            files.Add(key);
            for (int separator = key.IndexOf('/'); separator >= 0; separator = key.IndexOf('/', separator + 1))
                directories.Add(key[..separator]);
        }
        Stack<string> pending = new();
        pending.Push(root);
        while (pending.TryPop(out string? directory))
        {
            foreach (string path in Directory.EnumerateFileSystemEntries(directory))
            {
                cancellationToken.ThrowIfCancellationRequested();
                FileAttributes attributes = File.GetAttributes(path);
                if ((attributes & FileAttributes.ReparsePoint) != 0)
                    return false;
                string key = Path.GetRelativePath(root, path).Replace('\\', '/');
                if ((attributes & FileAttributes.Directory) != 0)
                {
                    if (!directories.Remove(key))
                        return false;
                    pending.Push(path);
                    continue;
                }
                if (!files.Remove(key))
                    return false;
            }
        }
        if (files.Count != 0 || directories.Count != 0)
            return false;
        foreach (ContentEntry entry in index.entries)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                using FileStream input = new(Resolve(root, entry.key), FileMode.Open, FileAccess.Read, FileShare.Read);
                if (input.Length != entry.length)
                    return false;
                string hash = ComputeHash(input, cancellationToken);
                if (!string.Equals(hash, entry.contentHash, StringComparison.Ordinal))
                    return false;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
            catch (DirectoryNotFoundException)
            {
                return false;
            }
        }
        return true;
    }

    internal static string Resolve(
        string root,
        ContentKey key
    ) => PathBoundary.Resolve(root, key.value!);

    private static string ComputeHash(
        Stream input,
        CancellationToken cancellationToken
    ) {
        byte[] buffer = ArrayPool<byte>.Shared.Rent(64 * 1024);
        try
        {
            using IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
            int length;
            while ((length = input.Read(buffer)) != 0)
            {
                cancellationToken.ThrowIfCancellationRequested();
                hash.AppendData(buffer, 0, length);
            }
            cancellationToken.ThrowIfCancellationRequested();
            return Convert.ToHexString(hash.GetHashAndReset());
        }
        finally
        {
            ArrayPool<byte>.Shared.Return(buffer);
        }
    }
}
