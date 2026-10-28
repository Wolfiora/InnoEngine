using System.Collections.Generic;

namespace Inno.Core.Serialization;

internal struct SerializationArrayCapture(ArraySerializationNode? previous)
{
    private readonly ArraySerializationNode? m_initialNode = previous is null ? new ArraySerializationNode() : null;
    private List<SerializationNode>? m_values;
    private int m_count;

    internal SerializationNode? Next => previous is not null && m_count < previous.values.Count
        ? previous.values[m_count]
        : null;

    internal ArraySerializationNode? initialNode => m_initialNode;

    internal void Add(SerializationNode value)
    {
        if (m_initialNode is not null)
            m_values = m_initialNode.values;
        if (m_values is null && !ReferenceEquals(Next, value))
            m_values = previous is null ? [] : new(previous.values);
        if (m_values is not null)
        {
            if (m_count < m_values.Count)
                m_values[m_count] = value;
            else
                m_values.Add(value);
        }
        m_count++;
    }

    internal ArraySerializationNode Complete()
    {
        if (m_initialNode is not null)
            return m_initialNode;
        if (previous is not null && m_count == previous.values.Count && m_values is null)
            return previous;
        m_values ??= previous is null ? [] : new(previous.values);
        if (m_values.Count > m_count)
            m_values.RemoveRange(m_count, m_values.Count - m_count);
        return new ArraySerializationNode(m_values);
    }
}
