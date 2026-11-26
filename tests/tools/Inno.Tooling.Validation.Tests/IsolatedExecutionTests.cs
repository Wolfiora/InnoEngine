using System;
using System.IO;
using System.Xml.Linq;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class IsolatedExecutionTests
{
    [Theory]
    [InlineData(1, 1, 0, "Run", "Passed", true)]
    [InlineData(0, 0, 0, "Run", "Passed", false)]
    [InlineData(1, 0, 1, "Run", "Failed", false)]
    [InlineData(1, 0, 0, "Run", "NotExecuted", false)]
    [InlineData(1, 1, 0, "Other", "Passed", false)]
    [InlineData(2, 2, 0, "Run", "Passed", false)]
    public void RequiresSuccessfulExecutionOfTheSelectedContract(
        int executed,
        int passed,
        int failed,
        string method,
        string outcome,
        bool valid
    ) {
        string path = Path.GetTempFileName();
        try
        {
            new XElement(
                "TestRun",
                new XElement("Results", new XElement("UnitTestResult", new XAttribute("outcome", outcome))),
                new XElement(
                    "TestDefinitions",
                    new XElement(
                        "UnitTest",
                        new XElement(
                            "TestMethod",
                            new XAttribute("className", "Inno.Contracts"),
                            new XAttribute("name", method)
                        )
                    )
                ),
                new XElement(
                    "ResultSummary",
                    new XElement(
                        "Counters",
                        new XAttribute("executed", executed),
                        new XAttribute("passed", passed),
                        new XAttribute("failed", failed),
                        new XAttribute("error", 0)
                    )
                )
            ).Save(path);
            if (valid)
            {
                ValidationTestExecution.Verify(path, "Inno.Contracts.Run");
            }
            else
            {
                Assert.Throws<InvalidDataException>(() => ValidationTestExecution.Verify(path, "Inno.Contracts.Run"));
            }
        }
        finally
        {
            File.Delete(path);
        }
        Assert.Throws<FileNotFoundException>(() => ValidationTestExecution.Verify(path, "Inno.Contracts.Run"));
    }
}
