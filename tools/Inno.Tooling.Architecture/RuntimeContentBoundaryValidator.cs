using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

internal static class RuntimeContentBoundaryValidator
{
    private static readonly HashSet<string> PhysicalIoTypes = new(StringComparer.Ordinal)
    {
        "System.IO.File", "System.IO.Directory", "System.IO.Path", "System.IO.FileStream",
        "System.IO.FileInfo", "System.IO.DirectoryInfo", "Inno.Core.IO.AtomicFile",
        "Inno.Core.IO.AtomicDirectory", "Inno.Core.IO.FileLease", "Inno.Core.IO.FileByteDocumentStore"
    };

    internal static bool ShouldAudit(string relative)
        => IsContentOwner(relative) || relative.StartsWith("build/", StringComparison.Ordinal)
            || relative.EndsWith("/EditorHost.cs", StringComparison.Ordinal);

    internal static void Validate(
        string relative,
        SemanticModel? model,
        ICollection<string> failures
    ) {
        if (model is null)
            return;
        bool contentOwner = IsContentOwner(relative);
        bool compositionOwner = relative.StartsWith("build/composition/Inno.Build.Composition/", StringComparison.Ordinal);
        foreach (SyntaxNode expression in model.SyntaxTree.GetRoot().DescendantNodes().Where(static node =>
                     node is InvocationExpressionSyntax or ObjectCreationExpressionSyntax or ImplicitObjectCreationExpressionSyntax))
        {
            ISymbol? symbol = model.GetSymbolInfo(expression).Symbol;
            string? type = symbol?.ContainingType?.ToDisplayString();
            if (contentOwner && type is not null && PhysicalIoTypes.Contains(type))
                failures.Add($"{relative}: shared content owners must receive reading contracts instead of calling {type}.");
            if (!compositionOwner && (expression is ObjectCreationExpressionSyntax or ImplicitObjectCreationExpressionSyntax)
                && (type is "Inno.Build.Windows.WindowsX64GameBuildTarget"
                    or "Inno.Build.MacOS.MacOSArm64GameBuildTarget"
                    or "Inno.Build.Browser.BrowserWasm32GameBuildTarget"))
                failures.Add($"{relative}: built-in targets must be registered by the shared build composition.");
        }
    }

    private static bool IsContentOwner(string relative)
        => relative.StartsWith("src/content/deployment/Inno.Content/", StringComparison.Ordinal)
            || relative.StartsWith("src/composition/player/Inno.Player.Runtime/", StringComparison.Ordinal)
            || relative.StartsWith("src/foundation/extensibility/Inno.Extensibility.Modules/", StringComparison.Ordinal)
            || relative.StartsWith("src/content/assets/Inno.Assets/Runtime/AssetDatabase", StringComparison.Ordinal)
            || relative.StartsWith("src/runtime/engine/Inno.Runtime/Hosting/RuntimeSession", StringComparison.Ordinal);
}
