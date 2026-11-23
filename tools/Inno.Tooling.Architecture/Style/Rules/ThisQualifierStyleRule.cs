using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// E-08: a constructor assigns an instance member that is not an `m_` field through `this.`, and no other member
// writes `this.` unless a parameter or local variable hides the name. The check reads syntax only, so it knows
// the members that the same type declaration declares.
internal sealed class ThisQualifierStyleRule : StyleRule
{
    private const string C_ID = "E-08";
    private const string C_OWNER_PREFIX = "m_";

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        foreach (TypeDeclarationSyntax type in source.root.DescendantNodes().OfType<TypeDeclarationSyntax>())
        {
            HashSet<string> instanceMembers = InstanceMembers(type);
            HashSet<string> members = type.Members.SelectMany(Names).ToHashSet(StringComparer.Ordinal);
            foreach (MemberDeclarationSyntax member in type.Members)
            {
                if (member is BaseTypeDeclarationSyntax or DelegateDeclarationSyntax) continue;
                HashSet<string> hidden = Hidden(member);
                if (member is ConstructorDeclarationSyntax constructor && !constructor.Modifiers.Any(SyntaxKind.StaticKeyword))
                {
                    CheckConstructor(source, findings, constructor, instanceMembers, hidden);
                }
                else
                {
                    CheckMember(source, findings, member, members, hidden);
                }
            }
        }
    }

    private static void CheckConstructor(
        StyleSource source,
        ICollection<StyleFinding> findings,
        ConstructorDeclarationSyntax constructor,
        HashSet<string> instanceMembers,
        HashSet<string> hidden
    ) {
        foreach (AssignmentExpressionSyntax assignment in constructor.DescendantNodes().OfType<AssignmentExpressionSyntax>())
        {
            if (assignment.Parent is InitializerExpressionSyntax) continue;
            IEnumerable<ExpressionSyntax> targets = assignment.Left is TupleExpressionSyntax tuple
                                                    ? tuple.Arguments.Select(static argument => argument.Expression)
                                                    : [assignment.Left];
            foreach (IdentifierNameSyntax target in targets.OfType<IdentifierNameSyntax>())
            {
                string name = target.Identifier.ValueText;
                if (!instanceMembers.Contains(name) || hidden.Contains(name)) continue;
                findings.Add(
                    source.Finding(
                        C_ID,
                        target.SpanStart,
                        "Assign an instance member through `this.` inside a constructor. Found `" + name + "`."
                    )
                );
            }
        }
    }

    private static void CheckMember(
        StyleSource source,
        ICollection<StyleFinding> findings,
        MemberDeclarationSyntax member,
        HashSet<string> members,
        HashSet<string> hidden
    ) {
        foreach (MemberAccessExpressionSyntax access in member.DescendantNodes().OfType<MemberAccessExpressionSyntax>())
        {
            if (access.Expression is not ThisExpressionSyntax) continue;
            string name = access.Name.Identifier.ValueText;
            if (!members.Contains(name) || hidden.Contains(name)) continue;
            findings.Add(
                source.Finding(
                    C_ID,
                    access.SpanStart,
                    "Write `this.` only inside a constructor or to disambiguate. Found `this." + name + "`."
                )
            );
        }
    }

    private static HashSet<string> InstanceMembers(TypeDeclarationSyntax type)
        => type.Members
            .Where(
                static member => (
                    member is FieldDeclarationSyntax or PropertyDeclarationSyntax or EventFieldDeclarationSyntax
                    && !member.Modifiers.Any(SyntaxKind.StaticKeyword)
                    && !member.Modifiers.Any(SyntaxKind.ConstKeyword)
                )
            )
            .SelectMany(Names)
            .Where(static name => !name.StartsWith(C_OWNER_PREFIX, StringComparison.Ordinal))
            .ToHashSet(StringComparer.Ordinal);

    private static IEnumerable<string> Names(MemberDeclarationSyntax member)
        => member switch
    {
        BaseFieldDeclarationSyntax field => field.Declaration.Variables.Select(static variable => variable.Identifier.ValueText),
        PropertyDeclarationSyntax property => [property.Identifier.ValueText],
        EventDeclarationSyntax @event => [@event.Identifier.ValueText],
        MethodDeclarationSyntax method => [method.Identifier.ValueText],
        _ => []
    };

    // The parameters and local variables that a member declares anywhere in its body can hide a member name.
    private static HashSet<string> Hidden(MemberDeclarationSyntax member)
    {
        var names = new HashSet<string>(StringComparer.Ordinal);
        foreach (SyntaxNode node in member.DescendantNodes())
        {
            switch (node)
            {
                case ParameterSyntax parameter:
                    names.Add(parameter.Identifier.ValueText);
                    break;
                case VariableDeclaratorSyntax variable:
                    names.Add(variable.Identifier.ValueText);
                    break;
                case SingleVariableDesignationSyntax designation:
                    names.Add(designation.Identifier.ValueText);
                    break;
                case ForEachStatementSyntax loop:
                    names.Add(loop.Identifier.ValueText);
                    break;
                case LocalFunctionStatementSyntax function:
                    names.Add(function.Identifier.ValueText);
                    break;
            }
        }

        return names;
    }
}
