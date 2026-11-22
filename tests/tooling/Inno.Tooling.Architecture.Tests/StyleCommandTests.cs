using System;
using System.IO;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class StyleCommandTests
{
    [Theory]
    [MemberData(nameof(Fixes))]
    public void FixRewritesTheReportedViolationAndLeavesNoFinding(
        string rule,
        string before,
        string after
    ) {
        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, "src", "Probe.cs");
            string report = Path.Combine(root, "report.tsv");
            File.WriteAllText(path, before);

            Assert.Equal(1, StyleCommand.Execute(["check", root, "--rule", rule, "--report", report]));
            Assert.StartsWith(rule + "\tsrc/Probe.cs\t", File.ReadAllText(report), StringComparison.Ordinal);
            Assert.Equal(before, File.ReadAllText(path));

            Assert.Equal(0, StyleCommand.Execute(["fix", root]));
            Assert.Equal(after, File.ReadAllText(path));
            Assert.Equal(0, StyleCommand.Execute(["check", root]));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [MemberData(nameof(Kept))]
    public void ExemptConstructsAreNeitherReportedNorRewritten(
        string rule,
        string text
    ) {
        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, "src", "Probe.cs");
            File.WriteAllText(path, text);

            Assert.Equal(0, StyleCommand.Execute(["check", root, "--rule", rule]));
            Assert.Equal(0, StyleCommand.Execute(["fix", root]));
            Assert.Equal(text, File.ReadAllText(path));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("T-03", "internal class Probe;")]
    [InlineData("T-03", "internal record Probe(int value);")]
    [InlineData("T-04", "internal record struct Probe(int value);")]
    [InlineData("T-01", "partial class Probe;")]
    [InlineData("N-01", "namespace Probe\n{\n    internal sealed class Sample;\n}")]
    [InlineData("N-03", "global using System;")]
    [InlineData("N-03", "namespace Probe;\n\nusing System;")]
    [InlineData("N-03", "internal sealed class Probe\n{\n    private readonly global::System.Text.StringBuilder m_text = new();\n}")]
    [InlineData("NM-01", "internal sealed class probe;")]
    [InlineData("NM-02", "internal interface Probe;")]
    [InlineData("NM-02", "internal sealed class Probe<Value>;")]
    [InlineData("NM-03", "internal sealed class Probe\n{\n    internal void run() {}\n}")]
    [InlineData("NM-04", "internal sealed class Probe\n{\n    internal Task Run() => Task.CompletedTask;\n}")]
    [InlineData("NM-05", "internal sealed class Probe\n{\n    internal int Count => 0;\n}")]
    [InlineData("NM-06", "internal sealed class Probe\n{\n    private int count;\n}")]
    [InlineData("NM-06", "internal sealed class Probe\n{\n    internal int m_count;\n}")]
    [InlineData("NM-06", "internal sealed class Probe\n{\n    private int m_first, m_second;\n}")]
    [InlineData("NM-07", "internal sealed class Probe\n{\n    private const int LIMIT = 1;\n}")]
    [InlineData("NM-07", "internal sealed class Probe\n{\n    internal void Run()\n    {\n        const int limit = 1;\n    }\n}")]
    [InlineData("NM-08", "internal sealed class Probe\n{\n    private static readonly int[] S_SIZES = [];\n}")]
    [InlineData("NM-08", "internal sealed class Probe\n{\n    private static readonly int[] sizes = [];\n}")]
    [InlineData("NM-09", "internal sealed class Probe\n{\n    private static int m_next;\n}")]
    [InlineData("NM-10", "internal sealed class Probe\n{\n    internal event Action Changed;\n}")]
    [InlineData("NM-11", "internal enum Probe\n{\n    first\n}")]
    [InlineData("NM-12", "internal sealed class Probe\n{\n    internal void Run(int Count) {}\n}")]
    [InlineData("NM-12", "internal sealed class Probe\n{\n    internal (int Width, int height) size => (0, 0);\n}")]
    [InlineData("NM-13", "internal sealed class UiProbe;")]
    [InlineData("NM-13", "internal sealed class Probe\n{\n    internal int assetId => 0;\n}")]
    [InlineData("NM-13", "internal sealed class Probe\n{\n    internal void Run(int[] assetIds) {}\n}")]
    public void FindingsThatNeedTheAuthorAreReportedAndLeftUnchanged(
        string rule,
        string text
    ) {
        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, "src", "Probe.cs");
            string report = Path.Combine(root, "report.tsv");
            File.WriteAllText(path, Source(text));

            Assert.Equal(1, StyleCommand.Execute(["check", root, "--rule", rule, "--report", report]));
            Assert.StartsWith(rule + "\tsrc/Probe.cs\t", File.ReadAllText(report), StringComparison.Ordinal);
            Assert.Equal(1, StyleCommand.Execute(["fix", root, "--rule", rule]));
            Assert.Equal(Source(text), File.ReadAllText(path));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("src/Generated/Probe.cs", "internal sealed class Probe { }\n")]
    [InlineData("src/Probe.cs", "// <auto-generated/>\ninternal sealed class Probe { }\n")]
    [InlineData("src/Probe.cs", "[GeneratedCode(\"Tool\", \"1\")]\ninternal sealed class Probe { }\n")]
    [InlineData("extern/Probe.cs", "internal sealed class Probe { }\n")]
    [InlineData("src/obj/Probe.cs", "internal sealed class Probe { }\n")]
    public void GeneratedAndThirdPartySourcesAreNeitherReportedNorRewritten(
        string relativePath,
        string content
    ) {
        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content);

            Assert.Equal(0, StyleCommand.Execute(["check", root]));
            Assert.Equal(0, StyleCommand.Execute(["fix", root]));
            Assert.Equal(content, File.ReadAllText(path));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("// <auto-generated/>")]
    [InlineData("[GeneratedCode(\\\"Tool\\\", \\\"1\\\")]")]
    public void SourcesThatOnlyWriteAGeneratedMarkerAreHandwritten(string marker)
    {
        string content =
            "internal sealed class Probe\n{\n    internal string Run() { return \"" + marker + "\"; }\n}\n";

        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, "src", "Probe.cs");
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            File.WriteAllText(path, content);

            Assert.Equal(1, StyleCommand.Execute(["check", root]));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void ARuleFilterReportsOnlyTheRequestedRuleOfARuleFamily()
    {
        string root = CreateRoot();
        try
        {
            string report = Path.Combine(root, "report.tsv");
            File.WriteAllText(
                Path.Combine(root, "src", "Probe.cs"),
                Source("internal sealed class Probe\n{\n    internal int Count => 0;\n\n    internal int assetId => 0;\n}")
            );

            Assert.Equal(1, StyleCommand.Execute(["check", root, "--rule", "NM-13", "--report", report]));
            string line = Assert.Single(File.ReadAllLines(report));
            Assert.StartsWith("NM-13\tsrc/Probe.cs\t5\t", line, StringComparison.Ordinal);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void FixKeepsTextInsideMultiLineStrings()
    {
        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, "src", "Probe.cs");
            string content = "internal sealed class Probe\n{\n    internal string Text() => \"\"\"\n        if (first) {   \n            second();\n        \"\"\";\n}\n";
            File.WriteAllText(path, content);

            Assert.Equal(0, StyleCommand.Execute(["fix", root]));
            Assert.Equal(content, File.ReadAllText(path));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void FixLeavesASourceWithSyntaxErrorsUnchangedAndFails()
    {
        string root = CreateRoot();
        try
        {
            string path = Path.Combine(root, "src", "Probe.cs");
            string content = "internal sealed class Probe {\n    internal void Run( {\n}\n";
            File.WriteAllText(path, content);

            Assert.Equal(1, StyleCommand.Execute(["fix", root]));
            Assert.Equal(content, File.ReadAllText(path));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void PathsFromLimitsTheRunToTheListedSources()
    {
        string root = CreateRoot();
        try
        {
            string listed = Path.Combine(root, "src", "Listed.cs");
            string other = Path.Combine(root, "src", "Other.cs");
            string paths = Path.Combine(root, "paths.txt");
            File.WriteAllText(listed, "internal sealed class Listed { }\n");
            File.WriteAllText(other, "internal sealed class Other { }\n");
            File.WriteAllText(paths, "src/Listed.cs\n");

            Assert.Equal(0, StyleCommand.Execute(["fix", root, "--paths-from", paths]));
            Assert.Equal("internal sealed class Listed;\n", File.ReadAllText(listed));
            Assert.Equal("internal sealed class Other { }\n", File.ReadAllText(other));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData]
    [InlineData("check")]
    [InlineData("format", ".")]
    [InlineData("check", ".", "--rule")]
    [InlineData("check", ".", "--unknown", "value")]
    public void InvalidArgumentsAreRejected(params string[] arguments) => Assert.Throws<ArgumentException>(
        () => StyleCommand.Execute(arguments)
    );

    public static TheoryData<string, string, string> Fixes()
        => new()
    {
        {
            "F-01",
            "internal sealed class Probe;  \r\n\r\n",
            Source(
                """
                internal sealed class Probe;
                """
            )
        },
        {
            "N-04",
            Source(
                """
                using Inno.Core;
                using static System.Math;
                using System;

                namespace Probe;
                """
            ),
            Source(
                """
                using System;
                using Inno.Core;

                using static System.Math;

                namespace Probe;
                """
            )
        },
        {
            "F-03",
            "\nusing System;\nnamespace Probe;\ninternal sealed class Probe;\n",
            Source(
                """
                using System;

                namespace Probe;

                internal sealed class Probe;
                """
            )
        },
        {
            "S-03",
            Source(
                """
                internal sealed class Probe
                {
                    internal void First() {}
                    internal void Second() {}
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void First() {}

                    internal void Second() {}
                }
                """
            )
        },
        {
            "S-04",
            Source(
                """
                internal sealed class Probe
                {

                    internal void First() {}

                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void First() {}
                }
                """
            )
        },
        {
            "A-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal int Run() => Add(1,
                        2);
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal int Run() => Add(1, 2);
                }
                """
            )
        },
        {
            "A-02",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Register(firstArgumentWithALongName, secondArgumentWithALongName, thirdArgumentWithALongName,
                            fourthArgumentWithALongName, fifthArgumentWithALongName);
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Register(
                            firstArgumentWithALongName,
                            secondArgumentWithALongName,
                            thirdArgumentWithALongName,
                            fourthArgumentWithALongName,
                            fifthArgumentWithALongName
                        );
                    }
                }
                """
            )
        },
        {
            "P-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run(int first, int second) {}
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run(
                        int first,
                        int second
                    ) {}
                }
                """
            )
        },
        {
            "P-02",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run(
                        int first,
                        int second
                    )
                    {
                        Step();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run(
                        int first,
                        int second
                    ) {
                        Step();
                    }
                }
                """
            )
        },
        {
            "B-03",
            Source(
                """
                internal sealed class Probe : Base
                {
                    internal Probe(int value) : base(value) {}
                }
                """
            ),
            Source(
                """
                internal sealed class Probe : Base
                {
                    internal Probe(int value)
                        : base(value) {}
                }
                """
            )
        },
        {
            "M-03",
            Source(
                """
                internal sealed class Probe
                {
                    internal int Run() =>
                        m_value;
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal int Run() => m_value;
                }
                """
            )
        },
        {
            "W-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal bool Run() => m_first &&
                        m_second;
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal bool Run()
                        => m_first
                           && m_second;
                }
                """
            )
        },
        {
            "W-03",
            Source(
                """
                internal sealed class Probe
                {
                    internal int Run()
                    {
                        return m_values.Where(static value => value > 0).Select(static value => value * 2).
                            Sum();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal int Run()
                    {
                        return m_values.Where(static value => value > 0)
                            .Select(static value => value * 2)
                            .Sum();
                    }
                }
                """
            )
        },
        {
            "B-09",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run() { Step(); }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Step();
                    }
                }
                """
            )
        },
        {
            "B-04",
            Source(
                """
                internal sealed class Probe
                {
                    internal bool Run() => m_entries.All(static entry => entry.isReadyForPresentation && entry.owner.isVisibleInHierarchy
                                                                         && entry.isVisible);

                    internal Func<int> Count() => () => m_first.TryGetValue(
                        m_key,
                        out int value
                    ) && value > 0
                          ? value
                          : 0;
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal bool Run()
                        => m_entries.All(static entry => (
                            entry.isReadyForPresentation && entry.owner.isVisibleInHierarchy
                            && entry.isVisible
                        ));

                    internal Func<int> Count()
                        => () => (
                            m_first.TryGetValue(m_key, out int value) && value > 0
                            ? value
                            : 0
                        );
                }
                """
            )
        },
        {
            "F-04",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        if (m_width >= 500) Assert.True(m_details, "The details column is expected to be visible once the panel is at least five hundred wide.");
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        if (m_width >= 500)
                        {
                            Assert.True(m_details, "The details column is expected to be visible once the panel is at least five hundred wide.");
                        }
                    }
                }
                """
            )
        },
        {
            "B-01",
            Source(
                """
                internal sealed class Probe {
                    internal void Run() {
                        Step();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Step();
                    }
                }
                """
            )
        },
        {
            "B-07",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                    }

                    private sealed class Empty { }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run() {}

                    private sealed class Empty;
                }
                """
            )
        },
        {
            "W-05",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        if (m_first
                            && m_second)
                        {
                            Step();
                        }
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        if (m_first
                            && m_second
                        ) {
                            Step();
                        }
                    }
                }
                """
            )
        },
        {
            "B-05",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        if (m_first)
                            Step();
                        foreach (int value in m_values) Step();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        if (m_first)
                        {
                            Step();
                        }
                        foreach (int value in m_values)
                        {
                            Step();
                        }
                    }
                }
                """
            )
        },
        {
            "F-05",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        bool both = m_first
                                && m_second;
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        bool both = m_first
                                    && m_second;
                    }
                }
                """
            )
        },
        {
            "I-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        var options = new Options {
                            first = 1, second = 2 };
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        var options = new Options
                        {
                            first = 1,
                            second = 2
                        };
                    }
                }
                """
            )
        },
        {
            "I-05",
            Source(
                """
                internal enum Probe
                {
                    First,
                    Second,
                }
                """
            ),
            Source(
                """
                internal enum Probe
                {
                    First,
                    Second
                }
                """
            )
        },
        {
            "F-04",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Register(firstArgumentWithALongName, secondArgumentWithALongName, thirdArgumentWithALongName, fourthArgumentWithALongName, fifthArgumentWithALongName);
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Register(
                            firstArgumentWithALongName,
                            secondArgumentWithALongName,
                            thirdArgumentWithALongName,
                            fourthArgumentWithALongName,
                            fifthArgumentWithALongName
                        );
                    }
                }
                """
            )
        },
        {
            "M-03",
            Source(
                """
                internal sealed class Probe
                {
                    internal string Label() => m_isMissing ? "The asset is missing from the project." :
                        m_displayName;
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal string Label()
                        => m_isMissing
                           ? "The asset is missing from the project."
                           : m_displayName;
                }
                """
            )
        },
        {
            "I-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        var options = new ProbeOptions
                        { first = 1, second = 2 };
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        var options = new ProbeOptions { first = 1, second = 2 };
                    }
                }
                """
            )
        },
        {
            "F-04",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        List<string> names = m_entries.Where(static entry => !entry.isMissing && entry.isVisible).Select(static entry => entry.displayName).OrderBy(static name => name).ToList();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        List<string> names = m_entries
                            .Where(static entry => !entry.isMissing && entry.isVisible)
                            .Select(static entry => entry.displayName)
                            .OrderBy(static name => name)
                            .ToList();
                    }
                }
                """
            )
        },
        {
            "F-04",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        bool changed = previous.width != current.width || previous.height != current.height || previous.format != current.format || previous.depth != current.depth;
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        bool changed = previous.width != current.width
                                       || previous.height != current.height
                                       || previous.format != current.format
                                       || previous.depth != current.depth;
                    }
                }
                """
            )
        },
        {
            "F-04",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Report($"The viewport '{viewport.name}' does not support the requested mode '{Describe(defaultMode, viewport.capabilities)}' in '{viewport.owner}'.");
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Report(
                            $"The viewport '{viewport.name}' does not support the requested mode '{Describe(defaultMode, viewport.capabilities)}' in '{viewport.owner}'."
                        );
                    }
                }
                """
            )
        },
        {
            "E-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Probe value = new Probe();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        var value = new Probe();
                    }
                }
                """
            )
        },
        {
            "M-04",
            Source(
                """
                internal sealed class Probe
                {
                    private readonly List<int> m_values = new List<int>();
                    private int m_first = 1, m_second = 2;
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    private readonly List<int> m_values = new();
                    private int m_first = 1;
                    private int m_second = 2;
                }
                """
            )
        },
        {
            "E-07",
            Source(
                """
                internal sealed class Probe
                {
                    internal string Run(string value = "")
                    {
                        string text = "";
                        return text + value;
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal string Run(string value = "")
                    {
                        string text = string.Empty;
                        return text + value;
                    }
                }
                """
            )
        },
        {
            "E-11",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Func<int, int> twice = (value) => value * 2;
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        Func<int, int> twice = value => value * 2;
                    }
                }
                """
            )
        },
        {
            "E-10",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        float ratio = 1.0F;
                        uint mask = 0XFFu;
                        int count = 1000000;
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        float ratio = 1f;
                        uint mask = 0xFFu;
                        int count = 1_000_000;
                    }
                }
                """
            )
        },
        {
            "M-05",
            Source(
                """
                [Serializable, Obsolete()]
                internal sealed class Probe
                {
                    [ObsoleteAttribute] internal void Run() {}
                }
                """
            ),
            Source(
                """
                [Serializable]
                [Obsolete]
                internal sealed class Probe
                {
                    [Obsolete]
                    internal void Run() {}
                }
                """
            )
        },
        {
            "D-02",
            Source(
                """
                /// <summary>Runs the probe.</summary>
                internal sealed class Probe
                {
                    /// <returns>The value.</returns>
                    /// <param name="value">The input.</param>
                    ///
                    /// <summary>
                    /// Returns <c>null</c>.
                    /// </summary>
                    internal object Run(object value) => null;
                }
                """
            ),
            Source(
                """
                /// <summary>
                /// Runs the probe.
                /// </summary>
                internal sealed class Probe
                {
                    /// <summary>
                    /// Returns <see langword="null"/>.
                    /// </summary>
                    /// <param name="value">
                    /// The input.
                    /// </param>
                    /// <returns>
                    /// The value.
                    /// </returns>
                    internal object Run(object value) => null;
                }
                """
            )
        },
        {
            "D-05",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        int value = 1; //The first value.
                        Use(value);
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        // The first value.
                        int value = 1;
                        Use(value);
                    }
                }
                """
            )
        },
        {
            "B-09",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run(int kind)
                    {
                        int first = 1; int second = 2;
                        switch (kind)
                        {
                            case 0: Use(first); break;
                            default: Use(second); break;
                        }
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run(int kind)
                    {
                        int first = 1;
                        int second = 2;
                        switch (kind)
                        {
                            case 0:
                                Use(first);
                                break;
                            default:
                                Use(second);
                                break;
                        }
                    }
                }
                """
            )
        },
        {
            "T-01",
            Source(
                """
                sealed class Probe
                {
                    int m_count;

                    [Obsolete]
                    static void Run() {}

                    interface IProbe
                    {
                        public void Run();
                    }
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    private int m_count;

                    [Obsolete]
                    private static void Run() {}

                    private interface IProbe
                    {
                        void Run();
                    }
                }
                """
            )
        },
        {
            "T-02",
            Source(
                """
                internal abstract class Probe
                {
                    readonly static private int[] VALUES = [];

                    override public string ToString() => string.Empty;

                    async static internal Task RunAsync() => await Task.Yield();
                }
                """
            ),
            Source(
                """
                internal abstract class Probe
                {
                    private static readonly int[] VALUES = [];

                    public override string ToString() => string.Empty;

                    internal static async Task RunAsync() => await Task.Yield();
                }
                """
            )
        },
        {
            "E-03",
            Source(
                """
                using System;

                internal sealed class Probe
                {
                    private readonly Int32 m_count = Int32.Parse("1");

                    internal String Name(Object value) => String.Concat(value, m_count);
                }
                """
            ),
            Source(
                """
                using System;

                internal sealed class Probe
                {
                    private readonly int m_count = int.Parse("1");

                    internal string Name(object value) => string.Concat(value, m_count);
                }
                """
            )
        },
        {
            "O-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal Probe()
                    {
                        m_items = [];
                    }

                    private void Notify() => changed?.Invoke();

                    private readonly int[] m_items;

                    internal int count => m_items.Length;

                    private sealed class Entry;

                    private bool m_isDirty;
                    private const int C_LIMIT = 4;

                    internal event Action? changed;

                    private static readonly int[] EMPTY = [];
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    private const int C_LIMIT = 4;

                    private static readonly int[] EMPTY = [];

                    private readonly int[] m_items;
                    private bool m_isDirty;

                    internal Probe()
                    {
                        m_items = [];
                    }

                    internal int count => m_items.Length;

                    internal event Action? changed;

                    private void Notify() => changed?.Invoke();

                    private sealed class Entry;
                }
                """
            )
        },
        {
            "O-01",
            Source(
                """
                internal sealed class Probe
                {
                    public void Draw(
                        int mesh,
                        int material
                    ) {
                        Submit(mesh + material);
                    }

                    private void Submit(int item) {}

                    public static Probe Create() => new();

                    public void Clear() {}

                    public void Draw(int mesh) => Draw(mesh, 0);

                    void IDisposable.Dispose() {}

                    protected void Reset() {}

                    public int count => 0;
                }
                """
            ),
            Source(
                """
                internal sealed class Probe
                {
                    public int count => 0;

                    public void Draw(int mesh) => Draw(mesh, 0);

                    public void Draw(
                        int mesh,
                        int material
                    ) {
                        Submit(mesh + material);
                    }

                    public void Clear() {}

                    void IDisposable.Dispose() {}

                    public static Probe Create() => new();

                    protected void Reset() {}

                    private void Submit(int item) {}
                }
                """
            )
        }
    };

    public static TheoryData<string, string> Kept()
        => new()
    {
        {
            "B-09",
            Source(
                """
                internal sealed class Probe
                {
                    internal Probe(
                        float m11,
                        float m12,
                        float m21,
                        float m22
                    ) {
                        this.m11 = m11; this.m12 = m12;
                        this.m21 = m21; this.m22 = m22;
                    }
                }
                """
            )
        },
        {
            "E-01",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        var pair = new { name = "probe", count = 1 };
                        var values = new int[4];
                        int count = values.Length;
                    }
                }
                """
            )
        },
        {
            "E-07",
            Source(
                """
                internal sealed class Probe
                {
                    private const string C_NONE = "";

                    internal string Run(string value = "")
                        => value switch
                    {
                        "" => C_NONE,
                        _ => value
                    };
                }
                """
            )
        },
        {
            "E-10",
            Source(
                """
                internal sealed class Probe
                {
                    internal void Run()
                    {
                        decimal price = 1.50m;
                        double ratio = 1.0;
                        int year = 2026;
                    }
                }
                """
            )
        },
        {
            "T-01",
            Source(
                """
                using System;

                internal sealed partial class Probe : IDisposable
                {
                    static Probe() {}

                    ~Probe() {}

                    void IDisposable.Dispose() {}

                    partial void Changed();

                    private enum Mode
                    {
                        None,
                        Active
                    }

                    private interface IProbe
                    {
                        void Run();

                        protected void Reset();
                    }
                }

                file sealed class Local;
                """
            )
        },
        {
            "T-02",
            Source(
                """
                internal abstract class Probe
                {
                    private const int C_LIMIT = 1;

                    private static readonly int[] VALUES = [];

                    public sealed override string ToString() => string.Empty;

                    protected internal static new unsafe partial void Run();
                }
                """
            )
        },
        {
            "T-03",
            Source(
                """
                internal static class Helpers;

                internal abstract class Shape;

                /// <summary>
                /// Describes a node. Derive from it to add a node kind.
                /// </summary>
                internal class Node;

                internal sealed class Leaf : Node;

                internal partial class Generated;
                """
            )
        },
        {
            "T-04",
            Source(
                """
                internal readonly record struct ProbeID(int value);

                internal record struct SerializedProbe
                {
                    public string value { get; set; }
                }
                """
            )
        },
        {
            "E-03",
            Source(
                """
                using System;

                internal sealed class Probe
                {
                    internal object String() => this;

                    internal string Describe() => nameof(Int32) + typeof(System.String).Name;

                    /// <summary>
                    /// Returns a <see cref="String"/>.
                    /// </summary>
                    internal TypeCode Code() => TypeCode.Int32;
                }
                """
            )
        },
        {
            "O-01",
            Source(
                """
                internal struct Vertex
                {
                    public float x;

                    public float length => x;

                    public readonly float y;
                }

                [StructLayout(LayoutKind.Sequential)]
                internal sealed class NativeHeader
                {
                    public int size;
                    public readonly int version = 1;
                }

                internal sealed class Settings
                {
                    [SerializableProperty]
                    public string name { get; set; } = string.Empty;

                    [SerializableProperty]
                    private int m_revision;
                }

                internal sealed class Registry
                {
                    private static int s_next = 1;
                    private static readonly int FIRST = s_next++;
                }

                internal sealed class Platform
                {
                    internal void Run() {}

                #if DEBUG
                    private const int C_LIMIT = 1;
                #endif
                }
                """
            )
        },
        {
            "NM-03",
            Source(
                """
                internal static partial class NativeProbe
                {
                    [LibraryImport("probe")]
                    internal static partial int probe_open(int Handle);
                }
                """
            )
        },
        {
            "NM-04",
            Source(
                """
                internal sealed class ProbeTests
                {
                    [Fact]
                    public Task RunCompletes() => Task.CompletedTask;

                    [MetalShaderFact]
                    public Task ShaderCompiles() => Task.CompletedTask;
                }

                internal sealed class Worker : IWorker
                {
                    public Task Run() => Task.CompletedTask;
                }
                """
            )
        },
        {
            "NM-05",
            Source(
                """
                internal sealed class ProbeList : ICollection
                {
                    public int Count => 0;
                }
                """
            )
        },
        {
            "NM-06",
            Source(
                """
                internal sealed class Probe
                {
                    [SerializableProperty]
                    internal string m_targetId = string.Empty;
                }

                [StructLayout(LayoutKind.Sequential)]
                internal struct NativeProbe
                {
                    public int Width;
                    public int Height;
                }
                """
            )
        },
        {
            "NM-11",
            Source(
                """
                internal enum ProbeKey
                {
                    _0,
                    _1,
                    A
                }
                """
            )
        },
        {
            "NM-12",
            Source(
                """
                internal sealed class Probe
                {
                    internal Func<int, int> zero => static _ => 0;
                }
                """
            )
        },
        {
            "NM-13",
            Source(
                """
                internal sealed class ImGuiProbe
                {
                    [SerializableProperty]
                    internal Guid persistentId { get; set; }

                    internal Guid stableTypeId { get; set; }

                    internal RmlUiHandle rmlUiHandle { get; set; }
                }

                internal sealed record PageReference(string pageId);
                """
            )
        }
    };

    private static string CreateRoot()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoStyleCommandTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "src"));
        return root;
    }

    private static string Source(string text) => text + "\n";
}
