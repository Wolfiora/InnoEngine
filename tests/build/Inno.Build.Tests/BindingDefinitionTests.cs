using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Bindings;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class BindingDefinitionTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBindingDefinition", Guid.NewGuid().ToString("N"));

    public BindingDefinitionTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "Fixture.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(m_root, "config.json"), "{}");
    }

    [Theory]
    [InlineData("<PropertyGroup Condition=\"true\"><InnoBindingHostConfig>config.json</InnoBindingHostConfig></PropertyGroup>")]
    [InlineData("<PropertyGroup><InnoBindingHostConfig>$(SDK)/config.json</InnoBindingHostConfig></PropertyGroup>")]
    [InlineData("<PropertyGroup><InnoBindingHostConfig>../config.json</InnoBindingHostConfig></PropertyGroup>")]
    [InlineData("<Target Name=\"Execute\" />")]
    public async Task ExecutableOrNonLiteralDefinitionsFailBeforeOutputCreation(string body)
    {
        File.WriteAllText(Path.Combine(m_root, "bindings.props"), "<Project>" + body + "</Project>");
        var descriptor = new NativeComponentDescriptor("fixture", "Fixture.csproj", "Fixture.csproj", bindingDefinition: "bindings.props");
        var request = new NativeBindingGenerationRequest([descriptor], "fixture", NativeBindingOutputMode.HostSource);
        await Assert.ThrowsAsync<InvalidDataException>(async () => await new NativeBindingGenerator("dotnet")
            .GenerateAsync(new NativeBuildContext(m_root, "release"), request, CancellationToken.None));
        Assert.False(Directory.Exists(Path.Combine(m_root, "obj")));
    }

    [Fact]
    public async Task UndeclaredTargetNeverFallsBackToTheHostAbi()
    {
        File.WriteAllText(Path.Combine(m_root, "bindings.props"), """
            <Project><PropertyGroup><InnoBindingHostConfig>config.json</InnoBindingHostConfig></PropertyGroup></Project>
            """);
        var descriptor = new NativeComponentDescriptor("fixture", "Fixture.csproj", "Fixture.csproj", bindingDefinition: "bindings.props");
        var request = new NativeBindingGenerationRequest([descriptor], "unknown", NativeBindingOutputMode.TargetArtifacts);
        await Assert.ThrowsAsync<NotSupportedException>(async () => await new NativeBindingGenerator("dotnet")
            .GenerateAsync(new NativeBuildContext(m_root, "release"), request, CancellationToken.None));
        Assert.False(Directory.Exists(Path.Combine(m_root, "obj")));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);
}
