using Inno.Core.Serialization;

namespace Inno.Editor.Scripting.Tests;

internal sealed class CapturedScriptPayload(ISerializable payload) : ISerializable
{
    internal ISerializable payload { get; } = payload;
}
