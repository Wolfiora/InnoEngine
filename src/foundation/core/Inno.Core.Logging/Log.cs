using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Inno.Extensibility.Modules;

namespace Inno.Core.Logging;

/// <summary>
/// Writes logs with compiler-provided locations and explicitly declared type ownership across managed deployments.
/// </summary>
public static class Log
{
    private const string C_DEFAULT_CATEGORY = "Unknown";

    /// <summary>
    /// Writes a debug-level message using the object's string representation.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Debug(
        Type sourceType,
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Debug, $"{obj}", null, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a formatted debug-level message with its source location.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
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
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [Conditional("DEBUG")]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Debug(
        Type sourceType,
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Debug, message, arguments, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a info-level message using the object's string representation.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Info(
        Type sourceType,
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Info, $"{obj}", null, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a formatted info-level message with its source location.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
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
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Info(
        Type sourceType,
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Info, message, arguments, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a warn-level message using the object's string representation.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Warn(
        Type sourceType,
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Warn, $"{obj}", null, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a formatted warn-level message with its source location.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
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
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Warn(
        Type sourceType,
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Warn, message, arguments, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a error-level message using the object's string representation.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Error(
        Type sourceType,
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Error, $"{obj}", null, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a formatted error-level message with its source location.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
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
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Error(
        Type sourceType,
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Error, message, arguments, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a fatal-level message using the object's string representation.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
    /// <param name="obj">
    /// The object to log; null produces an empty message.
    /// </param>
    /// <param name="filePath">
    /// The source file supplied by the compiler; its file name is the category.
    /// </param>
    /// <param name="lineNumber">
    /// The source line supplied by the compiler.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Fatal(
        Type sourceType,
        object? obj,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Fatal, $"{obj}", null, sourceType, filePath, lineNumber);

    /// <summary>
    /// Writes a formatted fatal-level message with its source location.
    /// </summary>
    /// <param name="sourceType">
    /// The declaring type whose assembly supplies domain and scope metadata; it is borrowed only for this call.
    /// </param>
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
    /// <exception cref="ArgumentNullException">
    /// The source type is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The source assembly lacks valid domain or scope metadata.
    /// </exception>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static void Fatal(
        Type sourceType,
        string message,
        IReadOnlyList<object?>? arguments,
        [CallerFilePath] string filePath = "",
        [CallerLineNumber] int lineNumber = 0
    ) => Write(LogLevel.Fatal, message, arguments, sourceType, filePath, lineNumber);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void Write(
        LogLevel level,
        string message,
        IReadOnlyList<object?>? arguments,
        Type sourceType,
        string filePath,
        int lineNumber
    ) {
        ArgumentNullException.ThrowIfNull(sourceType);
        LogRouter router = LogRouter.current;
        if (!router.IsEnabled(level))
            return;

        AssemblyDomain domain = sourceType.Assembly.GetInnoAssemblyDomain();
        AssemblyScope scope = sourceType.Assembly.GetInnoAssemblyScope();
        string category = GetCategory(filePath);
        string rendered = arguments is null || arguments.Count == 0
            ? message : string.Format(message, arguments.ToArray());
        router.Dispatch(new LogEntry(
            level,
            domain,
            scope,
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

}
