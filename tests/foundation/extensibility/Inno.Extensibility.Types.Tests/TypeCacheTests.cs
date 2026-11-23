using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using ExternalNamespace;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Types.Tests;

[Collection(TypeCacheCollection.C_NAME)]
public sealed class TypeCacheTests : IDisposable
{
    private readonly string m_cacheDirectory = Path.Combine(
        Path.GetTempPath(),
        "InnoTypeCacheTests",
        Guid.NewGuid().ToString("N")
    );

    private readonly ModuleHost m_modules;
    private readonly TypeCatalog m_types;

    public TypeCacheTests()
    {
        m_modules = new ModuleHost(
            new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(TypeCacheTests).Assembly)}
        );
        m_types = new TypeCatalog(m_modules, new ReflectionTypeCatalogSource());
    }

    public void Dispose()
    {
        m_types.Dispose();
        m_modules.Dispose();
        if (Directory.Exists(m_cacheDirectory))
        {
            Directory.Delete(m_cacheDirectory, recursive: true);
        }
    }

    [Fact]
    public void CandidateConstructionRollbackReleasesAllNewResourcesAndKeepsLastGood()
    {
        using var registry = new CandidateOwnershipRegistry(m_types);
        registry.Refresh();
        registry.failBuild = true;
        Assert.Throws<InvalidOperationException>(m_types.Rebuild);
        Assert.Equal(new[] { 4, 3 }, registry.released);
        Assert.Equal(GenerationState.Ready, m_modules.generations.state);
    }

    [Fact]
    public void CandidateRollbackCleanupFailureFaultsGateAfterEveryResourceIsAttempted()
    {
        using var registry = new CandidateOwnershipRegistry(m_types);
        registry.Refresh();
        registry.failBuild = true;
        registry.failCleanup = true;
        try
        {
            Assert.ThrowsAny<Exception>(m_types.Rebuild);
            Assert.Equal(new[] { 4, 3 }, registry.released);
            Assert.Equal(GenerationState.Faulted, m_modules.generations.state);
        }
        finally
        {
            registry.failCleanup = false;
        }
    }

    [Fact]
    public void QueriesReturnOnlyConcreteMatchingTypes()
    {
        Assert.Contains(m_types.GetTypeRef(typeof(TestDerived)), m_types.GetSubTypesOf<TestBase>());
        Assert.DoesNotContain(m_types.GetTypeRef(typeof(TestAbstractDerived)), m_types.GetSubTypesOf<TestBase>());
        Assert.Contains(m_types.GetTypeRef(typeof(TestContractImpl)), m_types.GetTypesImplementing<ITestContract>());
        Assert.DoesNotContain(
            m_types.GetTypeRef(typeof(TestAbstractContractImpl)),
            m_types.GetTypesImplementing<ITestContract>()
        );
        Assert.Contains(
            m_types.GetTypeRef(typeof(AttributedType)),
            m_types.GetTypesWithAttribute<TestMarkerAttribute>()
        );
        Assert.DoesNotContain(
            m_types.GetTypeRef(typeof(AbstractAttributedType)),
            m_types.GetTypesWithAttribute<TestMarkerAttribute>()
        );
    }

    [Fact]
    public void StableAndRuntimeIdentitySurviveOrdinaryRebuild()
    {
        TypeRef first = m_types.GetTypeRef(typeof(TestDerived));
        TypeCacheSnapshot previous = m_types.current;

        m_types.Rebuild();

        TypeRef second = m_types.GetTypeRef(typeof(TestDerived));
        Assert.Equal(typeof(TestDerived), first.Resolve(m_types));
        Assert.Equal(first.stableID, second.stableID);
        Assert.Equal(first.runtimeID, second.runtimeID);
        Assert.True(m_types.current.version > previous.version);
    }

    [Fact]
    public void ExplicitAndGeneratedStableIdentitiesAreCanonical()
    {
        Assert.Equal(
            Guid.Parse("11111111-1111-1111-1111-111111111111"),
            m_types.GetTypeRef(typeof(StableAnnotatedTypeA)).stableID
        );
        Assert.Equal(
            Guid.Parse("44444444-4444-4444-4444-444444444444"),
            m_types.GetTypeRef(typeof(GeneratedMappedType)).stableID
        );

        Guid first = m_types.GetTypeRef(typeof(DeterministicStableType)).stableID;
        m_types.Rebuild();
        Assert.Equal(first, m_types.GetTypeRef(typeof(DeterministicStableType)).stableID);
    }

    [Fact]
    public void DiscoveryIncludesTypesOutsideInnoNamespace()
    {
        Assert.True(m_types.GetTypeRef(typeof(OutsideNamespaceType)).runtimeID > 0);
        Assert.Equal(
            Guid.Parse("33333333-3333-3333-3333-333333333333"),
            m_types.GetTypeRef(typeof(OutsideStableAnnotatedType)).stableID
        );
    }

    [Fact]
    public void NewlyLoadedHostAssemblyAppearsAfterCatalogRefresh()
    {
        Assembly assembly = Assembly.Load("Inno.Extensibility.Types.TestAssemblyA");
        Type marked = assembly.GetType(
            "Inno.Extensibility.Types.TestAssets.A.AssemblyAMarkedType",
            throwOnError: true
        )!;

        m_types.Rebuild();

        Assert.Contains(m_types.current.types, type => type.Resolve(m_types) == marked);
    }

    [Fact]
    public void UnknownAndEmptyReferencesFailAgainstExplicitCatalog()
    {
        TypeRef empty = default;
        var unknown = new TypeRef(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

        Assert.False(empty.IsValid(m_types));
        Assert.False(unknown.IsValid(m_types));
        Assert.Throws<InvalidOperationException>(() => empty.Resolve(m_types));
        Assert.Throws<InvalidOperationException>(() => unknown.Resolve(m_types));
        Assert.Throws<ArgumentNullException>(() => m_types.TryGetTypeRef(null!, out _));
    }

    [Fact]
    public void CandidateActivationFailureRestoresEveryRegistrySnapshot()
    {
        using var first = new TransactionalTestRegistry(m_types);
        using var second = new TransactionalTestRegistry(m_types);
        first.Refresh();
        second.Refresh();
        int firstPrevious = first.snapshotID;
        int secondPrevious = second.snapshotID;
        second.failActivation = true;

        Assert.Throws<InvalidOperationException>(m_types.Rebuild);

        Assert.Equal(firstPrevious, first.snapshotID);
        Assert.Equal(secondPrevious, second.snapshotID);
        Assert.Equal(1, first.rollbackCount);
        Assert.Equal(1, second.rollbackCount);
        Assert.Contains(firstPrevious + 1, first.disposedSnapshotIDs);
        Assert.Contains(secondPrevious + 1, second.disposedSnapshotIDs);
    }

    [Fact]
    public void SnapshotCleanupFailureKeepsCommittedCandidateButFaultsGeneration()
    {
        using var registry = new TransactionalTestRegistry(m_types);
        registry.Refresh();
        int previous = registry.snapshotID;
        registry.failSnapshotCleanup = true;

        Assert.Throws<AggregateException>(m_types.Rebuild);

        Assert.Equal(previous + 1, registry.snapshotID);
        Assert.Equal(0, registry.rollbackCount);
        Assert.Equal(1, registry.cleanupFailureCount);
        Assert.Contains(previous, registry.disposedSnapshotIDs);
        Assert.Equal(GenerationState.Faulted, m_modules.generations.state);
        Assert.Throws<InvalidOperationException>(() => m_modules.generations.AcquireRead("build"));
        registry.failSnapshotCleanup = false;
    }

    [Fact]
    public void RegistryRetiredAfterCandidatePreparationIsExcludedFromActivation()
    {
        using var retiring = new TransactionalTestRegistry(m_types);
        using var survivor = new TransactionalTestRegistry(m_types);
        retiring.Refresh();
        survivor.Refresh();
        int retiringPrevious = retiring.snapshotID;
        int survivorPrevious = survivor.snapshotID;
        retiring.disposeOnActivation = survivor;

        m_types.Rebuild();

        Assert.Equal(retiringPrevious + 1, retiring.snapshotID);
        Assert.Contains(survivorPrevious, survivor.disposedSnapshotIDs);
        Assert.Contains(survivorPrevious + 1, survivor.disposedSnapshotIDs);
    }

    [Fact]
    public void DisposedCatalogRejectsQueriesWithoutAStaticFallback()
    {
        m_types.Dispose();

        Assert.Throws<InvalidOperationException>(() => _ = m_types.current);
        Assert.Throws<InvalidOperationException>(() => m_types.GetSubTypesOf<TestBase>());
    }

    private sealed class CandidateOwnershipRegistry(TypeCatalog types) : TypeRegistry<object[]>(types)
    {
        private int m_nextID;
        internal bool failBuild;
        internal bool failCleanup;
        internal List<int> released { get; } = [];

        protected override object[] Build(TypeCacheSnapshot types)
        {
            Resource first = OwnCandidateExtension(new Resource(this, ++m_nextID));
            Resource second = OwnCandidateExtension(new Resource(this, ++m_nextID));
            if (failBuild)
            {
                throw new InvalidOperationException("Expected candidate validation failure.");
            }
            return [first, second, first];
        }

        protected override void DisposeSnapshot(object[] snapshot) => DisposeExtensions(snapshot);

        private sealed class Resource(
            CandidateOwnershipRegistry owner,
            int id
        ) : IDisposable
        {
            public void Dispose()
            {
                owner.released.Add(id);
                if (owner.failCleanup)
                {
                    throw new InvalidOperationException("Expected extension cleanup failure.");
                }
            }
        }
    }

    private sealed class TransactionalTestRegistry(TypeCatalog types) : TypeRegistry<TransactionalTestRegistry.Snapshot>(types)
    {
        private int m_nextSnapshotID;

        internal bool failActivation;
        internal bool failSnapshotCleanup;
        internal IDisposable? disposeOnActivation;
        internal int rollbackCount;
        internal int cleanupFailureCount;
        internal int snapshotID;
        internal List<int> disposedSnapshotIDs { get; } = [];

        protected override Snapshot Build(TypeCacheSnapshot types) => new(++m_nextSnapshotID);

        protected override void OnActivating(
            Snapshot? previous,
            Snapshot candidate
        ) {
            snapshotID = candidate.id;
            disposeOnActivation?.Dispose();
            disposeOnActivation = null;
            if (failActivation)
            {
                throw new InvalidOperationException("Injected registry activation failure.");
            }
        }

        protected override void OnActivationRolledBack(
            Snapshot? previous,
            Snapshot candidate
        ) {
            snapshotID = previous?.id ?? 0;
            rollbackCount++;
        }

        protected override void DisposeSnapshot(Snapshot snapshot)
        {
            disposedSnapshotIDs.Add(snapshot.id);
            if (failSnapshotCleanup)
            {
                throw new InvalidOperationException("Injected snapshot cleanup failure.");
            }
        }

        protected override void OnCleanupFailed(
            string phase,
            Exception exception
        ) => cleanupFailureCount++;

        internal sealed record Snapshot(int id);
    }
}
