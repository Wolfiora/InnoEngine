using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Inno.Tooling.Architecture;

// NM-02 to NM-13: every declared name follows the naming table. The check reads syntax only, so it leaves the
// names that a contract may dictate: overrides, explicit and possible implicit interface implementations, native
// interop declarations and serialized members.
internal sealed partial class NamingStyleRule : StyleRule
{
    private const string C_ID = "NM-02";
    private const string C_PREFIX = "NM-";

    private static readonly string[] CONTRACT_ATTRIBUTES = ["DllImport", "LibraryImport", "UnmanagedCallersOnly", "JSImport", "JSExport"];

    private static readonly string[] WIRE_ATTRIBUTES =
    [
        "SerializableProperty",
        "JsonPropertyName",
        "JsonInclude",
        "DataMember"
    ];

    // Members that reflection-based JSON models and the engine serializer persist without a marking attribute.
    private static readonly string[] WIRE_NAMES = ["logicalId", "pageId", "processId", "productId", "rootId", "stableTypeId"];

    internal override string id => C_ID;

    internal override StyleRuleKind kind => StyleRuleKind.Rewrite;

    internal override bool Covers(string ruleID) => ruleID.StartsWith(C_PREFIX, StringComparison.Ordinal);

    internal override void Check(
        StyleSource source,
        ICollection<StyleFinding> findings
    ) {
        bool isNative = source.path.Contains("/Inno.Native.", StringComparison.Ordinal);
        foreach (SyntaxNode node in source.root.DescendantNodes())
        {
            switch (node)
            {
                case BaseTypeDeclarationSyntax type when !isNative:
                    CheckType(source, findings, type.Identifier, type is InterfaceDeclarationSyntax);
                    break;
                case DelegateDeclarationSyntax type when !isNative:
                    CheckType(source, findings, type.Identifier, isInterface: false);
                    break;
                case TypeParameterSyntax parameter:
                    if (!TypeParameterName().IsMatch(parameter.Identifier.ValueText))
                    {
                        Add(source, findings, "NM-02", parameter.Identifier, "Name a type parameter `T` or `T` plus a description.");
                    }

                    break;
                case MethodDeclarationSyntax method when !isNative
                && !IsContract(method.Modifiers, method.AttributeLists, method.ExplicitInterfaceSpecifier):
                    CheckMethod(source, findings, method.Identifier, method.ReturnType, method);
                    break;
                case LocalFunctionStatementSyntax function:
                    CheckMethod(source, findings, function.Identifier, function.ReturnType, null);
                    break;
                case PropertyDeclarationSyntax property when !IsContract(
                    property.Modifiers,
                    property.AttributeLists,
                    property.ExplicitInterfaceSpecifier
                ):
                    CheckProperty(source, findings, property);
                    break;
                case EventDeclarationSyntax @event when !IsContract(
                    @event.Modifiers,
                    @event.AttributeLists,
                    @event.ExplicitInterfaceSpecifier
                ):
                    CheckEvent(source, findings, @event.Identifier, @event);
                    break;
                case EventFieldDeclarationSyntax @event when !IsContract(@event.Modifiers, @event.AttributeLists, null):
                    foreach (VariableDeclaratorSyntax variable in @event.Declaration.Variables)
                    {
                        CheckEvent(source, findings, variable.Identifier, @event);
                    }

                    break;
                case FieldDeclarationSyntax field when !isNative:
                    CheckField(source, findings, field);
                    break;
                case EnumMemberDeclarationSyntax member when !isNative:
                    CheckEnumMember(source, findings, member);
                    break;
                case LocalDeclarationStatementSyntax local:
                    foreach (VariableDeclaratorSyntax variable in local.Declaration.Variables)
                    {
                        if (local.IsConst)
                        {
                            CheckConstant(source, findings, variable.Identifier);
                        }
                        else
                        {
                            CheckLocal(source, findings, variable.Identifier);
                        }
                    }

                    break;
                case ParameterSyntax parameter when !isNative && !IsContractParameter(parameter) && !IsWireParameter(parameter):
                    CheckLocal(source, findings, parameter.Identifier);
                    break;
                case SingleVariableDesignationSyntax designation:
                    CheckLocal(source, findings, designation.Identifier);
                    break;
                case ForEachStatementSyntax loop:
                    CheckLocal(source, findings, loop.Identifier);
                    break;
                case CatchDeclarationSyntax @catch:
                    CheckLocal(source, findings, @catch.Identifier);
                    break;
                case TupleElementSyntax element:
                    CheckLocal(source, findings, element.Identifier);
                    break;
                case VariableDeclaratorSyntax { Parent: VariableDeclarationSyntax { Parent: ForStatementSyntax
                                                                                            or UsingStatementSyntax
                                                                                            or FixedStatementSyntax } } variable:
                    CheckLocal(source, findings, variable.Identifier);
                    break;
                case FromClauseSyntax from:
                    CheckLocal(source, findings, from.Identifier);
                    break;
                case LetClauseSyntax let:
                    CheckLocal(source, findings, let.Identifier);
                    break;
                case JoinClauseSyntax join:
                    CheckLocal(source, findings, join.Identifier);
                    break;
                case QueryContinuationSyntax continuation:
                    CheckLocal(source, findings, continuation.Identifier);
                    break;
            }
        }
    }

    private static void CheckType(
        StyleSource source,
        ICollection<StyleFinding> findings,
        SyntaxToken identifier,
        bool isInterface
    ) {
        string name = identifier.ValueText;
        if (!PascalName().IsMatch(name))
        {
            Add(source, findings, "NM-01", identifier, "Name a type in PascalCase.");
        }
        else if (isInterface && !InterfaceName().IsMatch(name))
        {
            Add(source, findings, "NM-02", identifier, "Start an interface name with `I`.");
        }
        else
        {
            CheckAcronym(source, findings, identifier);
        }
    }

    private static void CheckMethod(
        StyleSource source,
        ICollection<StyleFinding> findings,
        SyntaxToken identifier,
        TypeSyntax returnType,
        MethodDeclarationSyntax? method
    ) {
        string name = identifier.ValueText;
        if (!PascalName().IsMatch(name))
        {
            Add(source, findings, "NM-03", identifier, "Name a method in PascalCase.");
            return;
        }

        CheckAcronym(source, findings, identifier);
        if (name.EndsWith("Async", StringComparison.Ordinal) || !IsTask(returnType)) return;
        if (method is not null && (IsTest(method.AttributeLists) || MayImplementContract(method) || name == "Main"))
        {
            return;
        }
        Add(source, findings, "NM-04", identifier, "End the name of a method that returns a task with `Async`.");
    }

    private static void CheckProperty(
        StyleSource source,
        ICollection<StyleFinding> findings,
        PropertyDeclarationSyntax property
    ) {
        string name = property.Identifier.ValueText;
        if (!CamelName().IsMatch(name))
        {
            if (!MayImplementContract(property))
            {
                Add(source, findings, "NM-05", property.Identifier, "Name a property in camelCase.");
            }
            return;
        }

        if (!IsWire(property, property.AttributeLists) && !WIRE_NAMES.Contains(name))
        {
            CheckAcronym(source, findings, property.Identifier);
        }
    }

    private static void CheckEvent(
        StyleSource source,
        ICollection<StyleFinding> findings,
        SyntaxToken identifier,
        MemberDeclarationSyntax @event
    ) {
        if (!CamelName().IsMatch(identifier.ValueText))
        {
            if (!MayImplementContract(@event))
            {
                Add(source, findings, "NM-10", identifier, "Name an event in camelCase.");
            }
            return;
        }

        CheckAcronym(source, findings, identifier);
    }

    private static void CheckField(
        StyleSource source,
        ICollection<StyleFinding> findings,
        FieldDeclarationSyntax field
    ) {
        SyntaxTokenList modifiers = field.Modifiers;
        bool isLayout = field.Parent is TypeDeclarationSyntax owner && HasAttribute(owner.AttributeLists, ["StructLayout"]);
        if (field.Declaration.Variables.Count > 1)
        {
            Add(source, findings, "NM-06", field.Declaration.Variables[1].Identifier, "Declare one field per declaration.");
        }

        foreach (VariableDeclaratorSyntax variable in field.Declaration.Variables)
        {
            SyntaxToken identifier = variable.Identifier;
            string name = identifier.ValueText;
            if (modifiers.Any(SyntaxKind.ConstKeyword))
            {
                CheckConstant(source, findings, identifier);
            }
            else if (modifiers.Any(SyntaxKind.StaticKeyword) && modifiers.Any(SyntaxKind.ReadOnlyKeyword))
            {
                if (!SnakeName().IsMatch(name) || ReservedPrefix().IsMatch(name))
                {
                    Add(source, findings, "NM-08", identifier, "Name a `static readonly` field in UPPER_SNAKE without a prefix.");
                }
            }
            else if (isLayout || IsWire(field, field.AttributeLists))
            {
                continue;
            }
            else if (modifiers.Any(SyntaxKind.StaticKeyword))
            {
                bool isPrivate = IsPrivate(modifiers);
                if (!(isPrivate ? StaticFieldName() : CamelName()).IsMatch(name))
                {
                    Add(source, findings, "NM-09", identifier, "Name a private static mutable field `s_` plus camelCase.");
                }
                else
                {
                    CheckAcronym(source, findings, identifier);
                }
            }
            else
            {
                bool isOwned = IsPrivate(modifiers)
                               || (modifiers.Any(SyntaxKind.ProtectedKeyword) && !modifiers.Any(SyntaxKind.InternalKeyword));
                if (!(isOwned ? InstanceFieldName() : CamelName()).IsMatch(name))
                {
                    Add(
                        source,
                        findings,
                        "NM-06",
                        identifier,
                        isOwned
                        ? "Name a private or protected instance field `m_` plus camelCase."
                        : "Name an internal or public instance field in camelCase."
                    );
                }
                else
                {
                    CheckAcronym(source, findings, identifier);
                }
            }
        }
    }

    private static void CheckEnumMember(
        StyleSource source,
        ICollection<StyleFinding> findings,
        EnumMemberDeclarationSyntax member
    ) {
        string name = member.Identifier.ValueText;
        if (KeyDigitName().IsMatch(name)) return;
        if (!PascalName().IsMatch(name))
        {
            Add(source, findings, "NM-11", member.Identifier, "Name an enum member in PascalCase.");
        }
        else
        {
            CheckAcronym(source, findings, member.Identifier);
        }
    }

    private static void CheckConstant(
        StyleSource source,
        ICollection<StyleFinding> findings,
        SyntaxToken identifier
    ) {
        if (!ConstantName().IsMatch(identifier.ValueText))
        {
            Add(source, findings, "NM-07", identifier, "Name a constant `C_` plus UPPER_SNAKE.");
        }
    }

    private static void CheckLocal(
        StyleSource source,
        ICollection<StyleFinding> findings,
        SyntaxToken identifier
    ) {
        string name = identifier.ValueText;
        if (name.Length == 0 || name.All(static character => character == '_')) return;
        if (!CamelName().IsMatch(name))
        {
            Add(source, findings, "NM-12", identifier, "Name a parameter, local variable or tuple element in camelCase.");
        }
        else
        {
            CheckAcronym(source, findings, identifier);
        }
    }

    private static void CheckAcronym(
        StyleSource source,
        ICollection<StyleFinding> findings,
        SyntaxToken identifier
    ) {
        if (Acronym().IsMatch(ProductName().Replace(identifier.ValueText, "Product")))
        {
            Add(source, findings, "NM-13", identifier, "Write the two-letter acronyms as `ID`, `IO` and `UI`.");
        }
    }

    private static void Add(
        StyleSource source,
        ICollection<StyleFinding> findings,
        string rule,
        SyntaxToken identifier,
        string message
    ) => findings.Add(source.Finding(rule, identifier.SpanStart, message + " Found `" + identifier.ValueText + "`."));

    private static bool IsPrivate(SyntaxTokenList modifiers)
        => modifiers.Any(SyntaxKind.PrivateKeyword)
           || !modifiers.Any(
               static modifier => (
                   modifier.IsKind(SyntaxKind.PublicKeyword)
                   || modifier.IsKind(SyntaxKind.InternalKeyword)
                   || modifier.IsKind(SyntaxKind.ProtectedKeyword)
               )
           );

    private static bool IsTask(TypeSyntax type)
    {
        TypeSyntax unqualified = type is QualifiedNameSyntax qualified ? qualified.Right : type;
        string name = unqualified switch
        {
            GenericNameSyntax generic => generic.Identifier.ValueText,
            IdentifierNameSyntax identifier => identifier.Identifier.ValueText,
            _ => string.Empty
        };
        return name is "Task" or "ValueTask";
    }

    // An override, an explicit interface implementation and a native import keep the name of their contract.
    private static bool IsContract(
        SyntaxTokenList modifiers,
        SyntaxList<AttributeListSyntax> attributes,
        ExplicitInterfaceSpecifierSyntax? explicitInterface
    ) => explicitInterface is not null
         || modifiers.Any(SyntaxKind.OverrideKeyword)
         || modifiers.Any(SyntaxKind.ExternKeyword)
         || HasAttribute(attributes, CONTRACT_ATTRIBUTES);

    private static bool IsContractParameter(ParameterSyntax parameter)
        => parameter.Parent?.Parent switch
    {
        MethodDeclarationSyntax method => IsContract(method.Modifiers, method.AttributeLists, method.ExplicitInterfaceSpecifier),
        IndexerDeclarationSyntax indexer => IsContract(indexer.Modifiers, indexer.AttributeLists, indexer.ExplicitInterfaceSpecifier),
        _ => false
    };

    // A constructor parameter and a positional record parameter carry the name of the persisted member.
    private static bool IsWireParameter(ParameterSyntax parameter)
        => parameter.Parent?.Parent is ConstructorDeclarationSyntax or TypeDeclarationSyntax
           && WIRE_NAMES.Contains(parameter.Identifier.ValueText);

    private static bool IsTest(SyntaxList<AttributeListSyntax> lists)
        => lists
            .SelectMany(static list => list.Attributes)
            .Select(static attribute => attribute.Name.ToString())
            .Any(static name => name.EndsWith("Fact", StringComparison.Ordinal) || name.EndsWith("Theory", StringComparison.Ordinal));

    // Syntax cannot tell an implicit interface implementation from a new member, so a type with a base list is trusted.
    private static bool MayImplementContract(MemberDeclarationSyntax member)
        => member.Parent is BaseTypeDeclarationSyntax { BaseList: not null };

    // The serializer writes the member name as the persisted key.
    private static bool IsWire(
        MemberDeclarationSyntax member,
        SyntaxList<AttributeListSyntax> attributes
    ) => HasAttribute(attributes, WIRE_ATTRIBUTES)
         || (member.Parent is TypeDeclarationSyntax owner && HasAttribute(owner.AttributeLists, ["GenerateSerializationConverter"]));

    private static bool HasAttribute(
        SyntaxList<AttributeListSyntax> lists,
        IReadOnlyCollection<string> names
    ) => lists
        .SelectMany(static list => list.Attributes)
        .Select(static attribute => attribute.Name.ToString().Split('.')[^1])
        .Any(name => names.Contains(name.EndsWith("Attribute", StringComparison.Ordinal) ? name[..^9] : name));

    [GeneratedRegex("^[A-Z][A-Za-z0-9]*$")]
    private static partial Regex PascalName();

    [GeneratedRegex("^[a-z][A-Za-z0-9]*$")]
    private static partial Regex CamelName();

    [GeneratedRegex("^I[A-Z]")]
    private static partial Regex InterfaceName();

    [GeneratedRegex("^T([A-Z0-9][A-Za-z0-9]*)?$")]
    private static partial Regex TypeParameterName();

    [GeneratedRegex("^C_[A-Z0-9]+(_[A-Z0-9]+)*$")]
    private static partial Regex ConstantName();

    [GeneratedRegex("^[A-Z][A-Z0-9]*(_[A-Z0-9]+)*$")]
    private static partial Regex SnakeName();

    [GeneratedRegex("^[CS]_")]
    private static partial Regex ReservedPrefix();

    [GeneratedRegex("^s_[a-z][A-Za-z0-9]*$")]
    private static partial Regex StaticFieldName();

    [GeneratedRegex("^m_[a-z][A-Za-z0-9]*$")]
    private static partial Regex InstanceFieldName();

    [GeneratedRegex("(^|(?<=[A-Za-z0-9_]))(Id|Io|Ui)s?(?![a-z])")]
    private static partial Regex Acronym();

    // The digit keys of a keyboard enum.
    [GeneratedRegex("^_[0-9]$")]
    private static partial Regex KeyDigitName();

    [GeneratedRegex("[Rr]mlUi|[Ii]mGui")]
    private static partial Regex ProductName();
}
