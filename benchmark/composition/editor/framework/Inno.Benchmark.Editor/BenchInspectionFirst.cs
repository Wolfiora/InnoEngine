using System;
using Inno.Editor.Inspection;

namespace Inno.Benchmark.Editor;

[InspectionDrawer(typeof(BenchInspectionTarget), conditional: true)]
public sealed class BenchInspectionFirst : InspectionDrawer<BenchInspectionTarget>
{
    public override string icon => string.Empty;

    protected override bool CanInspect(BenchInspectionTarget target) => target.kind == 1;

    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        BenchInspectionTarget target
    ) => ("First", null);

    protected override void Draw(
        InspectionDrawContext context,
        BenchInspectionTarget target
    ) {}
}
