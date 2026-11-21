using System.Collections.Generic;

namespace Inno.Tooling.Architecture;

internal sealed record StyleReport(
    int files,
    int changedFiles,
    IReadOnlyList<StyleFinding> findings,
    IReadOnlyList<StyleRejection> rejections
);
