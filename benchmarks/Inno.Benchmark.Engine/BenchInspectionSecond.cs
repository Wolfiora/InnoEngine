using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;

namespace Inno.Benchmark.Engine;

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
