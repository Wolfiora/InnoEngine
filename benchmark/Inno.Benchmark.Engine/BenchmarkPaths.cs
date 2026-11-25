using System;
using System.IO;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Diagnostics;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Graph;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.Plugins.Authoring;

namespace Inno.Benchmark.Engine;

internal static class BenchmarkPaths
{
    internal static string CreateRoot(string name)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoBenchmarks", name, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        return root;
    }

    internal static void Delete(string path)
    {
        if (Directory.Exists(path))
        {
            Directory.Delete(path, recursive: true);
        }
    }
}
