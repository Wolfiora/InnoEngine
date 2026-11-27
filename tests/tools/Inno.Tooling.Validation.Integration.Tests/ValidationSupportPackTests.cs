using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Integration.Tests;

public sealed class ValidationSupportPackTests
{
    [Theory]
    [InlineData("miniaudio-release.dll", "payload", "payload", false, true)]
    [InlineData("libminiaudio-release.dylib", "payload", "payload", false, true)]
    [InlineData("miniaudio-release.dll", "payload", "changed", false, false)]
    [InlineData("miniaudio-release.dll", null, "payload", false, false)]
    [InlineData("miniaudio-release.dll", "payload", null, false, false)]
    [InlineData("miniaudio-release.dll", "payload", "payload", true, false)]
    public async Task RequiresMatchingRuntimeAndLinkLibraries(
        string library,
        string? runtime,
        string? link,
        bool duplicate,
        bool accepted
    ) {
        using var repository = new ValidationRepository();
        string archive = repository.PathOf("pack.zip");
        using (ZipArchive zip = ZipFile.Open(archive, ZipArchiveMode.Create))
        {
            Write("native/", runtime);
            Write("PlayerLink/native/", link);
            if (duplicate)
            {
                Write("native/duplicate/", runtime);
            }

            void Write(
                string prefix,
                string? content
            ) {
                if (content is null)
                {
                    return;
                }
                using var writer = new StreamWriter(zip.CreateEntry(prefix + library).Open());
                writer.Write(content);
            }
        }
        Task<int> check = ValidationCommand.ExecuteAsync(["support-pack", "--archive", archive]);
        if (accepted)
        {
            Assert.Equal(0, await check);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => check);
        }
    }
}
