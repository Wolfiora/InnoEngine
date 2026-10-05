using System;
using System.IO;
using Inno.Extensibility.Modules;
using Inno.Runtime;
using Xunit;

namespace Inno.Runtime.Tests;

public sealed class StaticDeploymentTests
{
    [Fact]
    public void ManifestMutationCannotChangeFrozenDeployment()
    {
        var assembly = new GameRuntimeAssembly { name = "Game.Code", contentFingerprint = new string('a', 64) };
        string[] dependencies = [];
        var module = new GameRuntimeModule
        {
            name = "Game", domain = AssemblyDomain.InnoScripting,
            assemblies = [assembly], dependencies = dependencies
        };
        GameCodeDeployment frozen = GameCodeDeployment.FromManifest([module]);
        assembly.name = "Different.Code";
        assembly.contentFingerprint = new string('b', 64);
        module.name = "Different";
        module.assemblies = [];
        Assert.Equal("Game", Assert.Single(frozen.modules).name);
        Assert.Equal("Game.Code", Assert.Single(frozen.modules[0].assemblies).name);
        Assert.Equal(new string('a', 64), frozen.modules[0].assemblies[0].contentFingerprint);
    }

    [Fact]
    public void LinkedIdentityRejectsDifferentCodeBeforeModuleActivation()
    {
        GameCodeDeployment linked = new([CreateModule("Game", "Game.Code", 'a')]);
        GameCodeDeployment replaced = new([CreateModule("Game", "Game.Code", 'b')]);
        Assert.Throws<InvalidDataException>(() => replaced.ValidateMatches(linked));
        linked.ValidateMatches(new GameCodeDeployment([CreateModule("Game", "Game.Code", 'a')]));
    }

    [Fact]
    public void ClosureRejectsSharedAssemblyOwnersAndMissingUpstreamModules()
    {
        Assert.Throws<InvalidDataException>(() => new GameCodeDeployment([
            CreateModule("One", "Shared.Code", 'a'), CreateModule("Two", "Shared.Code", 'a')
        ]));
        var missing = new GameCodeModule("Game", AssemblyDomain.InnoScripting,
            [new GameCodeAssembly("Game.Code", new string('a', 64))], ["Absent"]);
        Assert.Throws<InvalidDataException>(() => new GameCodeDeployment([missing]));
    }

    [Theory]
    [InlineData("Game.Code", "short")]
    [InlineData("../Game.Code", "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa")]
    [InlineData("Game.Code", "AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA")]
    public void CodeInputIdentityRejectsMalformedManifestData(
        string name,
        string fingerprint
    ) => Assert.Throws<InvalidDataException>(() => new GameCodeAssembly(name, fingerprint));

    private static GameCodeModule CreateModule(
        string name,
        string assembly,
        char fingerprint
    ) => new(name, AssemblyDomain.InnoScripting,
        [new GameCodeAssembly(assembly, new string(fingerprint, 64))], []);
}
