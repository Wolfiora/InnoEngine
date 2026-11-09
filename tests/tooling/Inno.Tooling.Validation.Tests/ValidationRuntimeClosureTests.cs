using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationRuntimeClosureTests
{
    [Fact]
    public async Task VerifiedDependenciesPreservePrivateTransitiveRuntimeFiles()
    {
        using var repository = new ValidationRepository();
        string root = TestRepository.GetRoot(typeof(ValidationRuntimeClosureTests).Assembly);
        repository.Write("Directory.Build.props", """
            <Project>
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <ImplicitUsings>disable</ImplicitUsings>
                <ShouldUnsetParentConfigurationAndPlatform>false</ShouldUnsetParentConfigurationAndPlatform>
              </PropertyGroup>
            </Project>
            """);
        var targets = new XElement("Project", new XElement("Import", new XAttribute("Project", Path.Combine(root, "Directory.Build.targets"))));
        targets.Add(XElement.Parse("""
            <Target Name="RecordDependencyBuild" AfterTargets="Build" Condition="'$(MSBuildProjectName)' != 'App'">
              <WriteLinesToFile File="$(MSBuildThisFileDirectory)dependency-builds.log" Lines="$(MSBuildProjectName)" Overwrite="false" />
            </Target>
            """));
        repository.Write("Directory.Build.targets", targets.ToString());
        repository.Write("Leaf/Leaf.csproj", ValidationRepository.Project());
        repository.Write("Leaf/Leaf.cs", "public sealed class Leaf {}");
        repository.Write("Middle/Middle.csproj", ValidationRepository.Project("""
            <ItemGroup><ProjectReference Include="../Leaf/Leaf.csproj" PrivateAssets="compile" /></ItemGroup>
            """));
        repository.Write("Middle/Middle.cs", "public sealed class Middle { public static System.Type LeafType => typeof(Leaf); }");
        repository.Write("Bridge/Bridge.csproj", ValidationRepository.Project("""
            <PropertyGroup>
              <InnoExposeRuntimeClosure>true</InnoExposeRuntimeClosure>
              <CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies>
            </PropertyGroup>
            <ItemGroup><ProjectReference Include="../Middle/Middle.csproj" PrivateAssets="compile" /></ItemGroup>
            """));
        repository.Write("Bridge/Bridge.cs", "public sealed class Bridge {}");
        repository.Write("App/App.csproj", ValidationRepository.Project("""
            <PropertyGroup><OutputType>Exe</OutputType><CopyLocalLockFileAssemblies>true</CopyLocalLockFileAssemblies></PropertyGroup>
            <ItemGroup><ProjectReference Include="../Bridge/Bridge.csproj" PrivateAssets="compile" /></ItemGroup>
            """));
        repository.Write("App/Program.cs", """
            System.Reflection.Assembly.LoadFrom(System.IO.Path.Combine(System.AppContext.BaseDirectory, "Leaf.dll"));
            """);
        repository.Write("Build.slnx", "<Solution><Project Path=\"App/App.csproj\" /></Solution>");
        await repository.BuildAsync("Build.slnx", "-c", "Release");
        string application = repository.PathOf("App/bin/Release/net10.0/App.dll");
        await repository.SdkAsync(application);
        byte[] leaf = File.ReadAllBytes(repository.PathOf("App/bin/Release/net10.0/Leaf.dll"));
        string[] builds = File.ReadAllLines(repository.PathOf("dependency-builds.log"));
        Assert.Contains("Leaf", builds);
        await repository.SdkAsync("clean", "Build.slnx", "-c", "Release", "-p:BuildProjectReferences=false", "-p:InnoCleanProjectReferences=false",
            "--disable-build-servers", "-m:1", "-nr:false");
        Assert.False(File.Exists(application));
        await repository.BuildAsync("Build.slnx", "-c", "Release", "-p:InnoUseVerifiedProjectOutputs=true");
        await repository.SdkAsync(application);
        Assert.Equal(leaf, File.ReadAllBytes(repository.PathOf("App/bin/Release/net10.0/Leaf.dll")));
        Assert.Equal(builds, File.ReadAllLines(repository.PathOf("dependency-builds.log")));
    }
}
