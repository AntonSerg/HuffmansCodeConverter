using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;

namespace Haffman_sCodesWF
{
    public partial class Form1 : Form
    {
        string fileContent;
        Thread th;
        public Form1()
        {
            InitializeComponent();
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Get the path of specified file
                string filePath = openFileDialog.FileName;

                //Read the contents of the file into a stream
                var fileStream = openFileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    fileContent = reader.ReadToEnd();
                }
                textBox1.Text = fileContent;
            }
        }
        private void start()
        {
            textBox1.Invoke(new Action(() => { textBox1.Text = ""; }));

            Dictionary<char, int> counter = new Dictionary<char, int>();
            char[] fileContentChar = fileContent.ToCharArray();
            for (int i = 0; i < fileContentChar.Length; i++)
            {
                if (counter.ContainsKey(fileContentChar[i]))
                {
                    counter[fileContentChar[i]] += 1;
                }
                else
                {
                    counter.Add(fileContentChar[i], 1);
                }
            }
            Dictionary<char, string> codes = new Dictionary<char, string>();
            codes.Add(' ', intConvertToBite(1));
            for (int i = 0; i < 26; i++)
            {
                codes.Add(Convert.ToChar('a' + i), intConvertToBite(i + 2));
            }
            string textBoxContent = "";
            for (int i = 0; i < fileContentChar.Length; i++)
            {
                textBoxContent += codes[fileContentChar[i]];
            }

            textBox1.Invoke(new Action(() => { textBox1.Text = textBoxContent; }));

        }
        private string intConvertToBite(int n)
        {
            char[] array = new char[5] { '0', '0', '0', '0', '0' };
            for (int i = 4; i >= 0; --i)
            {
                if (n % 2 == 0)
                {
                    array[i] = '0';
                    n = n / 2;
                }
                else
                {
                    array[i] = '1';
                    n = n / 2;
                }
                if (n == 0)
                {
                    return new string(array);
                }
            }
            return new string(array);
        }
        private void btnConvert_Click(object sender, EventArgs e)
        {
            th = new Thread(start);
            th.Start();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            th.Abort();
        }
    }
}
