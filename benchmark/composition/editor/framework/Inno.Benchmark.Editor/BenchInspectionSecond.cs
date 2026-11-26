using System;
using Inno.Editor.Inspection;

namespace Inno.Benchmark.Editor;

[InspectionDrawer(typeof(BenchInspectionTarget), conditional: true)]
public sealed class BenchInspectionSecond : InspectionDrawer<BenchInspectionTarget>
{
    public override string icon => string.Empty;

    protected override bool CanInspect(BenchInspectionTarget target) => target.kind == 2;

    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        BenchInspectionTarget target
    ) => ("Second", null);

    protected override void Draw(
        InspectionDrawContext context,
        BenchInspectionTarget target
    ) {}
}
