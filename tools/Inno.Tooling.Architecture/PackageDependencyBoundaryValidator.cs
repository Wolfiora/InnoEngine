using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Inno.Tooling.Architecture;

internal static class PackageDependencyBoundaryValidator
{
    internal static void Validate(
        string project,
        XDocument document,
        ICollection<string> failures
    ) {
        bool test = project.StartsWith("tests/", StringComparison.Ordinal) || project.Contains("/tests/", StringComparison.Ordinal);
        bool generator = project == "build/bindings/Inno.Build.Bindings/Inno.Build.Bindings.csproj";
        foreach (XElement package in document.Descendants("PackageReference"))
        {
            string? id = package.Attribute("Include")?.Value;
            if (id != "BGCS" && id?.StartsWith("BGCS.", StringComparison.Ordinal) != true)
                continue;
            if (id == "BGCS.Runtime")
            {
                if (!test && !project.StartsWith("backends/", StringComparison.Ordinal))
                    failures.Add($"{project}: the interop runtime dependency belongs to a backend boundary.");
            }
            else if (!test && !generator)
                failures.Add($"{project}: binding generator packages belong only to their build application or declared extension.");
            string version = package.Attribute("Version")?.Value
                ?? package.Elements("Version").Select(static value => value.Value).SingleOrDefault() ?? string.Empty;
            if (version != "[$(InnoBgcsPackageVersion)]"
                && !(version.StartsWith('[') && version.EndsWith(']') && !version.Contains(',') && !version.Contains('*')))
                failures.Add($"{project}: BGCS packages require one exact published version, without floating or source fallback.");
        }
    }
}
