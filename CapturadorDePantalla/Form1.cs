using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Microsoft.Win32;
using System.IO;
using System.Xml.Linq;
using System.Xml;
using System.Threading;
using System.Xml.XPath;
using System.Configuration;
using System.Diagnostics;

namespace CapturadorDePantalla
{
    public partial class Form1 : Form
    {
        private Form1 _mainMenu = null;
        private Boolean Bandera = false;
        System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
        static System.Threading.Timer TTimer = null;
        private String idApp = "StarkIndustries";
        RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

        public Form1()
        {
            int temp = 0;
            InitializeComponent();
            Cargar();
            killAllSimilarProces();
            this.ControlBox = false;
            temp = getResu();
            int temp2 = 0;
            switch (temp)
            {
                case 1:
                    temp2 = 0;
                    temp2 = getResu2();
                    if (temp2 > 1)
                    {
                        DialogResult result = MessageBox.Show("Quieres abrir la Configuracion", "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("Se detendra la captura automatica mientras este en la configuracion");
                            killAllSimilarProces();
                            this.WindowState = FormWindowState.Normal;
                        }
                        else if (result == DialogResult.No)
                        {
                            checkBox1.Checked = true;
                            button1_Click(null, EventArgs.Empty);
                            this.WindowState = FormWindowState.Minimized;
                        }
                    }
                    else
                    {
                        checkBox1.Checked = true;
                        button1_Click(null, EventArgs.Empty);
                        this.WindowState = FormWindowState.Minimized;
                    }

                    
                    break;
                case 2:
                    temp2 = 0;
                    temp2 = getResu2();
                    if (temp2 > 1)
                    {
                        DialogResult result = MessageBox.Show("Quieres abrir la Configuracion", "Warning",
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (result == DialogResult.Yes)
                        {
                            MessageBox.Show("Se detendra la captura automatica mientras este en la configuracion");
                            killAllSimilarProces();
                            this.WindowState = FormWindowState.Normal;
                        }
                        else if (result == DialogResult.No)
                        {
                            checkBox1.Checked = true;
                            button1_Click(null, EventArgs.Empty);
                            this.WindowState = FormWindowState.Minimized;
                        }
                    }
                    else
                    {
                        this.WindowState = FormWindowState.Normal;
                    }
                    break;
            }

            bgw.WorkerSupportsCancellation = true;
            bgw.WorkerReportsProgress = true;
            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(bgw_RunWorkerCompleted);
            bgw.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(bgw_ProgressChanged);
        }

        private void killAllSimilarProces()
        {
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            string name = p.ProcessName;
            Process currentProcess = Process.GetCurrentProcess();
            string pid = currentProcess.Id.ToString();
            Process[] LocalByName = Process.GetProcessesByName(name);
            bool esta = false;
            foreach (Process proc in LocalByName)
            {
                if (proc.Id != id) {
                    proc.Kill();
                }
            }
        }

        private int getResu()
        {
            int temp = 0;
            if (rkApp.GetValue(idApp) == null)
            {
                temp = 2;
            }
            else
            {
                temp = 1;
            }
            return temp;

        }

        private int getResu2()
        {
            int temp = 0;
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            string name = p.ProcessName;
            Process currentProcess = Process.GetCurrentProcess();
            string pid = currentProcess.Id.ToString();
            Process[] LocalByName = Process.GetProcessesByName(name);
            bool esta = false;
            foreach (Process proc in LocalByName)
            {
                temp++;
            }
            return temp;
        }

        void bgw_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {

        }

        private void Cargar()
        {
            string configvalue1 = Properties.Settings.Default.Ruta;
            textBox1.Text = configvalue1;
            int configvalue2 = Properties.Settings.Default.Tiempo;
            textBox2.Text = configvalue2.ToString();
        }

        public void Guardar()
        {
            Properties.Settings.Default.Ruta = textBox1.Text;
            Properties.Settings.Default.Tiempo = int.Parse(textBox2.Text);
            Properties.Settings.Default.Save();
            Cargar();
        }

        Bitmap bmpScreenshot = null;
        Graphics gfxScreenshot = null;

        private void SoloCapturar()
        {
            string tiempo = DateTime.Now.ToString("MM-dd-yyyy HH-mm-ss");
            bmpScreenshot = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            bmpScreenshot.Save(Properties.Settings.Default.Ruta + "/" + tiempo + ".png", ImageFormat.Png);
            gfxScreenshot.Dispose();
            bmpScreenshot.Dispose();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                rkApp.SetValue(idApp, Application.ExecutablePath);
            }
            else
            {
                rkApp.DeleteValue(idApp, false);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fb = new FolderBrowserDialog();
            if (fb.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textBox1.Text = fb.SelectedPath;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                button1.PerformClick();
            }
            this.Hide();
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                Hide();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Guardar();
            System.Diagnostics.Process p = System.Diagnostics.Process.GetCurrentProcess();
            int id = p.Id;
            string name = p.ProcessName;
            Process[] LocalByName = Process.GetProcessesByName(name);
            this.Bandera = true;
            bgw.RunWorkerAsync();
            this.button1.Enabled = false;
            if (WindowState == FormWindowState.Normal)
            {
                Hide();
            }
        }

        void bgw_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
            }
            else if (e.Cancelled)
            {
                //MessageBox.Show("Ejecución Cancelada");
            }
            else
            {
                //MessageBox.Show("Ejecución Completada");
                //this.button1.IsEnabled = true;
            }
        }
        void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            StartCapture();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bgw.CancelAsync();
            this.button1.Enabled = true;
            this.Bandera = false;
        }

        public void StartCapture()
        {
            while (Bandera)
            {
                SoloCapturar();
                Thread.Sleep(Properties.Settings.Default.Tiempo * 1000);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Guardar();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Dio Click");
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
