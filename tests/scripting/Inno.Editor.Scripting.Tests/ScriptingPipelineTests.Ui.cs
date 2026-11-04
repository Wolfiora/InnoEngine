using Inno.Scripting.Compiler;
using Xunit;

namespace Inno.Editor.Scripting.Tests;

public sealed partial class ScriptingPipelineTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void GenericUiContractsCompileInAuthoringAndDeploymentGraphs(bool runtimeDeployment)
    {
        m_fixture.Write("UiContractConsumer.cs", """
            using InnoEngine.Input;
            using InnoEngine.Mathematics;
            using InnoEngine.UI;
            
            public static class UiContractConsumer
            {
                public static UiLayoutSnapshot Inspect(
                    IUiService service,
                    UiContextHandle context,
                    UiDocumentHandle document
                ) {
                    service.SetTimePaused(context, true);
                    service.AdvanceTime(context, 0.05);
                    _ = service.GetTime(context);
                    _ = service.SetValue(context, document, "name", UiControlValue.Text("editable"));
                    _ = service.FocusElement(context, document, "name");
                    _ = service.GetFocusedElement(context);
                    var changes = new InputSequence([
                        new PointerMoveInputChange(new Vector2(1, 2), new Vector2(3, 4), KeyModifier.None),
                        new KeyInputChange(KeyCode.LeftArrow, true, false, KeyModifier.None),
                        new TextInputChange("A", KeyModifier.None)
                    ]);
                    service.ProcessInput(context, new UiInputSnapshot(new Vector2(3, 4), KeyModifier.None, changes));
                    _ = service.ClearFocus(context);
                    return service.CaptureLayout(context, document, ["name"]);
                }
            }
            
            """);
        ScriptCompilationResult result = runtimeDeployment
            ? m_fixture.CompileRuntimeDeployment() : m_fixture.Compile();
        Assert.True(result.success, FormatDiagnostics(result));
    }
}
