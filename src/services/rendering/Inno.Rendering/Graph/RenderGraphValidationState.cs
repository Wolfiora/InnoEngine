namespace Inno.Rendering;

internal sealed record RenderGraphValidationState(
    RenderGraphValidationResult result,
    int[] schedule
);
