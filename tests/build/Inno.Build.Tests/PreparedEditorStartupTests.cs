using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Entry.Bootstrap;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Build.Tests;

public sealed class PreparedEditorStartupTests
{
    [Theory]
    [InlineData(false, false)]
    [InlineData(true, false)]
    [InlineData(false, true)]
    public async Task PreparedApplicationRequiresIntactContentAndRetainsOwnership(
        bool fail,
        bool damage
    ) {
        bool windows = OperatingSystem.IsWindows() && RuntimeInformation.ProcessArchitecture == Architecture.X64;
        bool mac = OperatingSystem.IsMacOS() && RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
        if (!windows && !mac)
            return;
        string target = windows ? "windows-x64" : "macos-arm64";
        string platform = windows ? "Windows" : "MacOS";
        string root = Path.Combine(Path.GetTempPath(), "InnoPreparedEntryTests", Guid.NewGuid().ToString("N"));
        string product = Path.Combine(root, "Product");
        string launch = Path.Combine(root, "Launch");
        Directory.CreateDirectory(product);
        Directory.CreateDirectory(launch);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
            string bootstrap = typeof(DevelopmentEntry).Assembly.Location;
            string io = typeof(FileLease).Assembly.Location;
            File.Copy(bootstrap, Path.Combine(launch, Path.GetFileName(bootstrap)));
            File.Copy(io, Path.Combine(launch, Path.GetFileName(io)));
            string application = Path.Combine(product, "Inno.Editor." + platform + ".dll");
            Emit(application, """
                using System;
                using System.IO;
                using System.Threading;
                namespace Inno.Editor.PLATFORM;
                public static class PLATFORMEditorComposition
                {
                    public static int Run(string[] arguments, CancellationToken cancellation)
                    {
                        foreach (string path in arguments[1..])
                        {
                            try
                            {
                                using var exclusive = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
                                return 91;
                            }
                            catch (IOException) { }
                        }
                        Console.WriteLine("PREPARED-APPLICATION: protected");
                        if (arguments[0] == "fail")
                            throw new InvalidOperationException("Fixture application failure.");
                        return 0;
                    }
                }
                """.Replace("PLATFORM", platform, StringComparison.Ordinal), OutputKind.DynamicallyLinkedLibrary);
            File.WriteAllText(Path.ChangeExtension(application, ".deps.json"), """
                {"runtimeTarget":{"name":".NETCoreApp,Version=v9.0"},"targets":{".NETCoreApp,Version=v9.0":{}},"libraries":{}}
                """);
            Directory.CreateDirectory(Path.Combine(product, "native"));
            File.WriteAllText(Path.Combine(product, "native", "fixture.data"), "A deployment inventory fixture; no native code is executed.");
            string probe = Path.Combine(launch, "EntryProbe.dll");
            Emit(probe, """
                using Inno.Entry.Bootstrap;
                internal static class Program
                {
                    private static int Main(string[] arguments) => DevelopmentEntry.RunEditor(arguments);
                }
                """, OutputKind.ConsoleApplication, bootstrap, io);
            File.WriteAllText(Path.Combine(launch, "EntryProbe.runtimeconfig.json"), """
                {"runtimeOptions":{"tfm":"net9.0","framework":{"name":"Microsoft.NETCore.App","version":"9.0.0"}}}
                """);
            string configuration = typeof(DevelopmentEntry).Assembly
                .GetCustomAttributes(false).OfType<System.Reflection.AssemblyConfigurationAttribute>()
                .Single().Configuration;
            string receiptDirectory = Path.Combine(root, "artifacts", "products", "editor", target, configuration);
            Directory.CreateDirectory(receiptDirectory);
            Dictionary<string, string> inventory = Directory.EnumerateFiles(product, "*", SearchOption.AllDirectories)
                .ToDictionary(path => Path.GetRelativePath(product, path).Replace('\\', '/'),
                    path => Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path))));
            File.WriteAllBytes(Path.Combine(receiptDirectory, "ready.json"), JsonSerializer.SerializeToUtf8Bytes(new
            {
                target,
                configuration,
                assemblyPath = Path.GetRelativePath(root, application),
                files = inventory
            }));
            if (damage)
            {
                string file = Path.Combine(product, "native", "fixture.data");
                DateTime written = File.GetLastWriteTimeUtc(file);
                byte[] bytes = File.ReadAllBytes(file);
                bytes[0] ^= 1;
                File.WriteAllBytes(file, bytes);
                File.SetLastWriteTimeUtc(file, written);
            }
            string checkoutLock = Path.Combine(root, "artifacts", "entry", "checkout.lock");
            string productLock = Path.Combine(receiptDirectory, "product.lock");
            string dotnet = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
                windows ? "dotnet.exe" : "dotnet");
            var start = new ProcessStartInfo(dotnet)
            {
                WorkingDirectory = root,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            foreach (string argument in new[] { probe, fail ? "fail" : "success", checkoutLock, productLock })
                start.ArgumentList.Add(argument);
            using Process process = Process.Start(start) ?? throw new InvalidOperationException("Entry fixture did not start.");
            Task<string> output = process.StandardOutput.ReadToEndAsync();
            Task<string> errors = process.StandardError.ReadToEndAsync();
            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(30));
            try
            {
                await process.WaitForExitAsync(timeout.Token);
            }
            catch
            {
                if (!process.HasExited)
                    process.Kill(entireProcessTree: true);
                await process.WaitForExitAsync();
                await Task.WhenAll(output, errors);
                throw;
            }
            string diagnostics = await output + await errors;
            Assert.True(process.ExitCode == (fail || damage ? 1 : 0), diagnostics);
            if (damage)
            {
                Assert.DoesNotContain("PREPARED-APPLICATION", diagnostics, StringComparison.Ordinal);
                Assert.DoesNotContain("Starting prepared Editor", diagnostics, StringComparison.Ordinal);
            }
            else
                Assert.Contains("PREPARED-APPLICATION: protected", diagnostics, StringComparison.Ordinal);
            Assert.DoesNotContain("INNO-TASK", diagnostics, StringComparison.Ordinal);
            Assert.False(Directory.Exists(Path.Combine(root, "artifacts", "build-tools")));
            using FileLease checkout = await FileLease.AcquireAsync(checkoutLock, TimeSpan.FromSeconds(1));
            using FileLease deployed = await FileLease.AcquireAsync(productLock, TimeSpan.FromSeconds(1));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static void Emit(
        string output,
        string source,
        OutputKind kind,
        params string[] dependencies
    ) {
        string[] platform = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        var references = platform.Concat(dependencies).Distinct(StringComparer.OrdinalIgnoreCase)
            .Select(static path => MetadataReference.CreateFromFile(path));
        var compilation = CSharpCompilation.Create(Path.GetFileNameWithoutExtension(output),
            [CSharpSyntaxTree.ParseText(source)], references, new CSharpCompilationOptions(kind));
        var result = compilation.Emit(output);
        Assert.True(result.Success, string.Join(Environment.NewLine, result.Diagnostics));
    }
}
