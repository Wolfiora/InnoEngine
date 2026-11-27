using System;
using System.IO;

namespace Inno.Benchmark.Runner;

public static class BenchmarkPaths
{
    public static string CreateRoot(string name)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoBenchmarks", name, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        return root;
    }

    public static void Delete(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }
}
