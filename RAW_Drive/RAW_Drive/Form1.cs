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
            disk.OpenDrive(2);

            disk.Position += 446;

            byte[] p1 = disk.Read(16);
            byte[] p2 = disk.Read(16);
            byte[] p3 = disk.Read(16);
            byte[] p4 = disk.Read(16);



            //disk.Flush();
            disk.Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
