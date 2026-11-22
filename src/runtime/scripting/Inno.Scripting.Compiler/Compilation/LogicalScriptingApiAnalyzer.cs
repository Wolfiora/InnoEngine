using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Enforces logical scripting namespaces and rejects direct implementation namespace access.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
[SuppressMessage(
    "MicrosoftCodeAnalysisCorrectness",
    "RS1041",
    Justification = "This analyzer is loaded only by the .NET 10 SDK compiler shared by authoring and generated IDE projects."
)]
public sealed class LogicalScriptingApiAnalyzer : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic identifier for direct implementation namespace access.
    /// </summary>
    public const string C_DIRECT_IMPLEMENTATION_NAMESPACE_DIAGNOSTIC_ID = "INNO2001";

    /// <summary>
    /// Diagnostic identifier for a missing logical namespace import.
    /// </summary>
    public const string C_MISSING_LOGICAL_NAMESPACE_DIAGNOSTIC_ID = "INNO2002";

    /// <summary>
    /// Diagnostic identifier for a forbidden compilation-wide namespace import.
    /// </summary>
    public const string C_COMPILATION_WIDE_USING_DIAGNOSTIC_ID = "INNO2003";

    private static readonly DiagnosticDescriptor DIRECT_IMPLEMENTATION_NAMESPACE = new(
        C_DIRECT_IMPLEMENTATION_NAMESPACE_DIAGNOSTIC_ID,
        "Implementation namespace is not part of the scripting API",
        "Use scripting namespace '{0}' instead of implementation namespace '{1}'",
        "Inno Scripting",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor MISSING_LOGICAL_NAMESPACE = new(
        C_MISSING_LOGICAL_NAMESPACE_DIAGNOSTIC_ID,
        "Scripting API namespace is not imported",
        "Type '{0}' requires 'using {1};'",
        "Inno Scripting",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    private static readonly DiagnosticDescriptor COMPILATION_WIDE_USING = new(
        C_COMPILATION_WIDE_USING_DIAGNOSTIC_ID,
        "Compilation-wide namespace imports are not supported",
        "Use an ordinary using directive in each source file",
        "Inno Scripting",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    /// <summary>
    /// Gets the complete compiler diagnostic set enforced by this analyzer.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
        => [DIRECT_IMPLEMENTATION_NAMESPACE, MISSING_LOGICAL_NAMESPACE, COMPILATION_WIDE_USING];

    /// <summary>
    /// Initializes owned resources and establishes the instance's active lifetime.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterCompilationStartAction(startContext =>
        {
            startContext.RegisterSyntaxNodeAction(AnalyzeCompilationWideUsing, SyntaxKind.UsingDirective);
            ScriptApiMap map = ScriptApiMap.Read(startContext.Options.AdditionalFiles, startContext.CancellationToken);
            if (map.namespaces.IsDefaultOrEmpty)
            {
                return;
            }
            startContext.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeUsing(nodeContext, map),
                SyntaxKind.UsingDirective
            );
            startContext.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeTypeReference(nodeContext, map),
                SyntaxKind.IdentifierName,
                SyntaxKind.GenericName
            );
            startContext.RegisterSyntaxNodeAction(
                nodeContext => AnalyzeQualifiedTypeReference(nodeContext, map),
                SyntaxKind.QualifiedName,
                SyntaxKind.AliasQualifiedName
            );
        });
    }

    private static void AnalyzeCompilationWideUsing(SyntaxNodeAnalysisContext context)
    {
        UsingDirectiveSyntax usingDirective = (UsingDirectiveSyntax)context.Node;
        if (usingDirective.GlobalKeyword == default)
        {
            return;
        }
        context.ReportDiagnostic(Diagnostic.Create(COMPILATION_WIDE_USING, usingDirective.GlobalKeyword.GetLocation()));
    }

    private static void AnalyzeUsing(
        SyntaxNodeAnalysisContext context,
        ScriptApiMap map
    ) {
        UsingDirectiveSyntax usingDirective = (UsingDirectiveSyntax)context.Node;
        if (usingDirective.Name is null)
        {
            return;
        }
        string namespaceName = usingDirective.Name.ToString();
        ScriptApiNamespaceMap? mapping = FindByImplementationNamespace(map, namespaceName);
        if (mapping is null)
        {
            return;
        }
        context.ReportDiagnostic(
            Diagnostic.Create(
                DIRECT_IMPLEMENTATION_NAMESPACE,
                usingDirective.Name.GetLocation(),
                mapping.apiNamespace,
                namespaceName
            )
        );
    }

    private static void AnalyzeTypeReference(
        SyntaxNodeAnalysisContext context,
        ScriptApiMap map
    ) {
        if (context.Node.AncestorsAndSelf().OfType<UsingDirectiveSyntax>().Any()
            || context.Node.Ancestors().Any(static ancestor => (
                ancestor is QualifiedNameSyntax or AliasQualifiedNameSyntax
            ))
        ) {
            return;
        }
        ISymbol? symbol = context.SemanticModel.GetSymbolInfo(context.Node, context.CancellationToken).Symbol;
        INamedTypeSymbol? type = symbol switch
        {
            INamedTypeSymbol namedType => namedType,
            IMethodSymbol { MethodKind: MethodKind.Constructor } constructor => constructor.ContainingType,
            _ => null
        };
        if (type is null)
        {
            return;
        }
        string implementationNamespace = type.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        ScriptApiNamespaceMap? mapping = FindByImplementationNamespace(map, implementationNamespace);
        if (mapping is null || HasLogicalUsing(context.Node, mapping.apiNamespace))
        {
            return;
        }
        context.ReportDiagnostic(
            Diagnostic.Create(
                MISSING_LOGICAL_NAMESPACE,
                context.Node.GetLocation(),
                type.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat),
                mapping.apiNamespace
            )
        );
    }

    private static void AnalyzeQualifiedTypeReference(
        SyntaxNodeAnalysisContext context,
        ScriptApiMap map
    ) {
        if (context.Node.AncestorsAndSelf().OfType<UsingDirectiveSyntax>().Any()
            || context.Node.Parent is QualifiedNameSyntax or AliasQualifiedNameSyntax
        ) {
            return;
        }
        if (context.SemanticModel.GetSymbolInfo(context.Node, context.CancellationToken).Symbol
            is not INamedTypeSymbol type
        ) {
            return;
        }
        string implementationNamespace = type.ContainingNamespace?.ToDisplayString() ?? string.Empty;
        ScriptApiNamespaceMap? mapping = FindByImplementationNamespace(map, implementationNamespace);
        if (mapping is null)
        {
            return;
        }
        context.ReportDiagnostic(
            Diagnostic.Create(
                DIRECT_IMPLEMENTATION_NAMESPACE,
                context.Node.GetLocation(),
                mapping.apiNamespace,
                implementationNamespace
            )
        );
    }

    private static ScriptApiNamespaceMap? FindByImplementationNamespace(
        ScriptApiMap map,
        string namespaceName
    ) => map.namespaces
        .SelectMany(mapping => mapping.implementationNamespaces.Select(
            implementation => new { mapping, implementation }
        ))
        .Where(candidate => (
            string.Equals(namespaceName, candidate.implementation, StringComparison.Ordinal)
            || namespaceName.StartsWith(candidate.implementation + ".", StringComparison.Ordinal)
        ))
        .OrderByDescending(static candidate => candidate.implementation.Length)
        .Select(static candidate => candidate.mapping)
        .FirstOrDefault();

    private static bool HasLogicalUsing(
        SyntaxNode node,
        string apiNamespace
    ) {
        var usings = new List<UsingDirectiveSyntax>();
        if (node.SyntaxTree.GetRoot() is CompilationUnitSyntax compilationUnit)
        {
            usings.AddRange(compilationUnit.Usings);
        }
        foreach (BaseNamespaceDeclarationSyntax declaration in node.Ancestors().OfType<BaseNamespaceDeclarationSyntax>())
        {
            usings.AddRange(declaration.Usings);
        }
        return usings.Any(usingDirective => (
            usingDirective.Alias is null
            && usingDirective.Name is not null
            && string.Equals(usingDirective.Name.ToString(), apiNamespace, StringComparison.Ordinal)
        ));
    }
}
