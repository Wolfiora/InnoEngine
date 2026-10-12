using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Inno.Build.Toolchains;

/// <summary>
/// Owns a complete ordered Native product closure and releases its reading leases together.
/// </summary>
public sealed class ProductNativeBuildResult : IReadOnlyList<NativeBuildProduct>, IDisposable
{
    private readonly NativeBuildProduct[] m_products;
    private readonly NativeBindingGenerationResult m_bindings;
    private int m_disposed;

    internal ProductNativeBuildResult(
        IEnumerable<NativeBuildProduct> products,
        NativeBindingGenerationResult bindings
    ) {
        m_products = products.ToArray();
        m_bindings = bindings;
    }

    /// <summary>
    /// Gets the number of products in the completed build plan.
    /// </summary>
    public int Count => m_products.Length;

    /// <summary>
    /// Borrows a product whose lease is owned by this result until disposal.
    /// </summary>
    /// <param name="index">
    /// The zero-based position in the plan's declaration order.
    /// </param>
    /// <returns>
    /// The borrowed product; the caller must finish access before disposing the result.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The result no longer owns the products.
    /// </exception>
    public NativeBuildProduct this[int index]
    {
        get
        {
            ObjectDisposedException.ThrowIf(m_disposed != 0, this);
            return m_products[index];
        }
    }

    /// <summary>
    /// Enumerates borrowed products in plan order without transferring their ownership.
    /// </summary>
    /// <returns>
    /// An enumerator which must finish before this result is disposed.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The result has already released its leases.
    /// </exception>
    public IEnumerator<NativeBuildProduct> GetEnumerator()
    {
        ObjectDisposedException.ThrowIf(m_disposed != 0, this);
        return ((IEnumerable<NativeBuildProduct>)m_products).GetEnumerator();
    }

    /// <summary>
    /// Releases products in reverse dependency order after all consumers have finished.
    /// Repeated disposal has no effect.
    /// </summary>
    /// <exception cref="AggregateException">
    /// One or more reading owners failed to release after every owner was attempted.
    /// </exception>
    public void Dispose()
    {
        if (Interlocked.Exchange(ref m_disposed, 1) != 0)
            return;
        Release(m_products, m_bindings);
    }

    internal static void Release(
        IReadOnlyList<NativeBuildProduct> products,
        NativeBindingGenerationResult bindings
    ) {
        List<Exception> failures = [];
        for (int index = products.Count - 1; index >= 0; index--)
        {
            try
            {
                products[index].Dispose();
            }
            catch (Exception failure)
            {
                failures.Add(failure);
            }
        }
        try
        {
            bindings.Dispose();
        }
        catch (Exception failure)
        {
            failures.Add(failure);
        }
        if (failures.Count != 0)
            throw new AggregateException("Native product reading ownership could not be completely released.", failures);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
