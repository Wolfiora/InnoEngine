using System;

namespace Inno.Rendering;

sealed partial class RenderGraphBuilder
{
    internal void EnsurePassMutable(RenderPassRecord pass)
    {
        EnsureMutating();
        if (pass.index < m_frozenPassCount || pass.index >= m_passes.Count
            || !ReferenceEquals(m_passes[pass.index], pass))
            throw new InvalidOperationException("The pass declaration was committed or removed by rollback.");
        InvalidateValidation();
    }

    internal void CommitMutation(RenderGraphMutationScope scope)
    {
        EnsureCurrentMutation(scope);
        if (scope.isCommitted)
            throw new InvalidOperationException("The graph mutation has already been committed.");
        if (!GetValidation().result.isValid)
            throw new InvalidOperationException("Invalid graph additions cannot be committed.");
        m_frozenPassCount = m_passes.Count;
        scope.isCommitted = true;
    }

    internal void EndMutation(RenderGraphMutationScope scope)
    {
        EnsureCurrentMutation(scope);
        if (!scope.isCommitted)
        {
            m_textures.RemoveRange(scope.textureCount, m_textures.Count - scope.textureCount);
            m_buffers.RemoveRange(scope.bufferCount, m_buffers.Count - scope.bufferCount);
            m_passes.RemoveRange(scope.passCount, m_passes.Count - scope.passCount);
            for (int index = m_addedOutputs.Count - 1; index >= scope.outputCount; index--)
                m_outputs.Remove(m_addedOutputs[index]);
            m_addedOutputs.RemoveRange(scope.outputCount, m_addedOutputs.Count - scope.outputCount);
            if (m_nameScopes.Count > scope.nameScopeCount)
                m_nameScopes.RemoveRange(scope.nameScopeCount, m_nameScopes.Count - scope.nameScopeCount);
            m_frozenPassCount = Math.Min(m_frozenPassCount, m_passes.Count);
            InvalidateValidation();
            m_validation = scope.validation;
            m_validatedRevision = m_revision;
        }
        m_mutations.RemoveAt(m_mutations.Count - 1);
        if (m_mutations.Count == 0)
            m_addedOutputs.Clear();
    }

    private void EnsureCurrentMutation(RenderGraphMutationScope scope)
    {
        EnsureBuilding();
        if (m_mutations.Count == 0 || !ReferenceEquals(m_mutations[^1], scope))
            throw new InvalidOperationException("Render graph mutations must end in nesting order.");
    }

    private void EnsureMutating()
    {
        EnsureBuilding();
        if (m_mutations.Count != 0 && m_mutations[^1].isCommitted)
            throw new InvalidOperationException("End the committed mutation scope before adding graph declarations.");
    }

    private void MarkOutput(RenderResourceKey key)
    {
        if (!m_outputs.Add(key))
            return;
        if (m_mutations.Count != 0)
            m_addedOutputs.Add(key);
        InvalidateValidation();
    }

    private void InvalidateValidation()
    {
        m_revision = checked(m_revision + 1);
        m_validation = null;
    }

    private RenderGraphValidationState GetValidation()
    {
        if (m_validation is not null && m_validatedRevision == m_revision)
            return m_validation;
        m_validation = RenderGraphValidator.Validate(m_capabilities, m_textures, m_buffers, m_passes, m_outputs);
        m_validatedRevision = m_revision;
        return m_validation;
    }
}
