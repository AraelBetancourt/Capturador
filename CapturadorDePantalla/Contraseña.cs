using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapturadorDePantalla
{
    public partial class Contraseña : Form
    {
        public Contraseña()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string pass = textBox1.Text;
            string passProgram = Properties.Settings.Default.Contraseña;
            if (pass == passProgram)
            {
                this.Hide();
                Inicio ini = new Inicio();
                ini.Show();
            }
            else {
                MessageBox.Show("Contraseña Incorrecta");
            }
        }
    }
}
