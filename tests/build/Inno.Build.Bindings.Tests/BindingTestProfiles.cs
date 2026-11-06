using BGCS.Configuration;
using Inno.Build.Bindings;

namespace Inno.Build.Bindings.Tests;

internal static class BindingTestProfiles
{
    internal static NativeBindingGenerator Generator(string target = "fixture")
        => new(new NativeBindingProfile(target, new TargetConfiguration { id = "windows-x64-msvc" }));
}
