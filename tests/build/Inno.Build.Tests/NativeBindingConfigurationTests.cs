using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using BGCS;
using Xunit;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Testing.Source;

namespace Inno.Build.Tests;

public sealed class NativeBindingConfigurationTests
{
    [Fact]
    [Trait("ExternalDependency", "BGCS")]
    public void EveryDeclaredComponentConfigurationCanBeReadByTheInstalledSdk()
    {
        string root = TestRepository.GetRoot(typeof(NativeBindingConfigurationTests).Assembly);
        BuildCompositionContext environment = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, new BuildTargetId("windows-x64"));
        ProductNativeBuildPlan product = StandardBuildDistribution.Create(environment).build.ResolveNativeProduct("windows-x64", "editor").plan;
        foreach (NativeComponentDescriptor owner in product.steps.Select(step => step.component)
            .Where(owner => owner.bindingDefinition is not null).DistinctBy(owner => owner.nativeProject))
        {
            string definition = Path.GetFullPath(owner.bindingDefinition!, root);
            XDocument document = XDocument.Load(definition);
            string[] configurations = document.Descendants("InnoBindingConfig").Concat(document.Descendants("Config"))
                .Select(element => Path.GetFullPath(element.Value, Path.GetDirectoryName(definition)!))
                .Distinct(StringComparer.Ordinal).ToArray();
            Assert.NotEmpty(configurations);
            foreach (string configuration in configurations)
                Assert.Single(GenerationRequest.FromFile(configuration).configuration.targets);
        }
    }
}
