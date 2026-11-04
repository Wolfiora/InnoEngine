using System;

using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Extensibility.Types;
using Xunit;

namespace Inno.Editor.Interactions.Tests;

public sealed partial class EditorRuntimeTests
{
    [Fact]
    public void ConstructorFailureRetiresEarlierCandidateInstancesAndPreservesActiveRuntime()
    {
        var service = new ActivationProbeService();
        using EditorInteractionRuntime rejected = CreateRuntime(
            new EditorContext(m_projectRoot, new EditorKeyboardPolicy(KeyModifier.Control, "Super")),
            service);
        ExtensionActivationException failure = Assert.Throws<ExtensionActivationException>(rejected.Start);
        Assert.Equal(m_types.current.GetTypeRef(typeof(BRejectedActivationModule)).stableId, failure.typeId);
        Assert.Contains("Candidate construction rejected", failure.ToString());
        Assert.Equal(1, service.created);
        Assert.Equal(1, service.disposed);
        Assert.Contains(m_runtime.panels, static panel => panel.id == "tests.panel");
        service.reject = false;
        rejected.Start();
        Assert.Equal(2, service.created);
        Assert.Equal(1, service.disposed);
        rejected.Dispose();
        Assert.Equal(2, service.disposed);
    }
}

public sealed class ActivationProbeService
{
    public int created;
    public int disposed;
    public bool reject = true;
}

[EditorModule("tests.activation-owned")]
public sealed class AOwnedActivationModule : EditorModule
{
    private readonly ActivationProbeService m_service;

    public AOwnedActivationModule(ActivationProbeService service)
    {
        m_service = service;
        service.created++;
    }

    protected override void OnDispose() => m_service.disposed++;
}

[EditorModule("tests.activation-rejected")]
public sealed class BRejectedActivationModule : EditorModule
{
    public BRejectedActivationModule(ActivationProbeService service)
    {
        if (service.reject)
            throw new InvalidOperationException("Candidate construction rejected.");
    }
}
