using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Inno.Extensibility.Catalogs;
using Inno.Runtime;
using Inno.Runtime.Generators;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Runtime.Generators.Tests;

public sealed class RuntimeModuleCatalogGeneratorTests
{
    [Fact]
    public void AConverterWithoutAConstructionEntryFailsDuringCompilation()
    {
        const string source = """
            namespace Fixture;
            public sealed class Box<T>;
            public sealed class Model : Inno.Core.Serialization.ISerializable
            {
                [Inno.Core.Serialization.SerializableProperty]
                public Box<int>? value { get; set; }
            }
            public sealed class BoxConverter<T> : Inno.Core.Serialization.Converters.SerializationConverter<Box<T>>
            {
                public BoxConverter(int unused) { }
                public override void Write(Inno.Core.Serialization.SerializationWriter writer, Box<T> value) { }
                public override Box<T> Read(Inno.Core.Serialization.SerializationReader reader) => new();
            }
            """;
        string[] paths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        CSharpCompilation input = CSharpCompilation.Create("Inno.InvalidStaticCatalogFixture",
            [CSharpSyntaxTree.ParseText(source)], paths.Select(static path => MetadataReference.CreateFromFile(path)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new RuntimeModuleCatalogGenerator().AsSourceGenerator());
        driver.RunGeneratorsAndUpdateCompilation(input, out _, out var diagnostics);
        Diagnostic failure = Assert.Single(diagnostics, static diagnostic => diagnostic.Id == "INNORUN002");
        Assert.Equal(DiagnosticSeverity.Error, failure.Severity);
        Assert.Contains("parameterless constructor", failure.GetMessage());
    }

    [Fact]
    public void GeneratedCatalogCompilesAndPublishesExactMetadataAndFactories()
    {
        const string source = """
            using System;
            namespace Fixture;
            [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
            public sealed class MarkerAttribute : Attribute
            {
                public MarkerAttribute(string name) { this.name = name; }
                public string name { get; }
            }
            [AttributeUsage(AttributeTargets.Class)]
            public sealed class ShapeAttribute : Attribute
            {
                public ShapeAttribute(Type type, string[] labels) { }
            }
            [Marker("base")]
            public class Base
            {
                protected virtual void Tick() { }
            }
            public interface IValue<T> { }
            [Marker("derived")]
            [Shape(typeof(Nullable<>), new[] { "one", "two" })]
            public sealed class Derived : Base, IValue<int>
            {
                private Derived() { }
                protected override void Tick() { }
            }
            public class Generic<T> { public sealed class Nested { } }
            public class GenericUse { public Generic<int> value { get; } = new(); }
            public sealed class Box<T> { public T value { get; set; } = default!; }
            public sealed class BoxHost : Inno.Core.Serialization.ISerializable
            {
                [Inno.Core.Serialization.SerializableProperty]
                public Box<int>? box { get; set; }
                public Box<string>? unsupported { get; set; }
            }
            public sealed class BoxConverter<T> : Inno.Core.Serialization.Converters.SerializationConverter<Box<T>> where T : struct
            {
                private BoxConverter() { }
                public override void Write(Inno.Core.Serialization.SerializationWriter writer, Box<T> value)
                    => writer.Write("value", value.value);
                public override Box<T> Read(Inno.Core.Serialization.SerializationReader reader)
                    => new() { value = reader.Read<T>("value") };
            }
            public class RequiredBase { public required string name { get; init; } }
            public sealed class RequiredChild : RequiredBase { }
            """;
        string[] paths = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")
            ?? throw new InvalidOperationException("The test host provides no platform closure.")).Split(Path.PathSeparator);
        CSharpCompilation input = CSharpCompilation.Create("Inno.StaticCatalogFixture",
            [CSharpSyntaxTree.ParseText(source)], paths.Select(static path => MetadataReference.CreateFromFile(path)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new RuntimeModuleCatalogGenerator().AsSourceGenerator());
        driver.RunGeneratorsAndUpdateCompilation(input, out Compilation output, out var diagnostics);
        Assert.Empty(diagnostics.Where(static value => value.Severity == DiagnosticSeverity.Error));
        using var stream = new MemoryStream();
        var emitted = output.Emit(stream);
        Assert.True(emitted.Success, string.Join(Environment.NewLine, emitted.Diagnostics));
        stream.Position = 0;
        var lifetime = new AssemblyLoadContext("static-catalog-test", isCollectible: true);
        try
        {
            Assembly assembly = lifetime.LoadFromStream(stream);
            MethodInfo entry = assembly.GetType("Inno.StaticCatalogFixture.Generated.RuntimeModuleCatalog", throwOnError: true)!
                .GetMethod("Register", BindingFlags.Public | BindingFlags.Static)!;
            var register = entry.CreateDelegate<Action<ITypeCatalogRegistrar>>();
            var catalog = new StaticTypeCatalogSource([register]);
            Type derivedType = assembly.GetType("Fixture.Derived", throwOnError: true)!;
            TypeCatalogMetadata derived = catalog.GetMetadata(derivedType);
            Assert.Equal(derived.type, catalog.CreateInstance(derived.type).GetType());
            Assert.Equal("Fixture.Base", Assert.Single(derived.baseTypes).FullName);
            Assert.Equal(typeof(int), Assert.Single(derived.interfaces).GenericTypeArguments[0]);
            Attribute marker = Assert.Single(derived.inheritedAttributes, static attribute => attribute.GetType().Name == "MarkerAttribute");
            Assert.Equal("derived", marker.GetType().GetProperty("name")!.GetValue(marker));
            Assert.Contains(derived.parameterlessOverrides, static slot => slot.name == "Tick" && slot.declaringBase.FullName == "Fixture.Base");
            Assert.False(catalog.CanCreateInstance(assembly.GetType("Fixture.RequiredChild", throwOnError: true)!));
            Assert.False(catalog.CanCreateInstance(assembly.GetType("Fixture.Generic`1+Nested", throwOnError: true)!));
            Type generic = assembly.GetType("Fixture.Generic`1", throwOnError: true)!;
            Type constructed = catalog.ConstructGenericType(generic, [typeof(int)])!;
            Assert.Equal(constructed, catalog.CreateInstance(constructed).GetType());
            Assert.DoesNotContain(constructed, catalog.GetTypes(assembly));
            Assert.Throws<NotSupportedException>(() => catalog.ConstructGenericType(generic, [typeof(string)]));
            Type converter = assembly.GetType("Fixture.BoxConverter`1", throwOnError: true)!;
            Type linkedConverter = catalog.ConstructGenericType(converter, [typeof(int)])!;
            Assert.Equal(linkedConverter, catalog.CreateInstance(linkedConverter).GetType());
            Assert.Null(catalog.ConstructGenericType(converter, [typeof(string)]));
        }
        finally
        {
            lifetime.Unload();
        }
    }
}
