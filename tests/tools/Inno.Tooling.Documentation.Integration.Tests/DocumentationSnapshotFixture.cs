using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Integration.Tests;

public abstract class DocumentationSnapshotFixture : IDisposable
{
    protected const string C_SYMBOL = "M:Inno.Probe.Read(System.String)";
    protected readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDocumentationSnapshots", Guid.NewGuid().ToString("N"));

    protected DocumentationSnapshotFixture()
    {
        byte[] projects = JsonSerializer.SerializeToUtf8Bytes(new[]
        {
            new
            {
                name = "Inno.Probe",
                path = "src/Inno.Probe.csproj",
                entry = "probe",
                surfaces = new[] { "host-runtime", "runtime-script" }
            }
        });
        Write("reference/projects/catalog.json", Encoding.UTF8.GetString(projects));
        Write(
            "reference/engine.lock.json",
            JsonSerializer.Serialize(new
            {
                repository = "https://github.com/Wolfiora/InnoEngine",
                revision = new string('a', 40),
                sourceTree = new string('b', 40),
                projectManifest = "reference/projects/catalog.json",
                projectManifestIdentity = Hash(projects),
                toolingRevision = new string('a', 40)
            })
        );
        Write(
            "content/en/probe.md",
            Page(
                "probe",
                "Probe",
                "## Ownership\n\nBorrow the lease; the owner releases it after the final consumer.\n",
                "kind: project\nowners: [Inno.Probe]\naliases: [ProbeStyle, 控制台颜色]\n"
            )
        );
        Write("content/en/api.md", Api("probe-api", "host-runtime"));
    }

    public void Dispose()
    {
        // All test-owned sessions are disposed before removing this uniquely allocated fixture boundary.
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
    }

    protected string source => Path.Combine(m_root, "source");

    protected string cache => Path.Combine(m_root, "artifacts", "documentation");

    protected Task<DocumentationResult> IndexAsync() => DocumentationStore.IndexAsync(m_root, source, cache);

    protected void Write(
        string path,
        string text
    ) {
        string target = Path.Combine(source, path);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        File.WriteAllText(target, text, new UTF8Encoding(false));
    }

    protected Dictionary<string, string> Files()
        => Directory.GetFiles(cache, "*", SearchOption.AllDirectories)
            .Where(static path => !path.EndsWith(".lock", StringComparison.Ordinal))
            .ToDictionary(path => Path.GetRelativePath(cache, path), path => Hash(File.ReadAllBytes(path)), StringComparer.Ordinal);

    protected static string Hash(byte[] bytes) => Convert.ToHexStringLower(SHA256.HashData(bytes));

    protected static string Page(
        string uid,
        string title,
        string body,
        string metadata = "kind: explanation\n"
    ) => "---\nuid: " + uid + "\ntitle: " + title + "\n" + metadata + "---\n# " + title + "\n\n" + body;

    protected static string Api(
        string uid,
        string surface
    ) => Page(
        uid,
        "Probe API",
        "## Read\n\n<!-- api: " + C_SYMBOL + " " + new string('a', 64)
        + " -->\n\n| Declaration | Contract |\n| --- | --- |\n| <a id=\"api-" + Hash(Encoding.UTF8.GetBytes(C_SYMBOL))[..16]
        + "\"></a>`Read(string key)` | Returns the value; caller owns the lease. |\n",
        "kind: api\nowners: [Inno.Probe]\napiSurface: " + surface + "\ncontracts: [probe#ownership]\n"
    );
}
