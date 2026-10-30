using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Inno.Core.IO;
using Inno.Core.Input;
using Inno.Editor.Core;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class EditorLayoutInitializationTests : IDisposable
{
    private readonly string m_owner = Path.Combine(Path.GetTempPath(), "InnoInitialLayoutTests");
    private readonly string m_root;

    public EditorLayoutInitializationTests()
    {
        m_root = PathBoundary.Resolve(m_owner, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(m_root);
    }

    [Fact]
    public void MissingProjectDocumentReceivesOnlyTheStandardLayout()
    {
        string template = ReadTemplate();
        EditorContext context = CreateContext(template);
        Assert.Equal(template, File.ReadAllText(context.layoutPath));
        Assert.False(context.SaveLayoutIfChanged());
        Assert.Equal(["Panels"], context.GetLayoutSectionNames());
        Assert.True(context.TryGetLayoutSection("Panels", out var panels));
        Assert.Equal(8, panels.Count);
        Assert.All(panels.Values, value => Assert.Equal("true", value));
        Assert.DoesNotContain("Module.", template, StringComparison.Ordinal);
        Assert.DoesNotContain("FlappyBird", template, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("Sprite Animation", template, StringComparison.Ordinal);
        Assert.DoesNotContain("Compiling Scripts", template, StringComparison.Ordinal);
        Assert.DoesNotContain("Export as", template, StringComparison.Ordinal);
        Assert.DoesNotContain("Debug##", template, StringComparison.Ordinal);
    }

    [Theory]
    [InlineData("")]
    [InlineData("[Window][User]\nSize=123,456\n\n[InnoEditor][Panels]\nscene.hierarchy=false\n")]
    public void ExistingDocumentIsAuthoritativeIncludingAnIntentionallyEmptyFile(string document)
    {
        string path = Path.Combine(m_root, "editor.ini");
        File.WriteAllText(path, document);
        DateTime timestamp = new(2020, 2, 3, 4, 5, 6, DateTimeKind.Utc);
        File.SetLastWriteTimeUtc(path, timestamp);
        byte[] bytes = File.ReadAllBytes(path);
        EditorContext context = CreateContext(ReadTemplate());
        Assert.Equal(bytes, File.ReadAllBytes(path));
        Assert.Equal(timestamp, File.GetLastWriteTimeUtc(path));
        Assert.False(context.SaveLayoutIfChanged());
        Assert.Equal(bytes, File.ReadAllBytes(path));
    }

    [Fact]
    public void ConcurrentInitializersNeverReplaceTheWinningDocument()
    {
        var layouts = new ConcurrentBag<string>();
        Parallel.For(0, 16, index =>
        {
            string document = $"[Window][Creator {index}]\nSize=400,300\n";
            layouts.Add(CreateContext(document).imguiLayout);
        });
        string winner = CreateContext(null).imguiLayout;
        Assert.All(layouts, value => Assert.Equal(winner, value));
        Assert.Single(Directory.GetFiles(m_root));
    }

    [Fact]
    public void FailedCreationDoesNotLeaveAPartialFileOrStagingPayload()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "editor.ini"));
        Assert.ThrowsAny<IOException>(() => CreateContext(ReadTemplate()));
        Assert.Empty(Directory.GetFiles(m_root));
    }

    [Fact]
    public void OrdinarySaveAndReopenRetainLayoutAndIndependentStateSections()
    {
        EditorContext context = CreateContext(ReadTemplate());
        context.SetLayoutSection("Module.fixture", new Dictionary<string, string> { ["value"] = "42" });
        context.SetImGuiLayout("[Window][User layout]\nPos=10,20\nSize=600,400\n");
        context.SaveLayout();
        byte[] saved = File.ReadAllBytes(context.layoutPath);
        EditorContext reopened = CreateContext(ReadTemplate());
        Assert.Contains("[Window][User layout]", reopened.imguiLayout);
        Assert.True(reopened.TryGetLayoutSection("Module.fixture", out var values));
        Assert.Equal("42", values["value"]);
        Assert.True(reopened.TryGetLayoutSection("Panels", out var panels));
        Assert.Equal(8, panels.Count);
        Assert.Equal(saved, File.ReadAllBytes(reopened.layoutPath));
        Assert.Single(Directory.GetFiles(m_root));
    }

    [Fact]
    public void AContextWithoutACompositionDefaultRemainsPassive()
    {
        EditorContext context = CreateContext(null);
        Assert.False(File.Exists(context.layoutPath));
        Assert.Empty(context.imguiLayout);
        Assert.Empty(context.GetLayoutSectionNames());
    }

    public void Dispose() => Directory.Delete(PathBoundary.RequireUnlinkedPath(m_owner, m_root), recursive: true);

    private EditorContext CreateContext(string? initialDocument) => new(
        m_root, new EditorKeyboardPolicy(KeyModifier.Control, "Ctrl"), initialDocument);

    private static string ReadTemplate() => File.ReadAllText(Path.Combine(AppContext.BaseDirectory, "Fixtures", "DefaultEditorLayout.ini"));
}
