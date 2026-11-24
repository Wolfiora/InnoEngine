using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests : IDisposable
{
    private readonly string m_testRoot = Path.Combine(
        Path.GetTempPath(),
        "InnoSerializationTests",
        Guid.NewGuid().ToString("N")
    );

    private ModuleHost m_modules;
    private TypeCatalog m_types;
    private SerializationRegistry m_serialization;

    public SerializationBehaviorTests()
    {
        m_modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(SerializationBehaviorTests).Assembly)
        });
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
    }

    public void Dispose()
    {
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();
        if (Directory.Exists(m_testRoot))
        {
            Directory.Delete(m_testRoot, recursive: true);
        }
    }

    [Fact]
    public void IdentityRemapperRewritesNestedSerializedValuesWithoutChangingRawBinary()
    {
        Guid oldID = Guid.Parse("ad68b7e0-62aa-47ba-ab94-1b0509d67560");
        Guid newID = Guid.Parse("02809cba-0be5-4ac4-a690-78e116fc26c1");
        byte[] nested = m_serialization.Serialize(new DefaultSample
        {
            identity = oldID,
            name = "plugin::~Samples/Document.rml",
            bytes = [1, 2, 3]
        });
        byte[] source = m_serialization.Serialize(new DefaultSample
        {
            identity = oldID,
            bytes = nested
        });

        byte[] rewritten = SerializedIdentityRemapper.Rewrite(
            source,
            new Dictionary<Guid, Guid> { [oldID] = newID },
            new Dictionary<string, string> { ["plugin::~Samples/Document.rml"] = "plugin-Samples/Document.rml" }
        );
        DefaultSample outer = m_serialization.Deserialize<DefaultSample>(rewritten);
        DefaultSample inner = m_serialization.Deserialize<DefaultSample>(outer.bytes);

        Assert.Equal(newID, outer.identity);
        Assert.Equal(newID, inner.identity);
        Assert.Equal("plugin-Samples/Document.rml", inner.name);
        Assert.Equal(new byte[] { 1, 2, 3 }, inner.bytes);
    }

    [Fact]
    public void PublicOperationsRejectDisposedRegistry()
    {
        m_serialization.Dispose();

        Assert.Throws<ObjectDisposedException>(() => m_serialization.Serialize(new DefaultSample()));

        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
        Assert.NotEmpty(m_serialization.Serialize(new DefaultSample()));
    }

    [Fact]
    public async Task CapturedGenerationRemainsUsableAcrossRefreshAndWorkerContinuation()
    {
        var source = new DefaultSample { count = 73, name = "Pinned" };
        SerializationGeneration generation = m_serialization.CaptureGeneration();

        m_types.Rebuild();
        byte[] bytes = await Task.Run(() => generation.Serialize(source));
        DefaultSample restored = await Task.Run(() => generation.Deserialize<DefaultSample>(bytes));

        Assert.Equal(73, restored.count);
        Assert.Equal("Pinned", restored.name);
        generation.Dispose();
        Assert.Throws<ObjectDisposedException>(() => generation.Serialize(source));
    }

    [Fact]
    public void TypeCatalogConstructorRequiresActiveModuleHost()
    {
        m_serialization.Dispose();
        m_types.Dispose();
        m_modules.Dispose();

        Assert.Throws<InvalidOperationException>(() => new TypeCatalog(m_modules, new ReflectionTypeCatalogSource()));

        m_modules = new ModuleHost(new ModuleHostOptions
        {
            catalogSource = new DotNetAssemblyCatalogSource(typeof(SerializationBehaviorTests).Assembly)
        });
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
        m_serialization = new SerializationRegistry(m_types, new ReflectionSerializationMetadataSource());
    }

    [Fact]
    public void SerializeDeserializeRoundTripsSupportedDefaultValues()
    {
        var source = new DefaultSample
        {
            enabled = true,
            count = 42,
            ratio = 0.75f,
            price = 19.25m,
            name = "Player",
            identity = Guid.Parse("85228e75-032a-4056-a92f-358fc0fdba14"),
            mode = SampleMode.Hard,
            optional = 8,
            bytes = [1, 2, 3],
            numbers = [4, 5, 6],
            values = [7, 8],
            map = new Dictionary<int, string> { [2] = "two", [1] = "one" },
            data = StructSample.Create(11, 12, 13)
        };

        byte[] bytes = m_serialization.Serialize(source);
        DefaultSample restored = m_serialization.Deserialize<DefaultSample>(bytes);

        Assert.True(restored.enabled);
        Assert.Equal(42, restored.count);
        Assert.Equal(0.75f, restored.ratio);
        Assert.Equal(19.25m, restored.price);
        Assert.Equal("Player", restored.name);
        Assert.Equal(source.identity, restored.identity);
        Assert.Equal(SampleMode.Hard, restored.mode);
        Assert.Equal(8, restored.optional);
        Assert.Equal(new byte[] { 1, 2, 3 }, restored.bytes);
        Assert.Equal(new[] { 4, 5, 6 }, restored.numbers);
        Assert.Equal(new[] { 7, 8 }, restored.values);
        Assert.Equal("one", restored.map[1]);
        Assert.Equal("two", restored.map[2]);
        Assert.Equal(11, restored.data.x);
        Assert.Equal(12, restored.data.y);
        Assert.Equal(13, restored.data.hidden);
        Assert.Equal(0, restored.data.readOnly);
        Assert.Equal(24, restored.data.computed);
    }

    [Fact]
    public void TypeRefSerializationPersistsOnlyStableIdentity()
    {
        TypeRef active = m_types.GetTypeRef(typeof(DefaultSample));

        byte[] bytes = m_serialization.Encode(writer => writer.Write("type", active));
        TypeRef restored = m_serialization.Decode(bytes, reader =>
        {
            SerializationReader type = reader.ReadObject("type");
            Assert.True(type.Contains("stableId"));
            Assert.False(type.Contains("runtimeID"));
            Assert.False(type.Contains("isValid"));
            return reader.Read<TypeRef>("type");
        });

        Assert.Equal(active, restored);
        Assert.Equal(0, restored.runtimeID);
        Assert.Equal(active.Resolve(m_types), restored.Resolve(m_types));
    }

    [Fact]
    public void RestoreUpdatesExistingInstance()
    {
        var source = new DefaultSample { count = 77, name = "Restored" };
        var target = new DefaultSample { count = 1, name = "Old" };

        m_serialization.Restore(target, m_serialization.Serialize(source));

        Assert.Equal(77, target.count);
        Assert.Equal("Restored", target.name);
    }

    [Fact]
    public void CollectingPropertyRestoreCollectsInvalidMembersAndPreservesDefaults()
    {
        var previous = new PreviousSchemaSample
        {
            changed = 42,
            compatible = "preserved",
            removed = 9
        };
        IReadOnlyList<SerializationPropertySnapshot> snapshots = m_serialization.CaptureProperties(previous);
        var current = new CurrentSchemaSample();

        SerializationPropertyRestoreResult result = m_serialization.RestoreProperties(
            current,
            snapshots,
            SerializationPropertyRestoreMode.CollectFailures
        );

        Assert.False(result.success);
        Assert.Equal(1, result.restoredCount);
        Assert.Equal(1, result.ignoredCount);
        SerializationPropertyRestoreFailure failure = Assert.Single(result.failures);
        Assert.Equal(nameof(PreviousSchemaSample.changed), failure.name);
        Assert.Equal(typeof(int), failure.previousPropertyType);
        Assert.Equal(typeof(string), failure.currentPropertyType);
        Assert.Equal("default", current.changed);
        Assert.Equal("preserved", current.compatible);
        Assert.Equal(17, current.added);
        Assert.Equal(1, current.restoreCount);
    }

    [Fact]
    public void StrictPropertyRestoreThrowsForAnIncompatibleMember()
    {
        IReadOnlyList<SerializationPropertySnapshot> snapshots = m_serialization.CaptureProperties(
            new PreviousSchemaSample { changed = 42 }
        );
        var current = new CurrentSchemaSample();

        Assert.Throws<InvalidOperationException>(() => m_serialization.RestoreProperties(
            current,
            snapshots,
            SerializationPropertyRestoreMode.Strict
        ));

        Assert.Equal("default", current.changed);
        Assert.Equal(0, current.restoreCount);
    }

    [Fact]
    public void PropertyDataCapturesAndRestoresOnlyTheRequestedMember()
    {
        var source = new DefaultSample
        {
            count = 81,
            name = "Captured"
        };
        byte[] data = m_serialization.CapturePropertyData(source, nameof(DefaultSample.count));
        var target = new DefaultSample
        {
            count = 3,
            name = "Preserved"
        };

        SerializationPropertyRestoreResult result = m_serialization.RestorePropertiesData(target, data);

        Assert.True(result.success);
        Assert.Equal(1, result.restoredCount);
        Assert.Equal(81, target.count);
        Assert.Equal("Preserved", target.name);
    }

    [Fact]
    public void PropertyDataRoundTripsAllPersistentMembersWithoutTheOwningObject()
    {
        var source = new PreviousSchemaSample
        {
            changed = 19,
            compatible = "Value",
            removed = 27
        };
        byte[] data = m_serialization.CapturePropertiesData(source);
        var target = new PreviousSchemaSample();

        SerializationPropertyRestoreResult result = m_serialization.RestorePropertiesData(target, data);

        Assert.True(result.success);
        Assert.Equal(3, result.restoredCount);
        Assert.Equal(19, target.changed);
        Assert.Equal("Value", target.compatible);
        Assert.Equal(27, target.removed);
    }

    [Fact]
    public void PropertyDataRejectsMalformedOrTrailingBytes()
    {
        byte[] data = m_serialization.CapturePropertyData(
            new DefaultSample { count = 12 },
            nameof(DefaultSample.count)
        );
        var trailing = new byte[data.Length + 1];
        data.CopyTo(trailing, 0);

        Assert.Throws<InvalidDataException>(() => m_serialization.RestorePropertiesData(new DefaultSample(), trailing));
        Assert.ThrowsAny<Exception>(() => m_serialization.RestorePropertiesData(new DefaultSample(), [1, 2, 3]));
    }

    [Fact]
    public void DeserializeUsesNonPublicParameterlessConstructor()
    {
        byte[] bytes = m_serialization.Encode(static writer => writer.Write("value", 9));

        PrivateConstructorSample restored = m_serialization.Deserialize<PrivateConstructorSample>(bytes);

        Assert.True(restored.wasConstructed);
        Assert.Equal(9, restored.value);
    }

    [Fact]
    public void DeserializeWithoutParameterlessConstructorRequiresConverter()
    {
        byte[] bytes = m_serialization.Encode(static writer => writer.Write("value", 5));

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Deserialize<MissingConstructorSample>(bytes)
        );

        Assert.Contains(typeof(MissingConstructorSample).FullName!, exception.Message);
        Assert.Contains("parameterless constructor", exception.Message);
    }

    [Fact]
    public void DeserializeUsesConverterWhenNoParameterlessConstructorExists()
    {
        var source = new ConvertedConstructorSample(31);

        ConvertedConstructorSample restored = m_serialization.Deserialize<ConvertedConstructorSample>(
            m_serialization.Serialize(source)
        );

        Assert.Equal(31, restored.value);
    }

    [Fact]
    public void DeserializePreservesConstructorFailureAsInnerException()
    {
        byte[] bytes = m_serialization.Encode(static _ => {});

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Deserialize<ThrowingConstructorSample>(bytes)
        );

        Assert.IsType<ApplicationException>(exception.InnerException);
        Assert.Equal("constructor failure", exception.InnerException!.Message);
    }

    [Fact]
    public void RequiredConverterMissingThrowsClearError()
    {
        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Serialize(new MissingRequiredConverterSample())
        );

        Assert.Contains(typeof(MissingRequiredConverterSample).FullName!, exception.Message);
        Assert.Contains("requires an explicit serialization converter", exception.Message);
    }

    [Fact]
    public void VisibilityControlsPersistenceAndRuntimeMetadataStrictly()
    {
        var source = new VisibilitySample
        {
            shown = 1,
            hidden = 2,
            readOnly = 3,
            transient = 4,
            serializeOnly = 5,
            deserializeOnly = 6
        };

        VisibilitySample restored = m_serialization.Deserialize<VisibilitySample>(m_serialization.Serialize(source));
        Assert.Equal(1, restored.shown);
        Assert.Equal(2, restored.hidden);
        Assert.Equal(3, restored.readOnly);
        Assert.Equal(0, restored.transient);
        Assert.Equal(0, restored.serializeOnly);
        Assert.Equal(0, restored.deserializeOnly);

        IReadOnlyList<SerializedProperty> properties = m_serialization.GetProperties(source);
        Assert.DoesNotContain(properties, property => property.name == nameof(VisibilitySample.hidden));
        SerializedProperty shown = properties.Single(property => property.name == nameof(VisibilitySample.shown));
        Assert.True(shown.canRead);
        Assert.True(shown.canWrite);
        shown.SetValue(10);
        Assert.Equal(10, source.shown);

        SerializedProperty readOnly = properties.Single(property => property.name == nameof(VisibilitySample.readOnly));
        Assert.True(readOnly.canRead);
        Assert.False(readOnly.canWrite);
        Assert.Throws<InvalidOperationException>(() => readOnly.SetValue(12));

        byte[] manual = m_serialization.Encode(static writer =>
        {
            writer.Write(nameof(VisibilitySample.deserializeOnly), 99);
            writer.Write(nameof(VisibilitySample.serializeOnly), 98);
        });
        _ = m_serialization.Decode(manual, reader =>
        {
            reader.RestoreProperties(source);
            return true;
        });
        Assert.Equal(99, source.deserializeOnly);
        Assert.Equal(5, source.serializeOnly);
    }

    [Fact]
    public void MetadataValidationRejectsInvalidMembersAndDuplicateKeys()
    {
        Assert.Throws<InvalidOperationException>(() => m_serialization.GetProperties(new MissingSetterSample()));
        Assert.Throws<InvalidOperationException>(() => m_serialization.GetProperties(new DuplicateKeyDerivedSample()));
    }

    [Fact]
    public void ClassMemberWithoutConverterFailsWithCompletePath()
    {
        var source = new MissingClassHost { child = new MissingChild { value = 1 } };

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Serialize(source)
        );

        Assert.Contains("$.child", exception.Message);
        Assert.Contains("SerializationConverter", exception.Message);
    }

    [Fact]
    public void ClassCollectionElementWithoutConverterFailsWithIndexedPath()
    {
        var source = new MissingClassCollectionHost
        {
            children = [new MissingChild { value = 2 }]
        };

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Serialize(source)
        );

        Assert.Contains("$.children[0]", exception.Message);
    }

    [Fact]
    public void ExplicitConverterRoundTripsNestedClass()
    {
        var source = new ConvertedChildHost
        {
            child = new ConvertedChild { value = 14 }
        };

        ConvertedChildHost restored = m_serialization.Deserialize<ConvertedChildHost>(
            m_serialization.Serialize(source)
        );

        Assert.NotNull(restored.child);
        Assert.Equal(14, restored.child!.value);
    }

    [Fact]
    public void ExplicitConverterCycleFailsWithBothPaths()
    {
        var cycle = new CycleValue();
        cycle.next = cycle;
        var source = new CycleHost { value = cycle };

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Serialize(source)
        );

        Assert.Contains("Serialization cycle detected", exception.Message);
        Assert.Contains("$.value", exception.Message);
        Assert.Contains("$.value.next", exception.Message);
    }

    [Fact]
    public void DeclaredTypeValueOperationsMatchGenericBytesAndPreserveNullableValues()
    {
        byte[] generic = m_serialization.Encode(writer =>
        {
            writer.Write("number", 42);
            writer.Write<int?>("optional", null);
            writer.Write("box", new GenericBox<int>(73));
        });
        byte[] declared = m_serialization.Encode(writer =>
        {
            writer.Write("number", 42, typeof(int));
            writer.Write("optional", null, typeof(int?));
            writer.Write("box", new GenericBox<int>(73), typeof(GenericBox<int>));
        });

        Assert.Equal(generic, declared);
        m_serialization.Decode(declared, reader =>
        {
            Assert.Equal(42, reader.Read("number", typeof(int)));
            Assert.Null(reader.Read("optional", typeof(int?)));
            Assert.Equal(73, Assert.IsType<GenericBox<int>>(reader.Read("box", typeof(GenericBox<int>))).value);
            Assert.Throws<ArgumentNullException>(() => reader.Read("number", null!));
            return true;
        });
        Assert.Throws<ArgumentNullException>(() => m_serialization.Encode(writer => writer.Write("value", 1, null!)));
    }

    [Fact]
    public void MetadataLookupExposesTheCurrentGenerationAndRejectsInvalidOwners()
    {
        SerializationTypeMetadata metadata = m_serialization.GetMetadata(typeof(DefaultSample));
        Assert.Equal(typeof(DefaultSample), metadata.type);
        Assert.Contains(metadata.members, member => member.name == "count" && member.type == typeof(int));
        Assert.Throws<ArgumentNullException>(() => m_serialization.GetMetadata(null!));
        m_serialization.Dispose();
        Assert.Throws<ObjectDisposedException>(() => m_serialization.GetMetadata(typeof(DefaultSample)));
    }

    [Fact]
    public void ConverterSelectionPrefersNearestBaseThenExactType()
    {
        var nearestSource = new NearestHost { item = new NearestLeaf { value = 21 } };
        NearestHost nearest = m_serialization.Deserialize<NearestHost>(m_serialization.Serialize(nearestSource));
        Assert.Equal("mid", nearest.item!.selectedBy);
        Assert.Equal(21, nearest.item.value);

        var exactSource = new ExactHost { item = new ExactLeaf { value = 22 } };
        ExactHost exact = m_serialization.Deserialize<ExactHost>(m_serialization.Serialize(exactSource));
        Assert.Equal("exact", exact.item!.selectedBy);
        Assert.Equal(22, exact.item.value);
    }

    [Fact]
    public void ConverterSelectionRejectsEqualDistanceAmbiguity()
    {
        var source = new AmbiguousHost { item = new AmbiguousValue() };

        InvalidOperationException exception = Assert.Throws<InvalidOperationException>(
            () => m_serialization.Serialize(source)
        );

        Assert.Contains("ambiguous converters", exception.Message);
        Assert.Contains(typeof(AmbiguousValue).FullName!, exception.Message);
    }

    [Fact]
    public void ConverterSelectionRanksVariantInterfacesAsAssignableInterfaces()
    {
        byte[] bytes = m_serialization.Encode(writer => writer.Write("value", new VariantValue("variant")));
        VariantValue restored = m_serialization.Decode(bytes, reader => reader.Read<VariantValue>("value"));
        Assert.Equal("variant", restored.value);
    }

    [Fact]
    public void OpenGenericConverterIsClosedFromRequestedType()
    {
        var source = new GenericBoxHost { box = new GenericBox<int>(45) };

        GenericBoxHost restored = m_serialization.Deserialize<GenericBoxHost>(m_serialization.Serialize(source));

        Assert.NotNull(restored.box);
        Assert.Equal(45, restored.box!.value);
    }

    [Fact]
    public void SerializationContextIsImmutableAndUsesExactTypeKeys()
    {
        var service = new SampleContext();
        SerializationContext original = SerializationContext.empty;
        SerializationContext updated = original.With<ISampleContext>(service);

        Assert.False(original.TryGet<ISampleContext>(out _));
        Assert.True(updated.TryGet<ISampleContext>(out ISampleContext? resolved));
        Assert.Same(service, resolved);
        Assert.False(updated.TryGet<SampleContext>(out _));
        Assert.Same(service, updated.GetRequired<ISampleContext>());
        Assert.Throws<InvalidOperationException>(() => updated.GetRequired<SampleContext>());
    }

    [Fact]
    public void ReaderWriterReportDuplicateMissingAndTypePaths()
    {
        InvalidOperationException duplicate = Assert.Throws<InvalidOperationException>(() =>
            m_serialization.Encode(writer =>
            {
                writer.Write("value", 1);
                writer.Write("value", 2);
            }));
        Assert.Contains("$", duplicate.Message);
        Assert.Contains("value", duplicate.Message);

        byte[] arrayBytes = m_serialization.Encode(writer =>
            writer.WriteObjectArray("items", new[] { 1 }, static (
                _,
                _
            ) => {}));
        InvalidOperationException missing = Assert.Throws<InvalidOperationException>(() =>
            m_serialization.Decode(arrayBytes, static reader =>
                reader.ReadObjectArray("items")[0].Read<int>("missing")));
        Assert.Contains("$.items[0].missing", missing.Message);

        byte[] valueBytes = m_serialization.Encode(static writer => writer.Write("value", "text"));
        InvalidOperationException mismatch = Assert.Throws<InvalidOperationException>(() =>
            m_serialization.Decode(valueBytes, static reader => reader.Read<int>("value")));
        Assert.Contains("$.value", mismatch.Message);

        InvalidOperationException tryReadMismatch = Assert.Throws<InvalidOperationException>(() =>
            m_serialization.Decode(valueBytes, static reader => reader.TryRead<int>("value", out _)));
        Assert.Contains("$.value", tryReadMismatch.Message);
        Assert.False(m_serialization.Decode(valueBytes, static reader => reader.TryRead<int>("missing", out _)));
    }

    [Fact]
    public void ReaderWriterAreInvalidOutsideTheirOperation()
    {
        SerializationWriter? capturedWriter = null;
        _ = m_serialization.Encode(writer => capturedWriter = writer);
        Assert.Throws<InvalidOperationException>(() => capturedWriter!.Write("late", 1));

        byte[] bytes = m_serialization.Encode(static writer => writer.Write("value", 1));
        SerializationReader? capturedReader = null;
        _ = m_serialization.Decode(bytes, reader =>
        {
            capturedReader = reader;
            return true;
        });
        Assert.Throws<InvalidOperationException>(() => capturedReader!.Contains("value"));
    }

    [Fact]
    public void RestoreHooksRunOnceBaseToDerivedForDefaultAndConverterPaths()
    {
        byte[] bytes = m_serialization.Encode(static writer => writer.Write("value", 3));
        var target = new HookDerivedSample();
        _ = m_serialization.Decode(bytes, reader =>
        {
            reader.RestoreProperties(target);
            reader.RestoreProperties(target);
            return true;
        });
        Assert.Equal(new[] { "base", "derived" }, target.calls);

        var converted = new ConvertedHookSample { value = 8 };
        ConvertedHookSample restored = m_serialization.Deserialize<ConvertedHookSample>(
            m_serialization.Serialize(converted)
        );
        Assert.Equal(1, restored.hookCount);

        var existing = new ConvertedHookSample();
        m_serialization.Restore(existing, m_serialization.Serialize(converted));
        Assert.Equal(8, existing.value);
        Assert.Equal(1, existing.hookCount);
    }

    [Fact]
    public void CompletionCallbacksRunOnlyAfterSuccessfulDecode()
    {
        byte[] bytes = m_serialization.Encode(static writer => writer.Write("value", 1));
        List<string> calls = [];
        int value = m_serialization.Decode(bytes, reader =>
        {
            reader.OnCompleted(() => calls.Add("completed"));
            Assert.Empty(calls);
            return reader.Read<int>("value");
        });
        Assert.Equal(1, value);
        Assert.Equal(new[] { "completed" }, calls);

        calls.Clear();
        Assert.Throws<InvalidOperationException>(() => m_serialization.Decode(bytes, reader =>
        {
            reader.OnCompleted(() => calls.Add("should-not-run"));
            return reader.Read<int>("missing");
        }));
        Assert.Empty(calls);
    }

    [Fact]
    public void NonStringMapEncodingIsDeterministic()
    {
        var left = new MapHost { values = new Dictionary<int, string> { [9] = "nine", [1] = "one" } };
        var right = new MapHost { values = new Dictionary<int, string> { [1] = "one", [9] = "nine" } };

        Assert.Equal(m_serialization.Serialize(left), m_serialization.Serialize(right));
    }

    [Fact]
    public void ForeignFormatHeaderIsRejected()
    {
        using var stream = new MemoryStream();
        using (var writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
        {
            writer.Write("FOREIGN");
        }

        InvalidDataException exception = Assert.Throws<InvalidDataException>(
            () => m_serialization.Decode(stream.ToArray(), static _ => true)
        );
        Assert.Contains("Invalid serialization magic", exception.Message);
    }
}

internal enum SampleMode
{
    Easy,
    Hard
}
