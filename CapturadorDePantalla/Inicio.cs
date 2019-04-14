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
    public partial class Inicio : Form
    {
        public Inicio()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string pass1 = textBox1.Text;
            string pass2 = textBox2.Text;
            if (pass1 == pass2)
            {
                Properties.Settings.Default.Contraseña=pass2;
                MessageBox.Show("Contraseña actalizada correctamente");
            }
            else
            {
                MessageBox.Show("No coinciden las contraseñas");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void Inicio_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }
    }
}
