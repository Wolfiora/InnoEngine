using System;
using System.Numerics;

using Inno.Editor.Core;

namespace Inno.Editor.Interactions;

/// <summary>
/// Describes one active modal extension.
/// </summary>
public sealed class EditorModalExtension
{
    private readonly Action<Exception> m_quarantine;
    private readonly EditorModal m_modal;
    private bool m_isQuarantined;

    internal EditorModalExtension(
        string id,
        string title,
        int order,
        EditorModal modal,
        Action<Exception> quarantine
    ) {
        this.id = id;
        this.title = title;
        this.order = order;
        m_modal = modal;
        m_quarantine = quarantine;
    }

    /// <summary>
    /// Gets the stable modal identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the visible modal title.
    /// </summary>
    public string title { get; }

    /// <summary>
    /// Gets the stable modal ordering value.
    /// </summary>
    public int order { get; }

    /// <summary>
    /// Prepares one presentation frame and quarantines callback failures within its owning presentation boundary.
    /// </summary>
    /// <param name="context">
    /// The active owner-thread context.
    /// </param>
    /// <param name="presentation">
    /// The window policy already captured for this presentation attempt.
    /// </param>
    /// <param name="availableContentSize">
    /// The bounded content size in current presentation coordinates.
    /// </param>
    /// <param name="contentSize">
    /// The measured extent, or null for explicit sizing or failed preparation.
    /// </param>
    /// <returns>
    /// True when preparation succeeded; false when this extension is quarantined.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    public bool TryPrepare(
        EditorContext context,
        Presentation presentation,
        Vector2 availableContentSize,
        out Vector2? contentSize
    ) {
        ArgumentNullException.ThrowIfNull(context);
        contentSize = null;
        if (m_isQuarantined)
            return false;
        try
        {
            contentSize = m_modal.Prepare(context, availableContentSize);
            if (contentSize is Vector2 size &&
                (!float.IsFinite(size.X) || !float.IsFinite(size.Y) || size.X < 0f || size.Y < 0f))
                throw new InvalidOperationException("Modal preparation returned an invalid content extent.");
            if (presentation.initialSize.Y <= 0f && contentSize is null)
                throw new InvalidOperationException("A content-sized modal must measure its current content before drawing.");
            return true;
        }
        catch (Exception exception)
        {
            contentSize = null;
            m_isQuarantined = true;
            m_quarantine(exception);
            return false;
        }
    }

    /// <summary>
    /// Safely captures generation-local modal presentation values.
    /// </summary>
    /// <param name="presentation">
    /// The captured immutable presentation, or its default value when the modal is quarantined.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when every presentation value was read successfully.
    /// </returns>
    public bool TryGetPresentation(out Presentation presentation)
    {
        if (m_isQuarantined)
        {
            presentation = default;
            return false;
        }
        try
        {
            presentation = new Presentation(
                m_modal.isVisible,
                m_modal.blocksInteraction,
                m_modal.canMove,
                m_modal.canResize,
                m_modal.allowScrolling,
                m_modal.initialSize,
                m_modal.minimumSize);
            Vector2 initial = presentation.initialSize;
            Vector2 minimum = presentation.minimumSize;
            if (!float.IsFinite(initial.X) || !float.IsFinite(initial.Y) || initial.X < 0f || initial.Y < 0f ||
                !float.IsFinite(minimum.X) || !float.IsFinite(minimum.Y) || minimum.X < 0f || minimum.Y < 0f)
                throw new InvalidOperationException("Modal dimensions must be finite and nonnegative; zero leaves that dimension unspecified.");
            return true;
        }
        catch (Exception exception)
        {
            presentation = default;
            m_isQuarantined = true;
            m_quarantine(exception);
            return false;
        }
    }

    /// <summary>
    /// Safely draws the modal body and quarantines a failing extension instance.
    /// </summary>
    /// <param name="context">
    /// The active editor context supplied by the presentation backend.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the modal completed drawing without being quarantined.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context"/> is <see langword="null"/>.
    /// </exception>
    public bool Draw(EditorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        if (m_isQuarantined)
            return false;
        try
        {
            m_modal.Draw(context);
            return true;
        }
        catch (Exception exception)
        {
            m_isQuarantined = true;
            m_quarantine(exception);
            return false;
        }
    }

    /// <summary>
    /// Stores immutable modal window policy values read from one extension generation.
    /// </summary>
    public readonly record struct Presentation
    {
        /// <summary>
        /// Creates an immutable modal presentation snapshot.
        /// </summary>
        /// <param name="isVisible">
        /// Whether the modal should currently be visible.
        /// </param>
        /// <param name="blocksInteraction">
        /// Whether the modal blocks regular editor interaction.
        /// </param>
        /// <param name="canMove">
        /// Whether the modal window can be moved.
        /// </param>
        /// <param name="canResize">
        /// Whether the modal window can be resized.
        /// </param>
        /// <param name="allowScrolling">
        /// Whether the modal window owns overflow scrolling.
        /// </param>
        /// <param name="initialSize">
        /// The initial size in unscaled editor units.
        /// </param>
        /// <param name="minimumSize">
        /// The minimum size in unscaled editor units.
        /// </param>
        public Presentation(
            bool isVisible,
            bool blocksInteraction,
            bool canMove,
            bool canResize,
            bool allowScrolling,
            Vector2 initialSize,
            Vector2 minimumSize
        ) {
            this.isVisible = isVisible;
            this.blocksInteraction = blocksInteraction;
            this.canMove = canMove;
            this.canResize = canResize;
            this.allowScrolling = allowScrolling;
            this.initialSize = initialSize;
            this.minimumSize = minimumSize;
        }

        /// <summary>
        /// Gets whether the modal should currently be visible.
        /// </summary>
        public bool isVisible { get; }

        /// <summary>
        /// Gets whether the modal blocks regular editor interaction.
        /// </summary>
        public bool blocksInteraction { get; }

        /// <summary>
        /// Gets whether the modal window can be moved.
        /// </summary>
        public bool canMove { get; }

        /// <summary>
        /// Gets whether the modal window can be resized.
        /// </summary>
        public bool canResize { get; }

        /// <summary>
        /// Gets whether overflow scrolling belongs to the modal window.
        /// </summary>
        public bool allowScrolling { get; }

        /// <summary>
        /// Gets the initial size in unscaled editor units.
        /// </summary>
        public Vector2 initialSize { get; }

        /// <summary>
        /// Gets the minimum size in unscaled editor units.
        /// </summary>
        public Vector2 minimumSize { get; }
    }
}
