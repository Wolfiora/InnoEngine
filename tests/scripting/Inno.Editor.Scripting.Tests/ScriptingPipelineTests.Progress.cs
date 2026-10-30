using System.Threading.Tasks;
using Inno.Scripting.Compiler;
using Inno.Scripting.Reload;
using Xunit;

namespace Inno.Editor.Scripting.Tests;

public sealed partial class ScriptingPipelineTests
{
    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public void SupersededCompilationCannotPublishIntoTheNextProgressOperation()
    {
        m_fixture.Write("Progress.cs", "public sealed class ProgressPayload { }");
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        reload.RecompileScripting();
        ScriptReloadProgress first = reload.progress;
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? running));
        reload.RecompileScripting();
        ScriptReloadProgress queued = reload.progress;
        Assert.True(queued.operationId > first.operationId);
        Assert.False(reload.CancelCompilation(first.operationId));
        _ = CompleteCompilationOnOwner(running!);
        Assert.Same(queued, reload.progress);
        Assert.Equal(0f, queued.fraction);
        Assert.Contains("queued", queued.stage);
        Assert.False(queued.canCancel);
        Assert.True(reload.TryCompilePending(out Task<ScriptCompilationResult>? replacement));
        Assert.True(CompleteCompilationOnOwner(replacement!).success);
        ScriptReloadProgress compiled = reload.progress;
        Assert.Equal(queued.operationId, compiled.operationId);
        Assert.Equal(0.8f, compiled.fraction);
        Assert.Contains("waiting for generation activation", compiled.stage);
        Assert.False(compiled.canCancel);
        Assert.Equal(0f, queued.fraction);
    }

    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public void CompilationFailureDoesNotPublishSuccessfulWorkflowCompletion()
    {
        m_fixture.Write("InvalidProgress.cs", "public sealed class Broken {");
        using ScriptReloadHost reload = m_fixture.CreateReloadHost();
        reload.Start();
        ScriptCompilationResult result = m_fixture.CompilePending(reload);
        Assert.False(result.success);
        Assert.Contains("failed", reload.progress.stage);
        Assert.False(reload.progress.canCancel);
        Assert.False(reload.progress.cancellationRequested);
    }

    // Reload and fixture retirement must remain on their creating owner thread.
    private static ScriptCompilationResult CompleteCompilationOnOwner(Task<ScriptCompilationResult> compilation) => compilation.GetAwaiter().GetResult();
}
