using System;
using System.IO;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

public sealed class StyleCommandTests
{
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
        }
    };

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

    private static string CreateRoot()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoStyleCommandTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(Path.Combine(root, "src"));
        return root;
    }

    private static string Source(string text) => text + "\n";
}
