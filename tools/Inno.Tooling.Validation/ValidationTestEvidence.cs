using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Linq;

namespace Inno.Tooling.Validation;

internal static class ValidationTestEvidence
{
    internal static Dictionary<string, (bool passed, string detail)> Read(
        string root,
        Dictionary<string, string> projects,
        string directory
    ) {
        string[] reports = Directory.GetFiles(directory, "*.trx", SearchOption.AllDirectories);
        var assemblies = projects.ToDictionary(static pair => Path.GetFileNameWithoutExtension(pair.Value), static pair => pair.Key,
            StringComparer.OrdinalIgnoreCase);
        var hosts = projects.ToDictionary(static pair => pair.Key, static pair => ValidationTestHosts.Read(pair.Value), StringComparer.Ordinal);
        var executed = projects.Keys.ToDictionary(static project => project, static _ => 0, StringComparer.Ordinal);
        var excluded = projects.Keys.ToDictionary(static project => project, static _ => new List<string>(), StringComparer.Ordinal);
        var failures = projects.Keys.ToDictionary(static project => project, static _ => new List<string>(), StringComparer.Ordinal);
        foreach (string report in reports)
        {
            XDocument document = XDocument.Load(report);
            XElement counters = document.Descendants().Single(element => element.Name.LocalName == "Counters");
            XElement summary = document.Descendants().Single(element => element.Name.LocalName == "ResultSummary");
            XElement[] records = document.Descendants().Where(static element => element.Name.LocalName == "UnitTestResult").ToArray();
            int passed = records.Count(static result => (string?)result.Attribute("outcome") == "Passed");
            int failed = records.Count(static result => (string?)result.Attribute("outcome") == "Failed");
            if ((int?)counters.Attribute("executed") != passed + failed || (int?)counters.Attribute("failed") != failed
                || (int?)counters.Attribute("total") != records.Length || (int?)counters.Attribute("passed") != passed
                || (int?)counters.Attribute("error") is not 0 || (string?)summary.Attribute("outcome") is not ("Completed" or "Passed" or "Failed")
                || records.Any(static result => (string?)result.Attribute("outcome") is not ("Passed" or "Failed" or "NotExecuted")))
                throw new InvalidDataException($"Invalid or incomplete test execution: {report}");
            var definitions = document.Descendants().Where(static element => element.Name.LocalName == "UnitTest")
                .ToDictionary(static definition => (string)definition.Attribute("id")!, StringComparer.Ordinal);
            foreach (XElement record in records)
            {
                XElement definition = definitions[(string)record.Attribute("testId")!];
                string assembly = Path.GetFileNameWithoutExtension((string?)definition.Attribute("storage") ?? string.Empty);
                if (!assemblies.TryGetValue(assembly, out string? project))
                    throw new InvalidDataException($"Test evidence has an unplanned assembly: {assembly}");
                XElement method = definition.Elements().Single(static element => element.Name.LocalName == "TestMethod");
                string name = (string?)method.Attribute("className") + "." + (string?)method.Attribute("name");
                if ((string?)record.Attribute("outcome") == "Failed")
                {
                    failures[project].Add("Test failed: " + name + ". See " + Path.GetFileName(report) + ".");
                    continue;
                }
                if (hosts[project].TryGetValue(name, out string[]? required) && !ValidationTestHosts.Applies(required))
                {
                    excluded[project].Add((string?)record.Attribute("testName") + " requires " + string.Join('/', required));
                    continue;
                }
                if ((string?)record.Attribute("outcome") != "Passed")
                    failures[project].Add("Required test was skipped without an inapplicable compiled OS contract: " + name);
                else
                    executed[project]++;
            }
        }
        using JsonDocument execution = JsonDocument.Parse(File.ReadAllText(Path.Combine(directory, "execution-summary.json")));
        JsonElement[] stages = execution.RootElement.EnumerateArray().ToArray();
        if (stages.Count(static stage => stage.GetProperty("stage").GetString() == "Shared") != 1)
            throw new InvalidDataException("The test union is missing its unique shared execution receipt.");
        foreach (JsonElement stage in stages.Where(static stage => stage.GetProperty("stage").GetString() != "Shared"))
        {
            string owner = Path.GetRelativePath(root, stage.GetProperty("owner").GetString()!).Replace('\\', '/');
            if (!projects.ContainsKey(owner) || stage.GetProperty("stage").GetString() is not ("Discovery" or "Isolated"))
                throw new InvalidDataException("The test union contains an unplanned owner or execution stage.");
        }
        foreach (string project in projects.Keys)
        {
            JsonElement[] owned = stages.Where(stage => stage.GetProperty("stage").GetString() != "Shared"
                && Path.GetRelativePath(root, stage.GetProperty("owner").GetString()!).Replace('\\', '/') == project).ToArray();
            JsonElement[] discovery = owned.Where(static stage => stage.GetProperty("stage").GetString() == "Discovery").ToArray();
            if (discovery.Length != 1 || discovery[0].GetProperty("status").GetString() != "Passed")
                failures[project].Add("Required isolated-test discovery is missing, duplicated or unsuccessful.");
            else
            {
                string[] expected = discovery[0].GetProperty("contracts").EnumerateArray().Select(static item => item.GetString()!).Order(StringComparer.Ordinal).ToArray();
                JsonElement[] isolated = owned.Where(static stage => stage.GetProperty("stage").GetString() == "Isolated").ToArray();
                string[] actual = isolated.Select(static stage => stage.GetProperty("identity").GetString()!).Order(StringComparer.Ordinal).ToArray();
                if (expected.Distinct(StringComparer.Ordinal).Count() != expected.Length || !expected.SequenceEqual(actual, StringComparer.Ordinal)
                    || isolated.Any(static stage => stage.GetProperty("status").GetString() != "Passed"))
                    failures[project].Add("Required isolated execution is missing, duplicated or unsuccessful.");
            }
            if (executed[project] == 0)
                failures[project].Add("No executed test evidence for required owner: " + project);
        }
        File.WriteAllText(Path.Combine(directory, "host-contracts.json"), JsonSerializer.Serialize(excluded,
            new JsonSerializerOptions { WriteIndented = true }));
        return projects.Keys.ToDictionary(static project => project, project => (failures[project].Count == 0,
            failures[project].Count == 0
                ? $"{executed[project]} required cases passed; {excluded[project].Count} OS contracts NotApplicable. See host-contracts.json."
                : string.Join(Environment.NewLine, failures[project])), StringComparer.Ordinal);
    }
}
