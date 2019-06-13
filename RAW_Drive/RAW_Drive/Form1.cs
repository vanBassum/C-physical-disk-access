using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Management;

namespace RAW_Drive
{



    public partial class Form1 : Form
    {
        Disk disk = new Disk();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Button1_Click(null, null);





            /*
            disk.OpenDrive(2);

            disk.Read(446);

            Partition p1 = new Partition();

            byte[] buf = new byte[4];

            p1.BootFlag = (byte)disk.ReadByte();
            disk.Read(buf, 0, 3);
            p1.CHS_Begin = BitConverter.ToUInt32(buf, 0);
            p1.TypeCode = (byte)disk.ReadByte();
            disk.Read(buf, 0, 3);
            p1.CHS_End = BitConverter.ToUInt32(buf, 0);
            p1.LBA_Begin = BitConverter.ToUInt32(disk.Read(4), 0);
            p1.NumberOfSectors = BitConverter.ToUInt32(disk.Read(4), 0);

            disk.Seek(p1.LBA_Begin, SeekOrigin.Begin);

            buf = new byte[512];
            disk.Read(buf);

            //disk.Flush();
            disk.Close();
            */
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(WIN32_DiskDrive.GetDrives());

            if (comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            WIN32_DiskDrive selectedDrive = comboBox1.SelectedItem as WIN32_DiskDrive;
            disk.Open(selectedDrive);

            disk.Read(446);

            Partition p1 = new Partition();

            byte[] buf = new byte[4];

            p1.BootFlag = (byte)disk.ReadByte();
            disk.Read(buf, 0, 3);
            p1.CHS_Begin = BitConverter.ToUInt32(buf, 0);
            p1.TypeCode = (byte)disk.ReadByte();
            disk.Read(buf, 0, 3);
            p1.CHS_End = BitConverter.ToUInt32(buf, 0);
            p1.LBA_Begin = BitConverter.ToUInt32(disk.Read(4), 0);
            p1.NumberOfSectors = BitConverter.ToUInt32(disk.Read(4), 0);

            disk.Seek(p1.LBA_Begin, SeekOrigin.Begin);

            buf = new byte[512];
            disk.Read(buf);

            //disk.Flush();
            disk.Close();
        }
    }

    public class Partition
    {
        public byte BootFlag { get; set; }
        public UInt32 CHS_Begin { get; set; }
        public UInt32 CHS_End { get; set; }
        public byte TypeCode { get; set; }
        public UInt32 LBA_Begin { get; set; }
        public UInt32 NumberOfSectors { get; set; }
    }
}
