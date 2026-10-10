using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Integration.Browser.Bgfx;
using Inno.Build.Distribution.Standard;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeComponentBuildOptionsTests
{
    [Fact]
    public void OptionsFreezeOrderedDefinitionsAndRejectAmbiguousSdkOverrides()
    {
        string[] arguments = ["-DA=one", "-DB:STRING=two;three"];
        string[] inputs = ["config/profile.cs"];
        var options = new NativeComponentBuildOptions(NativeLibraryKind.Static, arguments, inputs);
        arguments[0] = "-DA=changed";
        inputs[0] = "other.cs";
        Assert.Equal("-DA=one", options.cmakeArguments[0]);
        Assert.Equal("config/profile.cs", Assert.Single(options.inputPaths));
        Assert.Throws<ArgumentException>(() => new NativeComponentBuildOptions((NativeLibraryKind)99));
        foreach (string[] invalid in new[] {
            new[] { "-DCMAKE_TOOLCHAIN_FILE=other" }, new[] { "-DINNO_COMPONENT_BRIDGE=other" }, new[] { "-DINNO_LIBRARY_KIND=SHARED" },
            new[] { "-DA=one", "-DA:STRING=two" }, new[] { "--build" }, new[] { "-DA=one\0two" }
        })
            Assert.Throws<ArgumentException>(() => new NativeComponentBuildOptions(NativeLibraryKind.Shared, invalid));
        Assert.Throws<ArgumentException>(() => new NativeComponentBuildOptions(NativeLibraryKind.Static, inputPaths: ["../escape"]));
        Assert.Throws<ArgumentException>(() => new NativeComponentBuildOptions(NativeLibraryKind.Static, inputPaths: ["C:/absolute"]));
    }

    [Fact]
    public async Task RecipeOwnedDefinitionsCannotBeReplacedBeforeAnyToolStarts()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoComponentConflict", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            string tool = Path.Combine(root, "unexecutable.fixture");
            File.WriteAllText(tool, "This file must never execute.");
            var owner = new NativeComponentDescriptor("fixture", "Native.csproj", "Build.csproj");
            var selection = new NativeToolchainSelection("fixture-target", new BuildHostDescriptor("Windows", "x64"),
                new Dictionary<string, string> { ["cmake"] = tool }, new Dictionary<string, string>(), [], [], ".dll", false);
            var context = new NativeBuildContext(root, "debug").WithToolchain(selection).WithComponentOptions(
                owner, new NativeComponentBuildOptions(NativeLibraryKind.Shared, ["-DLINKAGE:BOOL=OFF"]));
            await Assert.ThrowsAsync<ArgumentException>(() => NativeCMakeExecutor.BuildAsync(context, owner, root,
                "fixture", ["-DLINKAGE=ON"], CancellationToken.None));
            Assert.Equal(0, context.statistics.nativeProcesses);
            Assert.False(Directory.Exists(Path.Combine(root, "obj")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void ProductPlanDeclaresStaticArchivesAndDoesNotGuessLinkageFromTheHost()
    {
        ProductNativeBuildPlan plan = StandardNativeBuildPlans.CreateStaticPlayer(BrowserBgfxIntegration.nativeOptions);
        Assert.All(plan.steps, static step => {
            Assert.Equal(NativeLibraryKind.Static, step.options.libraryKind);
            Assert.Contains("browser-wasm", step.component.staticBuild!.targetIds);
        });
        Assert.Contains(plan.steps, static step => step.options.cmakeArguments.Count > 0);
    }

    [Fact]
    public void LinkageOrderedArgumentsAndConfigurationBytesAllChangeRecipeIdentity()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoComponentOptionsTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), "fixture");
            Directory.CreateDirectory(Path.Combine(root, "component"));
            File.WriteAllText(Path.Combine(root, "component/Native.csproj"), "<Project />");
            File.WriteAllText(Path.Combine(root, "component/Toolchain.csproj"), "<Project />");
            string config = Path.Combine(root, "component/Profile.cs");
            File.WriteAllText(config, "one");
            var owner = new NativeComponentDescriptor("fixture", "component/Native.csproj", "component/Toolchain.csproj");

            string Identity(NativeComponentBuildOptions options)
            {
                var context = new NativeBuildContext(root, "debug").WithComponentOptions(owner, options);
                NativeBuildRecipe recipe = NativeBuildRecipe.CreateForComponent(context, owner, "fixture", "fixture-target", [], []);
                foreach (NativeBuildInput input in recipe.inputs)
                {
                    if (File.Exists(input.physicalPath) || Directory.Exists(input.physicalPath))
                        continue;
                    Directory.CreateDirectory(Path.GetDirectoryName(input.physicalPath)!);
                    File.WriteAllText(input.physicalPath, "executor");
                }
                return NativeBuildFingerprint.Create(recipe.declarations, recipe.inputs);
            }

            var options = new NativeComponentBuildOptions(NativeLibraryKind.Shared, ["-DA=one", "-DB=two"], ["component/Profile.cs"]);
            string identity = Identity(options);
            Assert.NotEqual(identity, Identity(new(NativeLibraryKind.Static, options.cmakeArguments, options.inputPaths)));
            Assert.NotEqual(identity, Identity(new(NativeLibraryKind.Shared, ["-DB=two", "-DA=one"], options.inputPaths)));
            DateTime timestamp = File.GetLastWriteTimeUtc(config);
            File.WriteAllText(config, "two");
            File.SetLastWriteTimeUtc(config, timestamp);
            Assert.NotEqual(identity, Identity(options));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
