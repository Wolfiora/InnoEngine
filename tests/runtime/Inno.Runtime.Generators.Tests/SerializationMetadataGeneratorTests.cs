using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using Inno.Adapter.Modules.DotNet;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Generators;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Runtime.Generators.Tests;

public sealed class SerializationMetadataGeneratorTests
{
    [Theory]
    [InlineData("[SerializableProperty] public int value => 1;", "INNOSER010")]
    [InlineData("[OnSerializableRestored] private int Restored() => 0;", "INNOSER011")]
    [InlineData("[OnSerializableRestored] private void First() {} [OnSerializableRestored] private void Second() {}", "INNOSER011")]
    public void InvalidDeclarationsFailDuringGeneration(
        string member,
        string expectedDiagnostic
    ) {
        string source = "using Inno.Core.Serialization; public class State : ISerializable { " + member + " }";
        CSharpCompilation input = CreateCompilation("Inno.InvalidSerializationFixture", source, []);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new SerializationMetadataGenerator().AsSourceGenerator());
        driver.RunGeneratorsAndUpdateCompilation(input, out _, out var diagnostics);
        Assert.Contains(diagnostics, diagnostic => diagnostic.Id == expectedDiagnostic && diagnostic.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void ExternalBaseMembersAndHooksUseTheirOwningAssemblyAccessors()
    {
        byte[] parent = Compile("Inno.SerializationParentFixture", """
            using Inno.Core.Serialization;
            namespace Fixture;
            public class Parent : ISerializable
            {
                [SerializableProperty]
                public int score { get; private set; }
                public int hooks { get; private set; }
                [OnSerializableRestored]
                private void Restored() => hooks++;
            }
            """, []);
        byte[] child = Compile("Inno.SerializationChildFixture", """
            namespace Fixture;
            public sealed class Child : Parent { private Child() {} }
            """, [MetadataReference.CreateFromImage(parent)]);
        var lifetime = new AssemblyLoadContext("serialization-inheritance-fixture", isCollectible: true);
        try
        {
            lifetime.LoadFromStream(new MemoryStream(parent));
            Assembly assembly = lifetime.LoadFromStream(new MemoryStream(child));
            Type catalog = assembly.GetType("Inno.SerializationChildFixture.Generated.RuntimeSerializationMetadataCatalog", throwOnError: true)!;
            var register = catalog.GetMethod("Register", BindingFlags.Public | BindingFlags.Static)!
                .CreateDelegate<Action<Action<SerializationTypeMetadata>>>();
            var source = new StaticSerializationMetadataSource([register]);
            Type declaration = assembly.GetType("Fixture.Child", throwOnError: true)!;
            SerializationTypeMetadata metadata = source.GetMetadata(declaration);
            object value = metadata.CreateInstance();
            SerializationMemberMetadata score = Assert.Single(metadata.members);
            score.SetValue(value, 42);
            metadata.restored!(value, SerializationContext.empty);
            Assert.Equal(42, score.GetValue(value));
            Assert.Equal(1, declaration.GetProperty("hooks")!.GetValue(value));
        }
        finally
        {
            lifetime.Unload();
        }
    }

    [Fact]
    public void GeneratedAccessorsRoundTripPrivateMembersHooksStructsAndClosedCollections()
    {
        const string source = """
            using System;
            using System.Collections.Generic;
            using System.Collections.ObjectModel;
            using System.Collections.Immutable;
            using Inno.Core.Serialization;
            namespace Fixture;
            public class Base : ISerializable
            {
                [SerializableProperty]
                private int m_value;
                public int value => m_value;
                public int hooks { get; private set; }
                public void Assign(int value) => m_value = value;
                [OnSerializableRestored]
                private void Restored() => hooks++;
            }
            public struct Score
            {
                public int count;
                public float distance { get; set; }
            }
            public sealed class IntBag : IEnumerable<int>
            {
                private readonly List<int> m_values = [];
                public void Add(int value) => m_values.Add(value);
                public IEnumerator<int> GetEnumerator() => m_values.GetEnumerator();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            }
            public sealed class FactorySequence : IEnumerable<int>
            {
                private readonly int[] m_values;
                private FactorySequence(int[] values) => m_values = values;
                public static FactorySequence CreateRange(List<int> values) => new(values.ToArray());
                public IEnumerator<int> GetEnumerator() => ((IEnumerable<int>)m_values).GetEnumerator();
                System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
            }
            public sealed class State : Base
            {
                private State() { }
                [SerializableProperty]
                public int[][] values { get; private set; } = [];
                [SerializableProperty]
                public Dictionary<string, Score> scores { get; private set; } = [];
                [SerializableProperty]
                public ReadOnlyCollection<int> ordered { get; private set; } = new(new List<int>());
                [SerializableProperty]
                public ImmutableArray<int> fixedValues { get; private set; } = [];
                [SerializableProperty]
                public IntBag bag { get; private set; } = new();
                [SerializableProperty]
                public ISet<int> unique { get; private set; } = new HashSet<int>();
                [SerializableProperty]
                public FactorySequence factory { get; private set; } = FactorySequence.CreateRange([]);
                public static State Create()
                {
                    var value = new State();
                    value.Assign(27);
                    value.values = [[1, 2], [3]];
                    value.scores.Add("best", new Score { count = 42, distance = 2.5f });
                    value.ordered = new(new List<int> { 9, 8 });
                    value.fixedValues = [7, 6];
                    value.bag.Add(15);
                    value.unique.Add(18);
                    value.factory = FactorySequence.CreateRange([24]);
                    return value;
                }
            }
            public static class Scenario
            {
                public static bool Run(SerializationRegistry serialization)
                {
                    State restored = serialization.Deserialize<State>(serialization.Serialize(State.Create()));
                    return restored.value == 27 && restored.hooks == 1 && restored.values[0][1] == 2
                        && restored.values[1][0] == 3 && restored.scores["best"].count == 42
                        && restored.scores["best"].distance == 2.5f && restored.ordered[1] == 8
                        && restored.fixedValues[0] == 7 && System.Linq.Enumerable.Single(restored.bag) == 15
                        && restored.unique.Contains(18) && System.Linq.Enumerable.Single(restored.factory) == 24;
                }
            }
            """;
        string[] references = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")
            ?? throw new InvalidOperationException("The test host supplies no platform references.")).Split(Path.PathSeparator);
        CSharpCompilation input = CSharpCompilation.Create("Inno.SerializationMetadataFixture",
            [CSharpSyntaxTree.ParseText(source)], references.Select(static path => MetadataReference.CreateFromFile(path)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new SerializationMetadataGenerator().AsSourceGenerator());
        driver.RunGeneratorsAndUpdateCompilation(input, out Compilation output, out var diagnostics);
        Assert.Empty(diagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        using var stream = new MemoryStream();
        var emitted = output.Emit(stream);
        Assert.True(emitted.Success, string.Join(Environment.NewLine, emitted.Diagnostics));
        stream.Position = 0;
        var lifetime = new AssemblyLoadContext("serialization-metadata-fixture", isCollectible: true);
        try
        {
            Assembly assembly = lifetime.LoadFromStream(stream);
            Type catalog = assembly.GetType("Inno.SerializationMetadataFixture.Generated.RuntimeSerializationMetadataCatalog", throwOnError: true)!;
            var register = catalog.GetMethod("Register", BindingFlags.Public | BindingFlags.Static)!
                .CreateDelegate<Action<Action<SerializationTypeMetadata>>>();
            var metadata = new StaticSerializationMetadataSource([register]);
            using var modules = new ModuleHost(new ModuleHostOptions
            {
                catalogSource = new DotNetAssemblyCatalogSource(assembly)            });
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            using var serialization = new SerializationRegistry(types, metadata);
            var run = assembly.GetType("Fixture.Scenario", throwOnError: true)!.GetMethod("Run")!
                .CreateDelegate<Func<SerializationRegistry, bool>>();
            Assert.True(run(serialization));
            Assert.Throws<InvalidOperationException>(() => metadata.GetMetadata(typeof(DateTime)));
        }
        finally
        {
            lifetime.Unload();
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PrivateDeclarationsRemainInsidePartialOwners(bool nested)
    {
        string source = """
            using System.Collections.Generic;
            using Inno.Core.Serialization;
            namespace Fixture;
            public static partial class Scenario
            {
                __OPEN__
                private struct Entry
                {
                    public int score;
                }
                private sealed class State : ISerializable
                {
                    [SerializableProperty]
                    public List<Entry> entries { get; set; } = [];
                }
                internal static bool Execute(SerializationRegistry serialization)
                {
                    var value = new State { entries = [new Entry { score = 42 }] };
                    State restored = serialization.Deserialize<State>(serialization.Serialize(value));
                    return restored.entries.Count == 1 && restored.entries[0].score == 42;
                }
                __CLOSE__
                public static bool Run(SerializationRegistry serialization) => __RECEIVER__Execute(serialization);
            }
            """;
        source = source.Replace("__OPEN__", nested ? "private static partial class Owner {" : "")
            .Replace("__CLOSE__", nested ? "}" : "")
            .Replace("__RECEIVER__", nested ? "Owner." : "");
        AssertScenario(source);
    }

    [Fact]
    public void PublicDeclarationCanKeepPrivateSerializedMemberTypes()
    {
        AssertScenario("""
            using Inno.Core.Serialization;
            namespace Fixture;
            public sealed partial class State : ISerializable
            {
                private struct Entry { public int score; }
                [SerializableProperty]
                private Entry[] m_entries = [];
                public void Assign() => m_entries = [new Entry { score = 42 }];
                public int Read() => m_entries[0].score;
            }
            public static class Scenario
            {
                public static bool Run(SerializationRegistry serialization)
                {
                    var value = new State();
                    value.Assign();
                    State restored = serialization.Deserialize<State>(serialization.Serialize(value));
                    return restored.Read() == 42;
                }
            }
            """);
    }

    [Fact]
    public void PrivateDeclarationsRequireAnExplicitPartialOwner()
    {
        CSharpCompilation input = CreateCompilation("Inno.PrivateSerializationFixture", """
            using Inno.Core.Serialization;
            public static class Owner { private sealed class State : ISerializable {} }
            """, []);
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new SerializationMetadataGenerator().AsSourceGenerator());
        driver.RunGeneratorsAndUpdateCompilation(input, out _, out var diagnostics);
        Assert.Contains(diagnostics, static diagnostic => diagnostic.Id == "INNOSER012"
            && diagnostic.Severity == DiagnosticSeverity.Error);
    }

    [Fact]
    public void OpenGenericContainingOwnersDoNotEmitUnboundShapes()
    {
        _ = Compile("Inno.OpenSerializationFixture", """
            using Inno.Core.Serialization;
            public sealed class Owner<T> { private sealed class State : ISerializable { } }
            """, []);
    }

    private static void AssertScenario(string source)
    {
        byte[] bytes = Compile("Inno.PrivateSerializationFixture", source, []);
        var lifetime = new AssemblyLoadContext("private-serialization-fixture", isCollectible: true);
        try
        {
            Assembly assembly = lifetime.LoadFromStream(new MemoryStream(bytes));
            Type catalog = assembly.GetType("Inno.PrivateSerializationFixture.Generated.RuntimeSerializationMetadataCatalog", throwOnError: true)!;
            var register = catalog.GetMethod("Register", BindingFlags.Public | BindingFlags.Static)!
                .CreateDelegate<Action<Action<SerializationTypeMetadata>>>();
            var metadata = new StaticSerializationMetadataSource([register]);
            using var modules = new ModuleHost(new ModuleHostOptions
            {
                catalogSource = new DotNetAssemblyCatalogSource(assembly)            });
            using var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
            using var serialization = new SerializationRegistry(types, metadata);
            var run = assembly.GetType("Fixture.Scenario", throwOnError: true)!.GetMethod("Run")!
                .CreateDelegate<Func<SerializationRegistry, bool>>();
            Assert.True(run(serialization));
        }
        finally
        {
            lifetime.Unload();
        }
    }

    private static CSharpCompilation CreateCompilation(
        string assemblyName,
        string source,
        MetadataReference[] additionalReferences
    ) {
        string[] references = ((string?)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")
            ?? throw new InvalidOperationException("The test host supplies no platform references.")).Split(Path.PathSeparator);
        return CSharpCompilation.Create(assemblyName, [CSharpSyntaxTree.ParseText(source)],
            references.Select(static path => MetadataReference.CreateFromFile(path)).Concat(additionalReferences),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary, nullableContextOptions: NullableContextOptions.Enable));
    }

    private static byte[] Compile(
        string assemblyName,
        string source,
        MetadataReference[] additionalReferences
    ) {
        GeneratorDriver driver = CSharpGeneratorDriver.Create(new SerializationMetadataGenerator().AsSourceGenerator());
        driver.RunGeneratorsAndUpdateCompilation(CreateCompilation(assemblyName, source, additionalReferences),
            out Compilation output, out var diagnostics);
        Assert.Empty(diagnostics.Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error));
        using var stream = new MemoryStream();
        var emitted = output.Emit(stream);
        Assert.True(emitted.Success, string.Join(Environment.NewLine, emitted.Diagnostics));
        return stream.ToArray();
    }
}
