using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;

namespace Inno.Benchmark.Runner;

internal static class BenchmarkEnvironment
{
    internal static void Write(
        Assembly assembly,
        string path,
        string[] arguments
    ) {
        Directory.CreateDirectory(Path.GetDirectoryName(Path.GetFullPath(path))!);
        using Stream binary = File.OpenRead(assembly.Location);
        var metadata = new
        {
            binarySha256 = Convert.ToHexString(SHA256.HashData(binary)),
            binaryVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion,
            sdk = assembly.GetCustomAttributes<AssemblyMetadataAttribute>().Single(item => item.Key == "BenchmarkSdk").Value,
            configuration = assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration,
            runtime = RuntimeInformation.FrameworkDescription,
            platform = RuntimeInformation.OSDescription,
            architecture = RuntimeInformation.ProcessArchitecture.ToString(),
            arguments
        };
        File.WriteAllText(path, JsonSerializer.Serialize(metadata, new JsonSerializerOptions { WriteIndented = true }));
    }
}
