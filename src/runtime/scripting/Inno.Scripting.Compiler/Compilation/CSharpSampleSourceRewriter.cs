using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Inno.Assets.Pipeline;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Gives imported C# sample scripts distinct type identities before their scene assets are remapped.
/// </summary>
[AssetSampleSourceRewriter("inno.scripting.csharp-sample")]
public sealed class CSharpSampleSourceRewriter : IAssetSampleSourceRewriter
{
    /// <summary>
    /// Rewrites explicit stable type identities in cloned C# scripts and registers their asset mappings.
    /// </summary>
    /// <param name="context">
    /// The private sample import stage and its identity map.
    /// </param>
    public void Transform(AssetSampleTransformContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        foreach (string path in Directory.GetFiles(context.stagedRoot, "*.cs", SearchOption.AllDirectories)
            .Order(StringComparer.Ordinal)
        ) {
            context.cancellationToken.ThrowIfCancellationRequested();
            string relative = Path.GetRelativePath(context.stagedRoot, path).Replace('\\', '/');
            if (!context.TryGetSourceIdentity(relative, out Guid oldSourceID, out Guid newSourceID))
            {
                throw new InvalidDataException($"Sample C# source '{relative}' has no asset identity metadata.");
            }
            context.MapType(
                ScriptTypeIdentity.CreateCanonical(oldSourceID),
                ScriptTypeIdentity.CreateCanonical(newSourceID)
            );

            byte[] bytes = File.ReadAllBytes(path);
            bool hasBom = bytes.AsSpan().StartsWith(Encoding.UTF8.GetPreamble());
            string source = File.ReadAllText(path, Encoding.UTF8);
            SyntaxNode root = CSharpSyntaxTree.ParseText(
                source,
                cancellationToken: context.cancellationToken
            ).GetRoot(context.cancellationToken);
            AttributeSyntax[] attributes = root.DescendantNodes()
                .OfType<AttributeSyntax>()
                .Where(static attribute => IsStableTypeID(attribute.Name.ToString()))
                .ToArray();
            if (attributes.Length == 0)
            {
                continue;
            }
            SyntaxNode rewritten = root.ReplaceNodes(attributes, (
                original,
                _
            ) =>
            {
                AttributeSyntax attribute = (AttributeSyntax)original;
                if (attribute.ArgumentList?.Arguments is not { Count: 1 } arguments
                    || arguments[0].Expression is not LiteralExpressionSyntax literal
                    || !literal.IsKind(SyntaxKind.StringLiteralExpression)
                    || !Guid.TryParse(literal.Token.ValueText, out Guid oldID)
                ) {
                    throw new InvalidDataException(
                        $"Sample C# source '{relative}' has a StableTypeID that cannot be cloned safely."
                    );
                }
                Guid newID = context.identityMap.TryGetValue(oldID, out Guid mapped)
                             ? mapped
                             : Guid.NewGuid();
                context.MapType(oldID, newID);
                LiteralExpressionSyntax replacement = SyntaxFactory
                    .LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(newID.ToString("D")))
                    .WithTriviaFrom(literal);
                return attribute.WithArgumentList(
                    attribute.ArgumentList.WithArguments(
                        SyntaxFactory.SingletonSeparatedList(arguments[0].WithExpression(replacement))
                    )
                );
            });
            File.WriteAllText(path, rewritten.ToFullString(), new UTF8Encoding(hasBom));
        }
    }

    private static bool IsStableTypeID(string name) => name.Split('.').Last() is "StableTypeID" or "StableTypeIDAttribute";
}
