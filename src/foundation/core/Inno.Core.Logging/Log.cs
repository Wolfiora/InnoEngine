using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Inno.Extensibility.Modules;

namespace Inno.Core.Logging;

/// <summary>
/// Writes script logs with compiler-provided source locations and the calling assembly's ownership.
/// </summary>
public static class Log
{
    private const string C_DEFAULT_CATEGORY = "Unknown";
    private static readonly ConditionalWeakTable<Assembly, AssemblySource> AssemblySources = new();

    /// <summary>
    /// Writes a debug-level message using the object's string representation.
    /// </summary>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Debug(
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Debug, $"{obj}", null, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a formatted debug-level message with its source location.
    /// </summary>
    /// <param name="message">
    /// The composite format string or final message.
    /// </param>
    /// <param name="arguments">
    /// Optional composite-format arguments; null preserves the message verbatim.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="FormatException">
    /// The composite format string is invalid for the supplied arguments.
    /// </exception>
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Debug(
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Debug, message, arguments, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a info-level message using the object's string representation.
    /// </summary>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Info(
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Info, $"{obj}", null, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a formatted info-level message with its source location.
    /// </summary>
    /// <param name="message">
    /// The composite format string or final message.
    /// </param>
    /// <param name="arguments">
    /// Optional composite-format arguments; null preserves the message verbatim.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="FormatException">
    /// The composite format string is invalid for the supplied arguments.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Info(
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Info, message, arguments, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a warn-level message using the object's string representation.
    /// </summary>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Warn(
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Warn, $"{obj}", null, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a formatted warn-level message with its source location.
    /// </summary>
    /// <param name="message">
    /// The composite format string or final message.
    /// </param>
    /// <param name="arguments">
    /// Optional composite-format arguments; null preserves the message verbatim.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="FormatException">
    /// The composite format string is invalid for the supplied arguments.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Warn(
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Warn, message, arguments, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a error-level message using the object's string representation.
    /// </summary>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Error(
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Error, $"{obj}", null, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a formatted error-level message with its source location.
    /// </summary>
    /// <param name="message">
    /// The composite format string or final message.
    /// </param>
    /// <param name="arguments">
    /// Optional composite-format arguments; null preserves the message verbatim.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="FormatException">
    /// The composite format string is invalid for the supplied arguments.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Error(
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Error, message, arguments, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a fatal-level message using the object's string representation.
    /// </summary>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Fatal(
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Fatal, $"{obj}", null, Assembly.GetCallingAssembly(), filePath, lineNumber);

    /// <summary>
    /// Writes a formatted fatal-level message with its source location.
    /// </summary>
    /// <param name="message">
    /// The composite format string or final message.
    /// </param>
    /// <param name="arguments">
    /// Optional composite-format arguments; null preserves the message verbatim.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="FormatException">
    /// The composite format string is invalid for the supplied arguments.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Fatal(
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Fatal, message, arguments, Assembly.GetCallingAssembly(), filePath, lineNumber);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Write(
        LogLevel level,
        string message,
        IReadOnlyList<object?>? arguments,
        Assembly callerAssembly,
        string filePath,
        int lineNumber
    ) {
        LogRouter router = LogRouter.current;
        if (!router.IsEnabled(level))
            return;

        AssemblySource source = AssemblySources.GetValue(callerAssembly, static assembly => new AssemblySource(
            assembly.GetInnoAssemblyDomain(), assembly.GetInnoAssemblyScope()));
        string category = GetCategory(filePath);
        string rendered = arguments is null || arguments.Count == 0
            ? message : string.Format(message, arguments.ToArray());
        router.Dispatch(new LogEntry(
            level,
            source.domain,
            source.scope,
            category,
            rendered,
            string.IsNullOrWhiteSpace(filePath) ? C_DEFAULT_CATEGORY : filePath,
            lineNumber,
            new StackTrace(2, true).ToString(),
            LogSessionContext.current));
    }

    private static string GetCategory(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            return C_DEFAULT_CATEGORY;

        // Compiler source paths belong to the authoring host, which can differ from the runtime host.
        int fileNameStart = Math.Max(filePath.LastIndexOf('/'), filePath.LastIndexOf('\\')) + 1;
        return Path.GetFileNameWithoutExtension(filePath.AsSpan(fileNameStart)).ToString();
    }

    private sealed record AssemblySource(
        AssemblyDomain domain,
        AssemblyScope scope
    );
}
