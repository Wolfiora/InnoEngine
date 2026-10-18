using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Inno.Build.Toolchains;

internal sealed class WindowsToolchainDirectory : IDisposable
{
    private const uint C_MOUNT_POINT_TAG = 0xA0000003;
    private const uint C_SET_REPARSE_POINT = 0x000900A4;
    private readonly string m_physicalPath;

    private WindowsToolchainDirectory(
        string physicalPath,
        string toolPath
    ) {
        m_physicalPath = physicalPath;
        this.toolPath = toolPath;
    }

    internal string toolPath { get; }

    internal static WindowsToolchainDirectory Create(string physicalPath)
    {
        Directory.CreateDirectory(physicalPath);
        if (!OperatingSystem.IsWindows())
            return new(physicalPath, physicalPath);

        // A stable alias keeps CMake and generated project caches valid across build attempts.
        string alias = ToolchainWorkingDirectory.GetAliasPath(physicalPath);
        Directory.CreateDirectory(Path.GetDirectoryName(alias)!);
        if (Directory.Exists(alias))
        {
            ValidateAlias(alias, physicalPath);
            return new(physicalPath, alias);
        }

        Directory.CreateDirectory(alias);
        try
        {
            byte[] substitute = Encoding.Unicode.GetBytes(physicalPath.StartsWith("\\\\", StringComparison.Ordinal)
                ? "\\??\\UNC\\" + physicalPath[2..] : "\\??\\" + physicalPath);
            byte[] printable = Encoding.Unicode.GetBytes(physicalPath);
            byte[] data = new byte[20 + substitute.Length + printable.Length];
            Write(data, 0, C_MOUNT_POINT_TAG);
            Write(data, 4, checked((ushort)(data.Length - 8)));
            Write(data, 10, checked((ushort)substitute.Length));
            Write(data, 12, checked((ushort)(substitute.Length + 2)));
            Write(data, 14, checked((ushort)printable.Length));
            substitute.CopyTo(data, 16);
            printable.CopyTo(data, 18 + substitute.Length);

            using SafeFileHandle handle = CreateFile(alias, 0x40000000, 7, nint.Zero, 3, 0x02200000, nint.Zero);
            if (handle.IsInvalid)
                throw new Win32Exception(Marshal.GetLastPInvokeError(), "Cannot open the owned toolchain directory alias.");
            if (!DeviceIoControl(handle, C_SET_REPARSE_POINT, data, (uint)data.Length,
                nint.Zero, 0, out _, nint.Zero))
                throw new Win32Exception(Marshal.GetLastPInvokeError(), "Cannot create the owned toolchain directory alias.");
            return new(physicalPath, alias);
        }
        catch
        {
            Directory.Delete(alias, recursive: false);
            throw;
        }
    }

    /// <inheritdoc />
    public void Dispose()
    {
        if (toolPath == m_physicalPath || !Directory.Exists(toolPath))
            return;
        ValidateAlias(toolPath, m_physicalPath);
        Directory.Delete(toolPath, recursive: false);
    }

    private static void ValidateAlias(
        string alias,
        string physicalPath
    ) {
        string? target = new DirectoryInfo(alias).LinkTarget;
        if (target is null || !Path.GetFullPath(target).Equals(physicalPath, StringComparison.OrdinalIgnoreCase))
            throw new IOException($"Toolchain directory alias '{alias}' is occupied by a different directory.");
    }

    private static void Write(
        byte[] destination,
        int offset,
        uint value
    ) => BitConverter.GetBytes(value).CopyTo(destination, offset);

    private static void Write(
        byte[] destination,
        int offset,
        ushort value
    ) => BitConverter.GetBytes(value).CopyTo(destination, offset);

    [DllImport("kernel32.dll", EntryPoint = "CreateFileW", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern SafeFileHandle CreateFile(
        string path,
        uint access,
        uint sharing,
        nint security,
        uint creation,
        uint flags,
        nint template
    );

    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool DeviceIoControl(
        SafeFileHandle handle,
        uint operation,
        byte[] input,
        uint inputSize,
        nint output,
        uint outputSize,
        out uint written,
        nint overlapped
    );
}
