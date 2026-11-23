using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// T-04: a record struct is readonly.
internal sealed class RecordStructStyleRule : StyleRule
{
    private const string C_ID = "T-04";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (TypeDeclarationSyntax type in source.root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            if (IsMutableRecordStruct(type))
            {
                findings.Add(source.Finding(C_ID, type.Identifier.SpanStart, "Declare a record struct as `readonly record struct`."));
            }
        }
    }

    private static bool IsMutableRecordStruct(TypeDeclarationSyntax type)
        => type is RecordDeclarationSyntax record
           && record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)
           && !type.Modifiers.Any(SyntaxKind.ReadOnlyKeyword)
           && !HasSerializedSetter(type);

    // The serializer writes a struct through its property setters and skips init-only ones, so such a struct stays mutable.
    private static bool HasSerializedSetter(TypeDeclarationSyntax type)
        => type.Members
            .OfType<PropertyDeclarationSyntax>()
            .Any(
                static property => (
                    property.AccessorList is not null
                    && property.AccessorList.Accessors.Any(SyntaxKind.SetAccessorDeclaration)
                )
            );
}
