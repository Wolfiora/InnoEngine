using System;
using Inno.Editor.Interactions;

namespace Inno.Editor.Core.Tests;

public sealed class TestDocumentProvider : EditorDocumentProvider
{
    public int openCount { get; private set; }
    public int saveCount { get; private set; }
    public int closeCount { get; private set; }
    public override string id => "tests.documents";

    public override bool CanOpen(string assetPath)
        => assetPath.EndsWith(".ispriteatlas2d", StringComparison.Ordinal)
           || assetPath.EndsWith(".itilemap2d", StringComparison.Ordinal);

    public override void Open(EditorDocumentContext context) => openCount++;

    public override bool Save(EditorDocumentContext context)
    {
        saveCount++;
        return true;
    }

    public override void Close(EditorDocumentContext context) => closeCount++;
}
