using System;
using System.Reflection;
using System.Reflection.Emit;
using Xunit;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;

namespace Inno.Extensibility.Types.Tests;

public sealed class AssemblyExtensionsTests
{
    [Fact]
    public void AssemblyClassificationReadsBothMetadataDimensions()
    {
        Assembly asm = BuildDynamicAssembly("InnoPlugin", "Editor");
        Assert.Equal(AssemblyDomain.InnoPlugin, asm.GetInnoAssemblyDomain());
        Assert.Equal(AssemblyScope.Editor, asm.GetInnoAssemblyScope());
    }

    [Fact]
    public void MissingClassificationIsRejected()
    {
        Assembly asm = BuildDynamicAssembly(null, null);
        Assert.Throws<InvalidOperationException>(() => asm.GetInnoAssemblyDomain());
        Assert.Throws<InvalidOperationException>(() => asm.GetInnoAssemblyScope());
    }

    private static Assembly BuildDynamicAssembly(
        string? domainName,
        string? scopeName
    ) {
        var asmName = new AssemblyName("Inno.Dynamic." + Guid.NewGuid().ToString("N"));
        AssemblyBuilder asmBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);

        if (domainName is not null && scopeName is not null)
        {
            ConstructorInfo ctor = typeof(AssemblyMetadataAttribute).GetConstructor([typeof(string), typeof(string)])!;
            asmBuilder.SetCustomAttribute(new CustomAttributeBuilder(ctor, ["Inno.AssemblyDomain", domainName]));
            asmBuilder.SetCustomAttribute(new CustomAttributeBuilder(ctor, ["Inno.AssemblyScope", scopeName]));
        }

        ModuleBuilder module = asmBuilder.DefineDynamicModule("main");
        TypeBuilder type = module.DefineType("Inno.Dynamic.Placeholder", TypeAttributes.Public | TypeAttributes.Class);
        _ = type.CreateType();

        return asmBuilder;
    }
}
