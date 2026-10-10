using System.Collections.Generic;
using System.Threading;

namespace Inno.Build.Toolchains;

internal sealed class NativeInputVerificationScope
{
    private readonly object m_gate = new();
    private readonly NativeInputReadCache m_inputs;

    internal NativeInputVerificationScope(
        NativeBuildInputState state,
        string phase
    ) => m_inputs = new NativeInputReadCache(state, phase);

    internal NativeInputSnapshot Capture(
        IEnumerable<NativeBuildInput> inputs,
        CancellationToken cancellationToken
    ) {
        lock (m_gate)
            return NativeInputSnapshot.Capture(inputs, m_inputs, cancellationToken);
    }
}
