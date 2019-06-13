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

namespace RAW_Drive
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Disk disk = new Disk();
            disk.Open("F");
            byte[] data = new byte[1024];
            disk.Write(data, 0, data.Length);
            disk.Flush();
            disk.Close();
        }

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }

    
}
