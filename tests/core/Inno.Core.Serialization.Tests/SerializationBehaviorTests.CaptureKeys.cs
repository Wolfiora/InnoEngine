using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Identity;
using Inno.Core.Serialization.Converters;
using Xunit;

namespace Inno.Core.Serialization.Tests;

public sealed partial class SerializationBehaviorTests
{
    [Fact]
    public void DeclaredCaptureKeysReuseOnlyCompleteCurrentInputsAndNeverChangeEncodedBytes()
    {
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot { number = 4, text = "text" };
        Assert.True(identities.Register(value));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        byte[] bytes = before.ToArray();
        Assert.Equal(m_serialization.Serialize(value), bytes);
        int writes = value.writeCalls;
        Assert.Same(before, m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(writes, value.writeCalls);

        value.Touch();
        value.number = 9;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.NotSame(before, changed);
        Assert.Equal(m_serialization.Serialize(value), changed.ToArray());
        Assert.Equal(bytes, before.ToArray());
        Assert.Same(changed, m_serialization.CaptureSnapshot(value, previous: changed));

        int keys = value.keyCalls;
        _ = m_serialization.Serialize(value);
        Assert.Equal(keys, value.keyCalls);
    }

    [Fact]
    public void AParentKeyCannotImplicitlyCertifyANestedConverterState()
    {
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot { converted = new CaptureConvertedValue { number = 4 } };
        Assert.True(identities.Register(value));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        int calls = CaptureConvertedValueConverter.calls;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(calls + 1, CaptureConvertedValueConverter.calls);
        Assert.NotEqual(before.ToArray(), changed.ToArray());
        Assert.Equal(2, value.writeCalls);
    }

    [Fact]
    public void CaptureKeysIncludeContextInputsAndCurrentConverterGeneration()
    {
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot();
        Assert.True(identities.Register(value));
        var dependency = new CaptureKeyContext { value = 1 };
        SerializationContext context = SerializationContext.empty.With(dependency);
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value, context);
        Assert.Same(before, m_serialization.CaptureSnapshot(value, context, before));
        dependency.value = 2;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, context, before);
        Assert.Equal(m_serialization.Serialize(value, context), changed.ToArray());
        Assert.NotEqual(before.ToArray(), changed.ToArray());

        int writes = value.writeCalls;
        m_types.Rebuild();
        SerializationSnapshot refreshed = m_serialization.CaptureSnapshot(value, context, changed);
        Assert.True(value.writeCalls > writes);
        Assert.Equal(changed.ToArray(), refreshed.ToArray());
        Assert.Same(refreshed, m_serialization.CaptureSnapshot(value, context, refreshed));
    }

    [Fact]
    public void CaptureKeysQualifyIdentityByDomainAndRejectRetiredBindings()
    {
        var first = new IdentityAllocator();
        var second = new IdentityAllocator();
        var original = new CaptureKeyRoot();
        var replacement = new CaptureKeyRoot();
        Assert.True(first.Register(original));
        Assert.True(second.Register(replacement, original.identity.persistentId));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(original);
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(replacement, previous: before);
        Assert.NotSame(before, changed);
        Assert.Equal(before.ToArray(), changed.ToArray());
        Assert.Equal(1, replacement.writeCalls);

        Assert.True(second.Unregister(replacement));
        Assert.True(second.Register(replacement));
        int writes = replacement.writeCalls;
        SerializationSnapshot rebound = m_serialization.CaptureSnapshot(replacement, previous: changed);
        Assert.True(replacement.writeCalls > writes);
        Assert.NotSame(changed, rebound);
        Assert.Equal(changed.ToArray(), rebound.ToArray());
    }

    [Fact]
    public void DeclinedAndUnavailableKeysCannotPreserveAnEarlierHint()
    {
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot();
        Assert.True(identities.Register(value));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        value.allowKey = false;
        SerializationSnapshot declined = m_serialization.CaptureSnapshot(value, previous: before);
        int writes = value.writeCalls;
        _ = m_serialization.CaptureSnapshot(value, previous: declined);
        Assert.Equal(writes + 1, value.writeCalls);
        Assert.Equal(before.ToArray(), declined.ToArray());

        value.allowKey = true;
        value.revision = null;
        value.ignoreDecline = true;
        value.number = 7;
        SerializationSnapshot unavailable = m_serialization.CaptureSnapshot(value, previous: declined);
        Assert.Equal(m_serialization.Serialize(value), unavailable.ToArray());
        writes = value.writeCalls;
        _ = m_serialization.CaptureSnapshot(value, previous: unavailable);
        Assert.Equal(writes + 1, value.writeCalls);
    }

    [Fact]
    public void UntrackedDirectPropertyWritesRemainAuthoritativeEvenWhenAProducerIgnoresDecline()
    {
        var identities = new IdentityAllocator();
        var properties = new UntrackedCaptureProperties();
        var value = new CaptureKeyRoot { properties = properties, ignoreDecline = true };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register(properties));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        properties.number = 27;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(m_serialization.Serialize(value), changed.ToArray());
        Assert.NotEqual(before.ToArray(), changed.ToArray());
        int writes = value.writeCalls;
        _ = m_serialization.CaptureSnapshot(value, previous: changed);
        Assert.Equal(writes + 1, value.writeCalls);
    }

    [Fact]
    public void ProvenPropertyRevisionsInvalidateWithoutPersistingTheirCounter()
    {
        var identities = new IdentityAllocator();
        var properties = new CaptureRevisionProperties { number = 3 };
        var value = new CaptureKeyRoot { properties = properties };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register(properties));
        _ = m_serialization.CapturePropertiesData(properties);
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        int writes = value.writeCalls;
        Assert.Same(before, m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(writes, value.writeCalls);

        properties.number = 12;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.NotEqual(before.ToArray(), changed.ToArray());
        Assert.Equal(m_serialization.Serialize(value), changed.ToArray());
        Assert.Single(m_serialization.CaptureProperties(properties));
        Assert.Equal("number", m_serialization.CaptureProperties(properties)[0].name);
    }

    [Fact]
    public void CaptureKeyQueriesDoNotConstructOrSkipStatefulConverters()
    {
        CaptureConvertedValueConverter.events.Clear();
        CaptureConvertedValueConverter.calls = 0;
        var identities = new IdentityAllocator();
        var properties = new ConvertedCaptureProperties();
        var value = new CaptureKeyRoot
        {
            properties = properties,
            onWrite = () => CaptureConvertedValueConverter.events.Add("root-write")
        };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register(properties));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        Assert.Equal(new[] { "root-write", "construct", "convert-write" }, CaptureConvertedValueConverter.events);
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(2, CaptureConvertedValueConverter.calls);
        Assert.NotEqual(before.ToArray(), changed.ToArray());
    }

    [Fact]
    public void PureFieldMetadataPreservesExactFloatBitsAcrossTrackedPropertyChanges()
    {
        var identities = new IdentityAllocator();
        var properties = new PureFieldCaptureProperties { value = new PureCaptureFields { number = -0.0f } };
        var value = new CaptureKeyRoot { properties = properties };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register(properties));
        _ = m_serialization.CapturePropertiesData(properties);
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        Assert.Same(before, m_serialization.CaptureSnapshot(value, previous: before));
        properties.value = new PureCaptureFields { number = 0.0f };
        SerializationSnapshot zero = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.NotEqual(before.ToArray(), zero.ToArray());
        foreach (int bits in new[] { 0x7fc00001, 0x7fc00002 })
        {
            properties.value = new PureCaptureFields { number = BitConverter.Int32BitsToSingle(bits) };
            SerializationSnapshot changed = m_serialization.CaptureSnapshot(value, previous: zero);
            Assert.NotEqual(zero.ToArray(), changed.ToArray());
            Assert.Equal(m_serialization.Serialize(value), changed.ToArray());
            zero = changed;
        }
    }

    [Fact]
    public void NestedPropertyGettersCannotBeMistakenForPureValueFields()
    {
        CaptureGetterStruct.calls = 0;
        var identities = new IdentityAllocator();
        var properties = new GetterCaptureProperties();
        var value = new CaptureKeyRoot { properties = properties };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register(properties));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        int calls = CaptureGetterStruct.calls;
        int writes = value.writeCalls;
        _ = m_serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(calls + 1, CaptureGetterStruct.calls);
        Assert.Equal(writes + 1, value.writeCalls);
    }

    [Fact]
    public void CaptureKeyFailureAndMutationNeverPublishOverThePreviousSnapshot()
    {
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot { number = 2 };
        Assert.True(identities.Register(value));
        SerializationSnapshot before = m_serialization.CaptureSnapshot(value);
        byte[] bytes = before.ToArray();
        value.failKey = true;
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Throws<InvalidOperationException>(() => value.savedBuilder!.AddValue(1));
        value.failKey = false;
        value.Touch();
        value.failWrite = true;
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value, previous: before));
        value.failWrite = false;
        value.onWrite = value.Touch;
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value, previous: before));
        Assert.Equal(bytes, before.ToArray());
        value.onWrite = null;
        Assert.Equal(m_serialization.Serialize(value), m_serialization.CaptureSnapshot(value, previous: before).ToArray());
    }

    [Fact]
    public void CaptureKeysValidateEveryBorrowedIdentityAfterTheWrite()
    {
        var identities = new IdentityAllocator();
        var properties = new CaptureRevisionProperties();
        var value = new CaptureKeyRoot { properties = properties };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register(properties));
        _ = m_serialization.CapturePropertiesData(properties);
        value.onWrite = () => Assert.True(identities.Unregister(properties));
        Assert.Throws<InvalidOperationException>(() => m_serialization.CaptureSnapshot(value));
        Assert.Throws<InvalidOperationException>(() => value.savedBuilder!.context);
        value.onWrite = null;
        Assert.True(identities.Register(properties));
        Assert.NotEmpty(m_serialization.CaptureSnapshot(value).ToArray());
    }

    [Fact]
    public void CaptureKeyBuildersRejectAnotherThreadAndRetireAfterEveryCallback()
    {
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot
        {
            onKey = key => Task.Run(() =>
                Assert.Throws<InvalidOperationException>(() => key.AddText("wrong thread"))).GetAwaiter().GetResult()
        };
        Assert.True(identities.Register(value));
        _ = m_serialization.CaptureSnapshot(value);
        SerializationCaptureKeyBuilder builder = value.savedBuilder!;
        Assert.Throws<InvalidOperationException>(() => builder.context);
        Assert.Throws<InvalidOperationException>(() => builder.EnsureCapacity(1));
        Assert.Throws<InvalidOperationException>(() => builder.TryAddOwner(value, 0));
    }

    [Fact]
    public void EquivalentFreshMetadataDeclinesHintsWithoutBreakingOrdinaryCapture()
    {
        using var serialization = new SerializationRegistry(m_types, new FreshCaptureMetadataSource());
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot();
        Assert.True(identities.Register(value));
        SerializationSnapshot before = serialization.CaptureSnapshot(value);
        int writes = value.writeCalls;
        SerializationSnapshot after = serialization.CaptureSnapshot(value, previous: before);
        Assert.Equal(writes + 1, value.writeCalls);
        Assert.Equal(before.ToArray(), after.ToArray());
        Assert.Equal(serialization.Serialize(value), after.ToArray());
    }

    [Fact]
    public void MetadataRevisionDeclarationsRejectCountersInTheSerializedPayload()
    {
        var metadata = new ReflectionSerializationMetadataSource();
        Assert.Throws<InvalidOperationException>(() => metadata.GetMetadata(typeof(SerializedCaptureCounter)));
    }

    [Fact]
    public void RetainedCaptureKeysAndRetiredBuildersDoNotPinACollectiblePropertyAssembly()
    {
        (SerializationSnapshot snapshot, SerializationCaptureKeyBuilder builder, WeakReference assembly, WeakReference context) =
            CaptureCollectibleProperties();
        for (int index = 0; index < 10 && (assembly.IsAlive || context.IsAlive); index++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
        Assert.False(assembly.IsAlive);
        Assert.False(context.IsAlive);
        Assert.Throws<InvalidOperationException>(() => builder.context);
        Assert.NotEmpty(snapshot.ToArray());
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private (SerializationSnapshot, SerializationCaptureKeyBuilder, WeakReference, WeakReference) CaptureCollectibleProperties()
    {
        var context = new CapturePropertyLoadContext();
        var assembly = context.LoadFromAssemblyPath(typeof(CaptureRevisionProperties).Assembly.Location);
        Type type = assembly.GetType(typeof(CaptureRevisionProperties).FullName!, throwOnError: true)!;
        var properties = (ISerializable)Activator.CreateInstance(type)!;
        var identities = new IdentityAllocator();
        var value = new CaptureKeyRoot { properties = properties };
        Assert.True(identities.Register(value));
        Assert.True(identities.Register((IdentityObject)properties));
        _ = m_serialization.CapturePropertiesData(properties);
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(value);
        Assert.Same(snapshot, m_serialization.CaptureSnapshot(value, previous: snapshot));
        SerializationCaptureKeyBuilder builder = value.savedBuilder!;
        Assert.True(identities.Unregister((IdentityObject)properties));
        Assert.True(identities.Unregister(value));
        value.properties = null;
        var assemblyReference = new WeakReference(assembly);
        var contextReference = new WeakReference(context);
        context.Unload();
        return (snapshot, builder, assemblyReference, contextReference);
    }
}

internal sealed class CaptureKeyRoot : IdentityObject, ISerializable
{
    internal ulong? revision = 0;
    internal int number;
    internal string? text;
    internal bool allowKey = true;
    internal bool ignoreDecline;
    internal bool failKey;
    internal bool failWrite;
    internal int writeCalls;
    internal int keyCalls;
    internal ISerializable? properties;
    internal CaptureConvertedValue? converted;
    internal Action? onWrite;
    internal Action<SerializationCaptureKeyBuilder>? onKey;
    internal SerializationCaptureKeyBuilder? savedBuilder;

    internal void Touch() => revision++;
}

internal sealed class CaptureKeyContext
{
    internal ulong value;
}

internal sealed class CaptureKeyRootConverter : SerializationConverter<CaptureKeyRoot>
{
    public override void Write(
        SerializationWriter writer,
        CaptureKeyRoot value
    ) {
        value.writeCalls++;
        value.onWrite?.Invoke();
        if (value.failWrite)
            throw new InvalidOperationException("Capture writer failure.");
        writer.Write("number", value.number);
        writer.Write("text", value.text);
        writer.Write("context", writer.context.TryGet(out CaptureKeyContext? context) ? context!.value : 0UL);
        if (value.properties is not null)
            writer.WritePropertiesData("properties", value.properties);
        if (value.converted.HasValue)
            writer.Write("converted", value.converted.Value);
    }

    public override CaptureKeyRoot Read(SerializationReader reader) => throw new NotSupportedException();

    protected override bool TryCaptureKey(
        CaptureKeyRoot value,
        SerializationCaptureKeyBuilder key
    ) {
        value.keyCalls++;
        value.savedBuilder = key;
        if (value.failKey)
            throw new InvalidOperationException("Capture key failure.");
        if (!value.allowKey)
            return false;
        key.EnsureCapacity(5);
        bool complete = key.TryAddOwner(value, value.revision);
        key.AddValue(key.context.TryGet(out CaptureKeyContext? context) ? context!.value : 0UL);
        if (value.properties is not null)
            complete &= key.TryAddProperties(value.properties);
        value.onKey?.Invoke(key);
        return complete || value.ignoreDecline;
    }
}

/// <summary>
/// Provides a real public extension type for collectible metadata and capture-key acceptance.
/// </summary>
[SerializationRevision("m_revision")]
public sealed class CaptureRevisionProperties : IdentityObject, ISerializable
{
    private ulong? m_revision = 0;
    private int m_number;

    /// <summary>
    /// Creates an initially empty tracked property source.
    /// </summary>
    public CaptureRevisionProperties() { }

    /// <summary>
    /// Gets or sets the serialized number, advancing its complete source-owned revision before mutation.
    /// </summary>
    [SerializableProperty]
    public int number
    {
        get => m_number;
        set
        {
            if (m_revision is { } revision)
                m_revision = revision == ulong.MaxValue ? null : revision + 1;
            m_number = value;
        }
    }
}

internal sealed class UntrackedCaptureProperties : IdentityObject, ISerializable
{
    [SerializableProperty] public int number;
}

[SerializationRevision("revision")]
internal sealed class ConvertedCaptureProperties : IdentityObject, ISerializable
{
    private ulong? revision => 0;
    [SerializableProperty] public CaptureConvertedValue value = new() { number = 1 };
}

[SerializationRevision("m_revision")]
internal sealed class PureFieldCaptureProperties : IdentityObject, ISerializable
{
    private ulong? m_revision = 0;
    private PureCaptureFields m_value;

    [SerializableProperty]
    public PureCaptureFields value
    {
        get => m_value;
        set
        {
            m_revision++;
            m_value = value;
        }
    }
}

internal struct PureCaptureFields
{
    public float number;
}

[SerializationRevision("revision")]
internal sealed class GetterCaptureProperties : IdentityObject, ISerializable
{
    private ulong? revision => 0;
    [SerializableProperty] public CaptureGetterStruct value = new();
}

internal struct CaptureGetterStruct
{
    internal static int calls;

    public int value
    {
        get
        {
            calls++;
            return 1;
        }
        set { }
    }
}

internal struct CaptureConvertedValue
{
    public int number;
}

internal sealed class CaptureConvertedValueConverter : SerializationConverter<CaptureConvertedValue>
{
    internal static readonly List<string> events = [];
    internal static int calls;

    public CaptureConvertedValueConverter() => events.Add("construct");

    public override void Write(
        SerializationWriter writer,
        CaptureConvertedValue value
    ) {
        events.Add("convert-write");
        writer.Write("number", value.number);
        writer.Write("call", ++calls);
    }

    public override CaptureConvertedValue Read(SerializationReader reader) => new() { number = reader.Read<int>("number") };
}

[SerializationRevision("revision")]
internal sealed class SerializedCaptureCounter : IdentityObject, ISerializable
{
    [SerializableProperty(PropertyVisibility.Serialize)] public ulong revision => 0;
}

internal sealed class FreshCaptureMetadataSource : ISerializationMetadataSource
{
    private readonly ReflectionSerializationMetadataSource m_source = new();

    public SerializationTypeMetadata GetMetadata(Type type)
    {
        SerializationTypeMetadata original = m_source.GetMetadata(type);
        return new(type, original.members, original.CreateInstance, original.restored, original.collection, original.requiresConverter);
    }
}

internal sealed class CapturePropertyLoadContext() : AssemblyLoadContext(isCollectible: true)
{
    protected override System.Reflection.Assembly? Load(System.Reflection.AssemblyName name)
        => Default.Assemblies.FirstOrDefault(assembly => assembly.GetName().Name == name.Name);
}
