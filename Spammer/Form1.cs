using System;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace Spammer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public Boolean running = false;
        public string file;

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            openFileDialog1.Filter = "Text|*.txt";
            if (result == DialogResult.OK)
            {
                file = openFileDialog1.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    richTextBox1.Text = text;
                }
                catch (IOException){
                    MessageBox.Show("Hiba történt a file olvasása közben", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (file != null)
            {
                if (running)
                {
                    running = false;
                    button2.Text = "Indítás";
                }
                else
                {
                    running = true;
                    button2.Text = "Leállítás";
                }
            }
            Thread.Sleep((int)numericUpDown1.Value * 1000);
            Thread thread = new Thread(new ThreadStart(send));
            thread.Start();
        }

        public void send()
        {
            int index = 0;
            if (file != null)
            {
                while (running)
                {
                    Thread.Sleep((int)numericUpDown2.Value);
                    try
                    {
                        if (file != null)
                        {
                            string[] textLines = File.ReadAllLines(file);
                            SendKeys.SendWait((string)textLines.GetValue(index) + "{ENTER}");
                            index++;
                            if (index == textLines.Length)
                            {
                                index = 0;
                            }
                        }
                    }
                    catch (AbandonedMutexException)
                    {
                        //
                    }
                }
            }
            else
            {
                MessageBox.Show("Nem választottál ki fájlt", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Closing(object sender, EventArgs e)
        {
           Environment.Exit(Environment.ExitCode);
        }
    }
}