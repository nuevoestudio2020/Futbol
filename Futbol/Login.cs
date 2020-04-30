using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Data.SQLite;

namespace Futbol
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }


        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int Iparam);


        private void Login_Load(object sender, EventArgs e)
        {

        }

        private void txtuser_Enter(object sender, EventArgs e)
        {
            if (txtuser.Text == "USUARIO")
            {
                txtuser.Text = "";
                txtuser.ForeColor = Color.Black;
            }
        }

        private void txtuser_Leave(object sender, EventArgs e)
        {
            if (txtuser.Text == "")
            {
                txtuser.Text = "USUARIO";
                txtuser.ForeColor = Color.Black;
            }
        }

        private void txtpass_Enter(object sender, EventArgs e)
        {
            if (txtpass.Text == "CONTRASEÑA")
            {
                txtpass.Text = "";
                txtpass.ForeColor = Color.Black;
                txtpass.UseSystemPasswordChar = true;
            }
        }

        private void txtpass_Leave(object sender, EventArgs e)
        {
            if (txtpass.Text == "")
            {
                txtpass.Text = "CONTRASEÑA";
                txtpass.ForeColor = Color.Black;
                txtpass.UseSystemPasswordChar = false;
            }
        }

        private void btncerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnminimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        //conexion a la bd
        string dbcon = @"Data Source = database.db; version = 3";

       

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnlogin_Click(object sender, EventArgs e)
        {
            SQLiteConnection sqlcon = new SQLiteConnection(dbcon);

            if ((txtuser.Text == "") && (txtpass.Text == "") || (txtuser.Text == "") || (txtpass.Text == ""))
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Usuario o contraseña vacio !";
            }

            else
            {
                try
                {
                    sqlcon.Open();
                    string query = "SELECT * FROM usuarios WHERE user = '" + txtuser.Text + "' AND pass = '" + txtpass.Text + "'";
                    SQLiteCommand com = new SQLiteCommand(query, sqlcon);
                    com.ExecuteNonQuery();
                    SQLiteDataReader dr = com.ExecuteReader();
                    int count = 0;
                    while (dr.Read())
                    {
                        count++;
                    }
                    if (count == 1)
                    {
                        this.Hide();
                        Bienvenida b = new Bienvenida();
                        b.ShowDialog();
                        Fondo fo = new Fondo();
                        fo.Show();

                    }
                    if (count < 1)
                    {
                        lblMsg.Visible = true;
                        lblMsg.Text = "Usuario o contraseña incorrecto !";
                        txtpass.Clear();
                        txtuser.Clear();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error en login: " + ex);
                }
            }
        }
    }
}
