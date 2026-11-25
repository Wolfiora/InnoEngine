using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Testing.Source;
using Inno.Testing.Validation;
using Inno.Tooling.Documentation;
using Inno.Tooling.Validation;

namespace Inno.Tooling.Validation.Tests;

public abstract class ValidationPlanFixture
{
    protected static string[] Owners(JsonDocument document)
        => document.RootElement.GetProperty("owners").EnumerateArray().Select(value => value.GetString()!).ToArray();

    protected static string[] Changes(JsonDocument document)
        => document.RootElement.GetProperty("changes").EnumerateArray().Select(value => value.GetString()!).ToArray();
}
