using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Core.Events;
using Inno.Core.Identity;
using Inno.Core.Logging;
using Inno.Scripting.Api;
using Inno.Editor.Core;

namespace Inno.Editor.Interactions;
/// <summary>
/// Provides the single presentation-independent entry point for editor actions, menus, selection, focus, and drag-and-drop.
/// </summary>
public sealed class EditorInteractions : IEditorSelectionCoordinator, IEditorHistoryIsolation
{
    private readonly EditorContext m_editor;
    private readonly Dictionary<IdentityDomainId, IdentityAllocator> m_identityDomains;
    private readonly int m_identityOwnerThread = Environment.CurrentManagedThreadId;
    private bool m_identityDomainsRetired;
    private readonly EditorHistory m_history;
    private readonly EditorDocumentService m_documents;
    private readonly Logger m_log;
    private EditorActionRouter? m_actions;
    private EditorExtensionCatalog? m_catalog;
    private EditorMenuCatalog? m_menus;
    private EditorToolbarCatalog? m_toolbars;
    private EditorDropRouter? m_drops;
    private string m_focusedArea = EditorBuiltInInteractionIds.C_GLOBAL_AREA;
    private EditorTargetReference? m_focus;
    private EditorTargetReference[]? m_previousGenerationSelection;
    private EditorTargetReference? m_previousGenerationFocus;
    private EditorTargetReference? m_previousGenerationAnchor;
    private bool m_generationTransitionPrepared;
    internal EditorInteractions(
        EditorContext editor,
        IEnumerable<IdentityAllocator> identityDomains,
        Logger log
    ) {
        m_editor = editor ?? throw new ArgumentNullException(nameof(editor));
        ArgumentNullException.ThrowIfNull(identityDomains);
        m_identityDomains = identityDomains.ToDictionary(static allocator => allocator.domainId);
        m_log = log ?? throw new ArgumentNullException(nameof(log));
        selection = new EditorSelectionState(this);
        m_documents = new EditorDocumentService(events);
        m_history = new EditorHistory(new EditorHistoryOptions { cacheDirectory = Path.Combine(editor.projectDirectory, "Library", "Editor", "History") }, log);
        m_history.Attach(editor, this);
    }

    /// <summary>
    /// Gets synchronous Editor lifecycle notifications implemented by the Core event dispatcher.
    /// </summary>
    /// <remarks>
    /// Own and dispose a hub rather than retaining callbacks beyond their extension generation.
    /// </remarks>
    [ScriptingApiIgnore]
    public EventDispatcher events { get; } = new();

    internal void PrepareOperation() => events.Emit(new EditorOperationPreparingEvent());
    /// <summary>
    /// Gets the shared read-only editor selection state.
    /// </summary>
    public EditorSelectionState selection { get; }

    object? IEditorSelectionCoordinator.selectedTarget => selection.selectedTarget;
    /// <summary>
    /// Gets the transactional Undo and Redo history owned by this editor runtime.
    /// </summary>
    public IEditorHistory history => m_history;
    /// <summary>
    /// Gets the headless reload-safe document lifetime used by dedicated asset editors and Inspectors.
    /// </summary>
    public IEditorDocumentService documents => m_documents;

    /// <summary>
    /// Resolves an active feature module for immediate use in the current Editor callback.
    /// </summary>
    /// <typeparam name="TModule">
    /// The feature module contract to resolve.
    /// </typeparam>
    /// <param name="module">
    /// The started, non-quarantined module, or null when unavailable.
    /// </param>
    /// <returns>
    /// True when the active generation supplies the requested module.
    /// </returns>
    /// <remarks>
    /// Do not retain the result across callbacks or generation changes. Candidate modules are never exposed.
    /// </remarks>
    public bool TryGetModule<TModule>(out TModule? module)
        where TModule : EditorModule
    {
        if (m_catalog is not null)
            return m_catalog.TryGetModule(out module);
        module = null;
        return false;
    }

    /// <summary>
    /// Starts an isolated temporary Undo and Redo branch while retaining the current editing branch.
    /// </summary>
    /// <remarks>
    /// Disposing the returned scope releases every temporary operation and restores the retained branch.
    /// This host-level boundary is intended for transient editor sessions such as Play Mode.
    /// </remarks>
    /// <returns>
    /// A scope that restores the retained history branch when disposed.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown during an Undo, Redo, transaction, or another isolated branch.
    /// </exception>
    [ScriptingApiIgnore]
    public IDisposable BeginHistoryIsolation() => m_history.BeginIsolation();
    internal EditorHistory historyHost => m_history;
    /// <summary>
    /// Gets the area that most recently received keyboard focus.
    /// </summary>
    public string focusedArea => m_focusedArea;

    /// <summary>
    /// Gets immutable discovery metadata for the active, available panels.
    /// </summary>
    /// <returns>
    /// Current panel declarations; the snapshot contains no panel or extension instances.
    /// </returns>
    public IReadOnlyList<EditorPanelAttribute> GetPanelDeclarations() => m_catalog?.GetPanelDeclarations() ?? Array.Empty<EditorPanelAttribute>();
    /// <summary>
    /// Reads whether one available panel is currently open.
    /// </summary>
    /// <param name="panelId">
    /// The stable panel identity.
    /// </param>
    /// <param name="isOpen">
    /// Receives the open state, or false when the panel is unavailable.
    /// </param>
    /// <returns>
    /// True when the current generation contains the requested available panel.
    /// </returns>
    public bool TryGetPanelState(
        string panelId,
        out bool isOpen
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(panelId);
        isOpen = false;
        return m_catalog?.TryGetPanelState(panelId, out isOpen) == true;
    }

    /// <summary>
    /// Gets the target associated with the focused area.
    /// </summary>
    public object? focusedTarget => m_focus?.Resolve(this);

    /// <summary>
    /// Creates a lightweight interaction handle for one area and optional target.
    /// </summary>
    /// <param name="area">
    /// The stable interaction area.
    /// </param>
    /// <param name="target">
    /// The optional object represented by the area.
    /// </param>
    /// <returns>
    /// A lightweight interaction handle.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="area"/> is empty.
    /// </exception>
    public EditorInteraction For(
        string area,
        object? target = null
    ) => new(this, area, target);
    /// <summary>
    /// Replaces the editor selection after closing presentations owned by other targets.
    /// </summary>
    /// <param name="target">
    /// The target to select, or <see langword="null"/> to clear the selection.
    /// </param>
    public void SetSelection(object? target)
    {
        PrepareOperation();
        if (target is null)
            selection.Clear();
        else
            selection.Select(target);
        PrepareSelectionChange(target);
    }

    /// <summary>
    /// Replaces the ordered selection; duplicates are removed and the final target becomes primary.
    /// </summary>
    /// <param name="targets">
    /// Registered objects resolved for the current callback.
    /// </param>
    /// <param name="anchor">
    /// The range anchor, or null to use the first selected target.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The target collection or one member is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A target has no live identity registration.
    /// </exception>
    public void SetSelection(
        IEnumerable<IdentityObject> targets,
        IdentityObject? anchor
    ) {
        PrepareOperation();
        selection.Replace(targets, anchor);
        PrepareSelectionChange(selection.selectedTarget);
    }

    /// <summary>
    /// Adds or removes a member while preserving the existing range anchor.
    /// </summary>
    /// <param name="target">
    /// A currently registered selection target.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The target is null.
    /// </exception>
    public void ToggleSelection(IdentityObject target)
    {
        ArgumentNullException.ThrowIfNull(target);
        var values = new List<IdentityObject>();
        _ = selection.CopyTargets(values);
        bool removed = values.Remove(target);
        if (!removed)
            values.Add(target);
        SetSelection(values, selection.anchor as IdentityObject ?? target);
    }

    /// <summary>
    /// Selects the inclusive visible range between the current anchor and the clicked target.
    /// </summary>
    /// <param name="orderedTargets">
    /// Targets in the current visible sort and filter order.
    /// </param>
    /// <param name="target">
    /// The clicked range endpoint, which becomes primary.
    /// </param>
    /// <param name="additive">
    /// Whether to preserve existing members outside the selected range.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The visible targets or endpoint is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The endpoint is not in the visible target order.
    /// </exception>
    public void SelectRange(
        IReadOnlyList<IdentityObject> orderedTargets,
        IdentityObject target,
        bool additive = false
    ) {
        ArgumentNullException.ThrowIfNull(orderedTargets);
        ArgumentNullException.ThrowIfNull(target);
        IdentityObject anchor = selection.anchor as IdentityObject ?? target;
        int first = -1;
        int last = -1;
        for (int i = 0; i < orderedTargets.Count; i++)
        {
            if (ReferenceEquals(orderedTargets[i], anchor))
                first = i;
            if (ReferenceEquals(orderedTargets[i], target))
                last = i;
        }

        if (last < 0)
            throw new ArgumentException("The selection endpoint is not in the visible order.", nameof(target));
        if (first < 0)
        {
            first = last;
            anchor = target;
        }

        var values = new List<IdentityObject>();
        if (additive)
            _ = selection.CopyTargets(values);
        for (int i = Math.Min(first, last); i <= Math.Max(first, last); i++)
        {
            IdentityObject value = orderedTargets[i];
            if (!ReferenceEquals(value, target) && !values.Contains(value))
                values.Add(value);
        }

        _ = values.Remove(target);
        values.Add(target);
        SetSelection(values, anchor);
    }

    /// <summary>
    /// Resolves the live source for an active runtime identity.
    /// </summary>
    /// <param name="identity">
    /// The domain-qualified runtime identity carried by the drag protocol.
    /// </param>
    /// <param name="source">
    /// The currently registered drag source when resolution succeeds.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the identity is active and resolves to a live source; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetDragSource(
        RuntimeIdentity identity,
        out IdentityObject? source
    ) => Drops.TryGetSource(identity, out source);
    /// <summary>
    /// Gets the active drag identity used to select the matching native payload domain.
    /// </summary>
    /// <param name="identity">
    /// The active runtime identity when one remains valid.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when a live drag source exists; otherwise, <see langword="false"/>.
    /// </returns>
    public bool TryGetActiveDragIdentity(out RuntimeIdentity identity) => Drops.TryGetActiveIdentity(out identity);
    /// <summary>
    /// Toggles one panel in the currently active extension generation.
    /// </summary>
    /// <param name="panelId">
    /// The stable panel identifier to resolve.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an available panel was found and toggled.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="panelId"/> is empty.
    /// </exception>
    public bool TogglePanel(string panelId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(panelId);
        return m_catalog?.TryTogglePanel(panelId) == true;
    }

    /// <summary>
    /// Makes a temporary Core identity domain available to selection, focus and drag resolution.
    /// </summary>
    /// <param name="allocator">
    /// The borrowed allocator owned by the temporary runtime session.
    /// </param>
    /// <returns>
    /// A routing lease; retire it after restoring Edit selection and before releasing the session.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The allocator is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The domain is already registered or the caller is not the owner thread.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The interaction runtime has retired its identity routing.
    /// </exception>
    [ScriptingApiIgnore]
    public IDisposable RegisterIdentityDomain(IdentityAllocator allocator)
    {
        ArgumentNullException.ThrowIfNull(allocator);
        EnsureIdentityDomainOwner();
        ObjectDisposedException.ThrowIf(m_identityDomainsRetired, this);
        if (!m_identityDomains.TryAdd(allocator.domainId, allocator))
            throw new InvalidOperationException("The Editor identity domain is already registered.");
        return new IdentityDomainLease(this, allocator.domainId);
    }

    /// <summary>
    /// Resolves a domain-qualified runtime identity through the Editor's complete identity-domain set.
    /// </summary>
    /// <param name="identity">
    /// Runtime identity captured from a live Editor target.
    /// </param>
    /// <param name="target">
    /// Receives the current live object, or <see langword="null"/> when its domain or generation is unavailable.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the exact identity resolves in its owning domain.
    /// </returns>
    [ScriptingApiIgnore]
    public bool TryResolveIdentity(
        RuntimeIdentity identity,
        out IdentityObject? target
    ) {
        target = m_identityDomains.TryGetValue(identity.domainId, out IdentityAllocator? allocator) ? allocator.Get<IdentityObject>(identity) : null;
        return target is not null;
    }

    /// <summary>
    /// Resolves a persistent identity through one explicitly selected Editor identity domain.
    /// </summary>
    /// <param name="domainId">
    /// Identity domain that owns the object and any replacement generation.
    /// </param>
    /// <param name="persistentId">
    /// Stable object identity to resolve inside that domain.
    /// </param>
    /// <param name="target">
    /// Receives the current live object, or <see langword="null"/> when the domain or object is unavailable.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the stable identity resolves in the requested domain.
    /// </returns>
    [ScriptingApiIgnore]
    public bool TryResolveIdentity(
        IdentityDomainId domainId,
        Guid persistentId,
        out IdentityObject? target
    ) {
        target = persistentId != Guid.Empty && m_identityDomains.TryGetValue(domainId, out IdentityAllocator? allocator) ? allocator.Get<IdentityObject>(persistentId) : null;
        return target is not null;
    }

    /// <summary>
    /// Opens and requests presentation focus for one panel in the active extension generation.
    /// </summary>
    /// <param name="panelId">
    /// The stable panel identifier to resolve.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an available panel was found.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="panelId"/> is empty.
    /// </exception>
    public bool OpenPanel(string panelId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(panelId);
        return m_catalog?.TryOpenPanel(panelId) == true;
    }

    /// <summary>
    /// Closes one panel in the active extension generation without toggling its current state.
    /// </summary>
    /// <param name="panelId">
    /// The stable panel identifier to resolve.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when an available panel was found.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="panelId"/> is empty.
    /// </exception>
    public bool ClosePanel(string panelId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(panelId);
        return m_catalog?.TryClosePanel(panelId) == true;
    }

    internal void Attach(EditorExtensionCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        m_catalog = catalog;
        m_actions = new EditorActionRouter(catalog, m_editor, this, m_log);
        m_menus = new EditorMenuCatalog(catalog, m_actions, m_log);
        m_toolbars = new EditorToolbarCatalog(catalog, m_actions);
        m_drops = new EditorDropRouter(catalog, m_identityDomains, m_log);
    }

    internal void Update()
    {
        ResolveGenerationTargets();
        Actions.Flush();
    }

    internal void PrepareGenerationTransition()
    {
        if (m_generationTransitionPrepared)
            throw new InvalidOperationException("An interaction generation transition is already active.");
        m_previousGenerationSelection = selection.CaptureReferences();
        m_previousGenerationAnchor = selection.anchorReference;
        m_previousGenerationFocus = m_focus;
        m_generationTransitionPrepared = true;
        Actions.ResetTransientState();
        Drops.Cancel();
    }

    internal void RollbackGenerationTransition()
    {
        Actions.ResetTransientState();
        if (m_previousGenerationSelection is not null)
            selection.Restore(m_previousGenerationSelection, m_previousGenerationAnchor);
        m_focus = m_previousGenerationFocus;
        CompleteGenerationTransition();
    }

    internal void CompleteGenerationTransition()
    {
        Actions.InvalidateDiscovery();
        selection.Rebind();
        m_focus = m_focus?.Rebind(this);
        m_previousGenerationSelection = null;
        m_previousGenerationAnchor = null;
        m_previousGenerationFocus = null;
        m_generationTransitionPrepared = false;
    }

    internal void Shutdown(IReadOnlyList<EditorExtensionCatalog.ActionRegistration> actions)
    {
        Actions.Clear(actions);
        Drops.Cancel();
        m_catalog = null;
        selection.Clear();
        m_focusedArea = EditorBuiltInInteractionIds.C_GLOBAL_AREA;
        m_focus = null;
        m_previousGenerationSelection = null;
        m_previousGenerationAnchor = null;
        m_previousGenerationFocus = null;
        m_documents.Shutdown();
        m_history.Dispose();
        m_identityDomains.Clear();
        m_identityDomainsRetired = true;
    }

    internal bool DispatchShortcut(KeyPressedEvent keyEvent) => Actions.DispatchShortcut(keyEvent, m_focusedArea, focusedTarget ?? selection.selectedTarget);
    internal void Focus(
        string area,
        object? target
    ) {
        m_focusedArea = area;
        m_focus = target is null ? null : EditorTargetReference.Capture(target);
    }

    internal void PrepareSelectionChange(object? target) => Actions.LosePresentationExcept(target);
    internal EditorActionState Query(
        string action,
        string area,
        object? target,
        object? argument
    ) => Actions.Query(action, CreateActionContext(area, target, argument));
    internal bool Execute(
        string action,
        string area,
        object? target,
        object? argument
    ) => Actions.Execute(action, CreateActionContext(area, target, argument));
    internal void Enqueue(
        string action,
        string area,
        object? target,
        object? argument
    ) => Actions.Enqueue(action, CreateActionContext(area, target, argument));
    internal bool Present(
        string action,
        string area,
        object? target,
        object? argument
    ) => Actions.Present(action, CreateActionContext(area, target, argument));
    internal bool IsActive(
        string action,
        string area,
        object? target
    ) => Actions.IsActive(action, CreateActionContext(area, target, null));
    internal EditorMenuModel BuildMenu(
        string area,
        object? target
    ) => Menus.Build(new EditorMenuContext(m_editor, this, area, target));
    internal EditorMenuModel BuildCommands(
        string area,
        object? target
    ) {
        EditorMenuModel menu = BuildMenu(area, target);
        var placed = new HashSet<string>(StringComparer.Ordinal);
        Collect(menu.items);
        IReadOnlyList<EditorMenuItem> additional = Actions.BuildUnplacedCommands(CreateActionContext(area, target, null), placed);
        return new EditorMenuModel([..menu.items, ..additional]);
        void Collect(IReadOnlyList<EditorMenuItem> items)
        {
            foreach (EditorMenuItem item in items)
            {
                if (item.actionId.Length != 0)
                    _ = placed.Add(item.actionId);
                Collect(item.children);
            }
        }
    }

    internal EditorToolbarModel BuildToolbar(
        string area,
        object? target
    ) => Toolbars.Build(CreateActionContext(area, target, argument: null));
    internal bool TryGetShortcut(
        string action,
        string area,
        object? target,
        out HotKeyGesture gesture
    ) => Actions.TryGetShortcut(action, area, target, out gesture);
    internal RuntimeIdentity BeginDrag(
        string area,
        EditorDragData data
    ) {
        if (m_catalog is null)
            throw new InvalidOperationException("Editor interactions are not attached to a runtime.");
        _ = m_catalog.extensions;
        return Drops.Begin(new EditorDragContext(m_editor, this, area, data));
    }

    internal EditorDropStatus QueryDrop(
        RuntimeIdentity identity,
        string area,
        object? target,
        EditorDropPlacement placement
    ) {
        if (target is null || !Drops.TryGetSource(identity, out IdentityObject? source) || source is null)
            return EditorDropStatus.rejected;
        return Drops.Query(identity, new EditorDropContext(m_editor, this, area, new EditorDragData(source, string.Empty), source, target, placement));
    }

    internal EditorDropResult Drop(
        RuntimeIdentity identity,
        string area,
        object? target,
        EditorDropPlacement placement
    ) {
        if (target is null || !Drops.TryGetSource(identity, out IdentityObject? source) || source is null)
            return EditorDropResult.rejected;
        return Drops.Drop(identity, new EditorDropContext(m_editor, this, area, new EditorDragData(source, string.Empty), source, target, placement));
    }

    private EditorActionRouter Actions => m_actions ?? throw new InvalidOperationException("Editor interactions are not attached to a runtime.");
    private EditorMenuCatalog Menus => m_menus ?? throw new InvalidOperationException("Editor interactions are not attached to a runtime.");
    private EditorDropRouter Drops => m_drops ?? throw new InvalidOperationException("Editor interactions are not attached to a runtime.");
    private EditorToolbarCatalog Toolbars => m_toolbars ?? throw new InvalidOperationException("Editor interactions are not attached to a runtime.");

    private EditorActionContext CreateActionContext(
        string area,
        object? target,
        object? argument
    ) => new(m_editor, this, area, target, argument);
    private void ResolveGenerationTargets()
    {
        selection.Rebind();
        m_focus = m_focus?.Rebind(this);
    }
    private void EnsureIdentityDomainOwner()
    {
        if (Environment.CurrentManagedThreadId != m_identityOwnerThread)
            throw new InvalidOperationException("Editor identity domains must be changed on the owning thread.");
    }

    private sealed class IdentityDomainLease(
        EditorInteractions owner,
        IdentityDomainId domain
    ) : IDisposable
    {
        private EditorInteractions? m_owner = owner;
        /// <inheritdoc />
        public void Dispose()
        {
            EditorInteractions? active = m_owner;
            if (active is null)
                return;
            active.EnsureIdentityDomainOwner();
            active.m_identityDomains.Remove(domain);
            m_owner = null;
        }
    }

}
