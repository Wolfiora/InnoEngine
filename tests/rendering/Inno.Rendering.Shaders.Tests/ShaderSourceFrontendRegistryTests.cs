using System;
using System.IO;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Shaders.Tests;

[Collection("Shader registry fault injection")]
public sealed class ShaderSourceFrontendRegistryTests
{
    [Fact]
    public void SharedRegistryDiscoversIndependentLanguagesAndRetiresItsProviders()
    {
        _ = new BgfxShaderSourceFrontend();
        using var fixture = new Fixture();
        using (var registry = new ShaderSourceFrontendRegistry(fixture.types))
        {
            Assert.Contains("inno.shader-language.bgfx-sc", registry.languageIDs);
            Assert.Contains("tests.registry.language", registry.languageIDs);
            var request = new ShaderSourceRequest(new ShaderSourceFile("value.ishadersource", "source"), "Value", new NoIncludes());
            Assert.True(registry.Analyze("tests.registry.language", request).succeeded);
            fixture.modules.Rebuild();
            Assert.True(registry.Analyze("tests.registry.language", request).succeeded);
            Assert.True(RegistryFrontendProbe.created > 1);
            Assert.Equal(RegistryFrontendProbe.created - 1, RegistryFrontendProbe.disposed);
        }
        Assert.Equal(RegistryFrontendProbe.created, RegistryFrontendProbe.disposed);
    }

    [Fact]
    public void DuplicateCandidateReleasesNewProvidersAndKeepsPreviousSnapshotUsable()
    {
        _ = new BgfxShaderSourceFrontend();
        using var fixture = new Fixture();
        using var registry = new ShaderSourceFrontendRegistry(fixture.types);
        Assert.Contains("tests.registry.language", registry.languageIDs);
        RegistryFrontendProbe.conflict = true;
        try
        {
            Assert.ThrowsAny<ArgumentException>(fixture.modules.Rebuild);
        }
        finally
        {
            RegistryFrontendProbe.conflict = false;
        }
        Assert.Contains("tests.registry.language", registry.languageIDs);
        Assert.Equal(1, RegistryFrontendProbe.created - RegistryFrontendProbe.disposed);
    }

    private sealed class NoIncludes : IShaderSourceResolver
    {
        public ShaderSourceFile ReadInclude(
            string includingFile,
            string include
        ) => throw new FileNotFoundException(include);
    }

    private sealed class Fixture : IDisposable
    {
        private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoShaderRegistry", Guid.NewGuid().ToString("N"));

        internal Fixture()
        {
            RegistryFrontendProbe.created = 0;
            RegistryFrontendProbe.disposed = 0;
            RegistryFrontendProbe.conflict = false;
            this.modules = new ModuleHost(
                new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(ShaderSourceFrontendRegistryTests).Assembly)}
            );
            this.types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        }

        public void Dispose()
        {
            types.Dispose();
            modules.Dispose();
            if (Directory.Exists(m_root)) Directory.Delete(m_root, true);
        }

        internal ModuleHost modules { get; }
        internal TypeCatalog types { get; }
    }
}
