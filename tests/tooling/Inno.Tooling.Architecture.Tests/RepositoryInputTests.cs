using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class RepositoryInputTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoRepositoryInputs", Guid.NewGuid().ToString("N"));

    public RepositoryInputTests() => Directory.CreateDirectory(m_root);

    [Fact]
    public void EachAssemblyUsesItsOwnDeclaredSource()
    {
        string first = CreateCheckout("first");
        string second = CreateCheckout("second");

        Assert.Equal(first, TestRepository.GetRoot(CreateAssembly(first)));
        Assert.Equal(second, TestRepository.GetRoot(CreateAssembly(second)));
        Assert.Equal(first, TestRepository.GetRoot(CreateAssembly(first)));
    }

    [Fact]
    public void NullAssemblyIsRejectedBeforeReadingMetadata()
    {
        Assert.Throws<ArgumentNullException>(() => TestRepository.GetRoot(null!));
    }

    [Fact]
    public void MissingRelativeAndUnavailableDeclarationsAreRejected()
    {
        foreach (Assembly assembly in new[]
        {
            CreateAssembly(),
            CreateAssembly("relative-checkout"),
            CreateAssembly(Path.Combine(m_root, "absent")),
            CreateAssembly(m_root)
        })
        {
            Assert.Throws<InvalidOperationException>(() => TestRepository.GetRoot(assembly));
        }
    }

    [Fact]
    public void DuplicateDeclarationsAreRejectedEvenWhenTheyAgree()
    {
        string checkout = CreateCheckout("duplicate");
        Assembly assembly = CreateAssembly(checkout, checkout);

        Assert.Throws<InvalidOperationException>(() => TestRepository.GetRoot(assembly));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private string CreateCheckout(string name)
    {
        string directory = Path.Combine(m_root, name);
        Directory.CreateDirectory(directory);
        File.WriteAllText(Path.Combine(directory, "InnoEngine.sln"), "fixture");
        return directory;
    }

    private static Assembly CreateAssembly(params string[] directories)
    {
        var name = new AssemblyName("Inno.RepositoryInputFixture." + Guid.NewGuid().ToString("N"));
        AssemblyBuilder assembly = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.RunAndCollect);
        ConstructorInfo constructor = typeof(AssemblyMetadataAttribute).GetConstructor([typeof(string), typeof(string)])!;
        foreach (string directory in directories)
            assembly.SetCustomAttribute(new CustomAttributeBuilder(constructor, ["Inno.Testing.RepositoryRoot", directory]));
        return assembly;
    }
}
