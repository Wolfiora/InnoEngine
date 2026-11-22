using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// O-01 and O-02: members follow the group order, and overloads are adjacent with the fewest parameters first.
// A type is left as written when moving its members could change a layout, a serialized order or an initialization order.
internal sealed class MemberOrderStyleRule : StyleRule
{
    private const string C_ID = "O-01";
    private const int C_CONSTANT = 1;
    private const int C_STATIC_FIELD = 2;
    private const int C_INSTANCE_FIELD = 3;
    private const int C_CONSTRUCTOR = 4;
    private const int C_PUBLIC = 5;
    private const int C_SHARED = 6;
    private const int C_PRIVATE = 7;
    private const int C_NESTED_TYPE = 8;

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Reorder;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (TypeDeclarationSyntax type in source.root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            MemberDeclarationSyntax[]? ordered = Ordered(type);
            if (ordered is null) continue;
            int index = 0;
            while (ordered[index] == type.Members[index])
            {
                index++;
            }

            findings.Add(
                source.Finding(
                    C_ID,
                    type.Members[index].GetFirstToken().SpanStart,
                    "Order the members: constants, static fields, instance fields, constructors, public, protected and internal, "
                    + "private, nested types; keep overloads adjacent with the fewest parameters first."
                )
            );
        }
    }

    internal override IReadOnlyList<StyleEdit> Fix(StyleSource source)
    {
        var edits = new List<StyleEdit>();
        int fixedEnd = -1;
        foreach (TypeDeclarationSyntax type in source.root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            // A nested type inside a type that moves in this round is ordered in the next round.
            if (type.SpanStart < fixedEnd) continue;
            MemberDeclarationSyntax[]? ordered = Ordered(type);
            if (ordered is null || !IsMovable(source, type)) continue;
            int start = type.Members[0].FullSpan.Start;
            int end = type.Members[^1].FullSpan.End;
            edits.Add(new(start, end, Compose(source, type, ordered)));
            fixedEnd = end;
        }

        return edits;
    }

    // Returns the members in rulebook order, or null when they already are or the type must keep its order.
    private static MemberDeclarationSyntax[]? Ordered(TypeDeclarationSyntax type)
    {
        if (type.Members.Count < 2 || type.Members.Any(static member => member is IncompleteMemberSyntax)) return null;
        if (HasDirective(type) || IsNativeInterface(type)) return null;
        bool isLayoutBound = type is StructDeclarationSyntax
                             || type is RecordDeclarationSyntax record && record.ClassOrStructKeyword.IsKind(SyntaxKind.StructKeyword)
                             || HasAttribute(type.AttributeLists, "StructLayout");
        var keys = new Dictionary<MemberDeclarationSyntax, (int group, int kind, int detail)>();
        foreach (MemberDeclarationSyntax member in type.Members)
        {
            keys[member] = Key(member, type, isLayoutBound);
        }

        MemberDeclarationSyntax[] grouped = type.Members
            .OrderBy(member => keys[member].group)
            .ThenBy(member => keys[member].kind)
            .ThenBy(member => keys[member].detail)
            .ToArray();
        MemberDeclarationSyntax[] ordered = Cluster(grouped, keys);
        if (ordered.SequenceEqual(type.Members)) return null;
        if (!Bound(type.Members, isLayoutBound).SequenceEqual(Bound(ordered, isLayoutBound))) return null;
        if (!Initialized(type.Members).SequenceEqual(Initialized(ordered)) && HasInitializationDependency(type))
        {
            return null;
        }
        return ordered;
    }

    private static (int group, int kind, int detail) Key(
        MemberDeclarationSyntax member,
        TypeDeclarationSyntax type,
        bool isLayoutBound
    ) {
        SyntaxTokenList modifiers = member.Modifiers;
        bool isStatic = modifiers.Any(SyntaxKind.StaticKeyword);
        switch (member)
        {
            case FieldDeclarationSyntax when modifiers.Any(SyntaxKind.ConstKeyword):
                return (C_CONSTANT, 0, 0);
            case FieldDeclarationSyntax when isStatic:
                return (C_STATIC_FIELD, modifiers.Any(SyntaxKind.ReadOnlyKeyword) ? 0 : 1, 0);
            case FieldDeclarationSyntax:
                return (C_INSTANCE_FIELD, isLayoutBound || modifiers.Any(SyntaxKind.ReadOnlyKeyword) ? 0 : 1, 0);
            case ConstructorDeclarationSyntax or DestructorDeclarationSyntax:
                return (C_CONSTRUCTOR, 0, 0);
            case BaseTypeDeclarationSyntax or DelegateDeclarationSyntax:
                return (C_NESTED_TYPE, 0, 0);
        }

        int kindRank = member switch
        {
            PropertyDeclarationSyntax or IndexerDeclarationSyntax => 0,
            EventDeclarationSyntax or EventFieldDeclarationSyntax => 1,
            _ => isStatic ? 3 : 2
        };
        bool isExplicit = member is MethodDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
                          or PropertyDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
                          or IndexerDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
                          or EventDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
                          or OperatorDeclarationSyntax { ExplicitInterfaceSpecifier: not null }
                          or ConversionOperatorDeclarationSyntax { ExplicitInterfaceSpecifier: not null };
        if (isExplicit) return (C_PUBLIC, kindRank, 1);
        bool isPublic = modifiers.Any(SyntaxKind.PublicKeyword);
        bool isProtected = modifiers.Any(SyntaxKind.ProtectedKeyword);
        bool isInternal = modifiers.Any(SyntaxKind.InternalKeyword);
        bool isPrivate = modifiers.Any(SyntaxKind.PrivateKeyword);
        if (isPublic) return (C_PUBLIC, kindRank, 0);
        if (isProtected && isInternal) return (C_SHARED, kindRank, 1);
        if (isProtected && isPrivate) return (C_SHARED, kindRank, 3);
        if (isProtected) return (C_SHARED, kindRank, 0);
        if (isInternal) return (C_SHARED, kindRank, 2);
        if (isPrivate) return (C_PRIVATE, kindRank, 0);

        // Without a modifier an interface member is public and any other member is private.
        return type is InterfaceDeclarationSyntax ? (C_PUBLIC, kindRank, 0) : (C_PRIVATE, kindRank, 0);
    }

    // Brings overloads next to the first of them and orders them by parameter count.
    private static MemberDeclarationSyntax[] Cluster(
        MemberDeclarationSyntax[] grouped,
        Dictionary<MemberDeclarationSyntax, (int group, int kind, int detail)> keys
    ) {
        var result = new List<MemberDeclarationSyntax>(grouped.Length);
        var taken = new HashSet<MemberDeclarationSyntax>();
        for (int index = 0; index < grouped.Length; index++)
        {
            MemberDeclarationSyntax member = grouped[index];
            if (!taken.Add(member)) continue;
            string? name = OverloadName(member);
            if (name is null)
            {
                result.Add(member);
                continue;
            }

            var overloads = new List<MemberDeclarationSyntax> { member };
            for (int next = index + 1; next < grouped.Length && keys[grouped[next]] == keys[member]; next++)
            {
                if (OverloadName(grouped[next]) == name && taken.Add(grouped[next])) overloads.Add(grouped[next]);
            }

            result.AddRange(overloads.OrderBy(ParameterCount));
        }

        return result.ToArray();
    }

    private static string? OverloadName(MemberDeclarationSyntax member)
        => member switch
    {
        MethodDeclarationSyntax method => method.ExplicitInterfaceSpecifier + method.Identifier.ValueText,
        ConstructorDeclarationSyntax constructor when !constructor.Modifiers.Any(SyntaxKind.StaticKeyword) => ".ctor",
        OperatorDeclarationSyntax @operator => "operator " + @operator.OperatorToken.Text,
        _ => null
    };

    private static int ParameterCount(MemberDeclarationSyntax member)
        => member is BaseMethodDeclarationSyntax method ? method.ParameterList.Parameters.Count : 0;

    // The instance state whose declaration order is observable: every field and property of a layout-bound type, and
    // every attributed field and property of any other type, because the serializer writes them in declaration order.
    private static IEnumerable<MemberDeclarationSyntax> Bound(
        IEnumerable<MemberDeclarationSyntax> members,
        bool isLayoutBound
    ) => members.Where(
        member => (
            member is FieldDeclarationSyntax or PropertyDeclarationSyntax
            && !member.Modifiers.Any(SyntaxKind.StaticKeyword)
            && !member.Modifiers.Any(SyntaxKind.ConstKeyword)
            && (isLayoutBound || member.AttributeLists.Count > 0)
        )
    );

    // Static initializers run in declaration order.
    private static IEnumerable<MemberDeclarationSyntax> Initialized(IEnumerable<MemberDeclarationSyntax> members)
        => members.Where(
            static member => (
                member.Modifiers.Any(SyntaxKind.StaticKeyword)
                && member switch
                {
                    FieldDeclarationSyntax field => field.Declaration.Variables.Any(
                        static variable => variable.Initializer is not null
                    ),
                    PropertyDeclarationSyntax property => property.Initializer is not null,
                    _ => false
                }
            )
        );

    // A static initializer depends on the order when it reads a member of its own type or creates an instance of it.
    private static bool HasInitializationDependency(TypeDeclarationSyntax type)
    {
        var names = new HashSet<string>(StringComparer.Ordinal) { type.Identifier.ValueText };
        foreach (MemberDeclarationSyntax member in type.Members)
        {
            switch (member)
            {
                case BaseFieldDeclarationSyntax field:
                    names.UnionWith(field.Declaration.Variables.Select(static variable => variable.Identifier.ValueText));
                    break;
                case PropertyDeclarationSyntax property:
                    names.Add(property.Identifier.ValueText);
                    break;
                case MethodDeclarationSyntax method:
                    names.Add(method.Identifier.ValueText);
                    break;
            }
        }

        foreach (MemberDeclarationSyntax member in Initialized(type.Members))
        {
            IEnumerable<EqualsValueClauseSyntax> initializers = member switch
            {
                FieldDeclarationSyntax field => field.Declaration.Variables.Select(static variable => variable.Initializer!),
                PropertyDeclarationSyntax property => [property.Initializer!],
                _ => []
            };
            TypeSyntax? declared = member switch
            {
                FieldDeclarationSyntax field => field.Declaration.Type,
                PropertyDeclarationSyntax property => property.Type,
                _ => null
            };
            foreach (EqualsValueClauseSyntax initializer in initializers.Where(static initializer => initializer is not null))
            {
                bool isOwnInstance = initializer.DescendantNodes().Any(static node => node is ImplicitObjectCreationExpressionSyntax)
                                     && declared?.DescendantNodesAndSelf().OfType<SimpleNameSyntax>().Any(
                                         name => name.Identifier.ValueText == type.Identifier.ValueText
                                     ) == true;
                if (isOwnInstance
                    || initializer.DescendantNodes().OfType<SimpleNameSyntax>().Any(name => names.Contains(name.Identifier.ValueText))
                ) {
                    return true;
                }
            }
        }

        return false;
    }

    private static bool HasDirective(TypeDeclarationSyntax type)
        => type.Members.Any(static member => member.GetLeadingTrivia().Any(static trivia => trivia.IsDirective))
           || type.CloseBraceToken.LeadingTrivia.Any(static trivia => trivia.IsDirective);

    // The member order of a COM or native callback interface is its binary layout.
    private static bool IsNativeInterface(TypeDeclarationSyntax type)
        => type is InterfaceDeclarationSyntax
           && (HasAttribute(type.AttributeLists, "ComImport")
               || HasAttribute(type.AttributeLists, "GeneratedComInterface")
               || HasAttribute(type.AttributeLists, "InterfaceType"));

    private static bool HasAttribute(
        SyntaxList<AttributeListSyntax> lists,
        string name
    ) => lists
        .SelectMany(static list => list.Attributes)
        .Any(
            attribute => (
                attribute.Name.ToString().Split('.')[^1] is string text
                && (text == name || text == name + "Attribute")
            )
        );

    // Every member must own whole lines, so that moving its text moves nothing else.
    private static bool IsMovable(
        StyleSource source,
        TypeDeclarationSyntax type
    ) => type.Members.All(
        member => (
            source.IsFirstOnLine(member.GetFirstToken())
            && source.Column(member.FullSpan.Start) == 0
            && member.FullSpan.End > 0
            && source.content[member.FullSpan.End - 1] == '\n'
        )
    );

    private static string Compose(
        StyleSource source,
        TypeDeclarationSyntax type,
        MemberDeclarationSyntax[] ordered
    ) {
        var builder = new StringBuilder();
        string newLine = source.content.Contains("\r\n", StringComparison.Ordinal) ? "\r\n" : "\n";
        MemberDeclarationSyntax? previous = null;
        foreach (MemberDeclarationSyntax member in ordered)
        {
            string text = member.ToFullString();
            int blankLines = 0;
            int offset = 0;
            while (offset < text.Length)
            {
                int lineEnd = text.IndexOf('\n', offset);
                if (lineEnd < 0 || !string.IsNullOrWhiteSpace(text[offset..lineEnd])) break;
                blankLines++;
                offset = lineEnd + 1;
            }

            text = text[offset..];
            int index = type.Members.IndexOf(member);
            bool hasSamePrevious = index > 0 && type.Members[index - 1] == previous;
            bool isSeparated = previous is not null
                               && (hasSamePrevious
                                   ? blankLines > 0
                                   : !(IsPlainField(previous) && IsPlainField(member) && IsSameGroup(previous, member)));
            if (isSeparated) builder.Append(newLine);
            builder.Append(text);
            previous = member;
        }

        return builder.ToString();
    }

    private static bool IsSameGroup(
        MemberDeclarationSyntax left,
        MemberDeclarationSyntax right
    ) => left.Modifiers.Any(SyntaxKind.ConstKeyword) == right.Modifiers.Any(SyntaxKind.ConstKeyword)
         && left.Modifiers.Any(SyntaxKind.StaticKeyword) == right.Modifiers.Any(SyntaxKind.StaticKeyword);

    // A field written on one line, without documentation, comments or attributes.
    private static bool IsPlainField(MemberDeclarationSyntax member)
        => member is FieldDeclarationSyntax && member.ToFullString().Trim().IndexOf('\n') < 0;
}
