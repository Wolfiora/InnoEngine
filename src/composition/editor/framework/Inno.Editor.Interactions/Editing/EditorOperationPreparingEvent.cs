using Inno.Core.Events;

namespace Inno.Editor.Interactions;
/// <summary>
/// Synchronously closes transient edits before command execution or an extension-handler generation switch.
/// </summary>
/// <remarks>
/// A handler failure aborts the boundary. Subscribers must use owned Core Event hubs and release them during retirement.
/// </remarks>
public sealed class EditorOperationPreparingEvent : Event;
