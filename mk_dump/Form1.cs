using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace mk_dump
{
    public partial class Form1 : Form
    {
        string default_filename = "dump.bin";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            makeDump();
        }

        private void makeDump()
        {
            List<Byte> lb = new List<Byte>();
            
            string filename = textBox2.Text;
            if (filename.Length == 0)
            {
                filename = this.default_filename;
            }
            if ( File.Exists(filename) )
            {
                File.Delete(filename);
            }
            FileStream fs = File.Create(filename);
            Byte[] bytes = System.Text.ASCIIEncoding.ASCII.GetBytes(textBox1.Text);

            int i = 0;
            while (i < bytes.Length - 1)
            {
                if (bytes[i] == 0x20 || bytes[i] == 0x0A || bytes[i] == 0x0D)
                {
                    i++; continue;
                }
                else
                { 
                    string str = System.Text.ASCIIEncoding.ASCII.GetString(new Byte[]{bytes[i], bytes[i+1] }, 0, 2 );
                    fs.WriteByte(Convert.ToByte(str,16));
                    i += 2;
                }  
            }
            
            fs.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
