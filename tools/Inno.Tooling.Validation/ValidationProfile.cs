using System;

namespace Inno.Tooling.Validation;

internal enum ValidationProfile
{
    Fast,
    PullRequest,
    Master,
    Full,
    Performance,
    NativeProducer
}

internal static class ValidationProfiles
{
    internal static ValidationProfile Parse(string value)
        => value switch
    {
        "fast" => ValidationProfile.Fast,
        "pull-request" => ValidationProfile.PullRequest,
        "master" => ValidationProfile.Master,
        "full" => ValidationProfile.Full,
        "performance" => ValidationProfile.Performance,
        "native-producer" => ValidationProfile.NativeProducer,
        _ => throw new ArgumentException("Unknown validation profile: " + value)
    };
}
