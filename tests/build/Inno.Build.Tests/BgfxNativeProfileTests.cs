using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BgfxNativeProfileTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeProfileTests", Guid.NewGuid().ToString("N"));

    public BgfxNativeProfileTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Fact]
    public async Task OpenTargetAcceptsItsExplicitSdkInvocationBeforeSourceValidation()
    {
        var profile = new FixtureProfile("fixture-new-target");
        NativeBuildContext context = CreateContext(profile.targetId);

        await Assert.ThrowsAsync<DirectoryNotFoundException>(() => BgfxNativeBuild.BuildAsync(context, profile, CancellationToken.None));

        Assert.Equal(1, profile.invocationCount);
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Fact]
    public async Task ForeignTargetIsRejectedBeforeInvokingTheProfile()
    {
        var profile = new FixtureProfile("fixture-new-target");
        NativeBuildContext context = CreateContext("fixture-other-target");

        await Assert.ThrowsAsync<ArgumentException>(() => BgfxNativeBuild.BuildAsync(context, profile, CancellationToken.None));

        Assert.Equal(0, profile.invocationCount);
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Fact]
    public void InvocationOwnsAnImmutableArgumentSnapshot()
    {
        string[] arguments = ["--sdk=fixture", "--abi=fixture-abi"];
        var invocation = new BgfxBuildInvocation("compiler", arguments);
        arguments[0] = "--sdk=changed";

        Assert.Equal("--sdk=fixture", invocation.arguments[0]);
        Assert.Throws<ArgumentException>(() => new BgfxBuildInvocation("compiler", [null!]));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SourceDeclarationFreezesRuntimeAndToolOptionsBeforeBindingExists(bool tools)
    {
        var profile = new FixtureProfile("fixture-new-target");
        NativeBuildContext context = CreateContext(profile.targetId);
        foreach (string source in new[] { "bgfx", "bx", "bimg" })
            Directory.CreateDirectory(Path.Combine(m_root, "extern", source));
        string project = Path.Combine(m_root, BgfxNativeBuild.componentDescriptor.toolchainProject);
        Directory.CreateDirectory(Path.GetDirectoryName(project)!);
        File.WriteAllText(project, "<Project />");
        NativeBuildRecipe recipe = tools ? BgfxToolsBuild.CreateRecipe(context, profile)
            : BgfxNativeBuild.CreateRecipe(context, profile);
        Assert.Equal(tools ? "bgfx-tools" : "bgfx", recipe.component);
        Assert.Equal(tools ? null : BgfxNativeBuild.componentDescriptor.bindingDefinition, recipe.owner.bindingDefinition);
        Assert.Equal(1, profile.invocationCount);
        Assert.Contains(recipe.declarations, value => value == "generate=--with-shared-lib");
        Assert.Equal(tools, recipe.declarations.Contains("generate=--with-tools"));
        Assert.Contains(recipe.declarations, value => value == "compile=--sdk=fixture");
        Assert.DoesNotContain(recipe.declarations, value => value.StartsWith("bindings=", StringComparison.Ordinal));
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Fact]
    public async Task OfflineToolsDoNotRequestRuntimeBindings()
    {
        NativeComponentDescriptor tools = BgfxToolsBuild.componentDescriptor;
        Assert.Equal(BgfxNativeBuild.componentDescriptor.nativeProject, tools.nativeProject);
        Assert.Equal(BgfxNativeBuild.componentDescriptor.toolchainProject, tools.toolchainProject);
        Assert.Null(tools.bindingDefinition);
        using NativeBindingGenerationResult generations = await NativeBindingPreparation.PrepareAsync(
            CreateContext("fixture-new-target"), [tools], CancellationToken.None);
        Assert.Empty(generations);
    }

    private NativeBuildContext CreateContext(string target)
    {
        string compiler = Path.Combine(m_root, "fixture-compiler");
        File.WriteAllText(compiler, "Not executable: this fixture stops before source preparation.");
        var selection = new NativeToolchainSelection(target, new BuildHostDescriptor("FixtureHost", "fixture-cpu"),
            new Dictionary<string, string> { ["compiler"] = compiler, ["genie"] = compiler }, new Dictionary<string, string>(), [], [], ".fixture", false);
        return new NativeBuildContext(m_root, "debug").WithToolchain(selection).WithComponentOptions(BgfxNativeBuild.componentDescriptor, new(NativeLibraryKind.Shared));
    }

    private sealed class FixtureProfile(string target) : BgfxNativeBuildProfile
    {
        public override NativeToolchainSelection SelectTools(NativeBuildContext context) => context.RequireToolchain();

        public int invocationCount { get; private set; }
        public override string targetId => target;
        public override IReadOnlyList<string> generatorArguments => ["fixture-generator"];
        public override string artifactPathToken => "/fixture-cpu/bin/";

        public override BgfxBuildInvocation CreateBuildInvocation(
            NativeBuildContext context,
            bool includeTools
        ) {
            invocationCount++;
            return new("compiler", ["--sdk=fixture", "--abi=fixture-abi"]);
        }
    }
}
