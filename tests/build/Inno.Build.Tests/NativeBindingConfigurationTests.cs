using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using BGCS;
using BGCS.Core.Hosting;
using BGCS.Hosting.Standard;
using Xunit;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Testing.Source;

namespace Inno.Build.Tests;

/// <summary>
/// Reads component-owned configuration through the installed SDK's public materialization contract.
/// </summary>
public sealed class NativeBindingConfigurationTests
{
    /// <summary>
    /// Materializes every configured component without invoking native generation or selecting a platform SDK.
    /// </summary>
    /// <returns>The completion of configuration validation for the declared product closure.</returns>
    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public async Task EveryDeclaredComponentConfigurationCanBeReadByTheInstalledSdk()
    {
        string root = TestRepository.GetRoot(typeof(NativeBindingConfigurationTests).Assembly);
        BuildCompositionContext environment = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId("windows-x64"));
        ProductNativeBuildPlan product = StandardBuildDistribution.Create(environment).build.ResolveNativeProduct("windows-x64", "editor").plan;
        using HostServices services = new HostServicesProvider().Create(new(root, JsonSerializer.SerializeToElement(new { })));
        foreach (NativeComponentDescriptor owner in product.steps.Select(step => step.component)
            .Where(owner => owner.bindingDefinition is not null).DistinctBy(owner => owner.nativeProject))
        {
            string definition = Path.GetFullPath(owner.bindingDefinition!, root);
            XDocument document = XDocument.Load(definition);
            string[] configurations = document.Descendants("InnoBindingConfig")
                .Select(element => Path.GetFullPath(element.Value, Path.GetDirectoryName(definition)!))
                .Distinct(StringComparer.Ordinal).ToArray();
            Assert.NotEmpty(configurations);
            foreach (string configuration in configurations)
            {
                GenerationRequest request = await GenerationRequest.FromFile(configuration)
                    .MaterializeAsync(services.paths, CancellationToken.None);
                Assert.Single(request.configuration.targets);
            }
        }
    }
}
