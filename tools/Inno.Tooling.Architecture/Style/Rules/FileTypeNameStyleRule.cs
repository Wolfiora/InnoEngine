using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// F-02: a file is named after its primary type, which comes first. `Type.Part.cs` holds a part of `Type`.
// Another top-level type shares the file only when it is an enum, a delegate or a positional record.
// Whether that companion is used only by the primary type is left to review.
internal sealed class FileTypeNameStyleRule : StyleRule
{
    private const string C_ID = "F-02";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        List<MemberDeclarationSyntax> types = [];
        Collect(((CompilationUnitSyntax)source.root).Members, types);
        if (types.Count == 0) return;

        string stem = Path.GetFileNameWithoutExtension(source.path);
        int separator = stem.IndexOf('.', StringComparison.Ordinal);
        string primary = separator < 0 ? stem : stem[..separator];
        if (!types.Any(type => Name(type) == primary))
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    types[0].SpanStart,
                    $"Name the file after its primary type. `{Path.GetFileName(source.path)}` declares no `{primary}`."
                )
            );
            return;
        }

        if (Name(types[0]) != primary)
        {
            findings.Add(
                source.Finding(
                    C_ID,
                    types[0].SpanStart,
                    $"Put the primary type `{primary}` first. Found `{Name(types[0])}`."
                )
            );
        }

        foreach (MemberDeclarationSyntax type in types)
        {
            if (Name(type) == primary || IsCompanion(type)) continue;
            findings.Add(
                source.Finding(
                    C_ID,
                    type.SpanStart,
                    $"Move `{Name(type)}` to its own file. Only an enum, a delegate or a positional record shares a file."
                )
            );
        }
    }

    private static void Collect(
        SyntaxList<MemberDeclarationSyntax> members,
        List<MemberDeclarationSyntax> types
    ) {
        foreach (MemberDeclarationSyntax member in members)
        {
            if (member is BaseNamespaceDeclarationSyntax scope)
            {
                Collect(scope.Members, types);
            }
            else if (member is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax)
            {
                types.Add(member);
            }
        }
    }

    private static string Name(MemberDeclarationSyntax type)
        => type is DelegateDeclarationSyntax callback
           ? callback.Identifier.ValueText
           : ((BaseTypeDeclarationSyntax)type).Identifier.ValueText;

    private static bool IsCompanion(MemberDeclarationSyntax type)
        => type is EnumDeclarationSyntax or DelegateDeclarationSyntax or RecordDeclarationSyntax { ParameterList: not null };
}
