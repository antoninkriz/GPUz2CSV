using System;
using System.IO;
using System.Security;
using System.Text;
using System.Windows.Forms;

namespace GPUz2CSV
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeOpenFileDialog();
        }

        private void InitializeOpenFileDialog()
        {
            openFileDialog1.Filter = "Text documents (*.txt)|*.txt|" + "All files (*.*)|*.*";
            openFileDialog1.Title = "GPUz2CSV";
            openFileDialog1.Multiselect = true;
            openFileDialog1.FileName = "GPU-Z Sensor Log.txt";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            checkBox3.Enabled = false;

            DialogResult dr = openFileDialog1.ShowDialog();

            string savedir = null;
            if (!checkBox3.Checked)
            {
                savedir = dirdialog();
            }

            if ((openFileDialog1.FileNames != null || (savedir != null && openFileDialog1.FileNames != null)) && dr == DialogResult.OK)
            {
                generateFiles(savedir);
            }

            button1.Enabled = true;
            checkBox1.Enabled = true;
            checkBox2.Enabled = true;
            checkBox3.Enabled = true;

        }

        private void generateFiles(string savedir)
        {
            if (openFileDialog1.FileNames != null || savedir != null)
            {
                Console.WriteLine(openFileDialog1.FileNames);
                Console.WriteLine(savedir);
                foreach (String file in openFileDialog1.FileNames)
                {
                    genfiles(file, savedir);
                }
            }
        }

        private void genfiles(string file, string savedir)
        {
            try
            {
                string content = File.ReadAllText(file, Encoding.UTF8);
                if (checkBox2.Checked)
                {
                    if (checkBox1.Checked)
                    {
                        content = File.ReadAllText(file, Encoding.UTF8).Replace(",", ";").Replace(".", ",");
                    }
                    else
                    {
                        content = File.ReadAllText(file, Encoding.UTF8).Replace(",", ";");
                    }
                }
                else if (checkBox1.Checked)
                {
                    content = File.ReadAllText(file, Encoding.UTF8).Replace(".", ",");
                }

                string newfilepath;
                if (checkBox3.Checked)
                {
                    newfilepath = Path.GetDirectoryName(file) + @"\" + Path.GetFileNameWithoutExtension(file) + ".csv";
                }
                else
                {
                    newfilepath = savedir + @"\" + Path.GetFileNameWithoutExtension(file) + ".csv";
                }


                File.CreateText(newfilepath).Close();

                File.WriteAllText(newfilepath, content, Encoding.UTF8);
            }
            catch (SecurityException ex)
            {
                MessageBox.Show("Security error. Please contact your administrator for details.\n\n" +
                    "Error message: " + ex.Message + "\n\n" +
                    "Details (send to Support):\n\n" + ex.StackTrace
                );
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot edit the files: " + file.Substring(file.LastIndexOf('\\'))
                    + ". You may not have permission to read the file, or " +
                    "it may be corrupt.\n\nReported error: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string savedir = null;
            savedir = dirdialog();
            if (savedir != null)
            {
                string[] files = Directory.GetFiles(savedir, "*.txt");
                if (files != null)
                {
                    foreach (String file in files)
                    {
                        genfiles(file, savedir);
                    }
                }
            }

        }


        private string dirdialog()
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    return fbd.SelectedPath;
                }
                return null;
            }
        }


    }
}
