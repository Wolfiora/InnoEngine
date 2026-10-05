using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

using Inno.Core.Collections;
using Inno.Core.Execution;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Owns the active managed assembly catalog and transactional module generations.
/// </summary>
public sealed class ModuleHost : IDisposable
{
    private readonly object m_sync = new();
    private readonly AssemblyCatalogCoordinator m_catalogs = new();
    private readonly Dictionary<AssemblyModuleHandle, AssemblyModuleEntry> m_modules = [];

    private readonly ModuleHostOptions m_options;
    private AssemblyCatalogSnapshot m_currentCatalog = new(0, []);
    private long m_catalogVersion;
    private volatile bool m_hostCatalogDirty;
    private bool m_catalogTransitionInProgress;
    private bool m_rebuildPending;
    private bool m_reloadInProgress;
    private bool m_assemblyLoadSubscribed;
    private Exception? m_retirementFailure;
    private object? m_retainedRetirement;

    /// <summary>
    /// Gets whether this module host can accept catalog operations.
    /// </summary>
    public bool isInitialized { get; private set; }

    /// <summary>
    /// Gets the admission gate and retirement owner shared by all module and host generation changes.
    /// </summary>
    public GenerationCoordinator generations { get; } = new();

    /// <summary>
    /// Gets non-owning information about active managed and external modules.
    /// </summary>
    public IReadOnlyList<AssemblyModuleInfo> modules
    {
        get
        {
            lock (m_sync)
            {
                return m_modules.Values
                    .OrderBy(static module => module.moduleName, StringComparer.Ordinal)
                    .Select(static module => module.CreateInfo())
                    .ToArray();
            }
        }
    }

    /// <summary>
    /// Creates a module host, discovers assemblies in its owning load context, and publishes the first catalog.
    /// </summary>
    /// <param name="options">
    /// The validated configuration that controls this operation.
    /// </param>
    public ModuleHost(ModuleHostOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        if (string.IsNullOrWhiteSpace(options.cacheDirectory))
            throw new ArgumentException("Assembly cache directory is required.", nameof(options));

        lock (m_sync)
        {
            m_options = new ModuleHostOptions
            {
                cacheDirectory = Path.GetFullPath(options.cacheDirectory),
                catalogSource = options.catalogSource
            };
            ArgumentNullException.ThrowIfNull(m_options.catalogSource);
            m_options.catalogSource.changed += OnAssemblyLoaded;
            m_assemblyLoadSubscribed = true;
            isInitialized = true;
            m_hostCatalogDirty = true;
            try
            {
                RebuildLocked();
            }
            catch
            {
                ShutdownLocked();
                throw;
            }
        }
    }

    /// <summary>
    /// Registers a transactional consumer of the assembly catalog and initializes it from the
    /// currently active generation.
    /// </summary>
    /// <param name="participant">
    /// The participant that derives state from catalog snapshots.
    /// </param>
    /// <returns>
    /// A registration that removes the participant when disposed.
    /// </returns>
    public IDisposable RegisterCatalogParticipant(IAssemblyCatalogParticipant participant)
    {
        ArgumentNullException.ThrowIfNull(participant);
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            CatalogParticipantRegistration registration = m_catalogs.Register(participant);
            IAssemblyCatalogTransaction? transaction = null;
            try
            {
                transaction = participant.Prepare(m_currentCatalog);
                transaction.Activate();
                try
                {
                    transaction.Complete();
                }
                catch (Exception exception)
                {
                    generations.Fault(exception);
                    Trace.TraceError(
                        "Assembly catalog participant '{0}' failed during initial cleanup: {1}",
                        participant.GetType().FullName,
                        exception);
                    throw;
                }
                return registration;
            }
            catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
            {
                RetainFailedRetirement(failure, (participant, transaction, registration));
                throw;
            }
            catch
            {
                try
                {
                    transaction?.Rollback();
                }
                catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
                {
                    RetainFailedRetirement(failure, (participant, transaction, registration));
                    throw;
                }
                catch (Exception exception)
                {
                    generations.Fault(exception);
                    Trace.TraceError(
                        "Assembly catalog participant '{0}' failed during initial rollback: {1}",
                        participant.GetType().FullName,
                        exception);
                }
                registration.Dispose();
                throw;
            }
        }
    }

    /// <summary>
    /// Loads and activates a new shadow-copied assembly module.
    /// </summary>
    /// <param name="request">
    /// The validated immutable request that defines this operation.
    /// </param>
    /// <returns>
    /// The validated assembly module handle that represents the completed operation.
    /// </returns>
    public AssemblyModuleHandle Load(IModuleSource request)
    {
        ArgumentNullException.ThrowIfNull(request);
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            ValidateUniqueModuleName(request.moduleName);
            if (request.domain != AssemblyDomain.InnoPlugin)
                ValidateUniqueReloadBoundary(request.domain, request.scope);

            AssemblyModuleHandle handle = new(Guid.NewGuid());
            IReadOnlyDictionary<string, PlannedAssembly> plannedAssemblies =
                BuildPlannedAssemblyMap([request]);
            AssemblyModuleEntry module = StageModule(
                handle,
                request,
                generation: 1,
                upstreamModules: GetUpstreamModules(request, []),
                plannedAssemblies);
            m_modules.Add(handle, module);
            try
            {
                RebuildLocked();
                return handle;
            }
            catch
            {
                m_modules.Remove(handle);
                BeginUnload(module);
                throw;
            }
        }
    }

    /// <summary>
    /// Registers assemblies owned by an external load context.
    /// </summary>
    /// <param name="moduleName">
    /// The module name text validated by the register operation.
    /// </param>
    /// <param name="assemblies">
    /// The assemblies consumed by register; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated assembly module handle that represents the completed operation.
    /// </returns>
    public AssemblyModuleHandle Register(
        string moduleName,
        IReadOnlyList<Assembly> assemblies
    ) {
        if (string.IsNullOrWhiteSpace(moduleName))
            throw new ArgumentException("Module name is required.", nameof(moduleName));
        ArgumentNullException.ThrowIfNull(assemblies);
        if (assemblies.Count == 0)
            throw new ArgumentException("At least one assembly is required.", nameof(assemblies));
        if (assemblies.Any(static assembly => assembly is null || assembly.IsDynamic))
            throw new ArgumentException("External modules must contain non-dynamic assemblies.", nameof(assemblies));

        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            ValidateUniqueModuleName(moduleName);

            if (!assemblies[0].TryGetInnoAssemblyClassification(
                    out AssemblyDomain domain,
                    out AssemblyScope scope) ||
                assemblies.Any(assembly =>
                    !assembly.TryGetInnoAssemblyClassification(
                        out AssemblyDomain candidateDomain,
                        out AssemblyScope candidateScope) ||
                    candidateDomain != domain ||
                    candidateScope != scope))
            {
                throw new ArgumentException(
                    "Every externally owned module assembly must declare the same valid domain and scope metadata.",
                    nameof(assemblies));
            }

            AssemblyModuleHandle handle = new(Guid.NewGuid());
            var module = new AssemblyModuleEntry
            {
                handle = handle,
                moduleName = moduleName,
                generation = 1,
                externallyOwned = true,
                collectible = false,
                domain = domain,
                scope = scope,
                assemblies = assemblies.Distinct().ToArray(),
                assemblyScopes = assemblies.Distinct().ToDictionary(static assembly => assembly, _ => scope),
                contribution = new ModuleCatalogContribution(moduleName, domain, scope,
                    assemblies.Distinct().ToArray(),
                    assemblies.Distinct().ToDictionary(static assembly => assembly, _ => scope))
            };
            m_modules.Add(handle, module);
            try
            {
                RebuildLocked();
                return handle;
            }
            catch
            {
                m_modules.Remove(handle);
                throw;
            }
        }
    }

    /// <summary>
    /// Stages and validates a replacement generation without publishing it.
    /// </summary>
    /// <param name="module">
    /// The module consumed by begin reload; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="request">
    /// The validated immutable request that defines this operation.
    /// </param>
    /// <returns>
    /// The validated assembly reload session that represents the completed operation.
    /// </returns>
    public AssemblyReloadSession BeginReload(
        AssemblyModuleHandle module,
        IModuleSource request
    ) {
        ArgumentNullException.ThrowIfNull(request);
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            if (!m_modules.TryGetValue(module, out AssemblyModuleEntry? previous))
                throw new ArgumentException("The assembly module is not active.", nameof(module));
            if (!string.Equals(previous.moduleName, request.moduleName, StringComparison.Ordinal))
            {
                throw new ArgumentException(
                    $"Reload request module name '{request.moduleName}' does not match '{previous.moduleName}'.",
                    nameof(request));
            }

            return BeginReloadLocked(
                [request],
                removedModuleNames: [],
                new Dictionary<string, AssemblyModuleHandle>(
                    StringComparer.Ordinal) { [request.moduleName] = module });
        }
    }

    /// <summary>
    /// Stages a dependency-ordered set of module additions or replacements as one atomic transaction.
    /// Existing modules are matched by their stable module names; an unknown name creates a new module.
    /// </summary>
    /// <param name="requests">
    /// The complete reverse-dependency reload closure.
    /// </param>
    /// <returns>
    /// A session that publishes or rolls back every candidate together.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="requests"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the request set is empty or contains duplicate module names.
    /// </exception>
    public AssemblyReloadSession BeginReload(IReadOnlyList<IModuleSource> requests)
    {
        ArgumentNullException.ThrowIfNull(requests);
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            return BeginReloadLocked(requests, removedModuleNames: [], forcedHandles: null);
        }
    }

    /// <summary>
    /// Stages additions, replacements, and removals as one atomic dependency-ordered transaction.
    /// </summary>
    /// <param name="requests">
    /// Complete candidate module additions and replacements.
    /// </param>
    /// <param name="removedModuleNames">
    /// Active stable module names omitted from the candidate generation.
    /// </param>
    /// <returns>
    /// A session that publishes or rolls back every candidate and removal together.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when either argument is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when no change is requested, a name is duplicated, or one module is both replaced and removed.
    /// </exception>
    public AssemblyReloadSession BeginReload(
        IReadOnlyList<IModuleSource> requests,
        IReadOnlyList<string> removedModuleNames
    ) {
        ArgumentNullException.ThrowIfNull(requests);
        ArgumentNullException.ThrowIfNull(removedModuleNames);
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            return BeginReloadLocked(requests, removedModuleNames, forcedHandles: null);
        }
    }

    /// <summary>
    /// Removes an active module and starts cooperative unload when it is manager-owned.
    /// </summary>
    /// <param name="module">
    /// The module consumed by unload; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated assembly unload monitor that represents the completed operation.
    /// </returns>
    public IAssemblyUnloadProbe Unload(AssemblyModuleHandle module)
    {
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            ValidateUnloadClosure([module]);
            if (!m_modules.Remove(module, out AssemblyModuleEntry? removed))
                throw new ArgumentException("The assembly module is not active.", nameof(module));
            try
            {
                RebuildLocked();
            }
            catch
            {
                m_modules.Add(module, removed);
                throw;
            }

            return BeginUnload(removed);
        }
    }

    /// <summary>
    /// Removes several active modules in one catalog transaction, then requests unload in reverse dependency order.
    /// </summary>
    /// <param name="modules">
    /// The distinct active module handles to remove.
    /// </param>
    /// <returns>
    /// A monitor that completes after every collectible context becomes unreachable.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="modules"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when a handle is duplicated or is not active.
    /// </exception>
    public IAssemblyUnloadProbe Unload(IReadOnlyList<AssemblyModuleHandle> modules)
    {
        ArgumentNullException.ThrowIfNull(modules);
        lock (m_sync)
        {
            EnsureInitialized();
            EnsureNoReloadInProgress();
            if (modules.Distinct().Count() != modules.Count)
                throw new ArgumentException("Module handles must be distinct.", nameof(modules));
            ValidateUnloadClosure(modules);
            var removed = new List<AssemblyModuleEntry>(modules.Count);
            foreach (AssemblyModuleHandle handle in modules)
            {
                if (!m_modules.TryGetValue(handle, out AssemblyModuleEntry? module))
                    throw new ArgumentException("An assembly module is not active.", nameof(modules));
                removed.Add(module);
            }
            foreach (AssemblyModuleEntry module in removed)
                m_modules.Remove(module.handle);
            try
            {
                RebuildLocked();
            }
            catch
            {
                foreach (AssemblyModuleEntry module in removed)
                    m_modules.Add(module.handle, module);
                throw;
            }

            AssemblyModuleEntry[] unloadOrder = removed
                .OrderByDescending(static module => module.domain == AssemblyDomain.InnoPlugin
                    ? 0
                    : module.scope == AssemblyScope.Runtime ? 1 : 2)
                .ToArray();
            var monitors = unloadOrder.Select(BeginUnload).ToArray();
            return monitors.Length == 1
                ? monitors[0]
                : new CompositeAssemblyUnloadProbe(monitors);
        }
    }

    /// <summary>
    /// Applies pending host assembly changes without rebuilding an unchanged catalog.
    /// </summary>
    public void Refresh()
    {
        lock (m_sync)
        {
            EnsureInitialized();
            if (m_hostCatalogDirty && !m_reloadInProgress)
            {
                RebuildLocked(deferWhileBusy: true);
            }
        }
    }

    /// <summary>
    /// Rebuilds the active assembly catalog and every registered derived-state participant.
    /// </summary>
    public void Rebuild()
    {
        lock (m_sync)
        {
            EnsureInitialized();
            // A participant may request another reconciliation of this same publication.
            // The lock confines this branch to reentry; it does not admit a new transaction.
            if (m_catalogTransitionInProgress)
            {
                m_hostCatalogDirty = true;
                m_rebuildPending = true;
                return;
            }
            EnsureNoReloadInProgress();
            RebuildLocked();
        }
    }

    /// <summary>
    /// Unsubscribes assembly discovery and begins unload of every module owned by this host.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// A generation still owns live work. Discovery stops, but modules are not unloaded or forgotten.
    /// </exception>
    public void Dispose()
    {
        lock (m_sync)
            ShutdownLocked();
        GC.SuppressFinalize(this);
    }

    internal void Activate(ReloadState state)
    {
        lock (m_sync)
        {
            EnsureActiveState(state);
            if (state.activated)
                return;

            foreach (AssemblyModuleEntry removed in state.removedModules)
                m_modules.Remove(removed.handle);
            for (int i = 0; i < state.candidateModules.Length; i++)
                m_modules[state.candidateModules[i].handle] = state.candidateModules[i];
            m_currentCatalog = state.candidateCatalog;
            try
            {
                state.refresh.Activate();
                state.activated = true;
            }
            catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
            {
                RetainFailedRetirement(failure, state);
                throw;
            }
            catch (Exception activationFailure)
            {
                RestorePreviousModules(state);
                m_currentCatalog = state.previousCatalog;
                state.finished = true;
                m_reloadInProgress = false;
                if (activationFailure is AggregateException)
                    generations.Fault(activationFailure);
                try
                {
                    state.refresh.Rollback();
                }
                catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
                {
                    RetainFailedRetirement(failure, state);
                    throw;
                }
                catch (Exception rollbackFailure)
                {
                    var failure = new AggregateException("Module activation and rollback failed.", activationFailure, rollbackFailure);
                    generations.Fault(failure);
                    throw failure;
                }
                finally
                {
                    if (m_retirementFailure is null)
                        BeginUnloadReverse(state.candidateModules);
                }
                throw;
            }
        }
    }

    internal IAssemblyUnloadProbe Complete(ReloadState state)
    {
        lock (m_sync)
        {
            EnsureActiveState(state);
            if (!state.activated)
                throw new InvalidOperationException("The reload session must be activated before completion.");

            state.finished = true;
            m_reloadInProgress = false;
            Exception? cleanupFailure = null;
            try
            {
                state.refresh.Complete();
            }
            catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
            {
                RetainFailedRetirement(failure, state);
                throw;
            }
            catch (Exception exception)
            {
                cleanupFailure = exception;
                generations.Fault(exception);
            }
            IAssemblyUnloadProbe monitor = BeginUnloadReverse(
                state.previousModules
                    .OfType<AssemblyModuleEntry>()
                    .Concat(state.removedModules)
                    .Distinct()
                    .ToArray());
            if (cleanupFailure is not null)
                throw new InvalidOperationException("The module generation committed but retirement failed; restart the host.", cleanupFailure);
            return monitor;
        }
    }

    internal void Rollback(ReloadState state)
    {
        lock (m_sync)
        {
            EnsureRetirementSafe();
            if (state.finished)
                return;
            if (state.activated)
            {
                RestorePreviousModules(state);
                m_currentCatalog = state.previousCatalog;
            }

            state.finished = true;
            m_reloadInProgress = false;
            try
            {
                state.refresh.Rollback();
            }
            catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
            {
                RetainFailedRetirement(failure, state);
                throw;
            }
            catch (Exception exception)
            {
                generations.Fault(exception);
                throw;
            }
            finally
            {
                if (m_retirementFailure is null)
                    BeginUnloadReverse(state.candidateModules);
            }
        }
    }

    private AssemblyReloadSession BeginReloadLocked(
        IReadOnlyList<IModuleSource> requests,
        IReadOnlyList<string> removedModuleNames,
        IReadOnlyDictionary<string, AssemblyModuleHandle>? forcedHandles
    ) {
        if (requests.Count == 0 && removedModuleNames.Count == 0)
            throw new ArgumentException("At least one module change is required.", nameof(requests));
        if (requests.Any(static request => request is null))
            throw new ArgumentException("Module reload requests cannot contain null entries.", nameof(requests));
        IModuleSource[] orderedRequests = OrderReloadRequests(requests);
        if (orderedRequests.Select(static request => request.moduleName).Distinct(StringComparer.Ordinal).Count() !=
            orderedRequests.Length)
        {
            throw new ArgumentException("A reload plan cannot contain duplicate module names.", nameof(requests));
        }
        if (removedModuleNames.Any(static name => string.IsNullOrWhiteSpace(name)) ||
            removedModuleNames.Distinct(StringComparer.Ordinal).Count() != removedModuleNames.Count)
        {
            throw new ArgumentException("Removed module names must be non-empty and distinct.", nameof(removedModuleNames));
        }
        if (orderedRequests.Any(request => removedModuleNames.Contains(request.moduleName, StringComparer.Ordinal)))
            throw new ArgumentException("A module cannot be replaced and removed together.", nameof(removedModuleNames));
        AssemblyModuleEntry[] removedModules = removedModuleNames.Select(name =>
        {
            AssemblyModuleEntry? module = m_modules.Values.SingleOrDefault(candidate =>
                string.Equals(candidate.moduleName, name, StringComparison.Ordinal));
            return module ?? throw new ArgumentException(
                $"Removed assembly module '{name}' is not active.",
                nameof(removedModuleNames));
        }).ToArray();
        ValidateReloadClosure(orderedRequests, removedModules);
        IReadOnlyDictionary<string, PlannedAssembly> plannedAssemblies =
            BuildPlannedAssemblyMap(orderedRequests);

        var previousModules = new AssemblyModuleEntry?[orderedRequests.Length];
        var candidates = new List<AssemblyModuleEntry>(orderedRequests.Length);
        try
        {
            for (int i = 0; i < orderedRequests.Length; i++)
            {
                IModuleSource request = orderedRequests[i];
                AssemblyModuleEntry? previous = FindPreviousModule(request, forcedHandles);
                previousModules[i] = previous;
                AssemblyModuleHandle handle = previous?.handle ?? new AssemblyModuleHandle(Guid.NewGuid());
                int generation = (previous?.generation ?? 0) + 1;
                IReadOnlyList<AssemblyModuleEntry> upstream = GetUpstreamModules(request, candidates);
                candidates.Add(StageModule(handle, request, generation, upstream, plannedAssemblies));
            }

            var replacements = candidates.ToDictionary(static module => module.handle);
            AssemblyCatalogSnapshot previousCatalog = m_currentCatalog;
            AssemblyCatalogSnapshot candidateCatalog = BuildCatalog(
                replacements,
                removedModules.Select(static module => module.handle).ToHashSet());
            AssemblyCatalogRefreshSet refresh = m_catalogs.Prepare(candidateCatalog);
            m_reloadInProgress = true;
            return new AssemblyReloadSession(this, new ReloadState
            {
                previousModules = previousModules,
                removedModules = removedModules,
                candidateModules = candidates.ToArray(),
                previousCatalog = previousCatalog,
                candidateCatalog = candidateCatalog,
                refresh = refresh
            });
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            RetainFailedRetirement(failure, (candidates, previousModules, m_catalogs));
            throw;
        }
        catch
        {
            BeginUnloadReverse(candidates);
            throw;
        }
    }

    private AssemblyModuleEntry? FindPreviousModule(
        IModuleSource request,
        IReadOnlyDictionary<string, AssemblyModuleHandle>? forcedHandles
    ) {
        if (forcedHandles is not null && forcedHandles.TryGetValue(request.moduleName, out AssemblyModuleHandle handle))
            return m_modules[handle];
        AssemblyModuleEntry? previous = m_modules.Values.SingleOrDefault(module =>
            string.Equals(module.moduleName, request.moduleName, StringComparison.Ordinal));
        if (previous is null && request.domain != AssemblyDomain.InnoPlugin)
            ValidateUniqueReloadBoundary(request.domain, request.scope);
        else if (previous is not null &&
                 (previous.domain != request.domain || previous.scope != request.scope))
            throw new InvalidOperationException(
                $"Module '{request.moduleName}' cannot change its domain or scope across generations.");
        return previous;
    }

    private IReadOnlyList<AssemblyModuleEntry> GetUpstreamModules(
        IModuleSource request,
        IReadOnlyList<AssemblyModuleEntry> stagedCandidates
    ) {
        IEnumerable<AssemblyModuleEntry> effectiveModules = m_modules.Values
            .Where(active => stagedCandidates.All(candidate => candidate.handle != active.handle))
            .Concat(stagedCandidates);
        if (request.upstreamModuleNames.Count == 0)
            return [];

        Dictionary<string, AssemblyModuleEntry> byName = effectiveModules.ToDictionary(
            static module => module.moduleName,
            StringComparer.Ordinal);
        return request.upstreamModuleNames.Select(name =>
        {
            if (!byName.TryGetValue(name, out AssemblyModuleEntry? module))
            {
                throw new InvalidOperationException(
                    $"Module '{request.moduleName}' requires unavailable upstream module '{name}'.");
            }
            return module;
        }).ToArray();
    }

    private int GetReloadOrder(IModuleSource request)
        => request.domain switch
        {
            AssemblyDomain.InnoPlugin => 0,
            AssemblyDomain.InnoScripting when request.scope == AssemblyScope.Runtime => 1,
            AssemblyDomain.InnoScripting => 2,
            _ => throw new ArgumentException("InnoInternal assemblies cannot be loaded into a collectible module.")
        };

    private IModuleSource[] OrderReloadRequests(IReadOnlyList<IModuleSource> requests)
    {
        Dictionary<string, IModuleSource> byName = requests.ToDictionary(
            static request => request.moduleName,
            StringComparer.Ordinal);
        IComparer<string> ordering = Comparer<string>.Create((
            left,
            right
        ) =>
        {
            int domainOrder = GetReloadOrder(byName[left]).CompareTo(GetReloadOrder(byName[right]));
            return domainOrder != 0
                ? domainOrder
                : StringComparer.Ordinal.Compare(left, right);
        });
        var graph = new DependencyGraph<string>(StringComparer.Ordinal, ordering);
        foreach (IModuleSource request in requests)
        {
            graph.AddNode(request.moduleName);
            foreach (string dependencyName in request.upstreamModuleNames
                         .OrderBy(static value => value, StringComparer.Ordinal))
            {
                if (!byName.TryGetValue(dependencyName, out IModuleSource? dependency))
                    continue;
                if (GetReloadOrder(dependency) > GetReloadOrder(request))
                {
                    throw new ArgumentException(
                        $"Module '{request.moduleName}' cannot depend on downstream module '{dependencyName}'.",
                        nameof(requests));
                }
                graph.AddDependency(request.moduleName, dependencyName);
            }
        }
        try
        {
            return graph.TopologicalSort().Select(name => byName[name]).ToArray();
        }
        catch (InvalidOperationException exception)
        {
            throw new ArgumentException(exception.Message, nameof(requests), exception);
        }
    }

    private void RestorePreviousModules(ReloadState state)
    {
        for (int i = 0; i < state.candidateModules.Length; i++)
        {
            AssemblyModuleEntry candidate = state.candidateModules[i];
            AssemblyModuleEntry? previous = state.previousModules[i];
            if (previous is null)
                m_modules.Remove(candidate.handle);
            else
                m_modules[candidate.handle] = previous;
        }
        foreach (AssemblyModuleEntry removed in state.removedModules)
            m_modules[removed.handle] = removed;
        m_currentCatalog = state.previousCatalog;
    }

    private void RebuildLocked(bool deferWhileBusy = false)
    {
        if (m_catalogTransitionInProgress)
        {
            m_hostCatalogDirty = true;
            m_rebuildPending = true;
            return;
        }

        if (!generations.TryAcquireChange("refresh the assembly catalog", out IDisposable? reservation))
        {
            m_hostCatalogDirty = true;
            if (deferWhileBusy)
                return;
            throw new InvalidOperationException("Catalog publication is blocked by a generation owner operation.");
        }
        using IDisposable publication = reservation!;
        do
        {
            m_rebuildPending = false;
            m_catalogTransitionInProgress = true;
            try
            {
                RebuildOnceLocked();
            }
            finally
            {
                m_catalogTransitionInProgress = false;
            }
        }
        while (m_rebuildPending);
    }

    private void RebuildOnceLocked()
    {
        AssemblyCatalogSnapshot previous = m_currentCatalog;
        m_hostCatalogDirty = false;
        try
        {
            AssemblyCatalogSnapshot candidate = BuildCatalog(
                new Dictionary<AssemblyModuleHandle, AssemblyModuleEntry>());
            AssemblyCatalogRefreshSet refresh = m_catalogs.Prepare(candidate);
            m_currentCatalog = candidate;
            bool activated = false;
            try
            {
                refresh.Activate();
                activated = true;
                refresh.Complete();
            }
            catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
            {
                RetainFailedRetirement(failure, (refresh, previous));
                throw;
            }
            catch (Exception failure)
            {
                if (activated)
                    generations.Fault(failure);
                else
                {
                    m_currentCatalog = previous;
                    try
                    {
                        refresh.Rollback();
                    }
                    catch (Exception pending) when (RetirementPendingException.Find(pending) is not null)
                    {
                        RetainFailedRetirement(pending, (refresh, previous));
                        throw;
                    }
                    catch (Exception rollbackFailure)
                    {
                        var aggregate = new AggregateException("Catalog activation and rollback failed.", failure, rollbackFailure);
                        generations.Fault(aggregate);
                        throw aggregate;
                    }
                }
                throw;
            }
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            if (m_retirementFailure is null)
                RetainFailedRetirement(failure, (m_catalogs, previous));
            throw;
        }
    }

    private AssemblyCatalogSnapshot BuildCatalog(
        IReadOnlyDictionary<AssemblyModuleHandle, AssemblyModuleEntry> replacements,
        IReadOnlySet<AssemblyModuleHandle>? removed = null
    ) {
        Assembly[] assemblies = GetActiveAssemblies(replacements, removed ?? new HashSet<AssemblyModuleHandle>());
        return new AssemblyCatalogSnapshot(++m_catalogVersion, assemblies);
    }

    private Assembly[] GetActiveAssemblies(
        IReadOnlyDictionary<AssemblyModuleHandle, AssemblyModuleEntry> replacements,
        IReadOnlySet<AssemblyModuleHandle> removed
    ) {
        IEnumerable<Assembly> host = m_options.catalogSource.GetAssemblies();
        IEnumerable<AssemblyModuleEntry> modules = m_modules.Values
            .Where(module => !removed.Contains(module.handle))
            .Select(module => replacements.GetValueOrDefault(module.handle, module))
            .Concat(replacements.Values.Where(candidate => !m_modules.ContainsKey(candidate.handle)));
        IEnumerable<Assembly> moduleAssemblies = modules.SelectMany(static module => module.assemblies);
        return host.Concat(moduleAssemblies).Distinct().ToArray();
    }






    private AssemblyModuleEntry StageModule(
        AssemblyModuleHandle handle,
        IModuleSource source,
        int generation,
        IReadOnlyList<AssemblyModuleEntry> upstreamModules,
        IReadOnlyDictionary<string, PlannedAssembly> plannedAssemblies
    ) {
        ValidateRequest(source);
        var context = new ModuleSourceContext(
            generation, m_options.cacheDirectory, m_options.catalogSource,
            upstreamModules.Select(static module => module.contribution).ToArray(),
            m_modules.Values.Select(static module => module.contribution).ToArray(),
            plannedAssemblies.ToDictionary(static pair => pair.Key,
                static pair => new ModuleAssemblyDescriptor(pair.Value.domain, pair.Value.scope),
                StringComparer.OrdinalIgnoreCase),
            generations.TrackRetirement);
        ModuleCatalogContribution contribution = source.Prepare(context);
        try
        {
            if (contribution.moduleName != source.moduleName || contribution.domain != source.domain ||
                contribution.scope != source.scope || (contribution.lifetime is not null) != source.collectible)
                throw new InvalidOperationException("A module source changed its declared ownership during preparation.");
            foreach (Assembly assembly in contribution.assemblies)
                assembly.RegisterInnoAssemblyClassification(contribution.domain, contribution.assemblyScopes[assembly]);
            return new AssemblyModuleEntry
            {
                handle = handle, moduleName = source.moduleName, generation = generation,
                externallyOwned = contribution.lifetime is null, collectible = source.collectible,
                domain = source.domain, scope = source.scope, assemblies = contribution.assemblies.ToArray(),
                assemblyScopes = contribution.assemblyScopes, upstreamModuleNames = source.upstreamModuleNames.ToArray(),
                contribution = contribution
            };
        }
        catch
        {
            if (contribution.lifetime is not null)
                generations.TrackRetirement(contribution.lifetime.BeginRetirement());
            throw;
        }
    }

    private void OnAssemblyLoaded() => m_hostCatalogDirty = true;

    private IAssemblyUnloadProbe BeginUnload(AssemblyModuleEntry module)
    {
        IAssemblyUnloadProbe probe = module.contribution.lifetime?.BeginRetirement()
            ?? new CompositeAssemblyUnloadProbe([]);
        generations.TrackRetirement(probe);
        return probe;
    }

    private IAssemblyUnloadProbe BeginUnloadReverse(IReadOnlyList<AssemblyModuleEntry> modules)
    {
        var monitors = new List<IAssemblyUnloadProbe>(modules.Count);
        for (int i = modules.Count - 1; i >= 0; i--)
            monitors.Add(BeginUnload(modules[i]));
        return monitors.Count == 1
            ? monitors[0]
            : new CompositeAssemblyUnloadProbe(monitors);
    }



    private void ValidateRequest(IModuleSource request)
    {
        if (string.IsNullOrWhiteSpace(request.moduleName))
            throw new ArgumentException("Module name is required.", nameof(request));
        if (!Enum.IsDefined(request.domain) || request.domain == AssemblyDomain.InnoInternal)
            throw new ArgumentException("A reloadable module must belong to InnoPlugin or InnoScripting.", nameof(request));
        if (!Enum.IsDefined(request.scope) || request.assemblyScopes.Values.Any(static scope => !Enum.IsDefined(scope)))
            throw new ArgumentException("A reloadable module contains an invalid assembly scope.", nameof(request));
        if (request.upstreamModuleNames.Any(static name => string.IsNullOrWhiteSpace(name)) ||
            request.upstreamModuleNames.Distinct(StringComparer.Ordinal).Count() !=
            request.upstreamModuleNames.Count ||
            request.upstreamModuleNames.Contains(request.moduleName, StringComparer.Ordinal))
        {
            throw new ArgumentException(
                "Upstream module names must be non-empty, distinct, and cannot reference the owning module.",
                nameof(request));
        }

        _ = request.GetAssemblyNames();
    }

    private IReadOnlyDictionary<string, PlannedAssembly> BuildPlannedAssemblyMap(IReadOnlyList<IModuleSource> requests)
    {
        var result = new Dictionary<string, PlannedAssembly>(StringComparer.OrdinalIgnoreCase);
        foreach (IModuleSource request in requests)
        {
            ValidateRequest(request);
            var ownedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string name in request.GetAssemblyNames())
            {
                ownedNames.Add(name);
                AssemblyScope scope = request.assemblyScopes.GetValueOrDefault(name, request.scope);
                if (!result.TryAdd(name, new PlannedAssembly(request.domain, scope)))
                    throw new InvalidDataException($"Reload plan contains duplicate managed assembly name '{name}'.");
            }
            string? unknownScope = request.assemblyScopes.Keys.FirstOrDefault(name => !ownedNames.Contains(name));
            if (unknownScope is not null)
            {
                throw new InvalidDataException(
                    $"Module '{request.moduleName}' declares a scope for unknown assembly '{unknownScope}'.");
            }
        }
        return result;
    }



    private void ValidateUniqueModuleName(string moduleName)
    {
        if (string.IsNullOrWhiteSpace(moduleName))
            throw new ArgumentException("Module name is required.", nameof(moduleName));
        if (m_modules.Values.Any(module => string.Equals(module.moduleName, moduleName, StringComparison.Ordinal)))
            throw new InvalidOperationException($"Assembly module '{moduleName}' is already active.");
    }

    private void ValidateUniqueReloadBoundary(
        AssemblyDomain domain,
        AssemblyScope scope
    ) {
        if (domain == AssemblyDomain.InnoPlugin)
            return;
        if (m_modules.Values.Any(module => module.domain == domain && module.scope == scope))
        {
            throw new InvalidOperationException(
                $"Only one {scope} scripting module can be active.");
        }
    }

    private void ValidateReloadClosure(
        IReadOnlyList<IModuleSource> requests,
        IReadOnlyList<AssemblyModuleEntry> removedModules
    ) {
        bool reloadsPlugins = requests.Any(static request => request.domain == AssemblyDomain.InnoPlugin) ||
                              removedModules.Any(static module => module.domain == AssemblyDomain.InnoPlugin);
        bool reloadsRuntime = requests.Any(static request =>
                                  request.domain == AssemblyDomain.InnoScripting &&
                                  request.scope == AssemblyScope.Runtime) ||
                              removedModules.Any(static module =>
                                  module.domain == AssemblyDomain.InnoScripting &&
                                  module.scope == AssemblyScope.Runtime);
        bool reloadsEditor = requests.Any(static request =>
                                 request.domain == AssemblyDomain.InnoScripting &&
                                 request.scope == AssemblyScope.Editor) ||
                             removedModules.Any(static module =>
                                 module.domain == AssemblyDomain.InnoScripting &&
                                 module.scope == AssemblyScope.Editor);
        if (reloadsPlugins && m_modules.Values.Any(static module => module.domain == AssemblyDomain.InnoScripting) &&
            (!reloadsRuntime || !reloadsEditor))
        {
            throw new InvalidOperationException(
                "Reloading plugins requires both dependent scripting modules in the same transaction.");
        }
        if (reloadsRuntime && m_modules.Values.Any(static module =>
                module.domain == AssemblyDomain.InnoScripting && module.scope == AssemblyScope.Editor) &&
            !reloadsEditor)
        {
            throw new InvalidOperationException(
                "Reloading runtime scripts requires the editor scripting module in the same transaction.");
        }
        HashSet<string> reloadedNames = requests
            .Select(static request => request.moduleName)
            .ToHashSet(StringComparer.Ordinal);
        reloadedNames.UnionWith(removedModules.Select(static module => module.moduleName));
        HashSet<string> reloadedPlugins = requests
            .Where(static request => request.domain == AssemblyDomain.InnoPlugin)
            .Select(static request => request.moduleName)
            .ToHashSet(StringComparer.Ordinal);
        reloadedPlugins.UnionWith(removedModules
            .Where(static module => module.domain == AssemblyDomain.InnoPlugin)
            .Select(static module => module.moduleName));
        string? omittedDependent = m_modules.Values.FirstOrDefault(module =>
            module.domain == AssemblyDomain.InnoPlugin &&
            !reloadedNames.Contains(module.moduleName) &&
            module.upstreamModuleNames.Any(reloadedPlugins.Contains))?.moduleName;
        if (omittedDependent is not null)
        {
            throw new InvalidOperationException(
                $"Reloading a Plugin module also requires dependent module '{omittedDependent}'.");
        }
    }

    private void ValidateUnloadClosure(IReadOnlyList<AssemblyModuleHandle> handles)
    {
        var removed = handles.ToHashSet();
        bool removesPlugin = m_modules.Values.Any(module =>
            removed.Contains(module.handle) && module.domain == AssemblyDomain.InnoPlugin);
        bool removesRuntime = m_modules.Values.Any(module =>
            removed.Contains(module.handle) &&
            module.domain == AssemblyDomain.InnoScripting &&
            module.scope == AssemblyScope.Runtime);
        if (removesPlugin && m_modules.Values.Any(module =>
                !removed.Contains(module.handle) && module.domain == AssemblyDomain.InnoScripting))
        {
            throw new InvalidOperationException(
                "Plugin unload requires all dependent scripting modules in the same transaction.");
        }
        HashSet<string> removedNames = m_modules.Values
            .Where(module => removed.Contains(module.handle))
            .Select(static module => module.moduleName)
            .ToHashSet(StringComparer.Ordinal);
        if (m_modules.Values.Any(module =>
                !removed.Contains(module.handle) &&
                module.upstreamModuleNames.Any(removedNames.Contains)))
        {
            throw new InvalidOperationException(
                "A module cannot unload while an active module depends on it.");
        }
        if (removesRuntime && m_modules.Values.Any(module =>
                !removed.Contains(module.handle) &&
                module.domain == AssemblyDomain.InnoScripting &&
                module.scope == AssemblyScope.Editor))
        {
            throw new InvalidOperationException(
                "Runtime scripting unload requires the editor scripting module in the same transaction.");
        }
    }

    private void EnsureInitialized()
    {
        if (!isInitialized)
            throw new InvalidOperationException("ModuleHost is not initialized.");
    }

    private void EnsureNoReloadInProgress()
    {
        generations.EnsureReady("change the module generation");
        if (m_reloadInProgress)
            throw new InvalidOperationException("Another assembly reload transaction is already active.");
    }

    private void EnsureActiveState(ReloadState state)
    {
        EnsureRetirementSafe();
        if (state.finished)
            throw new InvalidOperationException("The reload session is already finished.");
        if (!m_reloadInProgress)
            throw new InvalidOperationException("The reload session is no longer current.");
        for (int i = 0; i < state.candidateModules.Length; i++)
        {
            AssemblyModuleEntry candidate = state.candidateModules[i];
            AssemblyModuleEntry? expected = state.activated ? candidate : state.previousModules[i];
            bool exists = m_modules.TryGetValue(candidate.handle, out AssemblyModuleEntry? current);
            if (expected is null ? exists : !exists || !ReferenceEquals(current, expected))
                throw new InvalidOperationException("The reload session is no longer current.");
        }
        foreach (AssemblyModuleEntry removed in state.removedModules)
        {
            bool exists = m_modules.TryGetValue(removed.handle, out AssemblyModuleEntry? current);
            if (state.activated ? exists : !exists || !ReferenceEquals(current, removed))
                throw new InvalidOperationException("The reload session removal set is no longer current.");
        }
    }





    private readonly record struct PlannedAssembly(
        AssemblyDomain domain,
        AssemblyScope scope
    );


    private void ShutdownLocked()
    {
        if (m_assemblyLoadSubscribed)
        {
            m_options.catalogSource.changed -= OnAssemblyLoaded;
            m_assemblyLoadSubscribed = false;
        }

        EnsureRetirementSafe();
        var emptyCatalog = new AssemblyCatalogSnapshot(++m_catalogVersion, []);
        AssemblyCatalogRefreshSet? refresh = null;
        try
        {
            refresh = m_catalogs.Prepare(emptyCatalog);
            m_currentCatalog = emptyCatalog;
            refresh.Activate();
            refresh.Complete();
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            RetainFailedRetirement(failure, (object?)refresh ?? m_catalogs);
            throw;
        }

        foreach (AssemblyModuleEntry module in m_modules.Values
                     .OrderByDescending(static value => value.domain == AssemblyDomain.InnoPlugin
                         ? 0
                         : value.scope == AssemblyScope.Runtime ? 1 : 2))
        {
            BeginUnload(module);
        }
        m_modules.Clear();
        m_options.catalogSource.Dispose();
        isInitialized = false;
        m_hostCatalogDirty = false;
        m_catalogTransitionInProgress = false;
        m_rebuildPending = false;
        m_reloadInProgress = false;
        m_catalogVersion = 0;
        m_currentCatalog = new AssemblyCatalogSnapshot(0, []);
    }

    private void RetainFailedRetirement(
        Exception failure,
        object owner
    ) {
        m_retirementFailure = failure;
        m_retainedRetirement = owner;
        generations.Fault(failure);
    }

    private void EnsureRetirementSafe()
    {
        if (m_retirementFailure is not null)
        {
            GC.KeepAlive(m_retainedRetirement);
            throw m_retirementFailure;
        }
        generations.EnsureRetirementSafe();
    }
}
