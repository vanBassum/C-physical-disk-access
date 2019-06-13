﻿using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;



public class Disk : Stream
{
    Stream inner;
    IntPtr handle;

    public Disk()
    {
        
    }
    public bool Open(string driveLetter)
    {
        const int VolumeNameSize = 255;
        const int FileSystemNameBufferSize = 255;
        StringBuilder volumeNameBuffer = new StringBuilder(VolumeNameSize);
        StringBuilder fileSystemNameBuffer = new StringBuilder(FileSystemNameBufferSize);

        handle = NativeMethods.CreateFile(
            string.Format("\\\\.\\{0}:", driveLetter),
            NativeMethods.GenericRead | NativeMethods.GenericWrite,
            NativeMethods.FileShareRead | NativeMethods.Filesharewrite,
            IntPtr.Zero,
            NativeMethods.OpenExisting,
            0,
            IntPtr.Zero);

        inner = new FileStream(handle, FileAccess.ReadWrite);
        return handle != (IntPtr)(-1);
    }

    /*
    public bool Open(int driveNumber)
    {
        const int VolumeNameSize = 255;
        const int FileSystemNameBufferSize = 255;
        StringBuilder volumeNameBuffer = new StringBuilder(VolumeNameSize);
        StringBuilder fileSystemNameBuffer = new StringBuilder(FileSystemNameBufferSize);

        handle = NativeMethods.CreateFile(
            string.Format("\\\\.\\PHYSICALDRIVE{0}", driveNumber),
            NativeMethods.GenericRead,
            NativeMethods.FileShareRead | NativeMethods.Filesharewrite,
            IntPtr.Zero,
            NativeMethods.OpenExisting,
            0,
            IntPtr.Zero);

        inner = new FileStream(handle, FileAccess.ReadWrite);
        return handle != (IntPtr)(-1);
    }
    */
    public override void Close()
    {
        NativeMethods.CloseHandle(handle);
    }


    public override bool CanRead => inner.CanRead;

    public override bool CanSeek => inner.CanSeek;

    public override bool CanWrite => inner.CanWrite;

    public override long Length => inner.Length;

    public override long Position { get => inner.Position; set => inner.Position = value; }

    public override void Flush()
    {
        inner.Flush();
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        return inner.Read(buffer, offset, count);
    }

    public override long Seek(long offset, SeekOrigin origin)
    {
        return inner.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        inner.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        inner.Write(buffer, offset, count);
    }
}

internal static class NativeMethods
{
    public const uint GenericRead = ((uint)1 << 31);
    public const uint GenericWrite = ((uint)1 << 01);
    public const uint FileShareRead = 1;
    public const uint Filesharewrite = 2;
    public const uint OpenExisting = 3;
    public const uint IoctlVolumeGetVolumeDiskExtents = 0x560000;
    public const uint IncorrectFunction = 1;
    public const uint ErrorInsufficientBuffer = 122;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr handle);

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
    public static extern IntPtr CreateFile(
        string lpFileName,
        uint dwDesiredAccess,
        uint dwShareMode,
        IntPtr lpSecurityAttributes,
        uint dwCreationDisposition,
        int dwFlagsAndAttributes,
        IntPtr hTemplateFile);

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern bool GetVolumeInformationByHandleW(
        IntPtr hDisk,
        StringBuilder volumeNameBuffer,
        int volumeNameSize,
        ref uint volumeSerialNumber,
        ref uint maximumComponentLength,
        ref uint fileSystemFlags,
        StringBuilder fileSystemNameBuffer,
        int nFileSystemNameSize);

}