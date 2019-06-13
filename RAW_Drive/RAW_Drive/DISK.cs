using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.ComponentModel;
using System.Text;



public class Disk : Stream
{
    Stream innerStream;
    IntPtr handle;

    public Disk()
    {
        
    }
    public void OpenVolume(string driveLetter)
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

        int err = Marshal.GetLastWin32Error();

        if (err != 0)
            throw new Exception("Error: " + err);

        innerStream = new FileStream(handle, FileAccess.ReadWrite);
    }

    
    public void OpenDrive(int driveNumber)
    {
        const int VolumeNameSize = 255;
        const int FileSystemNameBufferSize = 255;
        StringBuilder volumeNameBuffer = new StringBuilder(VolumeNameSize);
        StringBuilder fileSystemNameBuffer = new StringBuilder(FileSystemNameBufferSize);



        handle = NativeMethods.CreateFile(
            string.Format("\\\\.\\PhysicalDrive{0}", driveNumber),
            NativeMethods.GenericRead,
            NativeMethods.FileShareRead | NativeMethods.Filesharewrite,
            IntPtr.Zero,
            NativeMethods.OpenExisting,
            0,
            IntPtr.Zero);

        int err = Marshal.GetLastWin32Error();

        if (err != 0)
            throw new Exception("Error: " + err);

        innerStream = new FileStream(handle, FileAccess.ReadWrite);
    }
    
    public override void Close()
    {
        NativeMethods.CloseHandle(handle);
    }


    public override bool CanRead => innerStream.CanRead;

    public override bool CanSeek => innerStream.CanSeek;

    public override bool CanWrite => innerStream.CanWrite;

    public override long Length => innerStream.Length;

    public override long Position { get => innerStream.Position; set => innerStream.Position = value; }

    public override void Flush()
    {
        innerStream.Flush();
    }

    /*
     * 
     * Once the logical or physical drive has been opened, you can then perform direct I/O to the data on the entire drive. When performing direct disk I/O, you must seek, read, and write in multiples of sector sizes of the device and on sector boundaries. Call DeviceIoControl() using IOCTL_DISK_GET_DRIVE_GEOMETRY to get the bytes per sector, number of sectors, sectors per track, and so forth, so that you can compute the size of the buffer that you will need.
     * 
     */
    public override int Read(byte[] buffer, int offset, int count)
    {
        return innerStream.Read(buffer, offset, count);
    }

    public int Read(byte[] buffer)
    {
        return Read(buffer, 0, buffer.Length);
    }

    public byte[] Read(int length)
    {
        byte[] data = new byte[length];
        Read(data, 0, data.Length);
        return data;
    }


    public override long Seek(long offset, SeekOrigin origin)
    {
        return innerStream.Seek(offset, origin);
    }

    public override void SetLength(long value)
    {
        innerStream.SetLength(value);
    }

    public override void Write(byte[] buffer, int offset, int count)
    {
        innerStream.Write(buffer, offset, count);
    }


}

internal static class NativeMethods
{
    public const uint GenericRead = ((uint)1 << 31);
    public const uint GenericWrite = ((uint)1 << 30);
    public const uint GenericAll = ((uint)1 << 28);
    public const uint FileShareRead = 1;
    public const uint Filesharewrite = 2;
    public const uint OpenExisting = 3;
    public const uint IoctlVolumeGetVolumeDiskExtents = 0x560000;
    public const uint IncorrectFunction = 1;
    public const uint ErrorInsufficientBuffer = 122;

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool CloseHandle(IntPtr handle);


    //https://docs.microsoft.com/en-us/windows/desktop/api/fileapi/nf-fileapi-createfilea
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
 